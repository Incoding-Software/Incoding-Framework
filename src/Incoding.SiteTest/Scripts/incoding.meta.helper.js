"use strict";

//#region class IncAjaxEvent

function IncAjaxEvent() {
    this.ResponseText = '';
    this.StatusCode = '';
    this.StatusText = '';
}

IncAjaxEvent.Create = function(response) {
    var res = new IncAjaxEvent();
    res.ResponseText = response.responseText;
    res.StatusCode = response.status;
    res.StatusText = response.statusText;
    return res;
};

//#endregion


//#region class IncSpecialBind

function IncSpecialBinds() {

}

IncSpecialBinds.Incoding = 'incoding';

IncSpecialBinds.InitIncoding = 'initincoding';

IncSpecialBinds.IncChangeUrl = 'incchangeurl';

IncSpecialBinds.IncAjaxBefore = 'incajaxbefore';

IncSpecialBinds.IncAjaxError = 'incajaxerror';

IncSpecialBinds.IncAjaxComplete = 'incajaxcomplete';

IncSpecialBinds.IncAjaxSuccess = 'incajaxsuccess';

IncSpecialBinds.IncInsert = 'incinsert';

IncSpecialBinds.DocumentBinds = [
    IncSpecialBinds.IncChangeUrl,
    IncSpecialBinds.IncAjaxBefore,
    IncSpecialBinds.IncAjaxError,
    IncSpecialBinds.IncAjaxComplete,
    IncSpecialBinds.IncAjaxSuccess,
    IncSpecialBinds.IncInsert
];

//#endregion

function AjaxAdapter() {

    this.params = function(data) {
        var res = [];
        $(data).each(function() {
            var name = this.name;
            var value = this.selector;

            if (ExecutableHelper.IsNullOrEmpty(value)) {
                res.push({ name : name, value : value });
                return;
            }
            
            var isElementCanArray =  $.byName(name).is('[type=checkbox],select,[type=radio]');
            var isValueCanArray = _.isArray(value) || value.toString().contains(',');

            if (_.isArray(value) || (isValueCanArray && isElementCanArray)) {
                $(value.toString().split(',')).each(function() {
                    res.push({ name : name, value : this });
                });
            }
            else {
                res.push({ name : name, value : value });
            }

        });
        return res;
    };

    this.request = function(options, callback) {

        $.extend(options, {
            headers : { "X-Requested-With" : "XMLHttpRequest" },
            dataType : 'JSON',
            success : function(data, textStatus, jqXHR) {
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxSuccess), IncodingResult.Success(IncAjaxEvent.Create(jqXHR)));
                var parseResult = new IncodingResult(data);
                callback(parseResult);
            },
            beforeSend : function(jqXHR, settings) {
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxBefore), IncodingResult.Success(IncAjaxEvent.Create(jqXHR)));
            },
            complete : function(jqXHR, textStatus) {
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxComplete), IncodingResult.Success(IncAjaxEvent.Create(jqXHR)));
            },
            error : function(jqXHR, textStatus, errorThrown) {
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxError), IncodingResult.Success(IncAjaxEvent.Create(jqXHR)));
            },
            data : this.params(options.data)
        });

        return $.ajax(options);
    };

}

AjaxAdapter.Instance = new AjaxAdapter();

//#region class ExecutableHelper

