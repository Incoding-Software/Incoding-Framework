"use strict";

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

            if ($('[name={0}]'.f(name)).is('[type=checkbox],select')) {
                value = value.toString().split(',');
            }

            if (_.isArray(value)) {
                $(value).each(function() {
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
            success : function(data) {
                var parseResult = new IncodingResult(data);
                callback(parseResult);
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxSuccess));
            },
            beforeSend : function(jqXHR, settings) {
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxBefore));
            },
            complete : function(jqXHR, textStatus) {
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxComplete));
            },
            error : function(jqXHR, textStatus, errorThrown) {
                $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxError));
            },
            data : this.params(options.data)
        });

        return $.ajax(options);
    };

}

AjaxAdapter.Instance = new AjaxAdapter();

//#region class ExecutableHelper

function ExecutableHelper() {

    var supportedElements = 'script,input,select,options,textarea';

    var getValAction = function(selector) {

        var hasNotSupportedElement = $(selector).not(supportedElements).length > 0;
        if (hasNotSupportedElement) {
            return selector;
        }

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

        if ($(selector).is('[multiple]')) {
            var res = [];
            $($(selector).val()).each(function() {
                if (!ExecutableHelper.IsNullOrEmpty(this)) {
                    res.push(this);
                }
            });
            return res;
        }

        if ($(selector).is(':radio')) {
            var nameSelector = '[name="{0}"]:checked'.f($(selector).prop('name'));
            return $(nameSelector).val();
        }

        return $(selector).val();
    };

    this.self = '';
    this.target = '';

    this.TryGetVal = function(selector) {

        if (ExecutableHelper.IsNullOrEmpty(selector)) {
            return selector;
        }

        if (selector instanceof jQuery) {
            return getValAction(selector);
        }

        var isPrimitiveType = (_.isNumber(selector) || _.isBoolean(selector) || _.isArray(selector) || _.isDate(selector) || _.isFunction(selector));
        var isObject = _.isObject(selector) && !_.isElement($(selector)[0]);
        if ((isPrimitiveType || isObject) && !_.isString(selector)) {
            return selector;
        }

        selector = selector.toString();

        var isJqueryVariable = selector.startsWith("$(") && selector.toString().endWith(")");

        if (isJqueryVariable) {
            return getValAction(eval(selector));
        }

        if (selector.contains("@@@@@@@")) {
            var options = $.parseJSON(selector.replaceAll("'@@@@@@@", '').replaceAll("@@@@@@@'", ''));
            if (!ExecutableHelper.IsNullOrEmpty(options.data)) {
                for (var i = 0; i < options.data.length; i++) {
                    options.data[i].selector = ExecutableHelper.Instance.TryGetVal(options.data[i].selector);
                }
            }
            var ajaxData;
            AjaxAdapter.Instance.request(options, function(result) {
                ajaxData = result.data;
            });
            return ajaxData;
        }

        var res = selector;
        if (selector.contains("@@@@@@")) {
            res = $.cookie(selector.replaceAll("'@@@@@@", '').replaceAll("@@@@@@'", ''));
        }
        else if (selector.contains("@@@@@")) {
            var clearValue = selector.replaceAll("'@@@@@", '').replaceAll("@@@@@'", '');
            res = $.url(window.location.href).fparam(clearValue.split(':')[0], clearValue.split(':')[1]);
        }
        else if (selector.contains("@@@@")) {
            res = $.url(window.location.href).furl(selector.replaceAll("'@@@@", '').replaceAll("@@@@'", ''));
        }
        else if (selector.contains("@@@")) {
            res = $.url(window.location.href).param(selector.replaceAll("'@@@", '').replaceAll("@@@'", ''));
        }        
        else if (selector.contains("@@javascript")) {
            res = eval(selector.replaceAll('@@','').split(":")[1]);
        }

        return ExecutableHelper.IsNullOrEmpty(res) ? '' : res;

    };

    this.TrySetValue = function(element, val) {

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
            $('input[name={0}][value="{1}"]'.f($(element).prop('name'), val)).prop('checked', true);
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
        return evaluated.call(data[property]);
    }

    var res = false;
    $(data).each(function() {
        if (evaluated.call(this[property])) {
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

    if (method == 'contains') {
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
    destentationUrl = decodeURIComponent(destentationUrl);

    var isSame = destentationUrl.contains('#') && window.location.hash.replace("#", "") == destentationUrl.split('#')[1];
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

function IncMustacheTemplate(data, template) {

    this.data = data;

    this.prepare = function(source) {

        var currentData = this.data;

        $(['Sum', 'Max', 'Min', 'First', 'Last', 'Average', 'Count']).each(function() {

            var pattern = "{{#IncTemplate" + this + "}}(.*?){{/IncTemplate" + this + "}}";
            if (!new RegExp(pattern, "g").test(source)) {
                return true;
            }

            var value = '';
            var property = new RegExp(pattern, "g").exec(source)[1];
            var isNumberType = this == 'Max' || this == 'Min' || this == 'Average' || this == 'Sum' || this == 'Count';

            if (ExecutableHelper.IsNullOrEmpty(currentData) && isNumberType) {
                value = 0;
            }
            else if (this == 'Count' && _.isArray(currentData)) {
                value = currentData.length;
            }
            else if (_.isArray(currentData)) {
                // ReSharper disable UnusedLocals
                var parseData = currentData.select(function(item) {
                    var itemValue = item[property];
                    if (isNumberType) {
                        {
                            var parseValue = parseFloat(itemValue);
                            if (_.isNumber(parseValue) && !_.isNaN(parseValue)) {
                                return parseValue;
                            }
                        }
                    }
                    return itemValue;
                });
                // ReSharper restore UnusedLocals    
                value = eval('parseData.{0}()'.f(this.toString().toLowerCase()));
            }
            else if (currentData.hasOwnProperty(property)) {
                value = currentData[property];
            }
            else {
                value = '';
            }

            source = source.replaceAll("{{#IncTemplate" + this + "}}" + property + "{{/IncTemplate" + this + "}}", value);
        });

        return source;
    };

    this.template = this.prepare(template);

    this.render = function() {
        return Mustache.to_html(this.template, { data : this.data });
    };

}

function TemplateFactory() {
}

TemplateFactory.Create = function(type, data, template) {
    return new IncMustacheTemplate(data, template);
};

//#endregion