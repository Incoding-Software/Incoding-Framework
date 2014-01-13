"use strict";

//#region class ExecutableFactory

function ExecutableFactory() {

}

// ReSharper disable UnusedParameter
ExecutableFactory.Create = function(type, data, self) {

    var json = _.isObject(data) ? data : $.parseJSON(data);

    var executable = eval('new ' + type + '();');
    executable.jsonData = json;
    executable.onBind = json.onBind;
    executable.self = self;
    executable.timeOut = json.timeOut;
    executable.interval = json.interval;
    executable.intervalId = json.intervalId;
    executable.ands = json.ands;
    executable.getTarget = function() {
        return eval(json.target);
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
    this.timeOut = 0;
    this.interval = 0;
    this.intervalId = '';
    this.target = '';
    this.ands = null;    
    this.getTarget = function() {
        return this.self;
    };

}

ExecutableBase.prototype = {
    // ReSharper disable UnusedParameter
    execute : function(data) {

        var current = this;
        current.target = current.getTarget();
        
        if (!current.isValid(data)) {
            return;
        }

        if (current.timeOut > 0) {
            window.setTimeout(function() {
                current.target = current.getTarget();
                current.internalExecute(data);
            }, current.timeOut);
            return;
        }

        if (current.interval > 0) {
            ExecutableBase.IntervalIds[current.intervalId] = window.setInterval(function() {
                current.target = current.getTarget();
                current.internalExecute(data);
            }, current.interval);
            return;
        }
        
        current.internalExecute(data);        
    },    
    internalExecute : function(data) {        
    },        
    isValid : function(data) {

        var current = this;

        if (ExecutableHelper.IsNullOrEmpty(current.ands)) {
            return true;
        }

        var res = false;

        $(current.ands).each(function() {

            var hasAny = false;

            $(this).each(function() {

                hasAny = ConditionalFactory.Create(this, current).isSatisfied(data);
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
    complete: function (result, state) {

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
        var executeState = function () {
            try {
                this.execute(resultData);
            }
            catch (e) {
                if (e instanceof IncClientException) {
                    hasBreak = true;
                    return false;//stop execute state
                }

                console.log('Incoding exception: {0}'.f(e.message));
                if (navigator.Ie8) {
                    return false;//stop execute state
                }
                throw e;
            }
        };

        $(result.success ? state.success : state.error).each(executeState);
        $(state.complete).each(executeState);
        if (hasBreak) {
            $(state.breakes).each(function () {
                this.execute(resultData);
            });
        }
    }
});

//#endregion

//#region class ExecutableDirectAction extend from ExecutableBase

incodingExtend(ExecutableDirectAction, ExecutableActionBase);

function ExecutableDirectAction() {
}

ExecutableDirectAction.prototype.internalExecute = function(data) {
    var result = ExecutableHelper.IsNullOrEmpty(this.jsonData.result) ? IncodingResult.Empty : new IncodingResult(this.jsonData.result);
    this.complete(result, data);
};

//#endregion

//#region class ExecutableEventAction extend from ExecutableBase

incodingExtend(ExecutableEventAction, ExecutableActionBase);

function ExecutableEventAction() {
}

ExecutableEventAction.prototype.internalExecute = function(data) {
    this.complete(data.eventResult, data);
};

//#endregion

//#region class ExecutableAjaxAction extend from ExecutableBase

incodingExtend(ExecutableAjaxAction, ExecutableActionBase);

function ExecutableAjaxAction() {
}

ExecutableAjaxAction.prototype.internalExecute = function(data) {

    var current = this;

    var ajaxOptions = $.extend(true, { data : [] }, this.jsonData.ajax);

    for (var i = 0; i < ajaxOptions.data.length; i++) {
        ajaxOptions.data[i].selector = this.tryGetVal(ajaxOptions.data[i].selector);
    }

    if (this.jsonData.hash) {
        var href = $.url(document.location.href);
        if (ExecutableHelper.IsNullOrEmpty(ajaxOptions.url)) {
            ajaxOptions.url = href.furl(this.jsonData.prefix);
        }

        var fragmentParams = href.fparam();
        $.eachProperties(fragmentParams, function() {
            var name = this.replace(current.jsonData.prefix + '__', '');
            ajaxOptions.data.push({ name : name, selector : fragmentParams[this] });
        });
    }

    AjaxAdapter.Instance.request(ajaxOptions, function(result) {
        current.complete(result, data);
    });

};

//#endregion

//#region class ExecutableSubmitAction extend from ExecutableBase

incodingExtend(ExecutableSubmitAction, ExecutableActionBase);

function ExecutableSubmitAction() {
}

ExecutableSubmitAction.prototype.internalExecute = function(data) {

    var current = this;
    var ajaxOptions = $.extend(true, {}, this.jsonData.options);
    if (!ExecutableHelper.IsNullOrEmpty(this.jsonData.options.data)) {
        for (var i = 0; i < this.jsonData.options.data.length; i++) {
            ajaxOptions.data[i].value = this.tryGetVal(this.jsonData.options.data[i].value);
        }
    }

    ajaxOptions.success = function(responseText, statusText, xhr, $form) {
        var result = new IncodingResult(responseText);
        current.complete(result, data);
    };

    var formSelector = eval(this.jsonData.formSelector);
    var form = $(formSelector).is('form') ? formSelector : $(formSelector).closest('form').first();

    ajaxOptions.beforeSubmit = function(formData, jqForm, options) {
        var isValid = $(form).valid();
        if (!isValid) {
            $(form).validate().focusInvalid();
        }
        return isValid;
    };

    $(form).ajaxSubmit(ajaxOptions);
};

//#endregion

//#endregion

//#region Core

//#region class ExecutableInsert extend from ExecutableBase

incodingExtend(ExecutableInsert, ExecutableBase);

function ExecutableInsert() {
}

// ReSharper disable UnusedLocals
// ReSharper disable AssignedValueIsNeverUsed
ExecutableInsert.prototype.internalExecute = function(data) {

    var current = this;

    var insertContent = ExecutableHelper.IsNullOrEmpty(data) ? '' : data;

    if (!ExecutableHelper.IsNullOrEmpty(current.jsonData.property)) {
        var insertObject = data;
        if (_.isArray(data)) {
            insertObject = data.length > 0 ? data[0] : {};
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
            var buildUrlKey = '||buildurl*' + templateId.substring(7, templateId.length);
            templateId = current.tryGetVal(buildUrlKey);
        }
        insertContent = TemplateFactory.ToHtml(ExecutableInsert.Template, templateId, function() {
            return current.tryGetVal(current.jsonData.template);
        }, insertContent);
    }

    if (ExecutableHelper.IsNullOrEmpty(insertContent)) {
        insertContent = '';
    }

    eval("$(current.target).{0}(insertContent.toString())".f(current.jsonData.insertType));

    IncodingEngine.Current.parse(current.target);
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

ExecutableTrigger.prototype.internalExecute = function(data) {

    var eventData = ExecutableHelper.IsNullOrEmpty(this.jsonData.property)
        ? data
        : data.hasOwnProperty(this.jsonData.property) ? data[this.jsonData.property] : '';
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
        $(this).rules("add", { required : true, equalTo : equalTo });
    });
};

//#endregion

//#region class ExecutableValidationRefresh extend from ExecutableBase

incodingExtend(ExecutableValidationRefresh, ExecutableBase);

function ExecutableValidationRefresh() {
}

ExecutableValidationRefresh.prototype.internalExecute = function(data) {

    var current = this;
    $(data).each(function() {

        var name = this.name.toString();
        var input = $('[name]', current.target).filter(function() {
            return $(this).attr('name').toLowerCase() == name.toLowerCase();
        });
        var span = $('[data-valmsg-for]', current.target).filter(function() {
            return $(this).attr('data-valmsg-for').toLowerCase() == name.toLowerCase();
        });

        var inputErrorClass = 'input-validation-error';
        var messageErrorClass = 'field-validation-error';
        var messageValidClass = 'field-validation-valid';

        if (ExecutableHelper.ToBool(this.isValid)) {
            $(input).removeClass(inputErrorClass);
            $(span)
                .removeClass(messageErrorClass)
                .addClass(messageValidClass);
        }
        else {
            $(input).addClass(inputErrorClass);
            $(span)
                .removeClass(messageValidClass)
                .addClass(messageErrorClass)
                .html(this.errorMessage);
        }

    });

    $($(current.target).is('form') ? current.target : $('form', this.target))
        .validate()
        .focusInvalid();
};

//#endregion

//#region class ExecutableEval extend from ExecutableBase

incodingExtend(ExecutableEval, ExecutableBase);

function ExecutableEval() {
}

ExecutableEval.prototype.internalExecute = function(data) {
    eval(this.jsonData.code);
};

//#endregion

//#region class ExecutableEvalMethod extend from ExecutableBase

incodingExtend(ExecutableEvalMethod, ExecutableBase);

function ExecutableEvalMethod() {
}

ExecutableEvalMethod.prototype.internalExecute = function(data) {

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

ExecutableBreak.prototype.internalExecute = function(result) {
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

    $.eachFormElements(this.target, function() {
        var name = $(this).prop('name');

        if (ExecutableHelper.IsNullOrEmpty(name)) {
            return;
        }

        var value = ExecutableHelper.Instance.TryGetVal($('[name={0}]'.f(name)));
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
        var key = prefix + $(this).prop('name');
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

ExecutableStoreManipulate.prototype.internalExecute = function(data) {
    var current = this;
    switch (current.jsonData.type) {
        case 'hash':
            var url = $.url(document.location.href);

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

            ExecutableHelper.RedirectTo(url.toHref());
            break;
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