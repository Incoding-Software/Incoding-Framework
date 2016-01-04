"use strict";

//#region class ExecutableFactory

function ExecutableFactory() {    
}

// ReSharper disable UnusedParameter

ExecutableFactory.Create = function(type, data, self) {

    if (!document[type]) {
        document[type] = eval('new ' + type + '();');
    }
    var executable = $.extend({}, document[type]);
    executable.jsonData = data;
    executable.onBind = data.onBind;
    executable.self = self;
    executable.timeOut = data.timeOut;
    executable.interval = data.interval;
    executable.intervalId = data.intervalId;
    executable.ands = data.ands;
    executable.getTarget = function() {
        return eval(data.target);
    };

    return executable;
};

// ReSharper restore UnusedParameter

//#endregion

//#region class ExecutableBase

function ExecutableBase() {
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
    this.getTarget = function() {
        return this.self;
    };

}

ExecutableBase.prototype = {
    // ReSharper disable UnusedParameter
    execute : function(state) {

        var current = this;
        current.target = current.getTarget();

        if (!current.isValid()) {
            return;
        }

        if (current.timeOut > 0) {
            window.setTimeout(function() {
                current.target = current.getTarget();
                current.internalExecute(state);
            }, current.timeOut);
            return;
        }

        if (current.interval > 0) {
            ExecutableBase.IntervalIds[current.intervalId] = window.setInterval(function() {
                current.target = current.getTarget();
                current.internalExecute(state);
            }, current.interval);
            return;
        }

        current.internalExecute(state);
    },
    internalExecute : function(state) {
    },

    isValid : function() {

        var current = this;

        if (ExecutableHelper.IsNullOrEmpty(current.ands)) {
            return true;
        }

        var res = false;

        $(current.ands).each(function() {

            var hasAny = false;

            $(this).each(function() {

                hasAny = ConditionalFactory.Create(this, current).isSatisfied(current.result);
                if (!hasAny) {
                    return false;
                }
            });

            if (hasAny) {
                res = true;
                return false;
            }
        });

        return res;
    },

    tryGetVal : function(variable) {
        ExecutableHelper.Instance.self = this.self;
        ExecutableHelper.Instance.target = this.target;
        ExecutableHelper.Instance.event = this.event;
        ExecutableHelper.Instance.result = this.result;
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
        var executeState = function() {
            try {
                this.result = resultData;
                this.execute();
            }
            catch (e) {
                if (e instanceof IncClientException) {
                    hasBreak = true;
                    return false; //stop execute
                }

                console.log('Incoding exception: {0}'.f(e.message ? e.message : e));
                if (navigator.Ie8) {
                    return false; //stop execute
                }
                throw e;
            }
        };

        $(result.success ? state.success : state.error).each(executeState);
        $(state.complete).each(executeState);
        if (hasBreak) {
            $(state.breakes).each(executeState);
        }
    }
});

//#endregion

//#region class ExecutableDirectAction extend from ExecutableBase

incodingExtend(ExecutableDirectAction, ExecutableActionBase);

function ExecutableDirectAction() {
}

ExecutableDirectAction.prototype.internalExecute = function(state) {
    var result = ExecutableHelper.IsNullOrEmpty(this.jsonData.result) ? IncodingResult.Empty : new IncodingResult(this.jsonData.result);
    this.complete(result, state);
};

//#endregion

//#region class ExecutableEventAction extend from ExecutableBase

incodingExtend(ExecutableEventAction, ExecutableActionBase);

function ExecutableEventAction() {
}

ExecutableEventAction.prototype.internalExecute = function(state) {
    this.complete(state.eventResult, state);
};

//#endregion

//#region class ExecutableAjaxAction extend from ExecutableBase

incodingExtend(ExecutableAjaxAction, ExecutableActionBase);

function ExecutableAjaxAction() {
}

