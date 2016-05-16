/*!
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
        log: function () {
        },
        error: function () {

        },
        dir: function () {
        }
    };
}

document.setTitle = function(value) {
    document.title = value;
};



function InitGps() {
    if (navigator.geolocation && !navigator.geolocation.currentPosition) {
        navigator.geolocation.currentPosition = { longitude: 0, latitude: 0 };
        navigator.geolocation.getCurrentPosition(function (position) {
            navigator.geolocation.currentPosition = position.coords;
        }, function () {
        });
    }
}


$.extend(String.prototype, {
    replaceAll: function (find, replace) {
        return this.split(find).join(replace);
    },
    endWith: function (suffix) {
        return (this.substr(this.length - suffix.length) === suffix);
    },
    toSelectorAsName: function () {
        var name = this;
        if (ExecutableHelper.IsNullOrEmpty(name)) {
            name = '';
        }
       return  '[name="{0}"]'.f(name.toString()
            .replaceAll("[", "\\[")
            .replaceAll("]", "\\]"));        
    },
    startsWith: function (prefix) {
        return (this.substr(0, prefix.length) === prefix);
    },
    trim: function () {
        return this.replace(/^\s\s*|\s\s*$/g, '');
    },
    ltrim: function () {
        return this.replace(/^\s+/, "");
    },
    rtrim: function () {
        return this.replace(/\s+$/, "");
    },
    cutLastChar: function () {
        return this.substring(0, this.length - 1);
    },
    contains: function (find) {
        return this.indexOf(find) !== -1;
    },
    f: function () {
        var s = this;
        var i = arguments.length;

        while (i--) {
            s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
        }

        return s;
    }
});

$.extend(Array.prototype, {
    contains: function (findValue) {
        return $.inArray(findValue, this) != -1;
    },
    remove: function (from, to) {
        var rest = this.slice((to || from) + 1 || this.length);
        this.length = from < 0 ? this.length + from : from;
        return this.push.apply(this, rest);
    },
    sum: function () {
        var sum = 0;
        $(this).each(function () {
            sum += parseFloat(this);
        });
        return sum;
    },
    max: function () {
        return this.length > 0 ? Math.max.apply(Math, this) : 0;
    },
    min: function () {
        return this.length > 0 ? Math.min.apply(Math, this) : 0;
    },
    average: function () {
        return this.length > 0 ? this.sum() / this.length : 0;
    },
    first: function () {
        return this.length > 0 ? this[0] : '';
    },
    last: function () {
        return this.length > 0 ? this[this.length - 1] : '';
    },
    count: function () {
        return this.length;
    },
    select: function (convert) {
        var res = [];
        $(this).each(function () {
            res.push(convert(this));
        });
        return res;
    }
});

function incodingExtend(child, parent) {
    var f = function () {
    };
    f.prototype = parent.prototype;
    child.prototype = new f();
    child.prototype.constructor = child;
    child.superclass = parent.prototype;
}

$.fn.extend({
    maxZIndex: function (opt) {
        var def = { inc: 10, group: "*" };
        $.extend(def, opt);
        var zMax = 0;
        $(def.group).each(function () {
            var cur = parseInt($(this).css('z-index'));
            zMax = cur > zMax ? cur : zMax;
        });

        return this.each(function () {
            zMax += def.inc;
            $(this).css({ "z-index": zMax });
        });
    },

    toggleProp: function (key) {
        if (this.prop(key)) {
            $(this).prop(key, false);
        }
        else {
            $(this).prop(key, true);
        }
        return this;
    },
    toggleAttr: function (key) {
        if (this.attr(key)) {
            $(this).removeAttr(key);
        }
        else {
            this.attr(key, key);
        }
        return this;
    },
    increment: function (step) {
        return $(this).each(function () {
            var val = parseInt($(this).val());
            $(this).val((_.isNaN(val) ? 0 : val) + parseInt(step));
        });
    },
    isFormElement: function () {
        return $(this).is('select,textarea,input');
    },

});

$.extend({
    eachProperties: function (ob, evaluated) {
        var i = 0;
        for (var property in ob) {
            var isValid = !_.isUndefined(property) || !_.isEmpty(property);
            if (isValid && ob.hasOwnProperty(property)) {
                evaluated.call(property, i);
            }
            i++;
        }
    },    
    eachFormElements: function (ob, evaluated) {
        var inputTag = 'input,select,textarea';
        var ignoreInputType = '[type="submit"],[type="reset"],[type="button"]';
        var targets;
        if ($(ob).is('input,select')) {
            targets = ob.length > 1 ? ob : [ob];
        }
        else {
            var allInput = $(ob).find(inputTag).not(ignoreInputType);
            targets = allInput.length > 1 ? allInput : [allInput];
        }

        $(targets).each(function() {
            var name = $(this).prop('name');

            if (ExecutableHelper.IsNullOrEmpty(name)) {
                return;
            }

            var isElement = ob.length === 1 && ob.isFormElement();
            if (isElement) {
                evaluated.call(ob);
            }
            else {
                var byFind = ob.find(name.toSelectorAsName());
                evaluated.call(byFind.length !== 0 ? byFind : ob.filter(name.toSelectorAsName()));
            }
        });
        
    },
    url: function (url) {
        return purl(url);
    }
});

//#region  Jquery cookies

(function ($, document) {

    var pluses = /\+/g;

    function decoded(s) {
        return decodeURIComponent(s.replace(pluses, ' '));
    }

    var config = $.cookie = function (key, value, options) {

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

    $.removeCookie = function (key, options) {
        if ($.cookie(key) !== null) {
            $.cookie(key, null, options);
            return true;
        }
        return false;
    };

})(jQuery, document);

navigator.Ie8 = (function () {
    var N = navigator.appName, ua = navigator.userAgent, tem;
    var M = ua.match(/(opera|chrome|safari|firefox|msie)\/?\s*(\.?\d+(\.\d+)*)/i);
    if (M && (tem = ua.match(/version\/([\.\d]+)/i)) != null) {
        M[2] = tem[1];
    }
    M = M ? [M[1], M[2]] : [N, navigator.appVersion, '-?'];
    return M.contains('MSIE') && M.contains('8.0');
})();



//#endregion