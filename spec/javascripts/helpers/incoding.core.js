"use strict";

if (!window.console)
    window.console = {
        log: function() {
        },
        dir: function() {
        }
    };

$.extend(String.prototype, {
    replaceAll: function(find, replace) {
        return this.split(find).join(replace);
    },

    endWith: function(suffix) {
        return (this.substr(this.length - suffix.length) === suffix);
    },

    startsWith: function(prefix) {
        return (this.substr(0, prefix.length) === prefix);
    },

    trim: function() {
        return this.replace(/^\s\s*|\s\s*$/g, '');
    },
    ltrim: function() {
        return this.replace(/^\s+/, "");
    },
    rtrim: function() {
        return this.replace(/\s+$/, "");
    },
    contains: function(find) {
        return this.indexOf(find) !== -1;
    },

    applyFormat: function() {
        var s = this;
        var i = arguments.length;

        while (i--)
            s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);

        return s;
    },

    f: function() {
        var s = this;
        var i = arguments.length;

        while (i--)
            s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);

        return s;
    },

    getQueryString: function() {

        var params = this.substr(this.indexOf("?") + 1);
        var sval = "";
        return params.split("&");

    }
});

$.extend(Date.prototype, {
    asString: function(withTime) {

        function zeroPad(num) {
            var s = '0' + num;
            return s.substring(s.length - 2);
        }

        var date = [zeroPad(this.getMonth() + 1), zeroPad(this.getDate()), this.getFullYear()].join('/');
        if (withTime) {
            var h = this.getHours();
            var time = ' ' + [zeroPad(h > 12 ? h % 12 : h), zeroPad(this.getMinutes())].join(':');
            time += (h >= 12) ? ' PM' : ' AM';
            date += time;
        }

        return date;
    }
});

$.extend(Array.prototype, {
    contains: function(findValue) {
        return $.inArray(findValue, this) != -1;
    },
    getIndex: function(findValue) {
        return $.inArray(findValue, this);
    },
    remove: function(from, to) {
        var rest = this.slice((to || from) + 1 || this.length);
        this.length = from < 0 ? this.length + from : from;
        return this.push.apply(this, rest);
    },
    removeOne: function(index) {
        this.remove(index, index);
    }
});

function incodingExtend(Child, Parent) {
    var F = function() {
    };
    F.prototype = Parent.prototype;
    Child.prototype = new F();
    Child.prototype.constructor = Child;
    Child.superclass = Parent.prototype;
}

//#region Jquery data selector

(function($) {
    var _dataFn = $.fn.data;
    $.fn.data = function(key, val) {
        if (typeof val !== 'undefined')
            $.expr.attrHandle[key] = function(elem) {
                return $(elem).attr(key) || $(elem).data(key);
            };
        return _dataFn.apply(this, arguments);
    };
})(jQuery);

//#endregion

$.maxZIndex = $.fn.maxZIndex = function(opt) {
    /// <summary>
    /// Returns the max zOrder in the document (no parameter)
    /// Sets max zOrder by passing a non-zero number
    /// which gets added to the highest zOrder.
    /// </summary>    
    /// <param name="opt" type="object">
    /// inc: increment value, 
    /// group: selector for zIndex elements to find max for
    /// </param>
    /// <returns type="jQuery" />
    var def = { inc: 10, group: "*" };
    $.extend(def, opt);
    var zmax = 0;
    $(def.group).each(function() {
        var cur = parseInt($(this).css('z-index'));
        zmax = cur > zmax ? cur : zmax;
    });
    if (!this.jquery)
        return zmax;

    return this.each(function() {
        zmax += def.inc;
        $(this).css({ "z-index": zmax });
    });
};