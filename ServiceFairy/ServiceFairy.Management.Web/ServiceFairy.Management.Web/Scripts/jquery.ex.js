
(function (jq, undefined) {

    jq.fn.extend({
        innerText: function (txt) {
            if (typeof (txt) == 'undefined') return $(this).attr('innerText');
            else { this.attr('innerText', txt == null ? '' : txt); return this; }
        },

        afterTo: function (parent) {
            parent.after(this);
            return this;
        },

        select: function (convertFunc, toJq) {
            var arr = [];
            this.each(function (key) { arr.push(convertFunc.call(this, key)); });
            return toJq ? jq(arr) : arr;
        },

        where: function (filter, toJq) {
            var arr = [];
            this.each(function (key) { if (filter.call(this, key)) arr.push(this); });
            return toJq ? jq(arr) : arr;
        },

        first: function (filter) {
            var r = null;
            this.each(function (key) { if (filter.call(this, key)) { r = this; return false; } });
            return r;
        },

        any: function (condition) {
            var func = (typeof (condition) == 'Function') ? condition : function () { return this == condition; };
            var r = false;
            this.each(function (key) { if (func.call(this, key)) { r = true; return false; } });
            return r;
        }
    });

    jq.extend(String.prototype, {

        startsWith: function (s) {
            if (s == null || s.length == 0) return true;
            return this.substr(0, s.length) == s;
        },

        endsWith: function (s) {
            if (s == null || s.length == 0) return true;
            return this.substr(this.length - s.length, s.length) == s;
        },

        paddingLeft: function (width, c) {
            var s = this;
            while (s.length < width) s = c + s;
            return s;
        },

        paddingRight: function (width, c) {
            var s = this;
            while (s.length < width) s += c;
            return s;
        },

        joinDesc: function (desc) {
            if (desc == null || desc == '') return this;
            return this + ' (' + desc + ')';
        }
    });

    function sBuilder() {
        this.arr = [];
        this.length = 0;
    }

    jq.extend({
        createSBuilder: function () {
            return new sBuilder();
        }
    });

    jq.extend(sBuilder.prototype, {
        append: function (s) {
            if (s == null) return;
            var type = typeof (s);
            if (type != 'string') s = s.toString();
            if (s != null && s.length > 0) {
                this.arr.push(s);
                this.length += s.length;
            }
            return this;
        },

        appendLine: function (s) {
            if (s == null) this.append('\r\n');
            else this.append(s + '\r\n');
            return this;
        },

        toString: function () {
            return this.arr.join('');
        }
    });

})(jQuery);
