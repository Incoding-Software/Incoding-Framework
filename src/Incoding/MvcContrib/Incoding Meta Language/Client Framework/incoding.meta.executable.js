"use strict";

//#region class ExecutableFactory

function ExecutableFactory() {    
}

// ReSharper disable UnusedParameter

ExecutableFactory.Create = function (type, data, self) {

    if (!document[type]) {
        document[type] = eval('new ' + type + '();');
    }
    return $.extend(false, document[type], {
        jsonData: data,
        onBind: data.onBind,
        self: $(self),
        timeOut: data.timeOut,
        interval: data.interval,
        intervalId: data.intervalId,
        ands: data.ands,
        target: data.target,
        getTarget: function () {
            if (ExecutableHelper.IsNullOrEmpty(data.target)) {
                return '';
            }
            if (data.target === "$(this.self)") {
                return this.self;
            }

            if (data.target.startsWith("||") && data.target.endWith("||")) {
                var selector = data.target.substring(2, data.target.length - 2).substring(data.target.indexOf('*') - 1, data.target.length);
                return $(selector);
            }
            else {
                return eval(data.target);
            }
        }
    });

};

// ReSharper restore UnusedParameter

//#endregion

//#region class ExecutableBase

function ExecutableBase() {
    this.name = '';
    this.jsonData = '';
    this.onBind = '';
    this.self = '';
    this.event = '';
    this.timeOut = 0;
    this.interval = 0;
    this.intervalId = '';
    this.target = '';
    this.ands = null;
    this.result = '';
    this.resultOfEvent = '';   
}

ExecutableBase.prototype = {
    // ReSharper disable UnusedParameter
    execute: function (state) {

        var current = this;
        this.target = this.getTarget();
        
        if (!this.isValid()) {
            return;
        }

        var delayExecute = function () {     
            current.target = current.getTarget();
            current.internalExecute(state);        
        };
        if (this.timeOut > 0) {
            window.setTimeout(delayExecute, current.timeOut);            
            return;
        }
        if (this.interval > 0) {
            ExecutableBase.IntervalIds[current.intervalId] = window.setInterval(delayExecute, current.interval);            
            return;
        }

        this.internalExecute(state);
    },
    internalExecute : function(state) {
    },

    isValid : function() {

        var current = this;

        if (ExecutableHelper.IsNullOrEmpty(current.ands)) {
            return true;
        }

        var res = false;

        for (var i = 0; i < current.ands.length; i++) {
            var hasAny = false;

            for (var j = 0; j < current.ands[i].length; j++) {
                hasAny = ConditionalFactory.Create(current.ands[i][j], current).isSatisfied(current.result);
                if (!hasAny) {
                    break;
                }
            }

            if (hasAny) {
                res = true;
                break;
            }

        }

        return res;
    },

    tryGetVal : function(variable) {
        ExecutableHelper.Instance.self = this.self;
        ExecutableHelper.Instance.target = this.target;
        ExecutableHelper.Instance.event = this.event;
        ExecutableHelper.Instance.result = this.result;
        ExecutableHelper.Instance.resultOfEvent = this.resultOfEvent;
        return ExecutableHelper.Instance.TryGetVal(variable);
    }
};

ExecutableBase.IntervalIds = {};

//#endregion

//#region Actions

//#region class ExecutableActionBase extend from ExecutableBase

incodingExtend(ExecutableActionBase, ExecutableBase);

function ExecutableActionBase() {
}

$.extend(ExecutableActionBase.prototype, {
    complete : function(result, state) {
        
        if (!ExecutableHelper.IsNullOrEmpty(result.redirectTo)) {
            ExecutableHelper.RedirectTo(result.redirectTo);
            return;
        }

        var resultData = result.data;

        if (!ExecutableHelper.IsNullOrEmpty(this.jsonData.filterResult)) {

            var filter = ConditionalFactory.Create(this.jsonData.filterResult, this);
            resultData = ExecutableHelper.Filter(result.data, filter);
        }

        var hasBreak = false;
        var executeState = function(executable) {
            try {
                executable.result = resultData;
                executable.execute();
            }
            catch (e) {
                if (e instanceof IncClientException) {
                    hasBreak = true;
                    return false; //stop execute
                }

                console.log('Incoding exception: {0}'.f(e.message ? e.message : e));
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncGlobalError));
                $(executable.self).trigger(jQuery.Event(IncSpecialBinds.IncError));
                if (navigator.Ie8) {
                    return false; //stop execute
                }
                throw e;
            }
        };
        var mainStates = result.success ? state.success : state.error;
        for (var i = 0; i < mainStates.length; i++) {
            executeState(mainStates[i]);
        }

        for (var j = 0; j < state.complete.length; j++) {
            executeState(state.complete[j]);
        }
        if (hasBreak) {
            for (var k = 0; k < state.breakes.length; k++) {
                executeState(state.breakes[k]);
            }
        }
    }
});

