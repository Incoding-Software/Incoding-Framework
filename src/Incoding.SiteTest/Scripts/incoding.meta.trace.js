"use strict";

function LoggerBase() {
    this.getStatus = function(status) {

        return {
            1 : 'Before',
            2 : 'Success',
            3 : 'Error',
            4 : 'Complete',
            5 : 'Breakes',
        }[status];
    };
    this.special= function(executable, state) {        
    };
    this.log = function(executable, state) {
        console.group("%s by status %s", executable.name,this.getStatus(executable.jsonData.onStatus));
        console.log("Target: %O", executable.target);
        console.log("Self: %O", executable.self);
        console.log('Time of execution: %O', executable.timeOfEndExecution - executable.timeOfStartExecution);
        this.special(executable, state);
        console.groupEnd();
    };
}


ExecutableBase.prototype.logger = new LoggerBase();

ExecutableTrigger.prototype.logger = $.extend(new LoggerBase(), {
    special : function(executable, data) {        
        console.log('Invoke to: %s', executable.jsonData.trigger);        
    }
});
ExecutableEval.prototype.logger = $.extend(new LoggerBase(), {
    special : function(executable, data) {        
        console.log('Code: %s', executable.jsonData.code);        
    }
});
ExecutableJquery.prototype.logger = $.extend(new LoggerBase(), {
    special : function(executable, data) {        
        console.log('Method: %s', executable.jsonData.method);
    }
});
