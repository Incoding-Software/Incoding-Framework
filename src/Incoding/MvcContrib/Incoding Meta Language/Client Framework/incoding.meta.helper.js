"use strict";

//#region class IncAjaxEvent

function IncAjaxEvent() {
    this.ResponseText = '';
    this.StatusCode = '';
    this.StatusText = '';
}

IncAjaxEvent.Create = function (response) {
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

IncSpecialBinds.IncGlobalError = 'incglobalerror';

IncSpecialBinds.IncError = 'incerror';

IncSpecialBinds.DocumentBinds = [
    IncSpecialBinds.IncChangeUrl,
    IncSpecialBinds.IncAjaxBefore,
    IncSpecialBinds.IncAjaxError,
    IncSpecialBinds.IncAjaxComplete,
    IncSpecialBinds.IncAjaxSuccess,
    IncSpecialBinds.IncGlobalError,
    IncSpecialBinds.IncInsert
];

//#endregion

function AjaxAdapter() {

    this.params = function (data) {
        var res = [];
        $(data).each(function () {
            var name = this.name;
            var value = this.selector;

            if (ExecutableHelper.IsNullOrEmpty(value)) {
                res.push({ name: name, value: value });
                return;
            }

            var isElementCanArray = $(name.toSelectorAsName()).is('[type=checkbox],select,[type=radio]');
            var isValueCanArray = _.isArray(value) || value.toString().contains(',');

            if (_.isArray(value) || (isValueCanArray && isElementCanArray)) {
                $(value.toString().split(',')).each(function () {
                    res.push({ name: name, value: this });
                });
            }
            else {
                res.push({ name: name, value: value });
            }

        });
        return res;
    };

    this.request = function(options, callback) {
        if (!options.hasOwnProperty('global')) {
            options.global = true;
        }

        $.extend(options, {
            url : options.url.split("?")[0],
            headers : { "X-Requested-With" : "XMLHttpRequest" },
            dataType : 'JSON',
            success : function(data, textStatus, jqXHR) {
                if (options.global) {
                    $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxSuccess), IncodingResult.Success(IncAjaxEvent.Create(jqXHR)));
                }
                var parseResult = new IncodingResult(data);
                callback(parseResult);
            },
            beforeSend : function(jqXHR, settings) {
                if (options.global) {
                    $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxBefore), IncodingResult.Success(IncAjaxEvent.Create(jqXHR)));
                }
            },
            complete : function(jqXHR, textStatus) {
                if (options.global) {
                    $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxComplete), IncodingResult.Success(IncAjaxEvent.Create(jqXHR)));
                }
            },
            error : function(jqXHR, textStatus, errorThrown) {
                if (options.global) {
                    $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxError), IncodingResult.Success(IncAjaxEvent.Create(jqXHR)));
                }
            },
            data : this.params(options.data)
        });

        return $.ajax(options);
    };

}

AjaxAdapter.Instance = new AjaxAdapter();

//#region class ExecutableHelper