//#endregion

//#region class ExecutableDirectAction extend from ExecutableBase

incodingExtend(ExecutableDirectAction, ExecutableActionBase);

function ExecutableDirectAction() {
}

ExecutableDirectAction.prototype.name = "Direct";
ExecutableDirectAction.prototype.internalExecute = function(state) {
    var result = ExecutableHelper.IsNullOrEmpty(this.jsonData.result) ? IncodingResult.Empty : new IncodingResult(this.jsonData.result);
    this.complete(result, state);
};

//#endregion

//#region class ExecutableEventAction extend from ExecutableBase

incodingExtend(ExecutableEventAction, ExecutableActionBase);

function ExecutableEventAction() {
}

ExecutableEventAction.prototype.name = "Event";
ExecutableEventAction.prototype.internalExecute = function (state) {    
    this.complete(IncodingResult.Success(this.resultOfEvent), state);
};

//#endregion

//#region class ExecutableAjaxAction extend from ExecutableBase

incodingExtend(ExecutableAjaxAction, ExecutableActionBase);

function ExecutableAjaxAction() {
}

ExecutableAjaxAction.prototype.name = "Ajax";
ExecutableAjaxAction.prototype.internalExecute = function(state) {

    var current = this;

    var ajaxOptions = $.extend(true, { data : [] }, current.jsonData.ajax);
    var url = ajaxOptions.url;
    var isEmptyUrl = ExecutableHelper.IsNullOrEmpty(url);
    if (!isEmptyUrl) {
        var queryFromString = $.url(url).param();
        $.eachProperties(queryFromString, function() {
            ajaxOptions.data.push({ name : this, selector : current.tryGetVal(queryFromString[this]) });
        });
        ajaxOptions.url = url.split('?')[0];
    }
    if (current.jsonData.hash) {
        var href = $.url(document.location.href);
        if (isEmptyUrl || ExecutableHelper.IsNullOrEmpty(url.split('?')[0])) {
            ajaxOptions.url = href.furl(current.jsonData.prefix);
        }

        var fragmentParams = href.fparam();
        $.eachProperties(fragmentParams, function() {
            var name = this.replace(current.jsonData.prefix + '__', '');
            if (!ExecutableHelper.IsNullOrEmpty(name)) {
                ajaxOptions.data.push({ name : name, selector : fragmentParams[this] });
            }
        });

    }

    AjaxAdapter.Instance.request(ajaxOptions, function(result) {
        current.complete(result, state);
    });

};

//#endregion

//#region class ExecutableSubmitAction extend from ExecutableBase

incodingExtend(ExecutableSubmitAction, ExecutableActionBase);

function ExecutableSubmitAction() {
}

ExecutableSubmitAction.prototype.name = "Submit";
ExecutableSubmitAction.prototype.internalExecute = function(state) {

    var current = this;
    var formSelector = eval(this.jsonData.formSelector);
    var form = $(formSelector).is('form') ? formSelector : $(formSelector).closest('form').first();

    var ajaxOptions = $.extend(true, {
        data : [],
        error : function(error) {
            $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxError), IncodingResult.Success(IncAjaxEvent.Create(error)));
            $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxComplete), IncodingResult.Success(IncAjaxEvent.Create(error)));
        },
        success : function(responseText, statusText, xhr, $form) {
            $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxSuccess), IncodingResult.Success(IncAjaxEvent.Create(xhr)));
            $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxComplete), IncodingResult.Success(IncAjaxEvent.Create(xhr)));
            current.complete(new IncodingResult(responseText), state);
        },
        beforeSubmit : function(formData, jqForm, options) {
            var isValid = $(form).valid();
            if (!isValid) {
                $(form).validate().focusInvalid();
            }
            else {
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxBefore), IncodingResult.Success({}));
            }
            return isValid;
        }
    }, this.jsonData.options);

    var url = this.jsonData.options.url || form.attr('action');
    if (!ExecutableHelper.IsNullOrEmpty(url)) {
        var queryFromString = $.url(url).param();
        $.eachProperties(queryFromString, function() {
            ajaxOptions.data.push({ name : this, value : current.tryGetVal(queryFromString[this]) });
        });
        ajaxOptions.url = url.split('?')[0];
        if (ExecutableHelper.IsNullOrEmpty(ajaxOptions.url)) {
            delete ajaxOptions.url;
        }
    }

    $(form).ajaxSubmit(ajaxOptions);
};

