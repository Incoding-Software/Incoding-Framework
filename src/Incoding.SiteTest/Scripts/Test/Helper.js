

function TestHelper() {

    this.SandboxSubmit = function() {
        var res = $('<input>').attr({ id : 'sandboxSubmit', type : 'submit', name : 'sandboxSubmit' });
        appendSetFixtures(res);
        return res;
    };

    this.SandboxInputButton = function() {
        var res = $('<input>').attr({ id : 'sandboxInputButton', type : 'button', name : 'sandboxInputButton' });
        appendSetFixtures(res);
        return res;
    };

    this.SandboxReset = function() {
        var res = $('<input>').attr({ id : 'sandboxReset', type : 'reset', name : 'sandboxReset' });
        appendSetFixtures(res);
        return res;
    };

    this.SandboxInput = function() {
        var res = $('<input>');
        appendSetFixtures(res);
        return res;
    };

    this.SandboxHidden = function() {
        var res = $('<input>').attr({ id : 'sandboxHidden', type : 'hidden', name : 'sandboxHidden' });
        appendSetFixtures(res);
        return res;
    };
    this.SandboxTextArea = function() {
        var res = $('<textarea>').attr({ id : 'sandboxTextarea', name : 'sandboxTextarea' });
        appendSetFixtures(res);
        return res;
    };
    this.SandboxTextBox = function(attr) {
        var res = $('<input>').attr({ id : 'sandboxTextBox', name : 'sandboxTextBox', type : 'textbox' });
        if (attr) {
            $(res).attr(attr);
        }
        appendSetFixtures(res);
        return res;
    };
    this.SandboxSelect = function(values) {
        var res = $('<select>').attr({ id : 'sandboxSelect', name : 'sandboxSelect' });
        if (values) {
            $(values).each(function() {
                var option = $("<option>").attr({ value : this.value });
                if (this.selected) {
                    $(option).attr({ selected : 'selected' });
                }

                $(res).append(option);
            });
        }
        appendSetFixtures(res);
        return res;
    };
    this.SandboxMultiSelect = function(values) {
        var res = $('<select>').attr({ id : 'sandboxMultiSelect', multiple : true, name : 'sandboxMultiSelect' });
        if (values) {
            $(values).each(function() {
                var option = $("<option>").attr({ value : this.value });
                if (this.selected) {
                    $(option).attr({ selected : 'selected' });
                }

                $(res).append(option);
            });
        }
        appendSetFixtures(res);
        return res;
    };

    this.SandboxCheckBox = function(checked, value) {
        var checkBox = this.CreateCheckBox(checked, value);
        appendSetFixtures(checkBox);
        return checkBox;
    };

    this.SandboxRadioButton = function(checked, value) {
        var checkBox = this.CreateRadioButton(checked, value);
        appendSetFixtures(checkBox);
        return checkBox;
    };

    this.SandboxSpan = function() {
        var span = this.CreateSpan();
        appendSetFixtures(span);
        return span;
    };

    this.CreateCheckBox = function(checked, value) {
        var res = $('<input>').attr({ id : 'sandboxCheckbox', name : 'sandboxCheckbox', type : 'checkbox' });
        if (checked) {
            res.prop('checked',true);
        }
        if (!_.isUndefined(value) && !_.isNull(value)) {
            res.val(value);
        }
        return res;
    };
    this.CreateRadioButton = function(checked, value) {
        var res = $('<input>').attr({ id : 'sandboxRadiobutton', name : 'sandboxRadiobutton', type : 'radio', value : value });
        if (checked) {
            res.prop('checked', true);
        }
        return res;
    };
    this.CreateOption = function(value, selected) {
        var res = $('<option>').attr({ value : value });
        if (selected) {
            res.prop('selected', true);            
        }

        return res;

    };
    this.CreateDiv = function() {
        return $('<div>');
    };

    this.CreateSpan = function() {
        return $('<span>');
    };
    this.SandboxA = function(href) {
        var res = $('<a>').prop({ id : 'sandobxA', href : href });
        setFixtures(res);
        return res;
    };
    this.SandboxLabel = function(content) {
        var res = $('<label>').prop({ id : 'sandboxLabel' });
        res.text(content);
        setFixtures(res);
        return res;
    };

}

TestHelper.prototype = {
    GetAjaxOptions : function(options) {

        var defOptions = {
            async : true,
            cache : true,
            crossDomain : false,
            global : true,
            data : '',
            dataType : 'json',
            traditional : true,
            type : 'GET',
            url : '',
            headers : { "X-Requested-With" : "XMLHttpRequest" }
        };

        $.extend(defOptions, options);

        return defOptions;
    },
    GetAjaxSubmitOptions : function(options) {
        var def = {
            forceSync : false,
            replaceTarget : false,
            resetForm : false,
            semantic : false,
            clearForm : false,
            dataType : 'json'
        };

        $.extend(def, options);

        return def;
    }
};

TestHelper.Instance = new TestHelper();