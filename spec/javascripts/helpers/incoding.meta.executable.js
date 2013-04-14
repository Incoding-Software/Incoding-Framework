"use strict";

//#region class ExecutableHelper

function ExecutableHelper() {

    var getValAction = function (selector) {

        if ($(selector).is('label,script,div,span,p,link')) {
            return function () {
                return $(this).html().trim();
            };
        } else if ($(selector).is(':checkbox') || $(selector).is(':radio')) {
            return function () {

                var isChecked = $(this).is(':checked');
                var val = $(this).val();
                if (!_.isUndefined(val) && !_.isEmpty(val)) {
                    if (!isChecked) {
                        throw "can't get val";
                    }

                    return val;
                }

                return isChecked;

            };
        } else if ($(selector).is('select')) {
            return function () {
                return ('options:selected', $(this)).val();
            };
        } else {
            return function () {
                return $(this).val();
            };
        }
    };

    this.TryGetVal = function (selector) {

        try {
            if ($(selector).length === 0) {
                return selector;
            }
        } catch (e) {
            return selector;
        }

        if ($(selector).length === 1) {
            return getValAction(selector).call(selector);
        }

        var result = [];
        $(selector).each(function () {
            try {
                result.push(getValAction(selector).call(this));
            } catch (exception) {
                console.log(exception);
            }
        });

        return result;
    };

}

ExecutableHelper.Instance = new ExecutableHelper();

ExecutableHelper.IsNullOrEmpty = function (val) {
    return _.isEmpty(val) || _.isUndefined(val);
};

ExecutableHelper.GetTargetOrContext = function (targetSelector, context) {
    var target = $(targetSelector);
    return $(target).length == 0 ? context : target;
};

ExecutableHelper.ExecuteAjax = function (ajaxOptions, callbacks) {
    new AjaxAdapter(ajaxOptions, callbacks);
};

ExecutableHelper.RedirectTo = function (destationUrl) {
    window.location = destationUrl;
};

//#endregion

function AjaxAdapter(ajaxOptions, callbacks) {

    var execute = function (options, callbacksSuccess) {

        console.log('Start ajax with options:');
        console.log(options);

        var insertData = '';
        var isNullOrEmpty = _.isEmpty(options.data) || _.isUndefined(options.data);
        if (!isNullOrEmpty) {
            insertData = [];
            var arrayData = _.isArray(options.data) ? options.data : $.parseJSON(options.data);
            $(arrayData).each(function () {
                insertData.push({ name: this.name, value: ExecutableHelper.Instance.TryGetVal(this.selector) });
            });
        }

        $.ajax({
            async: options.async,
            cache: options.cache,
            contentType: options.contentType,
            crossDomain: options.crossDomain,
            data: insertData.length != 0 ? $.param(insertData) : '',
            dataType: options.dataType,
            global: options.global,
            password: options.password,
            traditional: options.traditional,
            type: options.type,
            url: options.url,
            userName: options.userName,
            success: function (data) {
                console.log('Ajax Success with data:');
                console.log(data);

                var parseResult = new ParseServerResult(data);
                callbacksSuccess(parseResult);

            },
            error: function (event, jqXHR, ajaxSettings, thrownError) {
                console.log('Alax Fail with error:');
            }
        });

        console.log('Stop ajax');
    };

    execute.call(this, ajaxOptions, callbacks.success);

}

//#region class ExecutableFactory

function ExecutableFactory() {

}

ExecutableFactory.prototype =
    {
        create: function (type, data, context) {
            var fullFuncName = 'Executable' + type;
            return eval('new ' + fullFuncName + '(data,context);');
        }
    };

//#endregion

//#region class ExecutableBase

function ExecutableBase(data, context) {

    if (arguments.length > 0) {
        this.init(data, context);
    }

    this.jsonData;
    this.context;

}

ExecutableBase.prototype = {
    jsonData: '',
    context: '',

    init: function (data, context) {
        this.jsonData = _.isObject(data) ? data : $.parseJSON(data);
        this.context = context;
    },

    execute: function (state) {
        throw 'Need override this method';
    }
};

//#endregion

//#region Actions

//#region class ExecutableActionBase extend from ExecutableBase

incodingExtend(ExecutableActionBase, ExecutableBase);

function ExecutableActionBase(data, context) {
    this.init(data, context);
}