//#endregion

//#region Core

//#region class ExecutableInsert extend from ExecutableBase

incodingExtend(ExecutableInsert, ExecutableBase);

function ExecutableInsert() {
}

// ReSharper disable UnusedLocals
// ReSharper disable AssignedValueIsNeverUsed
ExecutableInsert.prototype.name = "Insert";
ExecutableInsert.prototype.internalExecute = function() {

    var current = this;

    var insertContent = ExecutableHelper.IsNullOrEmpty(current.jsonData.result)
        ? ExecutableHelper.IsNullOrEmpty(this.result) ? '' : this.result
        : this.tryGetVal(current.jsonData.result);

    if (!ExecutableHelper.IsNullOrEmpty(current.jsonData.property)) {
        var insertObject = this.result;
        if (_.isArray(this.result)) {
            insertObject = this.result.length > 0 ? this.result[0] : {};
        }
        insertContent = insertObject.hasOwnProperty(current.jsonData.property) ? insertObject[current.jsonData.property] : '';
    }

    if (ExecutableHelper.ToBool(current.jsonData.prepare)) {
        $(_.isArray(insertContent) ? insertContent : [insertContent]).each(function() {
            var item = this;
            $.eachProperties(this, function() {
                item[this] = current.tryGetVal(item[this]);
            });
        });
    }

    if (!ExecutableHelper.IsNullOrEmpty(current.jsonData.template)) {
        var templateId = current.jsonData.template;
        if (templateId.startsWith("||ajax*")) {

            var json = templateId.substring(2, templateId.length - 2)
                .substring(templateId.indexOf('*') - 1, templateId.length);
            templateId = current.tryGetVal('||buildurl*{0}||'.f($.parseJSON(json).url));
        }
        insertContent = TemplateFactory.ToHtml(ExecutableInsert.Template, templateId, function() {
            return current.tryGetVal(current.jsonData.template);
        }, insertContent);
    }

    if (ExecutableHelper.IsNullOrEmpty(insertContent)) {
        insertContent = '';
    }

    // temporary fix for IE9 (random rows in table having td offset)
    // https://issues.jboss.org/browse/JBPM-4396
    if (jQuery.browser.msie && jQuery.browser.version === '9.0') {
        if (typeof insertContent === 'string' || insertContent instanceof String) {
            insertContent = insertContent.replace(/>\s+(?=<\/?(t|c)[hardfob])/gm, '>');
        }
    }

    switch (current.jsonData.insertType) {
        case 'html':
            current.target.html(insertContent.toString());
            break;
        default:
    eval("$(current.target).{0}(insertContent.toString())".f(current.jsonData.insertType));
    }
    var target = current.target;
    if (current.jsonData.insertType.toLowerCase() === 'after') {
        IncodingEngine.Current.parse(current.target.nextAll());
    }
    else if (current.jsonData.insertType.toLowerCase() === 'before') {
        IncodingEngine.Current.parse(current.target.prevAll());
    }
    else {
        IncodingEngine.Current.parse(current.target);
    }
    $(document).trigger(jQuery.Event(IncSpecialBinds.IncInsert));

};

ExecutableInsert.Template = new IncHandlerbarsTemplate();
// ReSharper restore AssignedValueIsNeverUsed
// ReSharper restore UnusedLocals

//#endregion

//#region class ExecutableTrigger extend from ExecutableBase

incodingExtend(ExecutableTrigger, ExecutableBase);

function ExecutableTrigger() {
}

ExecutableTrigger.prototype.name = 'Trigger';
ExecutableTrigger.prototype.internalExecute = function() {

    var eventData = ExecutableHelper.IsNullOrEmpty(this.jsonData.property)
        ? this.result
        : this.result.hasOwnProperty(this.jsonData.property) ? this.result[this.jsonData.property] : '';

    this.target.trigger(this.jsonData.trigger, [eventData]);
};

//#endregion

//#region class ExecutableValidationParse extend from ExecutableBase

incodingExtend(ExecutableValidationParse, ExecutableBase);

function ExecutableValidationParse() {
}

