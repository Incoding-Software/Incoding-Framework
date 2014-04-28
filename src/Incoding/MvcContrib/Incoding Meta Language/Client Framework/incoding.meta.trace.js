/*"use strict";

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
}

ExecutableTrigger.prototype.logger = $.extend(new LoggerBase(), {
    to : function(executable, data) {
        console.group("Trigger %s", this.getStatus(executable.jsonData.onStatus));
        console.log("Target:%O", executable.target);
        console.log('Invoke:%s', executable.jsonData.trigger);
        console.log('Data:%O', data);
    },
    laterOn : function() {
        console.groupEnd();
    }
});

IncodingRunner.prototype.logger = {
    to : function(e, result) {
        console.group("%O even %s", e.srcElement, e.type);
    },

    toException : function(e) {
        console.error(e);
    },

    toAction : function() {
        console.group('Action');
    },

    laterOn : function() {
        console.groupEnd();
    }
};*/