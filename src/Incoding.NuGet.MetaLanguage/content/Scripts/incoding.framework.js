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

//#endregion﻿"use strict";

function purl(existsUrl) {

    function urlParser() {

        this.parseUri = function(url) {

            var uri = { attr : {}, param : {} };

            if (url.contains('#')) {
                uri.attr['fragment'] = url.split('#')[1].replace("!", "");

                var current = this;
                var fparams = {};
                $.each(uri.attr['fragment'].split('&'), function() {

                    var prefix = this.contains(":") ? this.split(':')[0] : 'root';
                    var fragmentQuery = this.contains("?") ? this.split('?')[1] : this;
                    fragmentQuery = fragmentQuery.replace(prefix + ':', '');

                    var paramsByPrefix = current.parseString(fragmentQuery, '/');
                    $.eachProperties(paramsByPrefix, function() {
                        var fullKey = "{0}__{1}".f(prefix, this);
                        fparams[fullKey] = paramsByPrefix[this];
                    });

                });
                uri.param['fragment'] = fparams;
            }
            else {
                uri.attr['fragment'] = '';
                uri.param['fragment'] = {};
            }

            if (url.split('#')[0].contains('?')) {
                uri.param['query'] = this.parseString(url.split('#')[0].split('?')[1], '&');
            }
            else {
                uri.param['query'] = {};
            }

            // compile a 'base' domain attribute
            uri.attr['base'] = url.split("#")[0].contains("?") ? url.split("?")[0] : url;
            uri.attr['fullBase'] = url.contains("#") ? url.split("#")[0] : url;

            return uri;
        };

        this.merge = function(parent, key, val) {
            this.set(parent.base, key, val);
            return parent;
        };

        this.parseString = function(str, charSplit) {
            var current = this;
            return this.reduce(String(str).split(charSplit), function(ret, pair) {
                try {
                    pair = ExecutableHelper.UrlDecode(pair.replace(/\+/g, ' '));
                }
                catch(e) {
                    // ignore
                }
                var eql = pair.indexOf('='),
                    brace = current.lastBraceInKey(pair),
                    key = pair.substr(0, brace || eql),
                    val = pair.substr(brace || eql, pair.length),
                    val = val.substr(val.indexOf('=') + 1, val.length);

                if ('' == key) {
                    key = pair, val = '';
                }

                return current.merge(ret, key, val);
            }, { base : {} }).base;
        };

        this.set = function(obj, key, val) {
            var v = obj[key];
            if (undefined === v) {
                obj[key] = val;
            }           
        };

        this.lastBraceInKey = function(str) {
            var len = str.length,
                brace, c;
            for (var i = 0; i < len; ++i) {
                c = str[i];
                if (']' == c) {
                    brace = false;
                }
                if ('[' == c) {
                    brace = true;
                }
                if ('=' == c && !brace) {
                    return i;
                }
            }
        };

        this.reduce = function(obj, accumulator) {
            var i = 0,
                l = obj.length >> 0,
                curr = arguments[2];
            while (i < l) {
                if (i in obj) {
                    curr = accumulator.call(undefined, curr, obj[i], i, obj);
                }
                ++i;
            }
            return curr;
        };

    }

    return {
        data : new urlParser().parseUri(existsUrl || window.location.toString()),

        // get various attributes from the URI
        attr : function(attr) {
            attr = { 'anchor' : 'fragment' }[attr] || attr;
            return typeof attr !== 'undefined' ? this.data.attr[attr] : this.data.attr;
        },

        // return query string parameters
        param : function(param) {
            return typeof param !== 'undefined' ? this.data.param.query[param] : this.data.param.query;
        },
        // return fragment parameters
        fparam : function(param, prefix) {

            if (arguments.length == 0) {
                return this.data.param.fragment;
            }
            var key = "{0}__{1}".f(prefix, param);
            return this.data.param.fragment.hasOwnProperty(key) ? ExecutableHelper.UrlDecode(this.data.param.fragment[key]) : '';
        },
        
        // set fragment parameters
        setFparam : function(param, value, prefix) {
            var fullParam = "{0}__{1}".f(prefix, param);
            return this.data.param.fragment[fullParam] = ExecutableHelper.UrlEncode(value);
        }, 
        
        // set fragment parameters
        removeFparam : function(param, prefix) {
            var fullParam = "{0}__{1}".f(prefix, param);
            if (this.data.param.fragment.hasOwnProperty(fullParam)) {
                delete this.data.param.fragment[fullParam];
            }
        },

        // clear fragment parameters
        clearFparam : function() {
            return this.data.param.fragment = [];
        },

        fprefixes : function() {
            var uniquePrefixes = ['root'];
            $.eachProperties(this.fparam(), function() {                
                var prefixKey = this.split('__')[0];
                if (uniquePrefixes.contains(prefixKey)) {
                    return true;
                }
                uniquePrefixes.push(prefixKey);
            });
            return uniquePrefixes;
        },

        furl : function(prefix) {

            var urls = { root : '' };

            var allUrls = this.data.attr['fragment'].contains("&") ? this.data.attr['fragment'].split('&') : [this.data.attr['fragment']];
            $(allUrls).each(function() {
                if (this.contains(':')) {
                    var splitByPrefix = this.split(':');
                    urls[splitByPrefix[0]] = splitByPrefix[1];
                }
                else {
                    urls.root = this;
                }
            });

            var resultUrl = urls[prefix];
            if (_.isUndefined(resultUrl)) {
                return '';
            }
            return resultUrl.contains("?") ? resultUrl.split("?")[0] : '';
        },

        setFurl : function(value) {
            var clearValue = value.contains('?') ? value : value + "?";
            this.data.attr['fragment'] = clearValue;
        },

        url : function() {
            return this.data.attr['fullBase'];
        },

        toHref : function() {

            var current = this;

            var hash = '#!';

            $.each(current.fprefixes(), function() {

                var currentPrefix = this;
                if (currentPrefix != 'root') {
                    hash += "{0}:".f(currentPrefix);
                }
                if (!_.isEmpty(current.furl(currentPrefix))) {
                    hash += current.furl(currentPrefix) + '?';
                }

                var queryParams = current.fparam();
                $.eachProperties(queryParams, function() {
                    var prefixKey = currentPrefix + "__";
                    if (!this.contains(prefixKey)) {
                        return true;
                    }

                    var clearKey = this.replace(prefixKey, '');
                    if (_.isEmpty(clearKey)) {
                        return true;
                    }

                    hash += "{0}={1}/".f(clearKey, ExecutableHelper.UrlEncode(queryParams[this]));
                });

                if (hash.charAt(hash.length - 1) == '/') {
                    hash = hash.cutLastChar(); //cut redundant '/'/
                }
                hash += "&";
            });
            
            return current.url() + hash.trim().cutLastChar();
        }
    };

}﻿/*!
 * Incoding framework v 1.0.252.1122
 * http://incframework.com
 *
 * Copyright 2013, 2013 Incoding software
 * Apache License
 * Version 2.0, January 2004
 * http://www.apache.org/licenses/ 
 */