ExecutableValidationParse.prototype.name = "Validation parse";
ExecutableValidationParse.prototype.internalExecute = function() {

    var form = this.target.is('form') ? this.target : this.target.closest('form').first();
    $(form).removeData('validator').removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);

    //bug in fluent validation. fixed for input
    $('[data-val-equalto-other]', form).each(function() {
        var equalTo = '#' + $(this).data('val-equalto-other').replaceAll('*.', 'Input_');
        if ($(equalTo).length > 0) {
            $(this).rules("add", { required : true, equalTo : equalTo });
        }
    });
};

//#endregion

//#region class ExecutableValidationRefresh extend from ExecutableBase

incodingExtend(ExecutableValidationRefresh, ExecutableBase);

function ExecutableValidationRefresh() {
}

ExecutableValidationRefresh.prototype.name = "Validation Refresh";
ExecutableValidationRefresh.prototype.internalExecute = function() {

    var inputErrorClass = 'input-validation-error';
    var messageErrorClass = 'field-validation-error';
    var messageValidClass = 'field-validation-valid';
    var attrSpan = 'data-valmsg-for';
    var result = ExecutableHelper.IsNullOrEmpty(this.result) ? [] : this.result;
    var isWasRefresh = false;
    for (var i = 0; i < result.length; i++) {
        var item = result[i];
        if (!item.hasOwnProperty('name') || !item.hasOwnProperty('isValid') || !item.hasOwnProperty('errorMessage')) {
            break;
        }
        if (!ExecutableHelper.IsNullOrEmpty(this.jsonData.prefix)) {
            item.name = "{0}.{1}".f(this.jsonData.prefix, item.name);
        }

        var input = $('[name]', this.target).filter(function() {
            return $(this).attr('name').toLowerCase() == item.name.toString().toLowerCase();
        });
        var span = $('[{0}]'.f(attrSpan), this.target).filter(function() {
            return $(this).attr(attrSpan).toLowerCase() == item.name.toString().toLowerCase();
        });

        if (ExecutableHelper.ToBool(item.isValid)) {
            $(input).removeClass(inputErrorClass);
            $(span).removeClass(messageErrorClass)
                .addClass(messageValidClass)
                .empty();
        }
        else {
            $(input).addClass(inputErrorClass);
            $(span).removeClass(messageValidClass)
                .addClass(messageErrorClass)
                .html($('<span/>')
                    .attr({ for : item.name, generated : true })
                    .html(item.errorMessage));
        }
        isWasRefresh = true;
    }

    if (!isWasRefresh) {
        this.target.find('.' + inputErrorClass).removeClass(inputErrorClass);
        $('[{0}]'.f(attrSpan), this.target).removeClass(messageErrorClass)
            .addClass(messageValidClass)
            .empty();
        this.target.find('.' + messageErrorClass).addClass(messageValidClass).removeClass(messageErrorClass).empty();
    }

    (this.target.is('form') ? this.target : $('form', this.target))
        .validate()
        .focusInvalid();
};

//#endregion

//#region class ExecutableEval extend from ExecutableBase

incodingExtend(ExecutableEval, ExecutableBase);

function ExecutableEval() {
}

ExecutableEval.prototype.name = "Eval";
ExecutableEval.prototype.internalExecute = function() {
    eval(this.jsonData.code);
};

//#endregion

//#region class ExecutableEvalMethod extend from ExecutableBase

incodingExtend(ExecutableEvalMethod, ExecutableBase);

function ExecutableEvalMethod() {
}

ExecutableEvalMethod.prototype.name = "Eval Method";
ExecutableEvalMethod.prototype.internalExecute = function() {

    var length = this.jsonData.args.length;

    var args = '';
    for (var i = 0; i < length; i++) {
        args += "this.tryGetVal(this.jsonData.args[{0}])".f(i);
        if (i != length - 1) {
            args += ',';
        }
    }

    var contextStr = ExecutableHelper.IsNullOrEmpty(this.jsonData.context) ? '' : this.jsonData.context + '.';
    eval("{0}{1}({2})".f(contextStr, this.jsonData.method, args));
};

//#endregion

//#region class ExecutableBreak extend from ExecutableBase

incodingExtend(ExecutableBreak, ExecutableBase);

function ExecutableBreak() {
}

ExecutableBreak.prototype.name = "Break";
ExecutableBreak.prototype.internalExecute = function() {
    throw new IncClientException();
};

//#endregion

//#region class ExecutableStoreInsert extend from ExecutableBase

incodingExtend(ExecutableStoreInsert, ExecutableBase);

function ExecutableStoreInsert() {
}

