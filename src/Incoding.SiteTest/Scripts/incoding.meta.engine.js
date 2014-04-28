"use strict";

//#region class IncodingMetaElement

function IncodingMetaElement(element) {

    var keyIncodingRunner = 'incoding-runner';

    var tryGetData = function(item, key) {

        var tempData = $(item).attr(key);
        if (!ExecutableHelper.IsNullOrEmpty(tempData)) {
            return tempData;
        }

        tempData = $(item).prop(key);
        if (!ExecutableHelper.IsNullOrEmpty(tempData)) {
            return tempData;
        }

        tempData = $.data(item, key);
        if (!ExecutableHelper.IsNullOrEmpty(tempData)) {
            return tempData;
        }

        return $(item).data(key);

    };

    this.element = element;
    this.runner = tryGetData(element, keyIncodingRunner);
    this.executables = !ExecutableHelper.IsNullOrEmpty(tryGetData(element, 'incoding')) ? $.parseJSON(tryGetData(element, 'incoding')) : '';
    this.bind = function(eventName, status) {

        var currentElement = this.element;

        $.each(IncSpecialBinds.DocumentBinds, function() {
            if (!eventName.contains(this)) {
                return true;
            }

            eventName = eventName.replaceAll(this, ''); //remove document bind from element bind           
            $(document).bind(this.toString(), function(e, result) { //this.toString() fixed for ie <10
                new IncodingMetaElement(currentElement)
                    .invoke(e, result);
                return false;
            });
        });

        if (eventName === '') {
            return;
        }

        $(this.element).bind(eventName.toString(), function(e, result) {

            new IncodingMetaElement(this)
                .invoke(e, result);

            var strStatus = status.toString();

            if (strStatus === '4' || eventName === IncSpecialBinds.Incoding) {
                return false;
            }

            if (strStatus === '2') {
                e.preventDefault();
            }
            if (strStatus === '3') {
                e.stopPropagation();
            }

            return true;
        });
    };
    this.invoke = function(e, result) {
        if (!ExecutableHelper.IsNullOrEmpty(this.runner)) {
            this.runner.DoIt(e, result);
        }
    };

    this.flushRunner = function(runner) {
        $.data(this.element, keyIncodingRunner, runner);
    };

}

//#endregion

//#region class Runner

function IncodingRunner() {
    this.actions = [];
    this.before = [];
    this.success = [];
    this.error = [];
    this.complete = [];
    this.breakes = [];
}

IncodingRunner.prototype = {
    DoIt : function(event, result) {

        var current = this;

        var filterFunc = function (executable) {            
            var isHas = $.trim(executable.onBind).split(' ').contains(event.type);
            if (isHas) {
                executable.event = event;
            }
            return isHas;
        };

        try {
            $($.grep(this.before, filterFunc)).each(function() {
                this.execute();
            });
        }
        catch(ex) {
            if (ex instanceof IncClientException) {
                $($.grep(this.breakes, filterFunc)).each(function() {
                    this.execute(ex);
                });
                return;
            }
            throw ex;
        }

        $($.grep(this.actions, filterFunc)).each(function() {
            this.execute({
                success : $.grep(current.success, filterFunc),
                error : $.grep(current.error, filterFunc),
                complete : $.grep(current.complete, filterFunc),
                breakes : $.grep(current.breakes, filterFunc),
                eventResult : result,
                event : event
            });
        });
    },

    Registry : function(metaType, onStatus, instance) {

        if (metaType.contains('Action')) {
            this.actions.push(instance);
        }
        else {

            switch (onStatus) {
                case 1:
                    this.before.push(instance);
                    break;
                case 2:
                    this.success.push(instance);
                    break;
                case 3:
                    this.error.push(instance);
                    break;
                case 4:
                    this.complete.push(instance);
                    break;
                case 5:
                    this.breakes.push(instance);
                    break;
                default:
                    throw 'Not found status {0}'.f(onStatus);
            }

        }
    }
};

//#endregion

//#region class IncClientException

function IncClientException() {
}

//#endregion

//#region class ParseServerResult

function IncodingResult(result) {

    var parse = function(json) {
        try {
            var res = _.isObject(json) ? json : $.parseJSON(json);
            var isSchemaValid = _.has(res, 'success') && _.has(res, 'redirectTo') && _.has(res, 'data');
            if (!isSchemaValid) {
                throw new 'Not valid json result';
            }
            return res;
        }
        catch(e) {
            console.log('fail parse result:{0}'.f(json));
            console.log('with exception:{0}'.f(e));
            return '';
        }
    };

    this.parseJson = parse(result);

    this.isValid = function() {
        return !ExecutableHelper.IsNullOrEmpty(this.parseJson);
    };

    this.redirectTo = this.isValid() ? this.parseJson.redirectTo : '';

    this.success = this.isValid() ? this.parseJson.success : false;

    this.data = this.isValid() ? this.parseJson.data : '';

}

IncodingResult.Success = function(data) {
    return new IncodingResult({ success : true, data : data, redirectTo : '' });
};

IncodingResult.Empty = new IncodingResult({ data : '', redirectTo : '', success : true });

//#endregion

//#region class IncodingEngine

function IncodingEngine() {

    this.parse = function (context) {

        var incSelector = '[incoding]';
        var defferedInit = [];
        $(incSelector, context)
            .add($(context).is(incSelector) ? context : '')
            .each(function() {                    
                var incodingMetaElement = new IncodingMetaElement(this);

                var runner = new IncodingRunner();
                var wasAddBinds = [];

                $(incodingMetaElement.executables).each(function() {
                    var metaData = this;

                    var jsonData = _.isObject(metaData.data) ? metaData.data : $.parseJSON(metaData.data);

                    var executableInstance = ExecutableFactory.Create(metaData.type, jsonData, incodingMetaElement.element);
                    runner.Registry(metaData.type, jsonData.onStatus, executableInstance);

                    var bindName = jsonData.onBind.toString();
                    if (wasAddBinds.contains(bindName)) {
                        return true;
                    }

                    wasAddBinds.push(bindName);
                    var onEventStatus = ExecutableHelper.IsNullOrEmpty(metaData.data.onEventStatus) ? '1' : metaData.data.onEventStatus;                
                    incodingMetaElement.bind(bindName
                        .toString()
                        .replaceAll(IncSpecialBinds.InitIncoding, '')
                        .replaceAll(' ' + IncSpecialBinds.InitIncoding, '')
                        .replaceAll(IncSpecialBinds.InitIncoding + ' ', '')                                                
                        .trim()
                        .toString(), onEventStatus.toString());
                });
                incodingMetaElement.flushRunner(runner);
                $(this).removeAttr('incoding');
                
                var hasInitIncoding = $.grep(wasAddBinds, function(r) {
                    return r.contains(IncSpecialBinds.InitIncoding);
                }).length != 0;

                if (hasInitIncoding) {
                    incodingMetaElement.bind(IncSpecialBinds.InitIncoding, '4');
                    defferedInit.push(this);
                }
                
            });

        $(defferedInit).each(function() {
            new IncodingMetaElement(this).invoke(jQuery.Event(IncSpecialBinds.InitIncoding));
        });

    };

}

IncodingEngine.Current = new IncodingEngine();

var initializedHistoryPlugin = false;
$(document).ready(function() {

    if ($.history) {
        $.history.init(function() {
            if (initializedHistoryPlugin) {
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncChangeUrl));
            }

            initializedHistoryPlugin = true;
        });
    }

    IncodingEngine.Current.parse(document);
});

//#endregion