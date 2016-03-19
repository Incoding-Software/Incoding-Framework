"use strict";

function LoggerBase() {

    this.timeOfStartExecution = 0;
    this.executable = '';
    this.state = '';
    this.getStatus = function (status) {

        return {
            1: 'Before',
            2: 'Success',
            3: 'Error',
            4: 'Complete',
            5: 'Breakes',
        }[status];
    };
    this.special = function () {
    };
    this.end = function () {
        var timeOfEndExecution = Date.now() / 1000;
        console.group();
        console.log("{0} by status {1}".f(this.executable.name, this.getStatus(this.executable.jsonData.onStatus)));
        console.log("Target: %", this.executable.target);
        console.log("Self: %", this.executable.self);
        console.log('Time of execution: {0}'.f(timeOfEndExecution - this.timeOfStartExecution));
        this.special();
        console.groupEnd();
    };

    this.start = function (executable, state) {
        this.executable = executable;
        this.state = state;
        this.timeOfStartExecution = Date.now() / 1000;
    }


}


ExecutableBase.prototype.logger = new LoggerBase();

ExecutableTrigger.prototype.logger = $.extend(new LoggerBase(), {
    special: function () {
        console.log('Invoke to: {0}'.f(this.executable.jsonData.trigger));
    }
});
ExecutableEval.prototype.logger = $.extend(new LoggerBase(), {
    special: function () {
        console.log('Code: {0}'.f(this.executable.jsonData.code));
    }
});
ExecutableInsert.prototype.logger = $.extend(new LoggerBase(), {
    special: function () {
        console.log('Property: {0}'.f(this.executable.jsonData.property));
        console.log('Prepare: {0}'.f(this.executable.jsonData.prepare));
        console.log('Template: {0}'.f(this.executable.jsonData.template));
        console.log('Type: {0}'.f(this.executable.jsonData.insertType));
    }
});
ExecutableJquery.prototype.logger = $.extend(new LoggerBase(), {
    special: function () {
        console.log('Method: {0}'.f(this.executable.jsonData.method));
    }
});