function ExecutableHelper() {

    var getValAction = function(selector) {

        var scripts = $(selector).filter('script');
        if (scripts.length > 0) {
            return $.trim($(scripts).html());
        }

        if ($(selector).is(':checkbox')) {
            var onlyCheckbox = $(selector).filter(':checkbox');
            if (onlyCheckbox.length == 1) {
                return $(onlyCheckbox).is(':checked');
            }
            if (onlyCheckbox.length > 1) {

                var res = [];
                $(onlyCheckbox).each(function() {
                    if (!$(this).is(':checked')) {
                        return true;
                    }
                    res.push($(this).val());
                    return true;
                });

                return res;
            }
        }

        if (($(selector).is('select') && $(selector).length > 1)) {
            var res = [];
            $(selector).each(function() {
                var val = $(this).val();
                if (!ExecutableHelper.IsNullOrEmpty(val)) {
                    res.push(val);
                }
            });
            return res;
        }

        if ($(selector).is('select[multiple]')) {
            var res = [];
            $($(selector).val()).each(function() {
                if (!ExecutableHelper.IsNullOrEmpty(this)) {
                    res.push(this);
                }
            });
            return res;
        }

        if ($(selector).is(':radio')) {
            return $.byName($(selector).prop('name'), ':checked').val();
        }

        return $(selector).val();
    };

    this.self = '';
    this.target = '';
    this.event = '';
    this.result = '';

    this.TryGetVal = function(selector) {

        if (ExecutableHelper.IsNullOrEmpty(selector)) {
            return selector;
        }
        if (selector instanceof jQuery) {
            return selector.length != 0 ? getValAction(selector) : '';
        }

        var isPrimitiveType = (_.isNumber(selector) || _.isBoolean(selector) || _.isArray(selector) || _.isDate(selector) || _.isFunction(selector));
        var isObject = _.isObject(selector) && !_.isElement($(selector)[0]);
        if ((isPrimitiveType || isObject) && !_.isString(selector)) {
            return selector;
        }

        selector = selector.toString();
        var isJqueryVariable = selector.startsWith("$(");
        var isSelector = (selector.startsWith("||") && selector.endWith("||"));

        if (!isJqueryVariable && !isSelector) {
            return selector;
        }

        if (isJqueryVariable) {
            return this.TryGetVal(eval(selector));
        }

        var res;

        if (isSelector) {

            var valueSelector = selector.substring(2, selector.length - 2);
            valueSelector = valueSelector.substring(selector.indexOf('*') - 1, selector.length);

            var isTypeSelector = function(type) {
                return selector.startsWith("||{0}*".f(type));
            };

            if (isTypeSelector('ajax') || isTypeSelector('buildurl')) {
                var options = $.parseJSON(valueSelector);
                if (!ExecutableHelper.IsNullOrEmpty(options.data)) {
                    for (var i = 0; i < options.data.length; i++) {
                        options.data[i].selector = ExecutableHelper.Instance.TryGetVal(options.data[i].selector);
                    }
                }
                if (isTypeSelector('buildurl')) {
                    var dataAsString = '';
                    if (!ExecutableHelper.IsNullOrEmpty(options.data)) {
                        for (var i = 0; i < options.data.length; i++) {
                            options.data[i].selector = this.TryGetVal(options.data[i].selector);
                        }
                        dataAsString = options.data.select(function(item) {
                            return '{0}={1}'.f(item.name, item.selector);
                        }).join('&');
                    }
                    res = '{0}?{1}'.f(options.url, dataAsString);
                }
                else {
                    var ajaxData;
                    AjaxAdapter.Instance.request(options, function(result) {
                        ajaxData = result.data;
                    });
                    res = ajaxData;
                }

            }
            else if (isTypeSelector('cookie')) {
                res = $.cookie(valueSelector);
            }
            else if (isTypeSelector('hashQueryString')) {
                res = $.url(window.location.href).fparam(valueSelector.split(':')[0], valueSelector.split(':')[1]);
            }
            else if (isTypeSelector('hashUrl')) {
                res = $.url(window.location.href).furl(valueSelector);
            }
            else if (isTypeSelector('queryString')) {
                res = $.url(window.location.href).param(valueSelector);
            }
            else if (isTypeSelector('javascript')) {
                res = eval(valueSelector);
            }
        }

        return ExecutableHelper.IsNullOrEmpty(res) ? '' : res;

    };

    this.TrySetValue = function(element, val) {

        if ($(element).is('[type=hidden]') && $.byName($(element).prop('name')).length == 2) {
            return;
        }

        if ($(element).is(':checkbox')) {
            var onlyCheckBoxes = $(element).filter(':checkbox');
            $(onlyCheckBoxes).prop('checked', false);
            if ($(onlyCheckBoxes).length == 1) {
                if (ExecutableHelper.ToBool(val)) {
                    $(onlyCheckBoxes).prop('checked', true);
                }
            }
            else {
                var arrayVal = _.isArray(val) ? val : val.split(',');
                $(onlyCheckBoxes).each(function() {
                    if (arrayVal.contains($(this).val())) {
                        $(this).prop('checked', true);
                    }
                    else {
                        $(this).prop('checked', false);
                    }
                });
            }
            return;
        }

        if ($(element).is('[multiple]')) {
            $(element).val(val.split(','));
            return;
        }

        if ($(element).is('select') && $(element).length > 1) {
            var arrayVal = _.isArray(val) ? val : val.split(',');
            $(arrayVal).each(function() {
                $('option[value="{0}"]'.f(this)).closest('select').val(this.toString()); //this.toString() fixed for ie < 9
            });
            return;
        }

        if ($(element).is(':radio')) {
            $.byName($(element).prop('name'), '[value="{0}"]'.f(val))
                .prop('checked', true);
            return;
        }

        if ($(element).is('script,label,div,span,p')) {
            $(element).html(val);
        }
        else {
            $(element).val(val);
        }

    };

}

ExecutableHelper.Instance = new ExecutableHelper();

ExecutableHelper.IsData = function(data, property, evaluated) {

    if (ExecutableHelper.IsNullOrEmpty(property)) {
        return evaluated.call(data);
    }

    if (!_.isArray(data)) {
        return evaluated.call(data[property] || '');
    }

    var res = false;
    $(data).each(function() {
        if (evaluated.call(this[property] || '')) {
            res = true;
            return false;
        }
    });

    return res;

};

ExecutableHelper.Filter = function(data, filter) {
    var res;
    if (_.isArray(data)) {
        res = [];
        $(data).each(function() {
            if (filter.isSatisfied(this)) {
                res.push(this);
            }
        });
    }
    else {
        res = filter.isSatisfied(data) ? data : {};
    }
    return res;
};