$.extend(ExecutableActionBase.prototype, {
    isDone: false,
    complete: function (result, state) {

        if (result.isRedirectTo()) {
            new ExecutableCallbackRedirect({ redirectTo: result.redirectTo }, this.context).execute();
            return;
        }

        if (result.isSuccess()) {
            $(state.success).each(function () {
                this.execute(result);
            });
        } else {
            $(state.error).each(function () {
                this.execute(result);
            });
        }

        $(state.complete).each(function () {
            this.execute(result);
        });

        this.isDone = true;
    }
});

//#endregion

//#region class ExecutableAction extend from ExecutableBase

incodingExtend(ExecutableAction, ExecutableActionBase);

function ExecutableAction(data, context) {
    this.init(data, context);
}

ExecutableAction.prototype.execute = function (state) {
    var result;
    var isHasJsonData = _.isUndefined(this.jsonData.result) || _.isNull(this.jsonData.result) || _.isEmpty(this.jsonData.result);
    result = isHasJsonData ? ParseServerResult.Empty : new ParseServerResult(this.jsonData.result);
    this.complete(result, state);
};

//#endregion

//#region class ExecutableAjaxAction extend from ExecutableBase

incodingExtend(ExecutableAjaxAction, ExecutableActionBase);

function ExecutableAjaxAction(data, context) {
    this.init(data, context);
}

ExecutableAjaxAction.prototype.execute = function (state) {

    var ajaxOptions = this.jsonData.ajax;
    ajaxOptions.url = (_.isEmpty(ajaxOptions.url) || _.isUndefined(ajaxOptions.url)) ? $(this.context).attr('href') : ajaxOptions.url;
    var current = this;
    ExecutableHelper.ExecuteAjax(ajaxOptions, {
        success: function (result) {
            current.complete(result, state);
        }
    });

};

//#endregion

//#region class ExecutableSubmitAction extend from ExecutableBase

incodingExtend(ExecutableSubmitAction, ExecutableActionBase);

function ExecutableSubmitAction(data, context) {
    this.init(data, context);
}

ExecutableSubmitAction.prototype.execute = function (state) {

    var ajaxOptions = this.jsonData.options;
    var current = this;

    ajaxOptions.success = function (responseText, statusText, xhr, $form) {
        var result = new ParseServerResult(responseText);
        current.complete(result, state);
    };

    var target = ExecutableHelper.GetTargetOrContext(this.jsonData.targetSelector, this.context);
    var form = $(target).is('form') ? target : $(target).parents().find('form').first();

    ajaxOptions.beforeSubmit = function (formData, jqForm, options) {
        return $(form).valid();
    };

    $(form).ajaxSubmit(ajaxOptions);

};

//#endregion

//#endregion

//#region Callbacks

//#region class ExecutableCallbackInsert extend from ExecutableBase

incodingExtend(ExecutableCallbackInsert, ExecutableBase);

function ExecutableCallbackInsert(data, context) {
    this.init(data, context);
}

ExecutableCallbackInsert.prototype.execute = function (state) {

    var target = ExecutableHelper.GetTargetOrContext(this.jsonData.targetSelector, this.context);
    var insertContent = !_.isEmpty(this.jsonData.template)
        ? Mustache.to_html(ExecutableHelper.Instance.TryGetVal(this.jsonData.template), state)
        : state.data;

    var evalExpression = "$(target).{0}(insertContent)".f(this.jsonData.insertType);
    eval(evalExpression);
    return true;

};

//#endregion

//#region class ExecutableCallbackTrigger extend from ExecutableBase

incodingExtend(ExecutableCallbackTrigger, ExecutableBase);

function ExecutableCallbackTrigger(data, context) {
    this.init(data, context);
}

ExecutableCallbackTrigger.prototype.execute = function (state) {

    var target = ExecutableHelper.GetTargetOrContext(this.jsonData.targetSelector, this.context);
    $(target).trigger(this.jsonData.trigger);
    return true;
};

//#endregion

//#region class ExecutableCallbackValidation extend from ExecutableBase

incodingExtend(ExecutableCallbackValidation, ExecutableBase);

function ExecutableCallbackValidation(data, context) {
    this.init(data, context);
}

