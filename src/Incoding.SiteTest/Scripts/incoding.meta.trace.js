"use strict";

persistence.store.websql.config(persistence, 'profiler', 'A database description', 5 * 1024 * 1024);
persistence.schemaSync();
var ExecutableOfCall = persistence.define('ExecutableOfCall', {
    name: "TEXT",
    data: "JSON",
    timeOfExecution: "REAL"
});
var Executable = persistence.define('Executable', {
    name: "TEXT"
});
ExecutableOfCall.hasOne('executable', Executable);

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
    this.end = function () {
        var timeOfExecution = Date.now() / 1000 - this.timeOfStartExecution;

        var executableOfCall = new ExecutableOfCall({ executable: this.executable.name, timeOfExecution: timeOfExecution, });
        persistence.add(executableOfCall);
        persistence.flush();
        //console.group();

        //console.log("{0} by status {1}".f(this.executable.name, this.getStatus(this.executable.jsonData.onStatus)));
        //console.log("Target: %", this.executable.target);
        //console.log("Self: %", this.executable.self);
        //console.log('Time of execution: {0}'.f(timeOfExecution));
        //var json = this.executable.jsonData;
        //$.eachProperties(this.executable.jsonData, function () {
        //    console.log("{0}: {1}".f(this, json[this]));
        //});

        //console.groupEnd();        
    };

    this.start = function (executable, state) {
        this.executable = executable;
        this.state = state;
        this.timeOfStartExecution = Date.now() / 1000;
    };
}

ExecutableBase.prototype.getLogger = function () {
    return new LoggerBase();
};