ExecutableStoreInsert.prototype.name = "Store Insert";
ExecutableStoreInsert.prototype.internalExecute = function() {

    var url = $.url(document.location.href);
    var prefix = this.jsonData.prefix;

    if (ExecutableHelper.ToBool(this.jsonData.replace)) {
        url.clearFparam();
    }

    var target = this.target;
    $.eachFormElements(target, function() {
        var name = $(this).prop('name');
        var value = ExecutableHelper.Instance.TryGetVal(this);

        if (ExecutableHelper.IsNullOrEmpty(value)) {
            url.removeFparam(name, prefix);
        }
        else {
            url.setFparam(name, value, prefix);
        }
    });

    ExecutableHelper.RedirectTo(url.toHref());
};

//#endregion

//#region class ExecutableStoreFetch extend from ExecutableBase

incodingExtend(ExecutableStoreFetch, ExecutableBase);

function ExecutableStoreFetch() {
}

ExecutableStoreFetch.prototype.name = "Store fetch";
ExecutableStoreFetch.prototype.internalExecute = function() {

    var prefix = '';
    var isHash = this.jsonData.type == 'hash';

    if (isHash) {
        prefix = this.jsonData.prefix + "__";
    }
    var params = [];
    if (isHash) {
        params = $.url(window.location.href).fparam();
    }
    else if (this.jsonData.type = 'queryString') {
        params = $.url(window.location.href).param();
    }

    $.eachFormElements(this.target, function() {
        var name = $(this).prop('name');
        var key = prefix + name;
        var value = '';
        if (params.hasOwnProperty(key)) {
            value = params[key];
        }
        ExecutableHelper.Instance.TrySetValue(this, value);
    });

};

//#endregion

//#region class ExecutableStoreManipulate extend from ExecutableBase

incodingExtend(ExecutableStoreManipulate, ExecutableBase);

function ExecutableStoreManipulate() {
}

ExecutableStoreManipulate.prototype.name = "Store Manipulate";
ExecutableStoreManipulate.prototype.internalExecute = function() {
    var current = this;
    switch (current.jsonData.type) {
        case 'hash':
            var url = $.url(document.location.href);
            url.encodeAllParams();

            var methods = $.parseJSON(current.jsonData.methods);
            $(methods).each(function() {
                switch (this.verb) {
                    case 'remove':
                        url.removeFparam(this.key, this.prefix);
                        break;
                    case 'set':
                        url.setFparam(this.key, current.tryGetVal(this.value), this.prefix);
                        break;
                }
            });

            return ExecutableHelper.RedirectTo(url.toHref());
        default:
            throw 'Argument out of range {0}'.f(this.jsonData.type);
    }
};

//#endregion

//#region class ExecutableForm extend from ExecutableBase

incodingExtend(ExecutableForm, ExecutableBase);

function ExecutableForm() {
}

ExecutableForm.prototype.name = "Form";
ExecutableForm.prototype.internalExecute = function() {
    var form = this.target.is('form') ? this.target : this.target.closest('form').first();

    var method = this.jsonData.method;
    switch (method) {
        case 'reset':
            $(form).resetForm();
            break;
        case 'clear':
            $(form).clearForm();
            break;
    }
};

//#endregion

//#region class ExecutableBind extend from ExecutableBase

incodingExtend(ExecutableBind, ExecutableBase);

function ExecutableBind() {
}

ExecutableBind.prototype.name = "Bind";
ExecutableBind.prototype.internalExecute = function() {
    var type = this.jsonData.type;
    switch (type) {
        case 'attach':
            this.target.removeData('incoding-runner')
                .attr('incoding', this.jsonData.meta);
            IncodingEngine.Current.parse(this.target);
            break;
        case 'detach':
            if (ExecutableHelper.IsNullOrEmpty(this.jsonData.bind)) {
                this.target.unbind();
            }
            else {
                this.target.unbind(this.jsonData.bind);
            }
            break;
    }
};

//#endregion

//#region class ExcutableJquery extend from ExecutableBase

incodingExtend(ExecutableJquery, ExecutableBase);

function ExecutableJquery() {
}

ExecutableJquery.prototype.name = "Jquery";
ExecutableJquery.prototype.internalExecute = function() {
    switch (this.jsonData.method) {
        case 1:
            this.target.addClass(ExecutableHelper.Instance.TryGetVal(this.jsonData.args[0]));
            break;
        case 2:
            this.target.removeClass(ExecutableHelper.Instance.TryGetVal(this.jsonData.args[0]));
            break;
        default:
            throw 'Not found method {0}'.f(this.jsonData.method);
    }
};

//#endregion