"use strict";

function toBeRun(option) {
    var start = Date.now() / 1000;
    option.run();
    var stop = Date.now() / 1000;
    var time = stop - start;
    expect(time).toBeLessThan(option.lessThanOrEqual);
}

function toBeRace(loss, win) {
    var start, stop;
    start = Date.now() / 1000;
    loss();
    stop = Date.now() / 1000;
    var lossTime = stop - start;

    start = Date.now() / 1000;
    win();
    stop = Date.now() / 1000;
    var winTime = stop - start;
    expect(lossTime).toBeGreaterThan(winTime);
}

describe('Performance', function() {

    describe('When parse', function() {

        var result;

        beforeEach(function() {
            result = '';
        });

        it('Should be race Document.AddClass vs Jquery.AddClass', function() {
            var uniqueId = Date.now() / 1000;
            $('<div>').attr({ id : uniqueId }).appendTo('body');
            toBeRace(function() {
                    for (var i = 0; i < 1000; i++) {
                        $('#' + uniqueId).addClass('active');
                    }
                },
                function() {
                    for (var i = 0; i < 1000; i++) {
                        document.getElementById(uniqueId).class = 'active';
                    }
                }
            );
        });  


        it('Should be parse simple 1000 elements', function() {
            toBeRun({
                run : function() {
                    new IncodingEngine().parse($('#simpleIML'));
                },
                lessThanOrEqual : 1
            });
        });

        it('Should be parse complex 1000 elements', function() {
            toBeRun({
                run : function() {
                    new IncodingEngine().parse($('#complexIML'));
                },
                lessThanOrEqual : 1
            });
        });

        it('Should be executable eval', function() {
            toBeRun({
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
            toBeRun({
                run : function() {
                    var container = $('<div>').hide().appendTo('body');
                    for (var i = 0; i < 1000; i++) {
                        var insert = new ExecutableInsert();
                        insert.jsonData = $.parseJSON($('#ExecutableInsertWithProperty').val());
                        insert.target = container;
                        insert.result = { prop1 : 1, Prop2 : 2 };
                        insert.internalExecute();
                    }
                },
                lessThanOrEqual : 1
            });

        });

    });

});