ExecutableCallbackValidation.prototype.execute = function (state) {

    var target = ExecutableHelper.GetTargetOrContext(this.jsonData.targetSelector, this.context);
    var form = $(target).is('form') ? target : $(target).parents().find('form').first();
    $(form).removeData('validator').removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(form);

    //bug in fluent validation. fixed for input
    $('[data-val-equalto-other]', form).each(function () {
        var equalTo = '#' + $(this).data('val-equalto-other').replaceAll('*.', 'Input_');
        $(this).rules("add", { required: true, equalTo: equalTo });
    });

    return true;
};

//#endregion

//#region class ExecutableCallbackRedirect extend from ExecutableBase

incodingExtend(ExecutableCallbackRedirect, ExecutableBase);

function ExecutableCallbackRedirect(data, context) {
    this.init(data, context);
}

ExecutableCallbackRedirect.prototype.execute = function (state) {

    var destationUrl = decodeURIComponent(ExecutableHelper.Instance.TryGetVal(this.jsonData.redirectTo));
    var queryParams = destationUrl.getQueryString();
    $.each(queryParams, function () {
        var rightPart = this.split('=')[1];
        destationUrl = destationUrl.replace(rightPart, ExecutableHelper.Instance.TryGetVal(rightPart));
    });

    ExecutableHelper.RedirectTo(encodeURIComponent(destationUrl));
    return true;

};
//#endregion

//#region class IncodingCallbackDialog extend from ExecutableBase

incodingExtend(ExecutableCallbackDialog, ExecutableBase);

function ExecutableCallbackDialog(data, context) {
    this.init(data, context);
}

ExecutableCallbackDialog.Dialogs = [];

ExecutableCallbackDialog.prototype.execute = function (state) {

    var incDialogClass = 'inc-dialog';
    var incActiveDialogClass = 'inc-dialog-active';
    var allIncDialogClass = incDialogClass + ' ' + incActiveDialogClass;
    var movetotop = 'moveToTop';

    var target = ExecutableHelper.GetTargetOrContext(this.jsonData.targetSelector, this.context);

    var tartgetId = $(target).attr('id');
    if (!ExecutableCallbackDialog.Dialogs.contains(tartgetId)) {
        ExecutableCallbackDialog.Dialogs.push(tartgetId);
    }

    var dialogClose = function () {
        $(this).removeClass(incActiveDialogClass);
        var closeIndex = ExecutableCallbackDialog.Dialogs.getIndex($(this).attr('id'));

        if (closeIndex != -1) {
            ExecutableCallbackDialog.Dialogs.removeOne(closeIndex);
            $(this).empty();
            $(this).dialog('destroy');
        }

        if (ExecutableCallbackDialog.Dialogs.length > 0) {
            var lastDialog = $('#' + ExecutableCallbackDialog.Dialogs[ExecutableCallbackDialog.Dialogs.length - 1]);
            $(lastDialog).dialog(movetotop);
            $(lastDialog).focus().maxZIndex({ inc: 5, group: '.' + incDialogClass });

        }
    };

    var dialogOpen = function () {
        var openIndex = ExecutableCallbackDialog.Dialogs.getIndex($(this).attr('id'));
        $(this).addClass(allIncDialogClass);

        for (var i = 0; i < ExecutableCallbackDialog.Dialogs.length; i++) {
            if (i != openIndex) {
                $('#' + ExecutableCallbackDialog.Dialogs[i]).removeClass(incActiveDialogClass);
            }
        }

    };

    var method = this.jsonData.method;
    if (method == 'open') {
        $(target).bind('dialogclose', function () {
            dialogClose.call(this);
        });
        $(target).bind('dialogopen dialogfocus', function () {
            dialogOpen.call(this);
        });
        target.dialog(this.jsonData.options);
        target.dialog(movetotop);
        $(target).maxZIndex({ inc: 5 });
    } else {
        target.dialog(method);
    }

    return true;

};

//#endregion

//#region Toggle

//#region class ExecutableToggleBase extend from ExecutableBase

incodingExtend(ExecutableToggleBase, ExecutableBase);

function ExecutableToggleBase(data, context) {
    this.init(data, context);
}

ExecutableToggleBase.prototype.execute = function (state) {
    var target = ExecutableHelper.GetTargetOrContext(this.jsonData.targetSelector, this.context);
    var val = ExecutableHelper.Instance.TryGetVal(this.jsonData.toggleValue);
    this.toggleValue(target, val);
};

