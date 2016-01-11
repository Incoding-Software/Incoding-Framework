"use strict";

function toBePerformance(option) {
    var start = Math.floor(Date.now() / 1000);
    option.run();
    var stop = Math.floor(Date.now() / 1000);
    var time = stop - start;
    expect(time <= option.lessThanOrEqual).toBeTruthy();
}

describe('Performance', function() {

    describe('When parse', function() {

        var result;

        beforeEach(function() {
            result = '';
        });

        it('Should be parse simple 1000 elements', function() {
            toBePerformance({
                run : function() {
                    new IncodingEngine().parse($('#simpleIML'));
                },
                lessThanOrEqual : 1
            });
        });

        it('Should be parse complex 1000 elements', function() {
            toBePerformance({
                run : function() {
                    new IncodingEngine().parse($('#complexIML'));
                },
                lessThanOrEqual : 1
            });
        });

        it('Should be executable eval', function() {
            toBePerformance({
                run : function() {
                    for (var i = 0; i < 1000; i++) {
                        var executableEval = new ExecutableEval();
                        executableEval.getTarget = function() {

                        };
                        executableEval.jsonData = {
                            code : "console.log('debug',123)"
                        };
                        executableEval.internalExecute();
                    }
                },
                lessThanOrEqual : 1
            });

        });

        it('Should be executable insert', function() {
            toBePerformance({
                run : function() {
                    for (var i = 0; i < 1000; i++) {
                        var insert = new ExecutableInsert();
                        insert.jsonData = $.parseJSON($('#ExecutableInsertWithProperty').val());
                        insert.getTarget = function() {
                            return $('body');
                        };
                        insert.result = { prop1 : 1, Prop2 : 2 };
                        insert.internalExecute();
                    }
                },
                lessThanOrEqual : 1
            });

        });

    });

});