"use strict";

//#region class IncodingInstrumentation

function DefaultIncodingInstrumentation() {

}

DefaultIncodingInstrumentation.prototype =
    {
        fireTriggerIncoding: function (context) {
            console.log('Invoke event incoding for element {0}'.f(context.element));
        },
        fireParse: function (context) {
            console.log('Found incoding element {0} with count meta {1}'.f(context.element, context.countMeta));
        },

        fireBind: function (context) {
            console.log('Invoke incoding element {0} by {1}'.f(context.element, context.event));
        },

        fireInitIncoding: function (context) {
            console.log('Init incoding element {0}'.f(context.element));
        }
    };

//#endregion

//#region class IncodingMetaElement

function IncodingMetaElement(element) {

    var keyIncodingRunner = 'incoding-runner';

    var incodingBind = 'incoding';

    var tryGetDataOrAttr = function (item, selector) {

        var tempData = $(item).attr(selector);
        if (!_.isUndefined(tempData) && !_.isNull(tempData)) {
            return tempData;
        }

        tempData = $.data(item, selector);
        if (!_.isUndefined(tempData) && !_.isNull(tempData)) {
            return tempData;
        }

        return $(item).data(selector);

    };

    this.Count = tryGetDataOrAttr(element, 'incoding-count');
    this.Element = element;
    this.IncodingRunner = tryGetDataOrAttr(element, keyIncodingRunner);

    this.bindIncoding = function () {
        $(this.Element).bind(incodingBind, function () {
            new IncodingMetaElement(this)
                .IncodingRunner.DoIt();
        });
    };

    this.invokeIncoding = function () {
        $(this.Element).trigger(incodingBind);
    };

    this.flushRunner = function (runner) {
        $.data(this.Element, keyIncodingRunner, runner);
    };

    this.getIncodingByIndex = function (index) {

        var fullDataSelector = 'incoding-' + index;
        var tempData = tryGetDataOrAttr(this.Element, fullDataSelector);
        return _.isObject(tempData) ? tempData : $.parseJSON(tempData);
    };

    this.IsNew = function () {

        var hasIncoding = !_.isUndefined(this.Count) && !_.isNull(this.Count);
        var hasRunner = !_.isUndefined(this.IncodingRunner) && !_.isNull(this.IncodingRunner);

        return hasIncoding && !hasRunner;
    };

}

//#endregion

//#region class IncodingEngine

function IncodingEngine(options) {

    this.executableFactory = (options) ? options.executableFactory : new ExecutableFactory();

    this.instrumentation = (options) ? options.instrumentation : new DefaultIncodingInstrumentation();

    this.parse = function (context) {

        var current = this;

        var allIncodingElements = $(context).add($('*', context)).filter(function () {
            return new IncodingMetaElement(this).IsNew();
        });
        $(allIncodingElements).each(function () {

            var incodingMetaElement = new IncodingMetaElement(this);
            current.instrumentation.fireParse({ element: this, countMeta: incodingMetaElement.Count });

            incodingMetaElement.bindIncoding();

            var findElement = this;

            var runner = new IncodingRunner();
            var bindName = '';

            for (var i = 1; i <= incodingMetaElement.Count; i++) {

                var metaData = incodingMetaElement.getIncodingByIndex(i);

                var jsonData = _.isObject(metaData.data)
                    ? metaData.data
                    : $.parseJSON(metaData.data);

                var executableInstance = current.executableFactory.create(metaData.type, jsonData, findElement);

                runner.Registry(metaData.type, jsonData.onStatus, executableInstance);

                if (!_.isEmpty(metaData.data.bind) && !_.isUndefined(metaData.data.bind)) {
                    bindName = metaData.data.bind;
                }
            }
            incodingMetaElement.flushRunner(runner);

            if (!_.isEmpty(bindName) && !_.isUndefined(bindName)) {

                $(findElement).bind(bindName.trim(), function (e) {
                    current.instrumentation.fireBind({ element: this, event: e });
                    new IncodingMetaElement(this).invokeIncoding();

                    e.preventDefault();
                    return false;
                });

                if (bindName.contains('initincoding')) {
                    current.instrumentation.fireInitIncoding({ element: findElement });
                    incodingMetaElement.invokeIncoding();
                }
            }

        });

    };

    this.domInspect = function () {
        var current = this;
        if (jQuery.browser.msie && jQuery.browser.version < 9) {

            var watch;
            watch = function (event) {
                if (event.originalEvent.propertyName != "innerHTML") {
                    return;
                }

                current.parse($(this));
                var descendants = this.getElementsByTagName("*");
                current.parse(descendants);
                for (var i = 0; i < descendants.length; ++i) {
                    $(descendants[i]).bind("propertychange", function (e) {
                        watch.call(this, e);
                        e.preventDefault();
                    });
                }

            };

            //watch everything
            var allElements = document.getElementsByTagName("*");
            for (var indexElement = 0; indexElement < allElements.length; ++indexElement) {
                $(allElements[indexElement]).bind("propertychange", function (e) {
                    watch.call(this, e);
                    e.preventDefault();
                });
            }

        } else {
            $(document).bind("DOMNodeInserted", function (e) {
                current.parse(e.target);
            });
        }
    };

    this.destroy = function () {
        $(document).unbind("DOMNodeInserted");
        $(document).unbind("propertychange");
        $(document).each(function () {
            $(this).unbind("propertychange");
        });
    };

    IncodingEngine.Current = this;
}

IncodingEngine.Current;

//#endregion

//#region class Runner

function IncodingRunner() {
    this.action = null;
    this.before = [];
    this.success = [];
    this.error = [];
    this.complete = [];
}

IncodingRunner.prototype = {
    action: null,
    before: [],
    success: [],
    error: [],
    complete: [],

    DoIt: function () {

        var isBefore = true;

        $(this.before).each(function () {
            isBefore = this.execute();
        });

        if (!isBefore) {
            return;
        }

        this.action.execute(
            {
                success: this.success,
                error: this.error,
                complete: this.complete
            });
    },

    Registry: function (metaType, onStatus, instance) {

        if (metaType.contains('Action')) {
            this.action = instance;
        } else if (metaType.contains('Callback')) {

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
                default:
            }

        }
    }
};

//#endregion

//#region class ParseServerResult

function ParseServerResult(result) {

    var parse = function (json) {
        try {
            var res = _.isObject(json) ? json : $.parseJSON(json);
            var isSchemaValid = _.has(res, 'success') && _.has(res, 'redirectTo') && _.has(res, 'data');
            if (!isSchemaValid) {
                throw new 'Not valid json result';
            }
            return res;
        } catch (e) {
            console.log('fail parse result:{0}'.f(json));
            console.log('with exception:{0}'.f(e));
            return '';
        }
    };

    this.parseJson = parse(result);

    this.isValid = function () {
        return !_.isEmpty(this.parseJson);
    };

    this.redirectTo = this.isValid() ? this.parseJson.redirectTo : '';
    this.success = this.isValid() ? this.parseJson.success : false;
    this.data = this.isValid() ? this.parseJson.data : '';

    this.isRedirectTo = function () {
        return !_.isEmpty(this.redirectTo);
    };

    this.isSuccess = function () {
        return this.success;
    };
}

ParseServerResult.Empty = new ParseServerResult({ data: '', redirectTo: '', success: true });


//#endregion