ExecutableHelper.Compare = function(actual, expected, method) {

    method = method.toString().toLowerCase();

    if (method == 'isempty') {
        return ExecutableHelper.IsNullOrEmpty(actual);
    }

    actual = ExecutableHelper.IsNullOrEmpty(actual) ? '' : actual.toString().toUpperCase();
    expected = ExecutableHelper.IsNullOrEmpty(expected) ? '' : expected.toString().toUpperCase();

    if (method == 'equal') {
        return actual === expected;
    }

    if (method == 'notequal') {
        return actual !== expected;
    }

    if (method == 'iscontains') {
        return actual.contains(expected);
    }

    if (method == 'lessthan') {
        return parseFloat(actual) < parseFloat(expected);
    }
    if (method == 'lessthanorequal') {
        return parseFloat(actual) <= parseFloat(expected);
    }

    if (method == 'greaterthan') {
        return parseFloat(actual) > parseFloat(expected);
    }

    if (method == 'greaterthanorequal') {
        return parseFloat(actual) >= parseFloat(expected);
    }

    throw new IncClientException({ message : "Can't compare by method {0}".f(method) }, {});
};

ExecutableHelper.IsNullOrEmpty = function(value) {
    var isNothing = _.isUndefined(value) || _.isNull(value) || _.isNaN(value) || value == "undefined";
    if (isNothing) {
        return true;
    }

    if (_.isString(value)) {
        return _.isEmpty(value.toString());
    }

    if (_.isArray(value)) {
        return value.length == 0;
    }

    if (_.isNumber(value) || _.isDate(value) || _.isBoolean(value) || _.isFunction(value)) {
        return false;
    }

    var hasOwnProperty = false;
    $.eachProperties(value, function() {
        hasOwnProperty = true;
    });

    return !hasOwnProperty;
};

ExecutableHelper.RedirectTo = function(destentationUrl) {
    var decodeUri = decodeURIComponent(destentationUrl);

    var isSame = decodeUri.contains('#') && window.location.hash.replace("#", "") == decodeUri.split('#')[1];
    if (isSame) {
        $(document).trigger(jQuery.Event(IncSpecialBinds.IncChangeUrl));
        return;
    }

    window.location = destentationUrl;
};

ExecutableHelper.UrlDecode = function(value) {
    return decodeURIComponent(value);
};

ExecutableHelper.UrlEncode = function(value) {
    if (ExecutableHelper.IsNullOrEmpty(value)) {
        return value;
    }

    var encode = function() {
        return this
            .replaceAll('&', '%26')
            .replaceAll('?', '%3F')
            .replaceAll('/', '%2F')
            .replaceAll('=', '%3D')
            .replaceAll(':', '%3A')
            .replaceAll('@', '%40')
            .replaceAll('#', '%23');
    };

    if (value instanceof Array) {
        for (var i = 0; i < value.length; i++) {
            value[i] = encode.call(value[i]);
        }
        return value;
    }
    else {
        return encode.call(value.toString());
    }
};

ExecutableHelper.ToBool = function(value) {

    if (ExecutableHelper.IsNullOrEmpty(value)) {
        return false;
    }

    if (value instanceof Boolean) {
        return value;
    }
    var toStringVal = value.toString().toLocaleLowerCase();

    return toStringVal === 'true';
};

//#endregion

//#region Templates

function IncMustacheTemplate() {

    this.compile = function(tmpl) {
        return tmpl;
    };

    this.render = function(tmpl, data) {
        var compile = Mustache.compile(tmpl);
        return compile(data);
    };

}

function IncHandlerbarsTemplate() {

    this.compile = function(tmpl) {
        return navigator.Ie8 ? tmpl
            : Handlebars.precompile(tmpl);
    };

    this.render = function(tmpl, data) {

        if (navigator.Ie8) {
            return Handlebars.compile(tmpl)(data);
        }

        if (!_.isFunction(tmpl)) {
            tmpl = eval("(" + tmpl + ")");
        }
        return Handlebars.template(tmpl)(data);
    };

}

function TemplateFactory() {
}

TemplateFactory.Version = '';

TemplateFactory.ToHtml = function (builder, selectorKey, evaluatedSelector, data) {

    selectorKey = selectorKey + TemplateFactory.Version;
    if (navigator.Ie8) {
        selectorKey = selectorKey + 'ie8';
    }

    var compile = '';
    var isLocalStore = typeof (Storage) !== "undefined";
    if (isLocalStore) {
        try {
            compile = localStorage.getItem(selectorKey);
        }
        catch (e) {
            compile = '';
        }
    }

    if (ExecutableHelper.IsNullOrEmpty(compile)) {
        var tmplContent = evaluatedSelector();
        if (ExecutableHelper.IsNullOrEmpty(tmplContent)) {
            throw 'Template is empty';
        }

        compile = builder.compile(tmplContent);
        if (isLocalStore) {
            try {
                localStorage.setItem(selectorKey, compile);
            }
            catch(e) {
                if (e.name === 'QUOTA_EXCEEDED_ERR') {
                    localStorage.clear();
                }
            }
        }
    }

    if (!_.isArray(data) && !ExecutableHelper.IsNullOrEmpty(data)) {
        data = [data];
    }

    return builder.render(compile, { data: data });
};

//#endregion