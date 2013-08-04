"use strict";

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

            if (!/^[0-9]+$/.test(key) && _.isArray(parent.base)) {
                var t = {};
                for (var k in parent.base) {
                    t[k] = parent.base[k];
                }
                parent.base = t;
            }
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
            else if (_.isArray(v)) {
                v.push(val);
            }
            else {
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

            hash = hash.trim().cutLastChar();
            if (hash.charAt(hash.length - 1) == '/') {
                hash = hash.cutLastChar(); //cut redundant '/'/
            }

            return current.url() + hash;
        }
    };

}