"use strict";

if (!window.console) {
    window.console = {
        log : function() {
        },
        error : function() {

        },
        dir : function() {
        }
    };
}

function InitGps() {
    if (navigator.geolocation && !navigator.geolocation.currentPosition) {
        navigator.geolocation.currentPosition = { longitude : 0, latitude : 0 };
        navigator.geolocation.getCurrentPosition(function(position) {
            navigator.geolocation.currentPosition = position.coords;
        }, function() {
        });
    }
}

$.extend(String.prototype, {
    replaceAll : function(find, replace) {
        return this.split(find).join(replace);
    },
    endWith : function(suffix) {
        return (this.substr(this.length - suffix.length) === suffix);
    },
    startsWith : function(prefix) {
        return (this.substr(0, prefix.length) === prefix);
    },
    trim : function() {
        return this.replace(/^\s\s*|\s\s*$/g, '');
    },
    ltrim : function() {
        return this.replace(/^\s+/, "");
    },
    rtrim : function() {
        return this.replace(/\s+$/, "");
    },
    cutLastChar : function() {
        return this.substring(0, this.length - 1);
    },
    contains : function(find) {
        return this.indexOf(find) !== -1;
    },

    f : function() {
        var s = this;
        var i = arguments.length;

        while (i--) {
            s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
        }

        return s;
    }
});

$.extend(Array.prototype, {
    contains : function(findValue) {
        return $.inArray(findValue, this) != -1;
    },
    remove : function(from, to) {
        var rest = this.slice((to || from) + 1 || this.length);
        this.length = from < 0 ? this.length + from : from;
        return this.push.apply(this, rest);
    },
    sum : function() {
        var sum = 0;
        $(this).each(function() {
            sum += parseFloat(this);
        });
        return sum;
    },
    max : function() {
        return this.length > 0 ? Math.max.apply(Math, this) : 0;
    },
    min : function() {
        return this.length > 0 ? Math.min.apply(Math, this) : 0;
    },
    average : function() {
        return this.length > 0 ? this.sum() / this.length : 0;
    },
    first : function() {
        return this.length > 0 ? this[0] : '';
    },
    last : function() {
        return this.length > 0 ? this[this.length - 1] : '';
    },
    count : function() {
        return this.length;
    },
    select : function(convert) {
        var res = [];
        $(this).each(function() {
            res.push(convert(this));
        });
        return res;
    }
});