$.extend(ExecutableToggleBase, {
    toggleValue: function (target, toggleValue) {
    }
});

//#endregion

//#region class ExecutableCallbackToggleClass extend from ExecutableToggleBase

incodingExtend(ExecutableCallbackToggleClass, ExecutableToggleBase);

function ExecutableCallbackToggleClass(data, context) {
    this.init(data, context);
}

ExecutableCallbackToggleClass.prototype.toggleValue = function (target, toggleValue) {

    if ($(target).hasClass(toggleValue)) {
        $(target).removeClass(toggleValue);
    } else {
        $(target).addClass(toggleValue);
    }

    return true;
};

//#endregion

//#region class ExecutableCallbackToggleAttr extend from ExecutableToggleBase

incodingExtend(ExecutableCallbackToggleAttr, ExecutableToggleBase);

function ExecutableCallbackToggleAttr(data, context) {
    this.init(data, context);
}

ExecutableCallbackToggleAttr.prototype.toggleValue = function (target, toggleValue) {
    if ($(target).is('[{0}]'.f(toggleValue))) {
        $(target).removeAttr(toggleValue);
    } else {
        $(target).attr(toggleValue, true);
    }
    return true;
};

//#endregion

//#endregion

//#region class ExecutableCallbackSetClass extend from ExecuteableBase

incodingExtend(ExecutableCallbackSetClass, ExecutableBase);

function ExecutableCallbackSetClass(data, context) {
    this.init(data, context);
}

ExecutableCallbackSetClass.prototype.execute = function (state) {
    var target = ExecutableHelper.GetTargetOrContext(this.jsonData.targetSelector, this.context);
    var value = ExecutableHelper.Instance.TryGetVal(this.jsonData.value);
    $(target).addClass(value);
    return true;
};

//#endregion

//#region class ExecutableCallbackSetAttr extend from ExecutableBase

incodingExtend(ExecutableCallbackSetAttr, ExecutableBase);

function ExecutableCallbackSetAttr(data, context) {
    this.init(data, context);
}

ExecutableCallbackSetAttr.prototype.execute = function (state) {
    var target = ExecutableHelper.GetTargetOrContext(this.jsonData.targetSelector, this.context);
    var value = ExecutableHelper.Instance.TryGetVal(this.jsonData.value);
    $(target).attr(this.jsonData.key, value);
    return true;
};

//#endregion

//#region class ExecutableCallbackSetCss extend from ExecuteableBase

incodingExtend(ExecutableCallbackSetCss, ExecutableBase);

function ExecutableCallbackSetCss(data, context) {
    this.init(data, context);
}

ExecutableCallbackSetCss.prototype.execute = function (state) {
    var target = ExecutableHelper.GetTargetOrContext(this.jsonData.targetSelector, this.context);
    var value = ExecutableHelper.Instance.TryGetVal(this.jsonData.value);
    $(target).css(this.jsonData.key, value);
    return true;
};

//#endregion

//#region class ExecutableCallbackEval extend from ExecutableBase

incodingExtend(ExecutableCallbackEval, ExecutableBase);

function ExecutableCallbackEval(data, context) {
    this.init(data, context);
}

ExecutableCallbackEval.prototype.execute = function (state) {
    var code = ExecutableHelper.Instance.TryGetVal(this.jsonData.code);
    eval(code);
    return true;
};

//#endregion

//#region class ExecutableCallbackAlert extend from ExecutableBase

incodingExtend(ExecutableCallbackAlert, ExecutableBase);

function ExecutableCallbackAlert(data, context) {
    this.init(data, context);
}

ExecutableCallbackAlert.prototype.execute = function (state) {
    var message = ExecutableHelper.Instance.TryGetVal(this.jsonData.message);
    window.alert(message);
    return true;
};

//#endregion

//#region class ExecutableCallbackConfirm extend from ExecutableBase

incodingExtend(ExecutableCallbackConfirm, ExecutableBase);

function ExecutableCallbackConfirm(data, context) {
    this.init(data, context);
}

ExecutableCallbackConfirm.prototype.execute = function (state) {
    var message = ExecutableHelper.Instance.TryGetVal(this.jsonData.message);
    return window.confirm(message);
};

//#endregion

//#endregion