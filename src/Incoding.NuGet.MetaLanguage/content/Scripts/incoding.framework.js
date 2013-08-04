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
            } else {
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

        var isPrimitiveType = (_.isNumber(selector) || _.isBoolean(selector) || _.isArray(selector) || _.isDate(selector) || _.isFunction(selector));
        var isObject = _.isObject(selector) && !_.isElement($(selector)[0]);
        if ((isPrimitiveType || isObject) && !_.isString(selector)) {
            return selector;
        }

        if (_.isString(selector)) {
            var isJqueryVariable = selector.toString().startsWith("$(") && selector.toString().endWith(")");
            selector = isJqueryVariable ? eval(selector) : selector.toString();
        }

        if (_.isString(selector)) {

            if (selector.contains("@@@@@@@")) {
                var options = $.parseJSON(selector.replaceAll("'@@@@@@@", '').replaceAll("@@@@@@@'", '').replaceAll("@@@@@@@", ''));
                if (!ExecutableHelper.IsNullOrEmpty(options.data)) {
                    options.data = $.parseJSON(options.data);
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

            if (selector.contains("@@@@@@")) {
                return $.cookie(selector.replaceAll("'@@@@@@", '').replaceAll("@@@@@@'", '').replaceAll("@@@@@@", ''));
            }

            if (selector.contains("@@@@@")) {
                var clearValue = selector.replaceAll("'@@@@@", '').replaceAll("@@@@@'", '').replaceAll("@@@@@", '');
                return $.url(window.location.href).fparam(clearValue.split(':')[0], clearValue.split(':')[1]);
            }

            if (selector.contains("@@@@")) {
                return $.url(window.location.href).furl(selector.replaceAll("'@@@@", '').replaceAll("@@@@'", '').replaceAll("@@@@", ''));
            }

            if (selector.contains("@@@")) {
                return $.url(window.location.href).param(selector.replaceAll("'@@@", '').replaceAll("@@@'", '').replaceAll("@@@", ''));
            }

            if (selector.contains("@@href@@")) {
                return document.location.href;
            }

        }

        try {
            if ($(selector).length === 0) {
                return selector;
            }
        } catch(e) {
            return selector;
        }

        return getValAction(selector);
    };

    this.TrySetValue = function(element, val) {

        if ($(element).is(':checkbox')) {
            var onlyCheckBoxes = $(element).filter(':checkbox');
            $(onlyCheckBoxes).prop('checked', false);
            if ($(onlyCheckBoxes).length == 1) {
                if (ExecutableHelper.ToBool(val)) {
                    $(onlyCheckBoxes).prop('checked', true);
                }
            } else {
                var arrayVal = _.isArray(val) ? val : val.split(',');
                $(onlyCheckBoxes).each(function() {
                    if (arrayVal.contains($(this).val())) {
                        $(this).prop('checked', true);
                    } else {
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
        } else {
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
    } else {
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
    var isNothing = _.isUndefined(value) || _.isNull(value) || _.isNaN(value);
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

ExecutableHelper.RedirectTo = function(detestationUrl) {

    var isSame = detestationUrl.contains('#') && window.location.hash.replace("#", "") == detestationUrl.split('#')[1];
    if (isSame) {
        $(document).trigger(jQuery.Event(IncSpecialBinds.IncChangeUrl));
        return;
    }

    window.location = detestationUrl;
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
    } else {
        return encode.call(value.toString());
    }
};

ExecutableHelper.ToBool = function(value) {

    if (ExecutableHelper.IsNullOrEmpty(value)) {
        return false;
    }

    if (value instanceof Boolean) {
        return this;
    }
    var toStringVal = value.toString().toLocaleLowerCase();

    return toStringVal === 'true';
};

ExecutableHelper.Guid = function() {
    var S4 = function() {
        return Math.floor(
            Math.random() * 0x10000 /* 65536 */
        ).toString(16);
    };

    return (
        S4() + S4() + "-" +
            S4() + "-" +
            S4() + "-" +
            S4() + "-" +
            S4() + S4() + S4()
    );
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
            } else if (this == 'Count' && _.isArray(currentData)) {
                value = currentData.length;
            } else if (_.isArray(currentData)) {
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
            } else if (currentData.hasOwnProperty(property)) {
                value = currentData[property];
            } else {
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
            } else {
                uri.attr['fragment'] = '';
                uri.param['fragment'] = {};
            }

            if (url.split('#')[0].contains('?')) {
                uri.param['query'] = this.parseString(url.split('#')[0].split('?')[1], '&');
            } else {
                uri.param['query'] = {};
            }

            // compile a 'base' domain attribute
            uri.attr['base'] = url.split("#")[0].contains("?") ? url.split("?")[0] : url;
            uri.attr['fullBase'] = url.contains("#") ? url.split("#")[0] : url;

            return uri;
        };

        this.promote = function(parent, key) {
            if (parent[key].length == 0) {
                return parent[key] = {};
            }
            var t = {};
            for (var i in parent[key]) {
                t[i] = parent[key][i];
            }
            parent[key] = t;
            return t;
        };

        this.parse = function(parts, parent, key, val) {
            var part = parts.shift();
            if (!part) {
                if (_.isArray(parent[key])) {
                    parent[key].push(val);
                } else if ('object' == typeof parent[key]) {
                    parent[key] = val;
                } else if ('undefined' == typeof parent[key]) {
                    parent[key] = val;
                } else {
                    parent[key] = [parent[key], val];
                }
            } else {
                var obj = parent[key] = parent[key] || [];
                if (']' == part) {
                    if (_.isArray(obj)) {
                        if ('' != val) {
                            obj.push(val);
                        }
                    } else if ('object' == typeof obj) {
                        obj[this.keys(obj).length] = val;
                    } else {
                        obj = parent[key] = [parent[key], val];
                    }
                } else if (~part.indexOf(']')) {
                    part = part.substr(0, part.length - 1);
                    if (!/^[0-9]+$/.test(part) && _.isArray(obj)) {
                        obj = promote(parent, key);
                    }
                    parse(parts, obj, part, val);
                    // key
                } else {
                    if (!/^[0-9]+$/.test(part) && _.isArray(obj)) {
                        obj = promote(parent, key);
                    }
                    parse(parts, obj, part, val);
                }
            }
        };

        this.merge = function(parent, key, val) {
            if (~key.indexOf(']')) {
                var parts = key.split('['),
                    len = parts.length,
                    last = len - 1;
                parse(parts, parent, 'base', val);
            } else {
                if (!/^[0-9]+$/.test(key) && _.isArray(parent.base)) {
                    var t = {};
                    for (var k in parent.base) {
                        t[k] = parent.base[k];
                    }
                    parent.base = t;
                }
                this.set(parent.base, key, val);
            }
            return parent;
        };

        this.parseString = function(str, charSplit) {
            var current = this;
            return this.reduce(String(str).split(charSplit), function(ret, pair) {
                try {
                    pair = ExecutableHelper.UrlDecode(pair.replace(/\+/g, ' '));
                } catch(e) {
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
            } else if (_.isArray(v)) {
                v.push(val);
            } else {
                obj[key] = [v, val];
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

        this.keys = function(obj) {
            var keys = [];
            $.eachProperties(obj, function() {
                keys.push(this);
            });
            return keys;
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

            var fullParam = "{0}__{1}".f(prefix, param);
            return ExecutableHelper.UrlDecode(this.data.param.fragment[fullParam]);
        },

        encodeAllParams : function() {
            var self = this;
            var params = self.fparam();
            $.eachProperties(params, function() {
                var key = this.split('__')[1];
                var prefix = this.split('__')[0];
                var value = params[this.toString()];
                self.setFparam(key, value, prefix);
            });
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
                if (_.isUndefined(this) || _.isEmpty(this)) {
                    return true;
                }
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
                } else {
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

            var currentUrl = this;

            var hash = '#!';

            var queryParams = currentUrl.fparam();

            $.each(currentUrl.fprefixes(), function() {

                var currentPrefix = this;

                if (currentPrefix != 'root') {
                    hash += "{0}:".f(currentPrefix);
                }

                if (!_.isEmpty(currentUrl.furl(currentPrefix))) {
                    hash += currentUrl.furl(currentPrefix) + '?';
                }

                $.eachProperties(queryParams, function() {
                    var prefixKey = currentPrefix + "__";
                    if (!this.contains(prefixKey)) {
                        return true;
                    }

                    var clearKey = this.replace(prefixKey, '');
                    if (_.isEmpty(clearKey)) {
                        return true;
                    }

                    hash += "{0}={1}/".f(clearKey, queryParams[this]);
                });

                if (hash.charAt(hash.length - 1) == '/') {
                    hash = hash.cutLastChar(); //cut redundant '/'/
                }
                hash += "&";
            });

            hash = hash.trim();
            hash = hash.cutLastChar(); //cut last symbol '&'

            if (hash.charAt(hash.length - 1) == '/') {
                hash = hash.cutLastChar(); //cut redundant '/'/
            }

            return currentUrl.url() + hash;
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
        if (!this.jquery) {
            return zMax;
        }

        return this.each(function() {
            zMax += def.inc;
            $(this).css({ "z-index" : zMax });
        });
    },

    toggleProp : function(key) {
        if (this.prop(key)) {
            $(this).prop(key, false);
        } else {
            $(this).prop(key, true);
        }
        return this;
    },
    toggleAttr : function(key) {
        if (this.attr(key)) {
            $(this).removeAttr(key);
        } else {
            this.attr(key, key);
        }
        return this;
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
    url : function(url) {
        return purl(url);
    }
});

//#region Jquery data selector

(function($) {
    var dataFn = $.fn.data;
    $.fn.data = function(key, val) {
        if (typeof val !== 'undefined') {
            $.expr.attrHandle[key] = function(elem) {
                return $(elem).prop(key) || $(elem).data(key);
            };
        }
        return dataFn.apply(this, arguments);
    };
})(jQuery);

//#endregion

//#region  Jquery cookies

(function($, document) {

    var pluses = /\+/g;

    function raw(s) {
        return s;
    }

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
                encodeURIComponent(key), '=', config.raw ? value : encodeURIComponent(value),
                options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
                options.path ? '; path=' + options.path : '',
                options.domain ? '; domain=' + options.domain : '',
                options.secure ? '; secure' : ''
            ].join(''));
        }

        // read
        var decode = config.raw ? raw : decoded;
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

//#endregion﻿"use strict";

//#region class IncodingInstrumentation

function DefaultIncodingInstrumentation() {

}

DefaultIncodingInstrumentation.prototype =
    {
        fireParse : function(context) {
            console.log('Found element {0} with count meta {1}'.f(context.element, context.countMeta));
        },

        fireInvoke : function(context) {
            /*console.log('Invoke {0} for {1}'.f(context.event, context.element));*/
        },

        fireInitIncoding : function(context) {
            /*console.log('Init incoding for {0}'.f(context.element));*/
        },

        fireUnHandleException : function(e) {
            /*console.error('Ajax un handle exception {0}'.f(e));*/
        },

        fireAjaxBeforeSend : function(jqXhr, settings) {
            console.log('Ajax before send:');
            console.log('xhr {0}'.f(jqXhr));
            console.log('settings {0}'.f(settings));
        },

        fireAjaxComplete : function(jqXhr, textStatus) {
            console.log('Ajax complete');
            console.log('xhr {0}'.f(jqXhr));
            console.log('status {0}'.f(textStatus));
        },

        fireAjaxSuccess : function(data) {
            console.log('Ajax Success:');
            console.log(data);
        },

        fireAjaxAfterParse : function(parseData) {
            console.log('Ajax parse data:');
            console.log(parseData);
        },

        fireAjaxError : function(event, jqXhr, ajaxSettings, thrownError) {
            console.log('Ajax error:');
            console.log(event);
            console.log(jqXhr);
            console.log(ajaxSettings);
            console.log(thrownError);
        },

        fireAjaxSubmitSuccess : function(responseText, statusText, xhr, $form) {
            console.log('Ajax submit success:');
            console.log(responseText);
            console.log(statusText);
            console.log(xhr);
            console.log($form);
        },

        fireAjaxSubmitBefore : function(formData, jqForm, options) {
            console.log('Ajax submit before:');
            console.log(formData);
            console.log(jqForm);
            console.log(options);
        }
    };

DefaultIncodingInstrumentation.Instance = new DefaultIncodingInstrumentation();

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

    this.IsNew = function() {
        return !ExecutableHelper.IsNullOrEmpty(this.executables) && ExecutableHelper.IsNullOrEmpty(this.runner);
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
    DoIt : function(e, result) {

        var current = this;
        var currentEventName = e.type;

        var filterFunc = function(r) {

            var currentBind = $.trim(r.onBind);

            var isHas = false;
            $.each(currentBind.split(' '),
                function() {
                    if (this == currentEventName) {
                        isHas = true;
                        return;
                    }
                    ;
                });

            return isHas;
        };

        try {
            $($.grep(this.before, filterFunc)).each(function() {
                this.execute();
            });
        } catch(e) {

            if (e instanceof IncClientException) {
                $($.grep(this.breakes, filterFunc)).each(function() {
                    this.execute(e);
                });
                return;
            }
            new DefaultIncodingInstrumentation().fireUnHandleException(e);
            throw e;
        }

        $($.grep(this.actions, filterFunc)).each(function() {
            this.execute(
                {
                    success : $.grep(current.success, filterFunc),
                    error : $.grep(current.error, filterFunc),
                    complete : $.grep(current.complete, filterFunc),
                    breakes : $.grep(current.breakes, filterFunc),
                    eventResult : result
                });
        });
    },

    Registry : function(metaType, onStatus, instance) {

        if (metaType.contains('Action')) {
            this.actions.push(instance);
        } else {

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
        } catch(e) {
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

IncodingResult.Empty = new IncodingResult({ data : '', redirectTo : '', success : true });

//#endregion

//#region class IncodingEngine

function IncodingEngine() {

    this.parse = function(context) {

        var allIncodingElements = $(context).add($('*', context)).filter(function() {
            return new IncodingMetaElement(this).IsNew();
        });

        $(allIncodingElements).each(function() {

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

                bindName = bindName
                    .toString()
                    .replaceAll(IncSpecialBinds.InitIncoding, '').trim()
                    .toString();

                var onEventStatus = ExecutableHelper.IsNullOrEmpty(metaData.data.onEventStatus) ? '1' : metaData.data.onEventStatus;
                incodingMetaElement.bind(bindName, onEventStatus.toString());
            });
            incodingMetaElement.flushRunner(runner);

            var hasInitIncoding = $.grep(wasAddBinds, function(r) {
                return r.contains(IncSpecialBinds.InitIncoding);
            }).length != 0;

            if (hasInitIncoding) {
                new  IncodingMetaElement(this).invoke(jQuery.Event(IncSpecialBinds.InitIncoding));
            }

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

    if (ExecutableHelper.IsNullOrEmpty(json.target)) {
        executable.getTarget = function() {
            return this.self;
        };
    } else {
        executable.getTarget = function() {
            return eval(json.target);
        };
    }
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
        throw new Error('Need override this method');
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

        if (ExecutableHelper.IsNullOrEmpty(variable)) {
            return ExecutableHelper.Instance.TryGetVal(variable);
        }

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
                this.execute(resultData);
            } catch(e) {
                if (e instanceof IncClientException) {
                    hasBreak = true;
                }
                return false;
            }
        };

        if (result.success) {
            $(state.success).each(executeState);
        } else {
            $(state.error).each(executeState);
        }
        $(state.complete).each(executeState);
        if (hasBreak) {
            $(state.breakes).each(function() {
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

    var ajaxOptions = {};
    $.extend(ajaxOptions, this.jsonData.ajax);

    if (ExecutableHelper.IsNullOrEmpty(ajaxOptions.data)) {
        ajaxOptions.data = [];
    } else {
        ajaxOptions.data = $.parseJSON(ajaxOptions.data);
        for (var i = 0; i < ajaxOptions.data.length; i++) {
            ajaxOptions.data[i].selector = this.tryGetVal(ajaxOptions.data[i].selector);
        }
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

    var ajaxOptions = {};
    $.extend(ajaxOptions, this.jsonData.options);

    var current = this;

    ajaxOptions.success = function(responseText, statusText, xhr, $form) {
        var result = new IncodingResult(responseText);
        current.complete(result, data);
    };

    var formSelector = eval(this.jsonData.formSelector);
    var form = $(formSelector).is('form') ? formSelector : $(formSelector).closest('form').first();

    ajaxOptions.beforeSubmit = function(formData, jqForm, options) {
        return $(form).valid();
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
        insertContent = TemplateFactory.Create('Mustache', insertContent, this.tryGetVal(templateId)).render();
    }

    eval("$(current.target).{0}(insertContent.toString())".f(current.jsonData.insertType));

    IncodingEngine.Current.parse(current.target);
    $(document).trigger(jQuery.Event(IncSpecialBinds.IncInsert));
};
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

    $(data).each(function() {

        var name = this.name.toString();
        var input = $('[name]', this.target).filter(function() {
            return $(this).prop('name').toLowerCase() == name.toLowerCase();
        });
        var span = $('[data-valmsg-for]', this.target).filter(function() {
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
        } else {
            $(input).addClass(inputErrorClass);
            $(span)
                .removeClass(messageValidClass)
                .addClass(messageErrorClass)
                .text(this.errorMessage);
        }

    });
};

//#endregion

//#region class ExecutableRedirect extend from ExecutableBase

incodingExtend(ExecutableRedirect, ExecutableBase);

function ExecutableRedirect() {
}

ExecutableRedirect.prototype.internalExecute = function() {
    var url = ExecutableHelper.IsNullOrEmpty(this.jsonData.redirectTo) ? document.location.href : this.jsonData.redirectTo;
    ExecutableHelper.RedirectTo(url);
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

    var setFurl = function(input) {
        var name = $(input).prop('name');

        if (ExecutableHelper.IsNullOrEmpty(name)) {
            return;
        }

        var value = ExecutableHelper.Instance.TryGetVal('[name={0}]'.f(name));
        if (ExecutableHelper.IsNullOrEmpty(value)) {
            url.removeFparam(name, prefix);
        } else {
            url.setFparam(name, value, prefix);
        }
    };

    var inputSelector = 'input,select';

    if ($(this.target).is(inputSelector)) {
        setFurl(this.target);
    } else {
        $(this.target)
            .find('input,select')
            .not('[type="submit"],[type="reset"],[type="button"]')
            .each(function() {
                setFurl(this);
            });
    }

    ExecutableHelper.RedirectTo(url.toHref());
};

//#endregion

//#region class ExecutableStoreFetch extend from ExecutableBase

incodingExtend(ExecutableStoreFetch, ExecutableBase);

function ExecutableStoreFetch() {
}

ExecutableStoreFetch.prototype.internalExecute = function() {

    var current = this;
    var prefix = this.jsonData.prefix + "__";

    var fparam = $.url(window.location.href).fparam();
    $.eachProperties(fparam, function() {
        if (!this.contains(prefix)) {
            return true;
        }

        var selector = '[name={0}]'.f(this.replace(prefix, ''));
        var value = fparam[this];
        $(current.target).each(function() {
            var element = $(this).is(selector) ? this : $(selector, this);
            if ($(element).length != 0) {
                ExecutableHelper.Instance.TrySetValue(element, value);
            }
        });

        return true;
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
            url.encodeAllParams();

            var methods = $.parseJSON(current.jsonData.methods);
            $(methods).each(function() {
                if (this.verb == 'remove') {
                    url.removeFparam(this.key, this.prefix);
                } else {
                    url.setFparam(this.key, this.value, this.prefix);
                }
            });

            ExecutableHelper.RedirectTo(url.toHref());
            break;
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
        default:
            throw 'argument of range {0}'.f(method);
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
            } else {
                $(this.target).unbind(this.jsonData.bind);
            }
            break;
        default:
            throw "Argument out of range {0}".f(type);
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
            throw new Error('Need override this method');
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