function incodingExtend(child, parent) {
    var f = function() {
    };
    f.prototype = parent.prototype;
    child.prototype = new f();
    child.prototype.constructor = child;
    child.superclass = parent.prototype;
}

$.fn.extend({
    maxZIndex : function(opt) {
        var def = { inc : 10, group : "*" };
        $.extend(def, opt);
        var zMax = 0;
        $(def.group).each(function() {
            var cur = parseInt($(this).css('z-index'));
            zMax = cur > zMax ? cur : zMax;
        });

        return this.each(function() {
            zMax += def.inc;
            $(this).css({ "z-index" : zMax });
        });
    },

    toggleProp : function(key) {
        if (this.prop(key)) {
            $(this).prop(key, false);
        }
        else {
            $(this).prop(key, true);
        }
        return this;
    },
    toggleAttr : function(key) {
        if (this.attr(key)) {
            $(this).removeAttr(key);
        }
        else {
            this.attr(key, key);
        }
        return this;
    },
    increment : function(step) {
        return $(this).each(function() {
            var val = parseInt($(this).val());
            $(this).val((_.isNaN(val) ? 0 : val) + parseInt(step));
        });
    }
});

$.extend({
    eachProperties : function(ob, evaluated) {
        for (var property in ob) {
            var isValid = !_.isUndefined(property) || !_.isEmpty(property);
            if (isValid && ob.hasOwnProperty(property)) {
                evaluated.call(property);
            }
        }
    },
    byName : function(name, nextSelector) {
        if (ExecutableHelper.IsNullOrEmpty(name)) {
            name = '';
        }
        var selectorByName = '[name="{0}"]'.f(name.toString()
            .replaceAll("[", "\\[")
            .replaceAll("]", "\\]"));
        if (ExecutableHelper.IsNullOrEmpty(nextSelector)) {
            return $(selectorByName);
        }
        else {
            return $(selectorByName + nextSelector);
        }
    },
    eachFormElements : function(ob, evaluated) {
        var inputTag = 'input,select';
        var ignoreInputType = '[type="submit"],[type="reset"],[type="button"]';
        var targets;
        if ($(ob).is('input,select')) {
            targets = ob.length > 1 ? ob : [ob];
        }
        else {
            var allInput = $(ob).find(inputTag).not(ignoreInputType);
            targets = allInput.length > 1 ? allInput : [allInput];
        }

        $(targets).each(evaluated);
    },
    url : function(url) {
        return purl(url);
    }
});

//#region  Jquery cookies

(function($, document) {

    var pluses = /\+/g;

    function decoded(s) {
        return decodeURIComponent(s.replace(pluses, ' '));
    }

    var config = $.cookie = function(key, value, options) {

        // write
        if (value !== undefined) {
            options = $.extend({}, config.defaults, options);

            if (value === null) {
                options.expires = -1;
            }

            if (typeof options.expires === 'number') {
                var days = options.expires, t = options.expires = new Date();
                t.setDate(t.getDate() + days);
            }

            value = config.json ? JSON.stringify(value) : String(value);

            return (document.cookie = [
                encodeURIComponent(key), '=', encodeURIComponent(value),
                options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
                options.path ? '; path=' + options.path : '',
                options.domain ? '; domain=' + options.domain : '',
                options.secure ? '; secure' : ''
            ].join(''));
        }

        // read
        var decode = decoded;
        var cookies = document.cookie.split('; ');
        for (var i = 0, l = cookies.length; i < l; i++) {
            var parts = cookies[i].split('=');
            if (decode(parts.shift()) === key) {
                var cookie = decode(parts.join('='));
                return config.json ? JSON.parse(cookie) : cookie;
            }
        }

        return null;
    };

    config.defaults = {};

    $.removeCookie = function(key, options) {
        if ($.cookie(key) !== null) {
            $.cookie(key, null, options);
            return true;
        }
        return false;
    };

})(jQuery, document);

navigator.Ie8 = (function() {
    var N = navigator.appName, ua = navigator.userAgent, tem;
    var M = ua.match(/(opera|chrome|safari|firefox|msie)\/?\s*(\.?\d+(\.\d+)*)/i);
    if (M && (tem = ua.match(/version\/([\.\d]+)/i)) != null) {
        M[2] = tem[1];
    }
    M = M ? [M[1], M[2]] : [N, navigator.appVersion, '-?'];
    return M.contains('MSIE') && M.contains('8.0');
})();