function ExecutableHelper() {
    var isSelector = function (selector) {
        return selector.startsWith("||") && selector.endWith("||");
    };
    var isJquerySelector = function (selector) {
        return selector.startsWith("$(");
    };
    var getJquery = function (selector) {

        if ($(selector).is(':checkbox')) {
            var onlyCheckbox = $(selector).filter(':checkbox');
            if (onlyCheckbox.length == 1) {
                return $(onlyCheckbox).is(':checked');
            }
            if (onlyCheckbox.length > 1) {

                var res = [];
                $(onlyCheckbox).each(function () {
                    if (!$(this).is(':checked')) {
                        return true;
                    }
                    res.push($(this).val());
                    return true;
                });

                return res;
            }
        }
        else if (($(selector).is("select") && $(selector).length > 1)) {
            var res = [];
            $(selector).each(function () {
                var val = $(this).val();
                if (!ExecutableHelper.IsNullOrEmpty(val)) {
                    res.push(val);
                }
            });
            return res;
        }
        else if ($(selector).is("select[multiple]")) {
            var res = [];
            $($(selector).val()).each(function () {
                if (!ExecutableHelper.IsNullOrEmpty(this)) {
                    res.push(this);
                }
            });
            return res;
        }
        else if ($(selector).is(":radio")) {
            return $($(selector).prop("name").toSelectorAsName() + ":checked").val();
        }
        else if ($(selector).isFormElement()) {
            return $(selector).val();
        }

        var something = $(selector).val();
        return ExecutableHelper.IsNullOrEmpty(something)
            ? $.trim($(selector).html())
            : something;
    };
    var getResult = function (selector, currentResult) {
        if (ExecutableHelper.IsNullOrEmpty(selector)) {
            return currentResult;
        }
        var res = [];
        if (selector.startsWith("[")) {
            var index = selector.substring(selector.indexOf('[') + 1, selector.indexOf(']'));
            currentResult = currentResult[index];
            selector = selector.substring(selector.indexOf(']') + 1, selector.length);
        }

        $(!_.isArray(currentResult) ? [currentResult] : currentResult).each(function () {

            var valueOfProperty = this;
            $(selector.split('.')).each(function () {
                if (ExecutableHelper.IsNullOrEmpty(this)) {
                    return true;
                }

                var index = this.substring(this.indexOf('[') + 1, this.indexOf(']'));
                if (!ExecutableHelper.IsNullOrEmpty(index)) {
                    var array = valueOfProperty[this.substring(0, this.indexOf('['))];
                    valueOfProperty = array[index];
                    return true;
                }

                var valueOfMethod = this.substring(this.indexOf('(') + 1, this.lastIndexOf(')'));
                if (!ExecutableHelper.IsNullOrEmpty(valueOfMethod)) {
                    var nameOfMethod = this.substring(0, this.indexOf('('));
                    if (nameOfMethod === 'Select') {
                        var tmpValueOfProperty = [];

                        $(valueOfProperty).each(function () {
                            tmpValueOfProperty.push(getResult(valueOfMethod, this));
                        });
                        valueOfProperty = tmpValueOfProperty;

                    }
                    if (nameOfMethod === 'Any') {
                        var res = false;
                        var splitValue = valueOfMethod.split(' ');
                        $(valueOfProperty).each(function () {
                            var helper = $.extend({}, ExecutableHelper.Instance, { result: this });
                            var actual = helper.TryGetVal(splitValue[0]);
                            var expected = helper.TryGetVal(splitValue[2]);
                            var method = splitValue[1];
                            res = ExecutableHelper.Compare(actual, expected, method);
                            return res ? false : true;
                        });
                        valueOfProperty = res;
                    }
                    return true;
                }

                valueOfProperty = valueOfProperty[this];

            });

            res.push(ExecutableHelper.IsNullOrEmpty(valueOfProperty) ? '' : valueOfProperty);
        });


        return res.length === 1 ? res[0] : res;

    };
    this.self = '';
    this.target = '';
    this.event = '';
    this.result = '';
    this.resultOfEvent = '';

    this.TryGetVal = function (selector) {

        if (ExecutableHelper.IsNullOrEmpty(selector)) {
            return selector;
        }
        if (selector instanceof jQuery) {
            return selector.length != 0 ? getJquery(selector) : '';
        }

        var isPrimitiveType = (_.isNumber(selector) || _.isBoolean(selector) || _.isArray(selector) || _.isDate(selector) || _.isFunction(selector));
        var isObject = _.isObject(selector) && !_.isElement($(selector)[0]);
        if ((isPrimitiveType || isObject) && !_.isString(selector)) {
            return selector;
        }

        selector = selector.toString();


        if (!isJquerySelector(selector) && !isSelector(selector)) {
            return selector;
        }

        if (isJquerySelector(selector)) {
            return this.TryGetVal(eval(selector));
        }

        var res;

        if (isSelector(selector)) {

            var valueSelector = selector.substring(2, selector.length - 2)
                .substring(selector.indexOf('*') - 1, selector.length);

            var isType = function(type) {
                return selector.startsWith("||{0}*".f(type));
            };

            if (isType('buildurl')) {
                var toBuildUrl = $.url(valueSelector);
                $.eachProperties(toBuildUrl.param(), function() {
                    toBuildUrl.setParam(this, ExecutableHelper.Instance.TryGetVal(toBuildUrl.param()[this]));
                });

                $.eachProperties(toBuildUrl.fparam(), function() {
                    toBuildUrl.setFparam(this, ExecutableHelper.Instance.TryGetVal(toBuildUrl.fparam()[this]));
                });
                return toBuildUrl.toHref();
            }
            if (isType('ajax')) {
                var options = $.extend(true, { data : [] }, $.parseJSON(valueSelector));
                var ajaxUrl = $.url(options.url);
                $.eachProperties(ajaxUrl.param(), function() {
                    options.data.push({ name : this, selector : ExecutableHelper.Instance.TryGetVal(ajaxUrl.param()[this]) });
                });
                var ajaxData;
                AjaxAdapter.Instance.request(options, function(result) {
                    ajaxData = result.data;
                });
                res = ajaxData;
            }
            else if (isType('cookie')) {
                res = $.cookie(valueSelector);
            }
            else if (isType('hashQueryString')) {
                res = $.url(window.location.href).fparam(valueSelector.split(':')[0], valueSelector.split(':')[1]);
            }
            else if (isType('hashUrl')) {
                res = $.url(window.location.href).furl(valueSelector);
            }
            else if (isType('queryString')) {
                res = $.url(window.location.href).param(valueSelector);
            }
            else if (isType('value')) {
                res = valueSelector.replaceAll('1ADC3DB4-D196-4E59-9FC0-6FAD2633EF07', "|")
                    .replaceAll("42CE7EF2-7812-4E6D-8071-676DA8CA7ED7", "*");
            }
            else if (isType('javascript')) {
                res = eval(valueSelector);
            }
            else if (isType('jquery')) {
                res = $(valueSelector);
            }
            else if (isType('result') || isType('resultOfevent')) {
                res = getResult(valueSelector, isType('result') ? this.result : this.resultOfEvent);
            }
        }

        return ExecutableHelper.IsNullOrEmpty(res) ? '' : res;

    };

    this.TrySetValue = function(element, val) {

        if ($(element).is('[type=hidden]') && $(element).is(':checkbox') && $(element).length == 2) {
            element = $(element).filter(':checkbox');
        } //fix CheckBoxFor

        if ($(element).is(':checkbox')) {
            var onlyCheckBoxes = element;
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

        if ($(element).is('select[multiple]')) {
            $(element).val(val.split(','));
            return;
        }

        if ($(element).is('select') && $(element).length > 1) {            
            $(_.isArray(val) ? val : val.split(',')).each(function() {
                if (this.toString() != '') // fix to not update different selects if val is empty
                {
                    $('option[value="{0}"]'.f(this)).closest('select').val(this.toString());
                } //this.toString() fixed for ie < 9
            });
            return;
        }

        if ($(element).is(':radio')) {
            $($(element).prop("name").toSelectorAsName() + "[value=\"{0}\"]".f(val)).prop('checked', true);
            return;
        }

        if ($(element).isFormElement()) {
            $(element).val(val);
        }
        else {
            $(element).html(val);
        }

    };

}

ExecutableHelper.Instance = new ExecutableHelper();

ExecutableHelper.IsData = function (data, property, evaluated) {

    if (ExecutableHelper.IsNullOrEmpty(property)) {
        return evaluated.call(data);
    }

    var res = false;
    $(!_.isArray(data) ? [data] : data).each(function () {
        var valueOfProperty = this[property];
        if (evaluated.call(ExecutableHelper.IsNullOrEmpty(valueOfProperty) ? '' : valueOfProperty)) {
            res = true;
            return false;
        }
    });

    return res;
};


ExecutableHelper.Filter = function (data, filter) {
    var res;
    if (_.isArray(data)) {
        res = [];
        $(data).each(function () {
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

ExecutableHelper.Compare = function (actual, expected, method) {

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

    throw new IncClientException({ message: "Can't compare by method {0}".f(method) }, {});
};

ExecutableHelper.IsNullOrEmpty = function (value) {
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
    $.eachProperties(value, function () {
        hasOwnProperty = true;
    });

    return !hasOwnProperty;
};

ExecutableHelper.PushState = function (rootOfRool, url) {
    history.pushState({}, 'Title', rootOfRool + "/" + url);
};

ExecutableHelper.RedirectTo = function(destentationUrl) {
    // decode url issue for special characters like % or /
    // fixed like here: https://github.com/medialize/URI.js/commit/fd8ee89a024698986ebef57393fcedbe22631616

    var safeGetUri = function(url) {        
        try {
           return   decodeURIComponent(url);
        }
        catch (ex) {
            return url;
        }        
    };
    var decodeUri = safeGetUri(destentationUrl);
    var decodeHash = safeGetUri(window.location.hash);

    var isSame = decodeUri.contains('#') && decodeHash.replace("#", "") == decodeUri.split('#')[1];
    if (isSame) {
        $(document).trigger(jQuery.Event(IncSpecialBinds.IncChangeUrl));
        return;
    }

    window.location = destentationUrl;
};

ExecutableHelper.UrlDecode = function (value) {
    return decodeURIComponent(value);
};

ExecutableHelper.UrlEncode = function (value) {
    if (ExecutableHelper.IsNullOrEmpty(value)) {
        return value;
    }

    var encode = function () {
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

ExecutableHelper.ToBool = function (value) {

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

    this.compile = function (tmpl) {
        return tmpl;
    };

    this.render = function (tmpl, data) {
        var compile = Mustache.compile(tmpl);
        return compile(data);
    };

}

function IncHandlerbarsTemplate() {

    this.compile = function (tmpl) {
        return navigator.Ie8 ? tmpl
            : Handlebars.precompile(tmpl);
    };

    this.render = function (tmpl, data) {

        if (navigator.Ie8) {
            return Handlebars.compile(tmpl)(data);
        }

        if (!_.isFunction(tmpl)) {
            tmpl = eval("(" + tmpl + ")");
        }
        return Handlebars.template(tmpl)(data);
    };

}

function IncDoTTemplate() {

    this.compile = function(tmpl) {
        return navigator.Ie8 ? tmpl
            : doT.compile(tmpl);
    };

    this.render = function(tmpl, data) {
        return doT.template(tmpl)(data);
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
                if (localStorage.remainingSpace < 2000) {
                    localStorage.clear();
                }
                localStorage.setItem(selectorKey, compile);
            }
            catch (e) {
                if (!ExecutableHelper.IsNullOrEmpty(e.name) && e.name.toUpperCase().indexOf('QUOTA') > -1) {
                    try {
                        localStorage.clear();
                    }
                    catch (e) {

                    }
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