ExecutableAjaxAction.prototype.internalExecute = function(state) {

    var current = this;

    var ajaxOptions = $.extend(true, { data : [] }, current.jsonData.ajax);
    var isEmptyUrl = ExecutableHelper.IsNullOrEmpty(ajaxOptions.url);
    var url = ajaxOptions.url;
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
            ajaxOptions.data.push({ name : name, selector : fragmentParams[this] });
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

    var url = this.jsonData.options.url;
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

    eval("$(current.target).{0}(insertContent.toString())".f(current.jsonData.insertType));

    var target = current.target;
    if (current.jsonData.insertType.toLowerCase() === 'after') {
        target = $(current.target).nextAll();
    }
    if (current.jsonData.insertType.toLowerCase() === 'before') {
        target = $(current.target).prevAll();
    }
    IncodingEngine.Current.parse(target);
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

ExecutableTrigger.prototype.internalExecute = function() {

    var eventData = ExecutableHelper.IsNullOrEmpty(this.jsonData.property)
        ? this.result
        : this.result.hasOwnProperty(this.jsonData.property) ? this.result[this.jsonData.property] : '';
    $(this.target).trigger(this.jsonData.trigger, new IncodingResult({ success : true, data : eventData, redirectTo : '' }));

};

//#endregion

//#region class ExecutableValidationParse extend from ExecutableBase

incodingExtend(ExecutableValidationParse, ExecutableBase);

function ExecutableValidationParse() {
}

ExecutableValidationParse.prototype.internalExecute = function() {

    var form = $(this.target).is('form') ? this.target : $(this.target).closest('form').first();
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

ExecutableValidationRefresh.prototype.internalExecute = function () {

    var current = this;
    var inputErrorClass = 'input-validation-error';
    var messageErrorClass = 'field-validation-error';
    var messageValidClass = 'field-validation-valid';
    var attrSpan = 'data-valmsg-for';
    var result = ExecutableHelper.IsNullOrEmpty(current.result) ? [] : current.result;
    $(result).each(function () {

        var name = this.name.toString();
        var input = $('[name]', current.target).filter(function () {
            return $(this).attr('name').toLowerCase() == name.toLowerCase();
        });
        var span = $('[{0}]'.f(attrSpan), current.target).filter(function () {
            return $(this).attr(attrSpan).toLowerCase() == name.toLowerCase();
        });

        if (ExecutableHelper.ToBool(this.isValid)) {
            $(input).removeClass(inputErrorClass);
            $(span).removeClass(messageErrorClass)
                .addClass(messageValidClass)
                .empty();
        }
        else {
            $(input).addClass(inputErrorClass);
            $(span)
                .removeClass(messageValidClass)
                .addClass(messageErrorClass)
                .html($('<span/>')
                    .attr({ for: name, generated: true })
                    .html(this.errorMessage));
        }

    });

    if ($(result).length === 0) {
        $(current.target).find('.' + inputErrorClass).removeClass(inputErrorClass);
        $('[{0}]'.f(attrSpan), current.target).removeClass(messageErrorClass)
            .addClass(messageValidClass)
            .empty();
        $(current.target).find('.' + messageErrorClass).addClass(messageValidClass).removeClass(messageErrorClass).empty();
    }

    $($(current.target).is('form') ? current.target : $('form', this.target))
        .validate()
        .focusInvalid();
};

//#endregion


//#region class ExecutableEval extend from ExecutableBase

incodingExtend(ExecutableEval, ExecutableBase);

function ExecutableEval() {
}

ExecutableEval.prototype.internalExecute = function() {
    eval(this.jsonData.code);
};

//#endregion

//#region class ExecutableEvalMethod extend from ExecutableBase

incodingExtend(ExecutableEvalMethod, ExecutableBase);

function ExecutableEvalMethod() {
}

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

ExecutableBreak.prototype.internalExecute = function() {
    throw new IncClientException();
};

//#endregion

//#region class ExecutableStoreInsert extend from ExecutableBase

incodingExtend(ExecutableStoreInsert, ExecutableBase);

function ExecutableStoreInsert() {
}

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

ExecutableStoreFetch.prototype.internalExecute = function() {

    var prefix = this.jsonData.prefix + "__";
    var fparam = $.url(window.location.href).fparam();

    $.eachFormElements(this.target, function() {
        var name = $(this).prop('name');
        var key = prefix + name;
        var value = '';
        if (fparam.hasOwnProperty(key)) {
            value = fparam[key];
        }
        ExecutableHelper.Instance.TrySetValue(this, value);
    });

};

//#endregion

//#region class ExecutableStoreManipulate extend from ExecutableBase

incodingExtend(ExecutableStoreManipulate, ExecutableBase);

function ExecutableStoreManipulate() {
}

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

ExecutableForm.prototype.internalExecute = function() {
    var form = $(this.target).is('form') ? this.target : $(this.target).closest('form').first();

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

ExecutableBind.prototype.internalExecute = function() {
    var type = this.jsonData.type;
    switch (type) {
        case 'attach':
            $(this.target).removeData('incoding-runner');
            $(this.target).attr('incoding', this.jsonData.meta);
            IncodingEngine.Current.parse(this.target);
            break;
        case 'detach':
            if (ExecutableHelper.IsNullOrEmpty(this.jsonData.bind)) {
                $(this.target).unbind();
            }
            else {
                $(this.target).unbind(this.jsonData.bind);
            }
            break;
    }
};

//#endregion

//#endregion