"use strict";




describe('Performance', function() {

    describe('When parse', function() {

        var result;

        beforeEach(function() {
            result = '';
        });

        it('Should be parse simple 1000 elements', function() {

            var start = Math.floor(Date.now() / 1000);
            new IncodingEngine().parse($('#simpleIML'));
            var stop = Math.floor(Date.now() / 1000);
            expect(stop - start).toBeLessThan(1);
        });


        it('Should be parse complex 1000 elements', function() {

            var start = Math.floor(Date.now() / 1000);
            new IncodingEngine().parse($('#complexIML'));
            var stop = Math.floor(Date.now() / 1000);
            expect(stop - start).toBeLessThan(1);
        });

    });

});