//#endregion﻿"use strict";

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

//#endregion﻿"use strict";

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
    this.event = '';
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
        ExecutableHelper.Instance.event = this.event;
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
                    return false;//stop execute
                }

                console.log('Incoding exception: {0}'.f(e.message ? e.messsage : e));
                if (navigator.Ie8) {
                    return false;//stop execute
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
    var formSelector = eval(this.jsonData.formSelector);
    var form = $(formSelector).is('form') ? formSelector : $(formSelector).closest('form').first();

    var ajaxOptions = $.extend(true, {
        error : function(error) {
            $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxError), IncodingResult.Success(IncAjaxEvent.Create(error)));
            $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxComplete), IncodingResult.Success(IncAjaxEvent.Create(error)));
        },
        success : function(responseText, statusText, xhr, $form) {
            $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxSuccess), IncodingResult.Success(IncAjaxEvent.Create(xhr)));                        
            $(document).trigger(jQuery.Event(IncSpecialBinds.IncAjaxComplete), IncodingResult.Success(IncAjaxEvent.Create(xhr)));
            current.complete(new IncodingResult(responseText), data);
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

    if (!ExecutableHelper.IsNullOrEmpty(this.jsonData.options.data)) {
        for (var i = 0; i < this.jsonData.options.data.length; i++) {
            ajaxOptions.data[i].value = this.tryGetVal(this.jsonData.options.data[i].value);
        }
    }

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

        var value = ExecutableHelper.Instance.TryGetVal($.byName(name));
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

//#endregion﻿"use strict";

//#region class ConditionalFactory

function ConditionalFactory() {
}

// ReSharper disable UnusedParameter
ConditionalFactory.Create = function(data, executable) {
    var conditional = eval('new ' + 'Conditional' + data.type + '();');
    conditional.jsonData = data;
    conditional.executable = executable;
    return conditional;
};
// ReSharper restore UnusedParameter

//#endregion

//#region class ConditionalBase

function ConditionalBase() {
    this.jsonData = '';
    this.executable = '';
    this.self = '';
    this.target = '';
}

ConditionalBase.prototype =
    {
        isSatisfied : function(data) {
            this.self = this.executable.self;
            this.target = this.executable.getTarget();
            var isSatisfied = this.isInternalSatisfied(data);
            return ExecutableHelper.ToBool(this.jsonData.inverse) ? !isSatisfied : isSatisfied;
        },
        // ReSharper disable UnusedParameter
        isInternalSatisfied : function(data) {
        // ReSharper restore UnusedParameter            
        },
        tryGetVal : function(variable) {
            return this.executable.tryGetVal(variable);
        }
    };

//#endregion

//#region class ConditionalData  extend from ConditionalBase

incodingExtend(ConditionalData, ConditionalBase);

function ConditionalData() {
}

ConditionalData.prototype.isInternalSatisfied = function(data) {

    var expectedVal = this.tryGetVal(this.jsonData.value);
    var method = this.jsonData.method;

    return ExecutableHelper.IsData(data, this.jsonData.property, function() {
        return ExecutableHelper.Compare(this, expectedVal, method);
    });
};

//#endregion

//#region class ConditionalDataIsId extend from ConditionalBase

incodingExtend(ConditionalDataIsId, ConditionalBase);

function ConditionalDataIsId() {
}

ConditionalDataIsId.prototype.isInternalSatisfied = function(data) {
    return ExecutableHelper.IsData(data, this.jsonData.property, function() {
        return $('#' + this).length > 0;
    });
};

//#endregion

//#region class ConditionalEval extend from ConditionalBase

incodingExtend(ConditionalEval, ConditionalBase);

function ConditionalEval() {
}

// ReSharper disable UnusedParameter
ConditionalEval.prototype.isInternalSatisfied = function(data) {
    return eval(this.jsonData.code);
};
// ReSharper restore UnusedParameter

//#endregion


//#region class ConditionalIs extend from ConditionalBase

incodingExtend(ConditionalIs, ConditionalBase);

function ConditionalIs() {
}

// ReSharper disable UnusedParameter
ConditionalIs.prototype.isInternalSatisfied = function(data) {
    var left = this.tryGetVal(this.jsonData.left);
    var right = this.tryGetVal(this.jsonData.right);

    return ExecutableHelper.Compare(left, right, this.jsonData.method);
};
// ReSharper restore UnusedParameter

//#endregion