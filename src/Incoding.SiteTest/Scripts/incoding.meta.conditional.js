"use strict";

//#region class ConditionalFactory

function ConditionalFactory() {
}

// ReSharper disable UnusedParameter
ConditionalFactory.Create = function(data, executable) {
    if (!document[data.type]) {
        document[data.type] = eval('new ' + 'Conditional' + data.type + '();');
    }
    return $.extend(false, document[data.type], {
        jsonData : data,
        executable : executable
    });

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