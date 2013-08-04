"use strict";

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
            res.prop('checked', true);
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
            dataType : 'JSON',
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

var newFakeEvalVariable = false;

describe('Incoding', function() {

    var instanceSandBox, outputPanel;

    beforeEach(function() {
        outputPanel = $('<div>');
        $('body').append(outputPanel);
        setFixtures(sandbox());
        instanceSandBox = $('#sandbox');
        $.browser.safari = ($.browser.webkit && !(/chrome/.test(navigator.userAgent.toLowerCase())));
    });

    describe('Framework', function() {

        describe('Infrastructure', function() {

            describe('Core', function() {

                describe('IncodingExtend', function() {

                    describe('When child with ctor', function() {

                        //#region Establish class

                        //#region class Base 

                        function Base(arg1, arg2) {

                            if (arguments.length > 0) {
                                this.init(arg1, arg2);
                            }
                            this.agr1 = '';
                            this.agr1 = '';
                        }

                        $.extend(Base.prototype, {
                            init : function(arg1, arg2) {
                                this.agr1 = arg1;
                                this.agr2 = arg2;
                            }
                        });

                        //#endregion

                        //#region class Derived extend from Base

                        incodingExtend(Derived, Base);

                        function Derived(arg1, arg2) {
                            this.init(arg1, arg2);
                        }

                        $.extend(Derived.prototype, {
                            newMethod : function() {
                                return 'newMethod';
                            },
                            staticField : ''
                        });

                        //#endregion

                        //#region class DerivedWithOverrideCtor extend from Base

                        incodingExtend(DerivedWithOverrideCtor, Base);

                        function DerivedWithOverrideCtor(arg1, arg2, arg3) {
                            this.init(arg1, arg2, arg3);
                            this.agr3;
                        }

                        DerivedWithOverrideCtor.prototype.init = function(arg1, arg2, arg3) {
                            DerivedWithOverrideCtor.superclass.init.call(this, arg1, arg2);
                            this.agr3 = arg3;
                        };

                        //#endregion

                        //#endregion

                        it('Should be init field through base ctor', function() {
                            var derived = new Derived('agr1', 'agr2');
                            expect(derived.agr1).toEqual('agr1');
                            expect(derived.agr2).toEqual('agr2');
                        });

                        it('Should be call new method', function() {
                            var derived = new Derived('agr1', 'agr2');
                            expect(derived.newMethod()).toEqual('newMethod');
                        });

                        it('Should be init field through base ctor and one field through override ctor', function() {
                            var derivedOverride = new DerivedWithOverrideCtor('agr1', 'agr2', 'agr3');
                            expect(derivedOverride.agr1).toEqual('agr1');
                            expect(derivedOverride.agr2).toEqual('agr2');
                            expect(derivedOverride.agr3).toEqual('agr3');

                        });

                        it('Should be field instance', function() {
                            var instance1 = new Derived('instance1', 'instance1');
                            var instance2 = new Derived('instance2', 'instance2');

                            expect(instance1.agr1).not.toEqual(instance2.agr1);
                        });

                        it('Should be static instance', function() {
                            var instance1 = new Derived();
                            instance1.staticField = 'Aws';
                            var instance2 = new Derived();

                            expect(instance1.staticField).not.toEqual(instance2.staticField);
                        });

                    });

                    describe('When child class', function() {

                        //#region  Establish class

                        function Parent() {
                            this.HasField = true;
                        }

                        Parent.prototype = { HasField : true };

                        function Child() {
                            this.NewHasField = false;
                        }

                        Child.prototype = {
                            NewHasField : false
                        };

                        incodingExtend(Child, Parent);
                        //#endregion

                        var childInstance;
                        var childInstance2;

                        beforeEach(function() {
                            childInstance = new Child();
                            childInstance2 = new Child();
                        });

                        it('Should be child has child type', function() {
                            expect(childInstance instanceof Child).toBeTruthy();
                        });

                        it('Should be child has parent type', function() {
                            expect(childInstance instanceof Parent).toBeTruthy();
                        });

                        it('Should be child has field from parent', function() {
                            expect(childInstance.HasField).toBeTruthy();
                            expect(childInstance.NewHasField).toBeFalsy();
                        });

                        it('Should be child 1 and child 2  has not static field "HasFiedl"', function() {
                            childInstance.HasField = this;
                            childInstance2.HasField = false;
                            expect(childInstance.HasField).toBeTruthy();
                            expect(childInstance2.HasField).toBeFalsy();

                        });

                    });

                });

                describe('When String extend', function() {

                    it('Should be f', function() {
                        var actual = 'Hello {0} is {1}'.f('Vlad', "Bob");
                        expect(actual).toEqual('Hello Vlad is Bob');
                    });

                    describe('When start and end', function() {

                        var originalString = 'Vlad';

                        it('Should be found startWith', function() {
                            expect(originalString.startsWith('Vl')).toBeTruthy();
                        });

                        it('Should be wrong startWith', function() {
                            expect(originalString.startsWith('lV')).toBeFalsy();
                        });

                        it('Should be found endWith', function() {
                            expect(originalString.endWith('ad')).toBeTruthy();
                        });

                        it('Should be wrong endWith', function() {
                            expect(originalString.startsWith('da')).toBeFalsy();
                        });

                    });

                    describe('When contains', function() {

                        it('Should be is contains', function() {
                            expect('Vlad Test'.contains('est')).toBeTruthy();
                        });

                        it('Should be wrong contains', function() {
                            expect('Vlad Test'.contains('tst')).toBeFalsy();
                        });

                    });

                    it('Should be trim', function() {
                        expect(' Vlad '.trim()).toEqual('Vlad');
                    });

                    it('Should be left trim', function() {
                        expect(' Vlad '.ltrim()).toEqual('Vlad ');
                    });

                    it('Should be right trim', function() {
                        expect(' Vlad '.rtrim()).toEqual(' Vlad');
                    });

                    it('Should be cut last char', function() {
                        expect('Vladd'.cutLastChar()).toEqual('Vlad');
                    });

                    it('Should be replace all', function() {
                        expect('Vlad and Vlad'.replaceAll('Vlad', 'Bob')).toEqual('Bob and Bob');
                    });
                });

                describe('When Array extend', function() {

                    describe('When contains', function() {

                        var array;

                        beforeEach(function() {
                            array = ['found', 'notFound'];
                        });

                        it('Should be in', function() {
                            expect(array.contains('found')).toBeTruthy();
                        });

                        it('Should be wrong in', function() {
                            expect(array.contains('any')).toBeFalsy();
                        });

                    });

                    it('Should be select', function() {
                        var expected = [{ property : 5 }, { property : 10 }].select(function(item) {
                            return item.property;
                        });
                        expect(expected).toEqual([5, 10]);
                    });

                    it('Should be count', function() {
                        var expected = [5, 10, 25].count();
                        expect(expected).toEqual(3);
                    });

                    it('Should be sum', function() {
                        var expected = [5, 10].sum();
                        expect(expected).toEqual(15);
                    });

                    it('Should be max', function() {
                        var expected = [5, 35, 25].max();
                        expect(expected).toEqual(35);
                    });

                    it('Should be max with empty', function() {
                        var expected = [].max();
                        expect(expected).toEqual(0);
                    });

                    it('Should be min', function() {
                        var actual = [5, 1, 25].min();
                        expect(actual).toEqual(1);
                    });

                    it('Should be min with empty', function() {
                        var expected = [].min();
                        expect(expected).toEqual(0);
                    });

                    it('Should be first', function() {
                        var expected = [5, 10].first();
                        expect(expected).toEqual(5);
                    });

                    it('Should be last', function() {
                        var expected = [5, 10].last();
                        expect(expected).toEqual(10);
                    });

                    it('Should be average', function() {
                        var expected = [5, 10, 20, 35].average();
                        expect(expected).toEqual(17.5);
                    });

                });

                describe('Url extend', function() {

                    describe('When read', function() {

                        var verify = function(url, options) {

                            expect(url.param('param')).toEqual('value');
                            expect(url.param('param2')).toEqual('value2');
                            expect(url.attr('fragment')).toEqual(options.fragment);
                            expect(url.attr('base')).toEqual(options.base);
                            expect(url.url()).toEqual(options.url);

                            if (!options.excludeHashQuery) {
                                expect(url.fparam('param', 'root')).toEqual('fragmentValue');
                                expect(url.fparam('param2', 'root')).toEqual('fragmentValue2');
                                expect(url.fparam('param', 'SearchUrl')).toEqual('value');
                                expect(url.fparam('param2', 'SearchUrl')).toEqual('value2');
                            }

                            expect(url.furl('root')).toEqual(options.rootUrl);
                            expect(url.furl('SearchUrl')).toEqual(options.searhUrl);
                            expect(url.toHref()).toEqual(options.href);
                        };

                        it('Should be parse absolute url', function() {
                            var href = 'http://sample.com/Home/Index?param=value&param2=value2#!Manager/Office?param=fragmentValue/param2=fragmentValue2&SearchUrl:Search/Index?param=value/param2=value2';
                            var url = $.url(href);
                            verify(url, {
                                base : 'http://sample.com/Home/Index',
                                url : 'http://sample.com/Home/Index?param=value&param2=value2',
                                rootUrl : 'Manager/Office',
                                searhUrl : 'Search/Index',
                                fragment : 'Manager/Office?param=fragmentValue/param2=fragmentValue2&SearchUrl:Search/Index?param=value/param2=value2',
                                href : href
                            });

                        });

                        it('Should be parse relative url', function() {
                            var href = 'Home/Index?param=value&param2=value2#!Manager/Office?param=fragmentValue/param2=fragmentValue2&SearchUrl:Search/Index?param=value/param2=value2';
                            var url = $.url(href);
                            verify(url, {
                                base : 'Home/Index',
                                url : 'Home/Index?param=value&param2=value2',
                                rootUrl : 'Manager/Office',
                                searhUrl : 'Search/Index',
                                fragment : 'Manager/Office?param=fragmentValue/param2=fragmentValue2&SearchUrl:Search/Index?param=value/param2=value2',
                                href : href
                            });
                        });

                        it('Should be parse without hash query', function() {
                            var href = 'Home/Index?param=value&param2=value2#!Manager/Office?&SearchUrl:Search/Index?';
                            var url = $.url(href);
                            verify(url, {
                                base : 'Home/Index',
                                url : 'Home/Index?param=value&param2=value2',
                                rootUrl : 'Manager/Office',
                                searhUrl : 'Search/Index',
                                excludeHashQuery : true,
                                fragment : 'Manager/Office?&SearchUrl:Search/Index?',
                                href : href
                            });
                        });

                        it('Should be parse without hash url', function() {
                            var href = 'Home/Index?param=value&param2=value2#!param=fragmentValue/param2=fragmentValue2&SearchUrl:param=value/param2=value2';
                            var url = $.url(href);
                            verify(url, {
                                base : 'Home/Index',
                                url : 'Home/Index?param=value&param2=value2',
                                rootUrl : '',
                                searhUrl : '',
                                fragment : 'param=fragmentValue/param2=fragmentValue2&SearchUrl:param=value/param2=value2',
                                href : href
                            });
                        });

                        it('Should be parse without hash', function() {
                            var href = 'Home/Index?param=value&param2=value2#!';
                            var url = $.url(href);
                            verify(url, {
                                base : 'Home/Index',
                                url : 'Home/Index?param=value&param2=value2',
                                rootUrl : '',
                                searhUrl : '',
                                fragment : '',
                                href : href,
                                excludeHashQuery : true
                            });
                        });

                        it('Should be parse hash query with brakes', function() {
                            var href = 'Home/Index#!value[0]=val/value[1]=val2';

                            var url = $.url(href);
                            expect(url.fparam('value[0]', 'root')).toEqual('val');
                            expect(url.fparam('value[1]', 'root')).toEqual('val2');
                        });

                        it('Should be to href with encode hash query string', function() {
                            spyOn(ExecutableHelper, "UrlEncode").andCallFake(function(value) {
                                if (value == 'val') {
                                    return 'encodeVal';
                                }
                            });

                            var href = 'Home/Index#!value[0]=val';
                            var url = $.url(href);
                            expect(url.toHref()).toEqual('Home/Index#!value[0]=encodeVal');
                        });

                        it('Should be to href with hash url', function() {

                            var href = 'Home/Index#!User/Index?';
                            var url = $.url(href);
                            expect(url.toHref()).toEqual('Home/Index#!User/Index?');
                        });

                        it('Should be to href with hash url', function() {

                            var href = 'Home/Index#!User/Index?';
                            var url = $.url(href);
                            expect(url.toHref()).toEqual('Home/Index#!User/Index?');
                        });

                    });

                    describe('When write', function() {

                        it('Should be replace hash parameter', function() {
                            var url = $.url('http://sample.com/Home/Index?param=value&param2=value2#!Manager/Office?param=fragmentValue/param2=fragmentValue2');

                            var newFragmentValue = 'newFragmentValue';
                            url.setFparam('param2', newFragmentValue, 'root');

                            expect(url.fparam('param2', 'root')).toEqual(newFragmentValue);
                        });

                        it('Should be remove hash parameter', function() {
                            var url = $.url('http://sample.com/Home/Index?param=value&param2=value2#!Manager/Office?param=fragmentValue/param2=fragmentValue2');

                            url.removeFparam('param2', 'root');

                            expect(url.fparam('param2', 'root')).toEqual('undefined');
                        });

                        it('Should be set new hash parameter', function() {
                            var url = $.url('http://sample.com/Home/Index?param=value&param2=value2#!Manager/Office');

                            var newFragmentValue = 'newFragmentValue';
                            url.setFparam('newParam', newFragmentValue, 'root');

                            expect(url.fparam('newParam', 'root')).toEqual(newFragmentValue);
                        });

                        it('Should be clear hash parameter', function() {
                            var url = $.url('http://sample.com/Home/Index?param=value&param2=value2#!Manager/Office?param=fragmentValue/param2=fragmentValue2');

                            url.clearFparam();

                            expect(url.furl('root')).toEqual('Manager/Office');
                            expect(url.fparam().length).toEqual(0);
                        });

                        it('Should be set hash url', function() {

                            var url = $.url('http://sample.com/Home/Index?param=value&param2=value2#!Manager/Office');

                            var newHashUrl = 'hashUrl';
                            url.setFurl(newHashUrl);

                            expect(url.furl('root')).toEqual(newHashUrl);

                        });

                    });

                });

                describe('When jquery extend', function() {

                    describe('When jquery max-z-index', function() {

                        var item1, item2;

                        beforeEach(function() {
                            item1 = sandbox({ id : 'item1' });
                            item2 = sandbox({ id : 'item2' });

                            $(item1).add(item2).css({ position : 'relative' });

                            $('body').append(item1, item2);
                        });

                        it('Should be set max z-index', function() {

                            $(item1).css({ 'z-index' : 5 });
                            $(item2).maxZIndex({ inc : 5 });
                            expect($(item2).css('z-index').toString()).toEqual('10');
                        });

                        afterEach(function() {
                            $(item1)
                                .add(item2)
                                .remove();
                        });
                    });

                    it('Should be eachProperties', function() {

                        var expected = [];
                        $.eachProperties({ prop1 : '', prop2 : '' }, function() {
                            expected.push(this);
                        });

                        expect(expected).toEqual(['prop1', 'prop2']);
                    });

                    it('Should be toggleProp', function() {

                        $(instanceSandBox).toggleProp('disabled');
                        expect(instanceSandBox).toHaveProp('disabled');

                        $(instanceSandBox).toggleProp('disabled');
                        expect(instanceSandBox).toHaveProp('disabled', false);
                    });

                    it('Should be toggleAttr', function() {

                        $(instanceSandBox).toggleAttr('disabled');
                        expect(instanceSandBox).toHaveAttr('disabled');

                        $(instanceSandBox).toggleAttr('disabled');
                        expect(instanceSandBox).not.toHaveAttr('disabled');
                    });

                });

            });

            describe('When IncodingMetaElement', function() {

                it('Should be runner from data', function() {
                    $.data(instanceSandBox, 'incoding-runner', true);
                    expect(new IncodingMetaElement(instanceSandBox).runner).toBeTruthy();
                });

                it('Should be runner from attribute', function() {
                    instanceSandBox.attr('incoding-runner', true);
                    expect(new IncodingMetaElement(instanceSandBox).runner).toBeTruthy();
                });

                it('Should be runner from prop', function() {
                    instanceSandBox.prop('incoding-runner', true);
                    expect(new IncodingMetaElement(instanceSandBox).runner).toBeTruthy();
                });

                it('Should be element', function() {
                    expect(new IncodingMetaElement(instanceSandBox).element).toEqual(instanceSandBox);
                });

                it('Should not be is new', function() {
                    instanceSandBox.data('incoding-runner', true);
                    instanceSandBox.data('incoding', '[{}]');
                    expect(new IncodingMetaElement(instanceSandBox).IsNew()).toBeFalsy();
                });

                it('Should not be is new without runner', function() {
                    expect(new IncodingMetaElement(instanceSandBox).IsNew()).toBeFalsy();
                });

                it('Should be is new without runner', function() {
                    instanceSandBox.data('incoding', '[{}]');
                    expect(new IncodingMetaElement(instanceSandBox).IsNew()).toBeTruthy();
                });

                it('Should be executables', function() {
                    var jsonData = '[{ "type" : "Action" }, { "type" : "Action2" }]';
                    instanceSandBox.attr({ "incoding" : jsonData.toString() });

                    var callbacks = new IncodingMetaElement(instanceSandBox).executables;

                    expect(callbacks.length).toEqual(2);
                    expect(callbacks[0]).toEqual({ type : 'Action' });
                    expect(callbacks[1]).toEqual({ type : 'Action2' });
                });

                it('Should be flushRunner', function() {
                    new IncodingMetaElement(instanceSandBox).flushRunner('data');
                    expect($.data(instanceSandBox, 'incoding-runner')).toEqual('data');
                });

                describe('When bind', function() {

                    var runner, result;

                    var createSpyEvent = function(bind) {
                        var event = {
                            type : bind,
                            stopPropagation : function() {
                            },
                            preventDefault : function() {
                            }
                        };

                        spyOn(event, 'stopPropagation');
                        spyOn(event, 'preventDefault');

                        return event;
                    };

                    beforeEach(function() {
                        result = { value : 'value' };

                        runner = jasmine.createSpyObj('runner', ['DoIt']);
                        $(instanceSandBox).data('incoding-runner', runner);
                    });

                    it('Should be incoding', function() {

                        var currentEvent = createSpyEvent('incoding');
                        spyOnEvent(instanceSandBox, currentEvent.type);

                        new IncodingMetaElement(instanceSandBox)
                            .bind(currentEvent.type, 1);

                        $(instanceSandBox).trigger(currentEvent, result);

                        expect(currentEvent.type).toHaveBeenTriggeredOn(instanceSandBox);
                        expect(currentEvent.preventDefault).toHaveBeenCalled();
                        expect(currentEvent.stopPropagation).toHaveBeenCalled();
                    });

                    it('Should be click', function() {

                        var eventClick = createSpyEvent('click');
                        spyOnEvent(instanceSandBox, eventClick.type);

                        new IncodingMetaElement(instanceSandBox)
                            .bind(eventClick.type, 1);

                        $(instanceSandBox).trigger(eventClick, result);

                        expect(eventClick.type).toHaveBeenTriggeredOn(instanceSandBox);
                        expect(eventClick.preventDefault).not.toHaveBeenCalled();
                        expect(eventClick.stopPropagation).not.toHaveBeenCalled();

                    });

                    it('Should be incoding and click', function() {

                        var eventClick = createSpyEvent('click');
                        spyOnEvent(instanceSandBox, eventClick.type);

                        new IncodingMetaElement(instanceSandBox)
                            .bind('click incoding', 1);

                        $(instanceSandBox).trigger(eventClick, result);

                        expect(eventClick.type).toHaveBeenTriggeredOn(instanceSandBox);
                        expect(eventClick.preventDefault).not.toHaveBeenCalled();
                        expect(eventClick.stopPropagation).not.toHaveBeenCalled();

                    });

                    it('Should be with prevent default', function() {

                        var eventClick = createSpyEvent('click');
                        spyOnEvent(instanceSandBox, eventClick.type);

                        new IncodingMetaElement(instanceSandBox)
                            .bind(eventClick.type, 2);

                        $(instanceSandBox).trigger(eventClick, result);

                        expect(eventClick.type).toHaveBeenTriggeredOn(instanceSandBox);
                        expect(eventClick.preventDefault).toHaveBeenCalled();
                        expect(eventClick.stopPropagation).not.toHaveBeenCalled();

                    });

                    it('Should be with stop propagation', function() {

                        var eventClick = createSpyEvent('click');
                        spyOnEvent(instanceSandBox, eventClick.type);

                        new IncodingMetaElement(instanceSandBox)
                            .bind(eventClick.type, 3);

                        $(instanceSandBox).trigger(eventClick, result);

                        expect(eventClick.type).toHaveBeenTriggeredOn(instanceSandBox);
                        expect(eventClick.preventDefault).not.toHaveBeenCalled();
                        expect(eventClick.stopPropagation).toHaveBeenCalled();

                    });

                    it('Should be with stop propagation and prevent default', function() {

                        var eventClick = createSpyEvent('click');
                        spyOnEvent(instanceSandBox, eventClick.type);

                        new IncodingMetaElement(instanceSandBox)
                            .bind(eventClick.type, 4);

                        $(instanceSandBox).trigger(eventClick, result);

                        expect(eventClick.type).toHaveBeenTriggeredOn(instanceSandBox);
                        expect(eventClick.preventDefault).toHaveBeenCalled();
                        expect(eventClick.stopPropagation).toHaveBeenCalled();

                    });

                    afterEach(function() {
                        expect(runner.DoIt).toHaveBeenCalledWith(jasmine.any(Object), result);
                    });

                });

                it('Should be bind special event', function() {

                    $(IncSpecialBinds.DocumentBinds).each(function() {

                        var currentBind = this;
                        if (currentBind == IncSpecialBinds.IncChangeUrl) {
                            return true;
                        }

                        $(instanceSandBox).removeData('incoding-runner');
                        var selfRunner = new IncodingRunner();
                        spyOn(selfRunner, 'DoIt');
                        new IncodingMetaElement(instanceSandBox).flushRunner(selfRunner);

                        new IncodingMetaElement(instanceSandBox).bind(currentBind);
                        $(document).trigger(jQuery.Event(currentBind));

                        expect(selfRunner.DoIt).toHaveBeenCalled();
                        expect(selfRunner.DoIt.argsForCall[0][0].type).toEqual(currentBind);
                    });

                });

            });

            describe('IncodingRunner', function() {

                var runner;
                beforeEach(function() {
                    runner = new IncodingRunner();
                });

                describe('When register', function() {

                    it('Should be registry action', function() {
                        runner.Registry('Action', undefined, { is : true });
                        expect(runner.actions[0].is).toBeTruthy();
                    });

                    it('Should be registry before', function() {
                        runner.Registry('Executable', 1, { is : true });
                        expect(runner.before[0].is).toBeTruthy();
                    });

                    it('Should be registry success', function() {
                        runner.Registry('Executable', 2, { is : true });
                        expect(runner.success[0].is).toBeTruthy();
                    });

                    it('Should be registry error', function() {
                        runner.Registry('Executable', 3, { is : true });
                        expect(runner.error[0].is).toBeTruthy();
                    });

                    it('Should be registry complete', function() {
                        runner.Registry('Executable', 4, { is : true });
                        expect(runner.complete[0].is).toBeTruthy();
                    });

                    it('Should be registry break', function() {
                        runner.Registry('Executable', 5, { is : true });
                        expect(runner.breakes[0].is).toBeTruthy();
                    });

                });

                describe('When doIt', function() {

                    var event, action;

                    var createCallback = function(bind) {
                        var callback = new ExecutableBase();
                        callback.onBind = bind;
                        spyOn(callback, 'execute');
                        return callback;
                    };

                    beforeEach(function() {
                        event = $.Event('click');
                        action = createCallback(event.type);
                        runner.actions.push(action);
                    });

                    it('Should be doIt break before', function() {
                        var before = new ExecutableBase();
                        before.onBind = event.type;
                        spyOn(before, 'execute').andThrow(new IncClientException());
                        runner.before.push(before);

                        var breakCallback = createCallback(event.type);
                        runner.breakes.push(breakCallback);

                        runner.DoIt(event);

                        expect(before.execute).toHaveBeenCalled();
                        expect(breakCallback.execute).toHaveBeenCalledWith(new IncClientException());
                        expect(action.execute).not.toHaveBeenCalled();
                    });

                    it('Should be doIt', function() {
                        var before = createCallback(event.type);
                        runner.before.push(before);

                        var breakCallback = createCallback(event.type);
                        runner.breakes.push(breakCallback);

                        runner.DoIt(event, IncodingResult.Empty);

                        expect(before.execute).toHaveBeenCalled();
                        expect(action.execute).toHaveBeenCalledWith({
                            success : runner.success,
                            error : runner.error,
                            complete : runner.complete,
                            breakes : runner.breakes,
                            eventResult : IncodingResult.Empty
                        });
                        expect(breakCallback.execute).not.toHaveBeenCalled();
                    });

                    it('Should be DoIt filter by event type', function() {

                        var noClickAction = createCallback('noclick');
                        runner.before.push(noClickAction);
                        runner.breakes.push(noClickAction);
                        runner.success.push(noClickAction);
                        runner.error.push(noClickAction);
                        runner.complete.push(noClickAction);

                        runner.DoIt(event, IncodingResult.Empty);

                        expect(noClickAction.execute).not.toHaveBeenCalled();
                        expect(action.execute).toHaveBeenCalledWith({
                            success : [],
                            error : [],
                            complete : [],
                            breakes : [],
                            eventResult : IncodingResult.Empty
                        });
                    });

                });

            });

            describe('When ExecutableFactory', function() {

                it('Should be create all executable', function() {
                    $('input', '#suportedExectuable').each(function() {
                        var executable = ExecutableFactory.Create($(this).val(), '{ "Key":"Value" }', instanceSandBox);
                        expect(_.isObject(executable)).toBeTruthy();
                    });
                });

                it('Should be create', function() {

                    var sandboxSubmit = TestHelper.Instance.SandboxSubmit();
                    var executable = ExecutableFactory.Create('ExecutableInsert', '{ "onBind":"Value","timeOut":5, "interval":10,"intervalId":"id","target":"$(\'#sandboxSubmit\')","ands":[1,2] }', instanceSandBox);

                    expect($(executable.self).get(0)).toEqual($(instanceSandBox).get(0));
                    expect($(executable.getTarget()).get(0)).toEqual($(sandboxSubmit).get(0));
                    expect(executable.onBind).toEqual('Value');
                    expect(executable.timeOut).toEqual(5);
                    expect(executable.interval).toEqual(10);
                    expect(executable.intervalId).toEqual('id');
                    expect(executable.ands).toEqual([1, 2]);
                    expect(executable.jsonData).toEqual({ onBind : "Value", timeOut : 5, interval : 10, intervalId : "id", target : "$('#sandboxSubmit')", ands : [1, 2] });
                });

                it('Should be create with self target', function() {
                    var executable = ExecutableFactory.Create('ExecutableInsert', '{ "onBind":"Value","timeOut":5,"target":"$(this.self)" }', instanceSandBox);

                    expect($(executable.self).get(0)).toEqual($(instanceSandBox).get(0));
                    expect($(executable.getTarget()).get(0)).toEqual($(instanceSandBox).get(0));
                });

            });

            describe('IncodingEngine', function() {

                describe('When parse', function() {

                    var engine;

                    beforeEach(function() {
                        engine = new IncodingEngine();
                    });

                    it('Should be call ExecutableFactory.Create', function() {

                        var incData = '[{ "type" : "DirectAction", "data" : { "innerText" : "innerText", "onBind" : "click" } }]';
                        $(instanceSandBox).attr('incoding', incData);

                        spyOn(ExecutableFactory, 'Create');

                        engine.parse(instanceSandBox);

                        expect(ExecutableFactory.Create).toHaveBeenCalledWith('DirectAction', { "innerText" : "innerText", "onBind" : "click" }, jasmine.any(Object));
                    });

                    it('Should be ignore element with exists runner', function() {

                        $(instanceSandBox).data('incoding-runner', {});

                        engine.parse(instanceSandBox);

                        expect($(instanceSandBox).data('events')).toBeFalsy();

                    });

                    describe('When bind', function() {

                        var stubMetaBind = function(bind) {

                            $(instanceSandBox).removeData('incoding-runner');
                            instanceSandBox = $('#sandbox')
                                .attr('incoding', '[{ "type" : "ExecutableDirectAction", "data" : { "onBind" : "' + bind + '" } }]');

                            var fakeExecute = new ExecutableDirectAction();
                            fakeExecute.onBind = bind;
                            spyOn(fakeExecute, 'execute');

                            ExecutableFactory.Create = function() {
                                return fakeExecute;
                            };
                            return fakeExecute;
                        };

                        it('Should be "click"', function() {

                            var fakeExecute = stubMetaBind('click');

                            engine.parse(instanceSandBox);

                            $(instanceSandBox).trigger('click');
                            expect(fakeExecute.execute).toHaveBeenCalled();
                        });

                        it('Should be "InitIncoding"', function() {

                            var fakeExecute = stubMetaBind('initincoding click');

                            engine.parse(instanceSandBox);

                            expect(fakeExecute.execute).toHaveBeenCalled();
                        });

                        it('Should be "IncChangeUrl"', function() {

                            var fakeExecute = stubMetaBind('incchangeurl');

                            engine.parse(instanceSandBox);

                            runs(function() {
                                document.location.hash = Date.now().toString();
                            });

                            waits(500);

                            runs(function() {
                                expect(fakeExecute.execute).toHaveBeenCalled();
                            });

                        });
                    });

                    it('Should be parse only once', function() {

                        $(instanceSandBox)
                            .data('incoding', '[{ "type" : "Action", "data" : { "onBind" : "click blur" } }]');

                        spyOn(ExecutableFactory, 'Create');

                        engine.parse(instanceSandBox);
                        engine.parse(instanceSandBox);

                        expect(ExecutableFactory.Create.callCount).toEqual(1);

                    });

                });

                it('Should be special binds', function() {
                    $.each($('input', '#incSpecialBinds'), function() {
                        var currentBind = $(this).val();
                        expect(eval('IncSpecialBinds.' + currentBind)).toEqual(currentBind.toLocaleLowerCase());
                    });
                });

                it('Should be DocumentBinds', function() {
                    expect(IncSpecialBinds.DocumentBinds.contains(IncSpecialBinds.IncAjaxBefore)).toBeTruthy();
                    expect(IncSpecialBinds.DocumentBinds.contains(IncSpecialBinds.IncAjaxComplete)).toBeTruthy();
                    expect(IncSpecialBinds.DocumentBinds.contains(IncSpecialBinds.IncChangeUrl)).toBeTruthy();
                    expect(IncSpecialBinds.DocumentBinds.contains(IncSpecialBinds.IncAjaxError)).toBeTruthy();
                    expect(IncSpecialBinds.DocumentBinds.contains(IncSpecialBinds.IncAjaxSuccess)).toBeTruthy();
                    expect(IncSpecialBinds.DocumentBinds.contains(IncSpecialBinds.IncInsert)).toBeTruthy();

                    expect(IncSpecialBinds.DocumentBinds.length).toEqual(6);

                });
            });

            describe('When AjaxAdapter', function() {

                var adapter;

                beforeEach(function() {
                    adapter = new AjaxAdapter();
                });

                describe('When request', function() {

                    var isCallback, fakeUrl, ajaxOptions, expectData;

                    beforeEach(function() {
                        isCallback = false;
                        fakeUrl = 'url';

                        ajaxOptions = TestHelper.Instance.GetAjaxOptions({ url : fakeUrl, data : [{ name : 'element', selector : 'val' }] });

                        $.mockjax({
                            url : ajaxOptions.url,
                            type : ajaxOptions.type,
                            data : [{ name : 'element', selector : 'val' }],
                            responseText : {
                                data : '',
                                success : true,
                                redirectTo : ''
                            }
                        });
                    });

                    it('Should be request', function() {

                        runs(function() {
                            var closureOptions = {};
                            $.extend(closureOptions, ajaxOptions);
                            adapter.request(closureOptions, function(data) {
                                expect(data instanceof IncodingResult).toBeTruthy();
                                isCallback = true;
                            });
                        });

                        waits(500);

                        runs(function() {
                            expect(isCallback).toBeTruthy();
                        });
                    });

                });

                describe('When params', function() {

                    it('Should params', function() {

                        var ajaxData = [{ name : 'hair', selector : 'hair' }, { name : 'eye', selector : 'red' }];
                        var params = adapter.params(ajaxData);
                        expect(params).toEqual([{ name : 'hair', value : 'hair' }, { name : 'eye', value : 'red' }]);

                    });

                    it('Should params empty array', function() {
                        var params = adapter.params([]);
                        expect(params).toEqual([]);
                    });

                    it('Should params empty selector', function() {
                        var params = adapter.params({ name : 'sandboxCheckbox', selector : '' });
                        expect(params).toEqual([{ name : 'sandboxCheckbox', value : '' }]);
                    });

                    it('Should be params with bool checkbox', function() {

                        var ajaxData = [{ name : 'sandboxCheckbox', selector : true }];
                        var params = adapter.params(ajaxData);
                        expect(params).toEqual([{ name : 'sandboxCheckbox', value : true }]);

                    });

                    it('Should be params with value checkboxs', function() {

                        appendSetFixtures($('<input>').attr({ name : 'sandboxCheckbox', type : 'checkbox' }));

                        var ajaxData = [{ name : 'sandboxCheckbox', selector : 'value,value2' }];
                        var params = adapter.params(ajaxData);
                        expect(params).toEqual([
                            { name : 'sandboxCheckbox', value : 'value' },
                            { name : 'sandboxCheckbox', value : 'value2' }
                        ]);

                    });

                    it('Should be params with value textbox', function() {

                        appendSetFixtures($('<input>').attr({ name : 'sandboxTextbox', type : 'text' }));

                        var ajaxData = [{ name : 'sandboxTextbox', selector : 'value,value2' }];
                        var params = adapter.params(ajaxData);
                        expect(params).toEqual([{ name : 'sandboxTextbox', value : 'value,value2' }]);
                    });

                    it('Should be params with value textare', function() {

                        appendSetFixtures($('<textarea>').attr({ name : 'sandboxTextarea' }));

                        var ajaxData = [{ name : 'sandboxTextarea', selector : 'value,value2' }];
                        var params = adapter.params(ajaxData);
                        expect(params).toEqual([{ name : 'sandboxTextarea', value : 'value,value2' }]);
                    });

                });

            });

            describe('IncodingResult', function() {

                describe('When wrong result', function() {

                    it('Should be with wrong json as string', function() {
                        expect(1);

                        var result = new IncodingResult('{notField:bad}');
                        expect(result.isValid()).toBeFalsy();
                    });

                    it('Should be with wrong json as object', function() {

                        try {
                            new IncodingResult({ fiedl1 : 'Aws', Field2 : 'Aws3' });
                        }
                        catch(e) {
                            expect(e).toEqual('Not valid json result');
                        }

                    });

                });

                describe('When success result', function() {

                    var successJsonResult = { success : true, data : "Message", redirectTo : "" };

                    function BehaviorsSuccessParseServerResult(result) {
                        expect(result.isValid()).toBeTruthy();
                        expect(result.success).toBeTruthy();
                        expect(result.redirectTo).toEqual('');
                        expect(result.data).toEqual('Message');
                    }

                    it('Should be with json as string', function() {
                        expect(4);

                        var result = new IncodingResult(JSON.stringify(successJsonResult));
                        BehaviorsSuccessParseServerResult(result);
                    });

                    it('Should be with json as object', function() {
                        expect(4);

                        var result = new IncodingResult(successJsonResult);
                        BehaviorsSuccessParseServerResult(result);
                    });
                });

                describe('When fail result', function() {

                    function BehaviorsFailParseServerResult(result) {
                        expect(result.isValid()).toBeTruthy();
                        expect(result.success).toBeFalsy();
                        expect(result.data).toEqual('Error message');
                        expect(result.redirectTo).toEqual('');
                    }

                    var failJsonResult = { success : false, data : "Error message", redirectTo : "" };

                    it('Should be with json as string', function() {
                        expect(5);

                        var result = new IncodingResult(JSON.stringify(failJsonResult));
                        BehaviorsFailParseServerResult(result);
                    });

                    it('Should be with json as object', function() {
                        expect(5);

                        var result = new IncodingResult(failJsonResult);
                        BehaviorsFailParseServerResult(result);
                    });
                });

            });

            describe('Executable helpers', function() {

                describe('When ExecutableHelper ToBool', function() {

                    it('Should be true', function() {
                        expect(ExecutableHelper.ToBool('trUE')).toBeTruthy();
                        expect(ExecutableHelper.ToBool(true)).toBeTruthy();
                        expect(ExecutableHelper.ToBool(new Boolean(true))).toBeTruthy();
                    });

                    it('Should be false', function() {
                        expect(ExecutableHelper.ToBool('False')).toBeFalsy();
                        expect(ExecutableHelper.ToBool(false)).toBeFalsy();
                        expect(ExecutableHelper.ToBool('')).toBeFalsy();
                        expect(ExecutableHelper.ToBool('aws')).toBeFalsy();
                    });

                });

                describe('When ExecutableHelper TrySetValue', function() {

                    it('Should be set val to span', function() {
                        var element = TestHelper.Instance.SandboxSpan();
                        var data = 'Data';

                        ExecutableHelper.Instance.TrySetValue(element, data);

                        expect($(element).text()).toEqual(data);
                    });

                    it('Should be set val to textbox', function() {
                        var element = TestHelper.Instance.SandboxTextBox();
                        var data = 'Data';

                        ExecutableHelper.Instance.TrySetValue(element, data);

                        expect($(element).val()).toEqual(data);
                    });

                    it('Should be set radio', function() {
                        var data = 'aws';
                        var elementWithData = TestHelper.Instance.SandboxRadioButton(false, data);
                        var elementWithoutData = TestHelper.Instance.SandboxRadioButton(true, '123');

                        ExecutableHelper.Instance.TrySetValue(elementWithData, data);

                        if ($.browser.webkit) {
                            expect(elementWithData).toHaveProp('checked');
                        }
                        else {
                            expect(elementWithData).toHaveProp('checked', true);
                        }

                        expect(elementWithoutData).toHaveProp('checked', false);
                    });

                    describe('Select', function() {

                        it('Should be set', function() {
                            var data = 'Data';
                            var element = TestHelper.Instance.SandboxSelect([
                                { value : data, selected : false },
                                { value : '123', selected : true }
                            ]);

                            ExecutableHelper.Instance.TrySetValue(element, data);

                            expect($(element).val()).toEqual(data);
                        });

                        it('Should be set more select', function() {

                            var element = TestHelper.Instance.SandboxSelect([
                                { value : 'Value', selected : false },
                                { value : '123', selected : true }
                            ]);
                            var element2 = TestHelper.Instance.SandboxSelect([
                                { value : '123', selected : true },
                                { value : 'Value2', selected : false }
                            ]);

                            ExecutableHelper.Instance.TrySetValue($(element).add(element2), 'Value,Value2');

                            expect($(element).val()).toEqual('Value');
                            expect($(element2).val()).toEqual('Value2');
                        });

                        it('Should be set multiselect', function() {

                            var element = TestHelper.Instance.SandboxMultiSelect([
                                { value : 1, selected : false },
                                { value : 2, selected : true },
                                { value : 3, selected : true },
                                { value : 4, selected : false }
                            ]);

                            ExecutableHelper.Instance.TrySetValue(element, '1,4');

                            expect($(element).val()).toEqual(['1', '4']);
                        });

                    });

                    describe('Checkbox', function() {

                        it('Should be set true checked checkbox', function() {
                            var element = TestHelper.Instance.SandboxCheckBox();
                            $(element).prop('checked', 'checked');

                            ExecutableHelper.Instance.TrySetValue('[name="sandboxCheckbox"]', true);

                            expect(element).toHaveProp('checked', true);
                        });

                        it('Should be set true unchecked checkbox', function() {
                            var element = TestHelper.Instance.SandboxCheckBox();

                            ExecutableHelper.Instance.TrySetValue('[name="sandboxCheckbox"]', true);
                            expect(element).toHaveProp('checked', true);

                        });

                        it('Should be set false checked checkbox ', function() {
                            var element = TestHelper.Instance.SandboxCheckBox();
                            $(element).prop('checked', 'checked');

                            ExecutableHelper.Instance.TrySetValue('[name="sandboxCheckbox"]', 'fAlse');

                            expect(element).toHaveProp('checked', false);
                        });

                        it('Should be set false unchecked checkbox ', function() {
                            var element = TestHelper.Instance.SandboxCheckBox();

                            ExecutableHelper.Instance.TrySetValue('[name="sandboxCheckbox"]', 'fAlse');
                            expect(element).toHaveProp('checked', false);

                        });

                        it('Should be set array to checkbox', function() {

                            var red = TestHelper.Instance.SandboxCheckBox(false, 'red');
                            var green = TestHelper.Instance.SandboxCheckBox(false, 'green');
                            var blue = TestHelper.Instance.SandboxCheckBox(true, 'blue');

                            ExecutableHelper.Instance.TrySetValue('[name="sandboxCheckbox"]', ['red', 'green']);

                            expect(red).toHaveProp('checked', true);
                            expect(green).toHaveProp('checked', true);
                            expect(blue).toHaveProp('checked', false);
                        });

                    });

                });

                describe('When ExecutableHelper TryGetVal', function() {

                    describe('When Common', function() {

                        it('Should be value', function() {
                            var selector = '{{item}},{value]}}}{{{';
                            var val = ExecutableHelper.Instance.TryGetVal(selector);

                            expect(val).toEqual(selector);
                        });

                        it('Should be value with html', function() {
                            var selector = "Aws <br/> <div>my new content </div> <input/>";
                            expect(ExecutableHelper.Instance.TryGetVal(selector)).toEqual(selector);
                        });

                        it('Should be value with html 2', function() {
                            var selector = "<div>my new content </div>";
                            expect(ExecutableHelper.Instance.TryGetVal(selector)).toEqual(selector);
                        });

                        it('Should be value as tag', function() {
                            var selector = "input";
                            expect(ExecutableHelper.Instance.TryGetVal(selector)).toEqual(selector);
                        });

                        it('Should be jquery object', function() {

                            var area = TestHelper.Instance.SandboxTextArea();
                            area.val('aws');
                            var res = ExecutableHelper.Instance.TryGetVal(area);

                            expect(res).toEqual('aws');
                        });

                        it('Should be jquery selector', function() {
                            TestHelper.Instance.SandboxTextArea().val('aws');
                            var res = ExecutableHelper.Instance.TryGetVal("$('#sandboxTextarea')");

                            expect(res).toEqual(res);
                        });

                        it('Should be with jquery selector as string', function() {
                            TestHelper.Instance.SandboxTextArea().val('aws');
                            var res = ExecutableHelper.Instance.TryGetVal(String('$("#sandboxTextarea")'));

                            expect(res).toEqual('aws');
                        });

                        it('Should be number', function() {
                            var res = ExecutableHelper.Instance.TryGetVal(1);
                            expect(res).toEqual(1);
                        });

                        it('Should be number as type number', function() {
                            var res = ExecutableHelper.Instance.TryGetVal(new Number(1));
                            expect(res).toEqual(1);
                        });

                        it('Should be func', function() {
                            var selector = function() {
                                return 5;
                            };

                            var val = ExecutableHelper.Instance.TryGetVal(selector);
                            expect(val).toEqual(selector);
                        });

                        it('Should be bool', function() {
                            var val = ExecutableHelper.Instance.TryGetVal(true);
                            expect(val).toBeTruthy();
                        });

                        it('Should be array', function() {
                            var selector = ['aws', 'aws2'];

                            var val = ExecutableHelper.Instance.TryGetVal(selector);
                            expect(val).toEqual(selector);
                        });

                    });

                    describe('When DOM', function() {

                        it('Should be script', function() {
                            var val = ExecutableHelper.Instance.TryGetVal($("#sandboxScript"));

                            var length = 385;
                            if (jQuery.browser.msie) {
                                if (jQuery.browser.version <= 8) {
                                    length = 390;
                                }
                            }
                            expect(val.length).toEqual(length);

                        });

                        it('Should be checkbox true', function() {
                            appendSetFixtures($('<input>').attr({
                                id : 'sandboxCheckbox',
                                name : 'sandboxCheckbox',
                                value : false,
                                type : 'hidden'
                            }));
                            TestHelper.Instance.SandboxCheckBox(true, true);

                            var val = ExecutableHelper.Instance.TryGetVal('$("[name=sandboxCheckbox]"');
                            expect(val).toBeTruthy();
                        });

                        it('Should be checkbox false', function() {
                            var val = ExecutableHelper.Instance.TryGetVal(TestHelper.Instance.SandboxCheckBox(false));
                            expect(val).toBeFalsy();
                        });

                        it('Should be checkboxs values', function() {
                            TestHelper.Instance.SandboxCheckBox(false, 'red');
                            TestHelper.Instance.SandboxCheckBox(true, 'green');
                            TestHelper.Instance.SandboxCheckBox(true, 'blue');

                            var val = ExecutableHelper.Instance.TryGetVal('$("[name=sandboxCheckbox]")');
                            expect(val).toEqual(['green', 'blue']);
                        });

                        it('Should be radio buttons', function() {
                            TestHelper.Instance.SandboxRadioButton(false, 'value');
                            TestHelper.Instance.SandboxRadioButton(false, 'value2');
                            TestHelper.Instance.SandboxRadioButton(true, 'value3');

                            var val = ExecutableHelper.Instance.TryGetVal('$("[name=sandboxRadiobutton]")');
                            expect(val).toEqual('value3');
                        });

                        it('Should be select', function() {
                            var val = ExecutableHelper.Instance.TryGetVal($(TestHelper.Instance.SandboxSelect())
                                .append(TestHelper.Instance.CreateOption(1, true), TestHelper.Instance.CreateOption(2, false)));

                            expect(val).toEqual('1');
                        });

                        it('Should be select values', function() {
                            TestHelper.Instance.SandboxSelect([{ value : 'value', selected : true }, { value : 'value', selected : false }]);
                            TestHelper.Instance.SandboxSelect([{ value : 'value2', selected : false }, { value : 'value3', selected : true }]);

                            var val = ExecutableHelper.Instance.TryGetVal('$("[name=sandboxSelect]")');
                            expect(val).toEqual(['value', 'value3']);
                        });

                        it('Should be select values ignore empty ', function() {
                            TestHelper.Instance.SandboxSelect([{ value : '', selected : true }, { value : 'value', selected : false }]);
                            TestHelper.Instance.SandboxSelect([{ value : 'value2', selected : false }, { value : 'value3', selected : true }]);

                            var val = ExecutableHelper.Instance.TryGetVal('$("[name=sandboxSelect]")');
                            expect(val).toEqual(['value3']);
                        });

                        it('Should be multiple select', function() {
                            var selector = $(TestHelper.Instance.SandboxMultiSelect())
                                .append(TestHelper.Instance.CreateOption(1, true), TestHelper.Instance.CreateOption(2, true), TestHelper.Instance.CreateOption(3, false));

                            var val = ExecutableHelper.Instance.TryGetVal(selector);
                            expect(val).toEqual(['1', '2']);
                        });

                        it('Should be multiple select with empty values', function() {
                            var selector = $(TestHelper.Instance.SandboxMultiSelect())
                                .append(TestHelper.Instance.CreateOption('', true), TestHelper.Instance.CreateOption(2, true), TestHelper.Instance.CreateOption('', true));

                            var val = ExecutableHelper.Instance.TryGetVal(selector);
                            expect(val).toEqual(['2']);
                        });

                        it('Should be textarea', function() {
                            var expectData = 'test';

                            var val = ExecutableHelper.Instance.TryGetVal($(TestHelper.Instance.SandboxTextArea()).val(expectData));
                            expect(val).toEqual(expectData);
                        });

                        it('Should be hidden', function() {
                            var expectData = 'expected';

                            var val = ExecutableHelper.Instance.TryGetVal($(TestHelper.Instance.SandboxHidden()).val(expectData));
                            expect(val).toEqual(expectData);
                        });

                    });

                    describe('When Incoding', function() {

                        it('Should be query string', function() {
                            var val = ExecutableHelper.Instance.TryGetVal($('#Selector_Incoding_QueryString').val());
                            expect(val).toEqual('incValue');
                        });

                        it('Should be query string with undefined', function() {
                            var val = ExecutableHelper.Instance.TryGetVal($('#Selector_Incoding_QueryString_Undefined').val());
                            expect(val).toEqual('');
                        });

                        it('Should be hash query string', function() {
                            window.location.hash = "!Index/Home?incodingParam=aws";

                            var val = ExecutableHelper.Instance.TryGetVal($('#Selector_Incoding_HashQueryString').val());
                            expect(val).toEqual('aws');
                        });

                        it('Should be hash query string with undefined', function() {
                            window.location.hash = "";

                            var val = ExecutableHelper.Instance.TryGetVal($('#Selector_Incoding_HashQueryString_Undefined').val());
                            expect(val).toEqual('');
                        });

                        it('Should be hash', function() {
                            window.location.hash = "!Index/Home?";

                            var val = ExecutableHelper.Instance.TryGetVal($('#Selector_Incoding_Hash').val());
                            expect(val).toEqual('Index/Home');
                        });

                        it('Should be cookie', function() {
                            var cookiesVal = 'cookiesVal';
                            $.cookie('incodingParam', cookiesVal);

                            var val = ExecutableHelper.Instance.TryGetVal($('#Selector_Incoding_Cookie').val());
                            expect(val).toEqual(cookiesVal);
                        });

                        it('Should be ajax', function() {
                            spyOn(AjaxAdapter.Instance, 'request');
                            TestHelper.Instance.SandboxTextBox({ value : 'typeValue' });

                            ExecutableHelper.Instance.TryGetVal($('#Selector_Incoding_Ajax').val());

                            expect(AjaxAdapter.Instance.request).toHaveBeenCalledWith({ cache : false, data : [{ name : 'Type', selector : 'typeValue' }], url : '/Labs/FetchCountry', async : false }, function() {
                            });
                        });

                    });

                    describe('When Js', function() {

                        describe('When DateTime', function() {

                            it('Should be getFullYear', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_DateTime_GetFullYear').val());
                                expect(res).toEqual(new Date().getFullYear());
                            });

                            it('Should be GetDay', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_DateTime_GetDay').val());
                                expect(res).toEqual(new Date().getDay());
                            });

                            it('Should be GetTimezoneOffset', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_DateTime_GetTimezoneOffset').val());
                                expect(res).toEqual(new Date().getTimezoneOffset());
                            });

                            it('Should be GetTimezoneOffset', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_DateTime_GetTimezoneOffset').val());
                                expect(res).toEqual(new Date().getTimezoneOffset());
                            });

                            it('Should be ToDateString', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_DateTime_ToDateString').val());
                                expect(res).toEqual(new Date().toDateString());
                            });

                            it('Should be ToTimeString', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_DateTime_ToTimeString').val());
                                expect(res).toEqual(new Date().toTimeString());
                            });

                        });

                        describe('When Location', function() {

                            it('Should be Href', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_Location_Href').val());
                                expect(res).toEqual(window.location.href);
                            });

                            it('Should be Host', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_Location_Host').val());
                                expect(res).toEqual(window.location.host);
                            });

                            it('Should be HostName', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_Location_HostName').val());
                                expect(res).toEqual(window.location.hostname);
                            });

                            it('Should be PathName', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_Location_PathName').val());
                                expect(res).toEqual(window.location.pathname);
                            });

                            it('Should be Protocol', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_Location_Protocol').val());
                                expect(res).toEqual(window.location.protocol);
                            });

                            it('Should be Search', function() {
                                var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Js_Location_Search').val());
                                expect(res).toEqual(window.location.search);
                            });

                        });

                    });

                });

                describe('When ExecutableHelper Compare', function() {

                    it('Should be exception if not found method', function() {
                        expect(function() {
                            ExecutableHelper.Compare('AA', 'aa', 'notfoundmethod');
                        }).toThrow(new IncClientException({ message : "Can't compare by method notfoundmethod" }, {}));
                    });

                    describe('Common', function() {

                        it('Should be equal bool with string', function() {
                            expect(ExecutableHelper.Compare(true, 'True', 'equal')).toBeTruthy();
                        });

                        it('Should be equal undefined with string empty', function() {
                            expect(ExecutableHelper.Compare(undefined, '', 'equal')).toBeTruthy();
                        });

                        it('Should be equal string empty with undefined', function() {
                            expect(ExecutableHelper.Compare('', undefined, 'equal')).toBeTruthy();
                        });

                        it('Should be equal string with undefined', function() {
                            expect(ExecutableHelper.Compare('awws', undefined, 'equal')).toBeFalsy();
                        });

                        it('Should be equal string with int', function() {
                            expect(ExecutableHelper.Compare('10', 10, 'equal')).toBeTruthy();
                        });

                        it('Should be equal with ignore case', function() {
                            expect(ExecutableHelper.Compare('AA', 'aa', 'equal')).toBeTruthy();
                        });

                    });

                    describe('Equal', function() {

                        it('Should be equal true', function() {
                            expect(ExecutableHelper.Compare('aws', 'aws', 'equal')).toBeTruthy();
                        });

                        it('Should be equal false', function() {
                            expect(ExecutableHelper.Compare('aws', 'aws1', 'equal')).toBeFalsy();
                        });

                    });

                    describe('Is Empty', function() {

                        it('Should be empty array true', function() {
                            expect(ExecutableHelper.Compare([], undefined, 'isempty')).toBeTruthy();
                        });

                        it('Should be empty array false', function() {
                            expect(ExecutableHelper.Compare([1, 2], undefined, 'isempty')).toBeFalsy();
                        });

                        it('Should be empty object true', function() {
                            expect(ExecutableHelper.Compare({}, undefined, 'isempty')).toBeTruthy();
                        });

                        it('Should be empty object false', function() {
                            expect(ExecutableHelper.Compare({ prop : 'aws' }, undefined, 'isempty')).toBeFalsy();
                        });

                    });

                    describe('NotEqual', function() {

                        it('Should be not equal false', function() {
                            expect(ExecutableHelper.Compare('aws', 'aws', 'notequal')).toBeFalsy();
                        });

                        it('Should be not equal true', function() {
                            expect(ExecutableHelper.Compare('aws', 'aws1', 'notequal')).toBeTruthy();
                        });

                    });

                    describe('LessThan', function() {

                        it('Should be less than false', function() {
                            expect(ExecutableHelper.Compare(10, '5', 'lessthan')).toBeFalsy();
                        });

                        it('Should be less than true', function() {
                            expect(ExecutableHelper.Compare(5, '10', 'lessthan')).toBeTruthy();
                        });

                    });

                    describe('LessThanOrEqual', function() {

                        it('Should be less than false', function() {
                            expect(ExecutableHelper.Compare(10, '5', 'lessthanorequal')).toBeFalsy();
                        });

                        it('Should be less than true', function() {
                            expect(ExecutableHelper.Compare(5, '10', 'lessthanorequal')).toBeTruthy();
                        });

                        it('Should be less than true equal', function() {
                            expect(ExecutableHelper.Compare(5, '5', 'lessthanorequal')).toBeTruthy();
                        });

                    });

                    describe('GreaterThan', function() {

                        it('Should be greater than false', function() {
                            expect(ExecutableHelper.Compare(5, '10', 'greaterthan')).toBeFalsy();
                        });

                        it('Should be greater than true', function() {
                            expect(ExecutableHelper.Compare(10, '5', 'greaterthan')).toBeTruthy();
                        });

                    });

                    describe('GreaterThanOrEqual', function() {

                        it('Should be greater than false', function() {
                            expect(ExecutableHelper.Compare(5, '10', 'greaterthanorequal')).toBeFalsy();
                        });

                        it('Should be greater than true', function() {
                            expect(ExecutableHelper.Compare(10, '5', 'greaterthanorequal')).toBeTruthy();
                        });

                        it('Should be greater than true equal', function() {
                            expect(ExecutableHelper.Compare(10, '10', 'greaterthanorequal')).toBeTruthy();
                        });

                    });

                    describe('Contains', function() {

                        it('Should be contains true', function() {
                            expect(ExecutableHelper.Compare('Vlad', 'ad', 'contains')).toBeTruthy();
                        });

                        it('Should be contains false', function() {
                            expect(ExecutableHelper.Compare('Vlad', 'sd', 'contains')).toBeFalsy();
                        });

                    });

                });

                describe('When ExecutableHelper RedirectTo', function() {

                    var isBind;

                    beforeEach(function() {
                        isBind = false;
                        $(document).bind(IncSpecialBinds.IncChangeUrl, function() {
                            isBind = true;
                        });
                    });

                    it('Should be redirect', function() {

                        runs(function() {
                            document.location.hash = 'aws';
                            var anotherUrl = document.location.href.replace(document.location.hash, '#!' + Date.now().toString());
                            ExecutableHelper.RedirectTo(anotherUrl);
                        });

                        waits(200);

                        runs(function() {
                            expect(isBind).toBeTruthy();
                        });
                    });

                    it('Should be redirect to self', function() {
                        runs(function() {
                            ExecutableHelper.RedirectTo(document.location.href);
                        });

                        waits(100);

                        runs(function() {
                            expect(isBind).toBeTruthy();
                        });
                    });

                    it('Should be redirect to self with encode', function() {
                        runs(function() {
                            document.location.hash = '~/Areas/Admin/Views/Category/Index.cshtml'; //set encode value
                            var selfEncode = '{0}!#{1}'.f(document.location.href.split("#")[0], '~%2FAreas%2FAdmin%2FViews%2FCategory%2FIndex.cshtml');
                            ExecutableHelper.RedirectTo(selfEncode);
                        });

                        waits(100);

                        runs(function() {
                            expect(isBind).toBeTruthy();
                        });
                    });

                });

                describe('When ExecutableHelper Filter', function() {

                    it('Should be array', function() {

                        var filter = {
                            isSatisfied : function(item) {
                                return item == 534;
                            }
                        };

                        var res = ExecutableHelper.Filter([12, 534, 4], filter);

                        expect(res).toEqual([534]);
                    });

                    it('Should be object satisfied', function() {

                        var filter = {
                            isSatisfied : function(item) {
                                return item == 534;
                            }
                        };

                        var res = ExecutableHelper.Filter(534, filter);

                        expect(res).toEqual(534);
                    });

                    it('Should not be object satisfied', function() {

                        var filter = {
                            isSatisfied : function(item) {
                                return item == 534;
                            }
                        };

                        var res = ExecutableHelper.Filter(200, filter);

                        expect(res).toEqual({});
                    });

                });

                describe('When ExecutableHelper IsNullOrEmpty', function() {

                    it('Should be empty string', function() {
                        expect(ExecutableHelper.IsNullOrEmpty('')).toBeTruthy();
                    });

                    it('Should be "undefined"', function() {
                        expect(ExecutableHelper.IsNullOrEmpty("undefined")).toBeTruthy();
                    });

                    it('Should be "undefined" as object', function() {
                        expect(ExecutableHelper.IsNullOrEmpty(undefined)).toBeTruthy();
                    });

                    it('Should be empty object', function() {
                        expect(ExecutableHelper.IsNullOrEmpty({})).toBeTruthy();
                    });

                    it('Should be null', function() {
                        expect(ExecutableHelper.IsNullOrEmpty(null)).toBeTruthy();
                    });

                    it('Should be undefined', function() {
                        expect(ExecutableHelper.IsNullOrEmpty(undefined)).toBeTruthy();
                    });

                    it('Should be empty array', function() {
                        expect(ExecutableHelper.IsNullOrEmpty([])).toBeTruthy();
                    });

                    it('Should be empty array object', function() {
                        expect(ExecutableHelper.IsNullOrEmpty(Array())).toBeTruthy();
                    });

                    it('Should not be empty string', function() {
                        expect(ExecutableHelper.IsNullOrEmpty('aws')).toBeFalsy();
                    });

                    it('Should not be empty string object', function() {
                        expect(ExecutableHelper.IsNullOrEmpty(String('aws'))).toBeFalsy();
                    });

                    it('Should not be empty number', function() {
                        expect(ExecutableHelper.IsNullOrEmpty(5)).toBeFalsy();
                    });

                    it('Should not be empty number object', function() {
                        expect(ExecutableHelper.IsNullOrEmpty(Number(5))).toBeFalsy();
                    });

                    it('Should not be empty date object', function() {
                        expect(ExecutableHelper.IsNullOrEmpty(Date('12/12/12/'))).toBeFalsy();
                    });

                    it('Should not be empty array', function() {
                        expect(ExecutableHelper.IsNullOrEmpty([1])).toBeFalsy();
                    });

                    it('Should not be empty array object', function() {
                        var array = Array();
                        array.push(1);
                        expect(ExecutableHelper.IsNullOrEmpty(array)).toBeFalsy();
                    });

                    it('Should not empty object with property', function() {
                        expect(ExecutableHelper.IsNullOrEmpty({ prop : 'aws' })).toBeFalsy();
                    });

                });

                describe('When ExecutableHelper Encode and Decode', function() {

                    var original = 'aws&?/=:#123@';

                    it('Should be encode', function() {

                        var afterIncEncode = ExecutableHelper.UrlEncode(original);
                        var afterEncode = encodeURIComponent(original);

                        expect(afterIncEncode).toEqual(afterEncode);
                    });

                    it('Should be encode with empty', function() {
                        var afterIncEncode = ExecutableHelper.UrlEncode('');
                        expect(afterIncEncode).toEqual(afterIncEncode);
                    });

                    it('Should be encode with undefined', function() {
                        var afterIncEncode = ExecutableHelper.UrlEncode(undefined);
                        expect(afterIncEncode).toEqual(afterIncEncode);
                    });

                    it('Should be encode array', function() {

                        var originalArray = [original];
                        var afterIncEncode = ExecutableHelper.UrlEncode(originalArray);
                        var afterEncode = [encodeURIComponent(original)];

                        expect(afterIncEncode).toEqual(afterEncode);
                    });

                    it('Should be encode with value special cahr', function() {
                        original = '&?/=:#@';

                        var afterIncEncode = ExecutableHelper.UrlEncode(original);
                        var afterEncode = encodeURIComponent(original);

                        expect(afterIncEncode).toEqual(afterEncode);
                    });

                    it('Should be decode', function() {
                        var decode = ExecutableHelper.UrlDecode(encodeURIComponent(original));
                        expect(decode).toEqual(original);
                    });

                });

            });

        });

        describe('Template', function() {

            describe('When special template', function() {

                var data;
                beforeEach(function() {
                    data = [{ price : 20 }, { price : 30 }];
                });

                it('Should be sum', function() {
                    var sumTemplate = 'Summary price : {{#IncTemplateSum}}price{{/IncTemplateSum}}';

                    var view = new IncMustacheTemplate(data, sumTemplate).render();
                    expect(view).toEqual('Summary price : 50');

                });

                it('Should be sum with empty', function() {
                    var sumTemplate = 'Summary price : {{#IncTemplateSum}}price{{/IncTemplateSum}}';

                    var view = new IncMustacheTemplate([], sumTemplate).render();
                    expect(view).toEqual('Summary price : 0');

                });

                it('Should be count', function() {
                    var template = 'Count: {{#IncTemplateCount}}{{/IncTemplateCount}}';

                    var view = new IncMustacheTemplate(data, template).render();
                    expect(view).toEqual('Count: 2');

                });

                it('Should be min', function() {
                    var template = 'Min: {{#IncTemplateMin}}price{{/IncTemplateMin}}';

                    var view = new IncMustacheTemplate(data, template).render();
                    expect(view).toEqual('Min: 20');

                });

                it('Should be max', function() {
                    var template = 'Max: {{#IncTemplateMax}}price{{/IncTemplateMax}}';

                    var view = new IncMustacheTemplate(data, template).render();
                    expect(view).toEqual('Max: 30');

                });

                it('Should be first', function() {
                    var template = 'First: {{#IncTemplateFirst}}price{{/IncTemplateFirst}}';

                    var view = new IncMustacheTemplate(data, template).render();
                    expect(view).toEqual('First: 20');

                });

                it('Should be last', function() {
                    var template = 'Last: {{#IncTemplateLast}}price{{/IncTemplateLast}}';

                    var view = new IncMustacheTemplate(data, template).render();
                    expect(view).toEqual('Last: 30');

                });

            });

            describe('When Mustaches', function() {

                var data, template;

                beforeEach(function() {
                    data = {
                        title : "Joe",
                        calc : function() {
                            return 2 + 4;
                        }
                    };
                    template = '{{#data}} {{title}} "spends" {{calc}} {{/data}}';
                });

                it('Should be render item', function() {

                    var view = new IncMustacheTemplate(data, template).render();
                    expect(view).toEqual(' Joe "spends" 6 ');

                });

                it('Should be render array item', function() {

                    var view = new IncMustacheTemplate([data, data], template).render();
                    expect(view).toEqual(' Joe "spends" 6  Joe "spends" 6 ');
                });

            });

        });

        describe('Executable', function() {

            describe('When ExecutableBase', function() {

                it('Should be default', function() {
                    var executable = new ExecutableBase();
                    expect(executable.onBind).toEqual('');
                    expect(executable.self).toEqual('');
                    expect(executable.timeOut).toEqual(0);
                    expect(executable.interval).toEqual(0);
                    expect(executable.intervalId).toEqual('');
                    expect(executable.target).toEqual('');
                    expect(executable.ands).toEqual(null);
                    expect(executable.getTarget()).toEqual('');
                });

                describe('Execute', function() {

                    var executable, expectedData;

                    beforeEach(function() {
                        expectedData = 'aws';
                        executable = new ExecutableBase();
                        executable.timeOut = 0;
                        executable.isValid = function(data) {
                            return data == expectedData;
                        };
                        executable.getTarget = function() {
                            return 5;
                        };
                    });

                    it('Should be execute', function() {
                        var callExecute = false;
                        executable.internalExecute = function(data) {
                            callExecute = data == expectedData;
                        };

                        executable.execute(expectedData);

                        expect(callExecute).toBeTruthy();
                        expect(executable.target).toEqual(5);
                    });

                    it('Should be execute broken valid', function() {
                        executable.isValid = function() {
                            return false;
                        };
                        var callExecute = false;
                        executable.internalExecute = function(data) {
                            callExecute = true;
                        };

                        executable.execute(expectedData);

                        expect(callExecute).toBeFalsy();
                        expect(executable.target).toEqual(5);
                    });

                    it('Should be execute time out', function() {
                        ExecutableHelper.Compare;
                        var callCount = 0;
                        executable.internalExecute = function(data) {
                            if (data == expectedData) {
                                callCount++;
                            }
                        };
                        executable.timeOut = 500;

                        runs(function() {
                            executable.execute(expectedData);
                            expect(callCount).toEqual(0);
                        });

                        waits(500);

                        runs(function() {
                            expect(callCount).toEqual(1);
                        });

                        waits(1500);

                        runs(function() {
                            expect(callCount).toEqual(1);
                        });

                    });

                    it('Should be execute interval', function() {
                        ExecutableHelper.Compare;
                        var callCount = 0;
                        executable.internalExecute = function(data) {
                            if (data == expectedData) {
                                callCount++;
                            }
                        };
                        executable.interval = 500;
                        executable.intervalId = '156EE3CE_9799_4BCB_9EE7_7FA61FC277B8';

                        runs(function() {
                            executable.execute(expectedData);
                            expect(callCount).toEqual(0);
                            expect(ExecutableBase.IntervalIds[executable.intervalId]).toBeGreaterThan(1);
                        });

                        waits(500);

                        runs(function() {
                            expect(callCount).toEqual(1);
                        });

                        waits(500);

                        runs(function() {
                            expect(callCount).toEqual(2);
                            window.clearInterval(ExecutableBase.IntervalIds[executable.intervalId]);
                        });

                        waits(1500);

                        runs(function() {
                            expect(callCount).toEqual(2);
                        });

                    });

                });

                describe('TryGetVal', function() {

                    var executable, target, value;

                    beforeEach(function() {
                        value = 'value';
                        spyOn(ExecutableHelper.Instance, 'TryGetVal').andReturn(value);

                        target = TestHelper.Instance.SandboxTextBox();
                        executable = new ExecutableBase();
                        executable.self = instanceSandBox;
                        executable.target = target;
                    });

                    it('Should be variable as value', function() {
                        var res = executable.tryGetVal('$(aws');
                        expect(res).toEqual(value);
                        expect(ExecutableHelper.Instance.TryGetVal).toHaveBeenCalledWith('$(aws');
                    });

                    it('Should be variable as jquery object', function() {
                        var res = executable.tryGetVal(instanceSandBox);
                        expect(res).toEqual(value);
                        expect(ExecutableHelper.Instance.TryGetVal).toHaveBeenCalledWith(instanceSandBox);
                    });

                });

                describe('IsValid', function() {

                    var executable;

                    beforeEach(function() {
                        executable = new ExecutableBase();
                    });

                    it('Should be null ands', function() {
                        expect(executable.isValid()).toBeTruthy();
                    });

                    describe('Ands', function() {
                        var trueConditional, falseConditional, expectedData;

                        beforeEach(function() {
                            expectedData = 'aws';

                            trueConditional = {
                                isSatisfied : function(data) {
                                }
                            };
                            spyOn(trueConditional, 'isSatisfied').andReturn(true);

                            falseConditional = {
                                isSatisfied : function(data) {
                                }
                            };
                            spyOn(falseConditional, 'isSatisfied').andReturn(false);

                            spyOn(ConditionalFactory, 'Create').andCallFake(function(conditional) {
                                return conditional.type ? trueConditional : falseConditional;
                            });
                        });

                        it('Should be first group all true', function() {
                            executable.ands = [[{ type : true }, { type : true }], [{ type : false }]];

                            expect(executable.isValid(expectedData)).toBeTruthy();

                            expect(trueConditional.isSatisfied.callCount).toEqual(2);
                            expect(trueConditional.isSatisfied).toHaveBeenCalledWith(expectedData);
                            expect(falseConditional.isSatisfied).not.toHaveBeenCalled();

                        });

                        it('Should be last group all true', function() {
                            executable.ands = [[{ type : false }], [{ type : true }, { type : true }]];

                            expect(executable.isValid(expectedData)).toBeTruthy();
                            expect(trueConditional.isSatisfied.callCount).toEqual(2);
                            expect(falseConditional.isSatisfied.callCount).toEqual(1);
                            expect(falseConditional.isSatisfied).toHaveBeenCalledWith(expectedData);
                        });

                        it('Should be group has false', function() {
                            executable.ands = [[{ type : true }, { type : false }]];

                            expect(executable.isValid(expectedData)).toBeFalsy();
                            expect(trueConditional.isSatisfied).toHaveBeenCalledWith(expectedData);
                            expect(falseConditional.isSatisfied).toHaveBeenCalledWith(expectedData);

                        });

                        it('Should be group false', function() {
                            executable.ands = [[{ type : false }, { type : false }]];

                            expect(executable.isValid(expectedData)).toBeFalsy();
                            expect(falseConditional.isSatisfied.callCount).toEqual(1);
                            expect(falseConditional.isSatisfied).toHaveBeenCalledWith(expectedData);
                        });

                    });

                });

            });

            describe('When ExecutableActionBase', function() {

                var action, result, state;

                beforeEach(function() {

                    state = {
                        success : [jasmine.createSpyObj('successCallback', ['execute'])],
                        error : [jasmine.createSpyObj('successCallback', ['execute'])],
                        complete : [jasmine.createSpyObj('successCallback', ['execute'])],
                        breakes : [jasmine.createSpyObj('successCallback', ['execute'])],
                    };
                    result = { redirectTo : '', success : true, data : 'data' };

                    action = new ExecutableActionBase();
                    action.jsonData = {};
                    spyOn(ExecutableHelper, "RedirectTo");
                });

                it('Should be redirect', function() {
                    result.redirectTo = 'url';

                    action.complete(result, state);

                    expect(ExecutableHelper.RedirectTo).toHaveBeenCalledWith(result.redirectTo);
                    expect(state.success[0].execute).not.toHaveBeenCalled();
                    expect(state.error[0].execute).not.toHaveBeenCalled();
                    expect(state.complete[0].execute).not.toHaveBeenCalled();
                    expect(state.breakes[0].execute).not.toHaveBeenCalled();
                });

                it('Should be filter ', function() {
                    var filter = 'filter';
                    spyOn(ConditionalFactory, 'Create').andReturn(filter);
                    spyOn(ExecutableHelper, 'Filter');

                    action.jsonData.filterResult = 'filterResult';

                    action.complete(result, state);

                    expect(ConditionalFactory.Create).toHaveBeenCalledWith(action.jsonData.filterResult, action);
                    expect(ExecutableHelper.Filter).toHaveBeenCalledWith(result.data, filter);
                });

                it('Should be success', function() {
                    action.complete(result, state);

                    expect(ExecutableHelper.RedirectTo).not.toHaveBeenCalled();
                    expect(state.success[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.complete[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.error[0].execute).not.toHaveBeenCalled();
                    expect(state.breakes[0].execute).not.toHaveBeenCalled();
                });

                it('Should be success with break', function() {
                    state.success[0] = {
                        execute : function() {
                        }
                    };
                    spyOn(state.success[0], 'execute').andThrow(new IncClientException());

                    action.complete(result, state);

                    expect(ExecutableHelper.RedirectTo).not.toHaveBeenCalled();

                    expect(state.success[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.complete[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.breakes[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.error[0].execute).not.toHaveBeenCalled();
                });

                it('Should be error', function() {
                    result.success = false;

                    action.complete(result, state);

                    expect(ExecutableHelper.RedirectTo).not.toHaveBeenCalled();
                    expect(state.error[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.complete[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.success[0].execute).not.toHaveBeenCalled();
                    expect(state.breakes[0].execute).not.toHaveBeenCalled();
                });

                it('Should be error with break', function() {
                    result.success = false;
                    state.error[0] = {
                        execute : function() {
                        }
                    };
                    spyOn(state.error[0], 'execute').andThrow(new IncClientException());

                    action.complete(result, state);

                    expect(ExecutableHelper.RedirectTo).not.toHaveBeenCalled();
                    expect(state.error[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.complete[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.breakes[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.success[0].execute).not.toHaveBeenCalled();
                });

                it('Should be complete with break', function() {
                    state.complete[0] = {
                        execute : function() {
                        }
                    };
                    spyOn(state.complete[0], 'execute').andThrow(new IncClientException());

                    action.complete(result, state);

                    expect(ExecutableHelper.RedirectTo).not.toHaveBeenCalled();
                    expect(state.complete[0].execute).toHaveBeenCalledWith(result.data);
                    expect(state.breakes[0].execute).toHaveBeenCalledWith(result.data);
                });

            });

            describe('Action', function() {

                var fakeSuccess, fakeError, fakeComplete, state;

                beforeEach(function() {

                    fakeSuccess = new ExecutableBase();
                    fakeComplete = new ExecutableBase();
                    fakeError = new ExecutableBase();

                    spyOn(fakeSuccess, 'execute');
                    spyOn(fakeError, 'execute');
                    spyOn(fakeComplete, 'execute');

                    state = { success : [fakeSuccess], error : [fakeError], complete : [fakeComplete] };
                });

                describe('When ExecutableDirectAction', function() {

                    var action;

                    beforeEach(function() {
                        action = new ExecutableDirectAction();
                        action.self = instanceSandBox;
                        action.target = instanceSandBox;
                    });

                    it('Should be execute result empty', function() {
                        action.jsonData = $.parseJSON($('#ExecutableDirectAction').val());

                        action.internalExecute(state);

                        expect(fakeComplete.execute).toHaveBeenCalledWith(IncodingResult.Empty.data);
                        expect(fakeError.execute).not.toHaveBeenCalled();
                    });

                    it('Should be execute result success', function() {

                        action.jsonData = $.parseJSON($('#ExecutableDirectActionWithSucces').val());

                        action.internalExecute(state);

                        expect(fakeSuccess.execute).toHaveBeenCalledWith('data');
                        expect(fakeComplete.execute).toHaveBeenCalledWith('data');
                        expect(fakeError.execute).not.toHaveBeenCalled();
                    });

                    it('Should be execute result error', function() {

                        action.jsonData = $.parseJSON($('#ExecutableDirectActionWithError').val());

                        action.internalExecute(state);

                        expect(fakeError.execute).toHaveBeenCalledWith('data');
                        expect(fakeSuccess.execute).not.toHaveBeenCalled();
                        expect(fakeComplete.execute).toHaveBeenCalled();
                    });

                    it('Should be execute result redirect', function() {

                        spyOn(ExecutableHelper, "RedirectTo");
                        action.jsonData = $.parseJSON($('#ExecutableDirectActionWithRedirect').val());

                        action.internalExecute(state);

                        expect(ExecutableHelper.RedirectTo).toHaveBeenCalledWith('redirectTo');
                        expect(fakeSuccess.execute).not.toHaveBeenCalled();
                        expect(fakeComplete.execute).not.toHaveBeenCalled();
                        expect(fakeError.execute).not.toHaveBeenCalled();
                    });

                });

                describe('When ExecutableEventAction', function() {
                    var eventAction;
                    beforeEach(function() {
                        eventAction = new ExecutableEventAction();
                        eventAction.jsonData = { target : '' };
                        eventAction.self = instanceSandBox;
                        eventAction.target = instanceSandBox;
                    });

                    it('Should be success', function() {

                        var jsonData = { data : 'aws', redirectTo : '', success : true };

                        var eventState = { eventResult : new IncodingResult(jsonData) };

                        $.extend(eventState, state);

                        eventAction.internalExecute(eventState);

                        expect(fakeSuccess.execute).toHaveBeenCalledWith(jsonData.data);
                        expect(fakeComplete.execute).toHaveBeenCalledWith(jsonData.data);
                        expect(fakeError.execute).not.toHaveBeenCalled();
                    });

                });

                describe('When ExecutableAjaxAction', function() {

                    var fakeUrl, action;

                    beforeEach(function() {

                        $.mockjaxClear();

                        fakeUrl = 'Url/Test';

                        action = new ExecutableAjaxAction();
                        action.jsonData = { target : '', ajax : TestHelper.Instance.GetAjaxOptions({ url : fakeUrl, data : [{ "name" : "Name", "selector" : "selector" }] }) };
                        action.self = instanceSandBox;
                        action.target = instanceSandBox;
                        spyOn(ExecutableHelper.Instance, 'TryGetVal').andReturn('Value');
                    });

                    it('Should be execute without ajax options', function() {

                        runs(function() {
                            $.mockjax({
                                url : fakeUrl,
                                data : [],
                                type : 'GET',
                                responseText : { data : 'Success', success : true, redirectTo : '' }
                            });
                            action.jsonData.ajax.data = [];
                            action.internalExecute(state);
                        });

                        waits(500);

                        runs(function() {
                            expect(fakeSuccess.execute).toHaveBeenCalledWith('Success');
                        });
                    });

                    it('Should be execute', function() {

                        runs(function() {
                            $.mockjax({
                                url : fakeUrl,
                                data : [{ Name : 'Value' }],
                                type : 'GET',
                                responseText : { data : 'Success', success : true, redirectTo : '' }
                            });
                            action.internalExecute(state);
                        });

                        waits(500);

                        runs(function() {
                            expect(fakeSuccess.execute).toHaveBeenCalledWith('Success');
                            expect(fakeComplete.execute).toHaveBeenCalledWith('Success');
                            expect(fakeError.execute).not.toHaveBeenCalled();
                            expect(ExecutableHelper.Instance.TryGetVal).toHaveBeenCalledWith("selector");
                        });
                    });

                    it('Should be execute with url from hash', function() {

                        runs(function() {
                            $.mockjax({
                                url : 'hash/fakeUrl',
                                data : [{ Name : 'Value' }, { Name2 : 'Value2' }, { Name3 : 'Value3' }],
                                type : 'GET',
                                responseText : {
                                    data : 'WithUrlFromHash',
                                    success : true,
                                    redirectTo : ''
                                }
                            });
                            window.location.hash = 'hash/fakeUrl?Name2=Value2/Name3=Value3';
                            action.jsonData.hash = true;
                            action.jsonData.prefix = 'root';
                            action.jsonData.ajax.url = '';

                            action.internalExecute(state);
                        });

                        waits(500);

                        runs(function() {
                            expect(fakeSuccess.execute).toHaveBeenCalledWith('WithUrlFromHash');
                            expect(fakeComplete.execute).toHaveBeenCalledWith('WithUrlFromHash');
                            expect(fakeError.execute).not.toHaveBeenCalled();
                            expect(ExecutableHelper.Instance.TryGetVal).toHaveBeenCalledWith('selector');
                        });

                    });

                    it('Should be execute with query string hash', function() {

                        runs(function() {
                            $.mockjax({
                                url : fakeUrl,
                                data : [{ Name : 'Value' }, { Name2 : 'Value2' }, { Name3 : 'Value3' }],
                                type : 'GET',
                                responseText : {
                                    data : 'WithQueryStringHash',
                                    success : true,
                                    redirectTo : ''
                                }
                            });
                            window.location.hash = 'Name2=Value2/Name3=Value3';
                            action.jsonData.hash = true;
                            action.jsonData.prefix = 'root';

                            action.internalExecute(state);
                        });

                        waits(500);

                        runs(function() {
                            expect(fakeSuccess.execute).toHaveBeenCalledWith('WithQueryStringHash');
                            expect(fakeComplete.execute).toHaveBeenCalledWith('WithQueryStringHash');
                            expect(fakeError.execute).not.toHaveBeenCalled();
                            expect(ExecutableHelper.Instance.TryGetVal).toHaveBeenCalledWith('selector');
                        });

                    });

                });

                describe('When ExecutableSubmitAction', function() {

                    var form, inputField, submitButton, action;

                    var fakeUrl = 'http://localhost:64225/Url/Test';

                    var setAjaxMock = function(response) {
                        $.mockjax({
                            url : fakeUrl,
                            data : 'name=value',
                            type : 'POST',
                            responseText : response
                        });

                    };

                    beforeEach(function() {

                        form = $('<form>').attr({ action : fakeUrl, method : 'POST', id : 'formSubmit' });
                        inputField = $('<input>').attr({ type : 'textbox', id : 'id', name : 'name', value : 'value' });
                        submitButton = $('<input>').attr({ type : 'submit' });
                        form.append(inputField).append(submitButton);
                        appendSetFixtures(form);

                        action = new ExecutableSubmitAction();
                        action.jsonData = { formSelector : '$(this.self)', options : TestHelper.Instance.GetAjaxSubmitOptions() };
                        action.self = submitButton;
                        action.target = submitButton;
                    });

                    it('Should be result success', function() {

                        runs(function() {
                            setAjaxMock({ data : 'Message', success : true, redirectTo : '' });
                            action.internalExecute(state);
                        });

                        waits(500);

                        runs(function() {
                            expect(fakeSuccess.execute).toHaveBeenCalled();
                            expect(fakeComplete.execute).toHaveBeenCalled();
                            expect(fakeError.execute).not.toHaveBeenCalled();
                        });

                    });

                    it('Should be result success with form', function() {
                        runs(function() {
                            setAjaxMock({ data : 'Message', success : true, redirectTo : '' });

                            action.jsonData.formSelector = "$('#formSubmit')";
                            action.internalExecute(state);
                        });

                        waits(500);

                        runs(function() {
                            expect(fakeSuccess.execute).toHaveBeenCalled();
                            expect(fakeComplete.execute).toHaveBeenCalled();
                            expect(fakeError.execute).not.toHaveBeenCalled();
                        });

                    });

                    it('Should be broken validate', function() {

                        spyOn($.fn, 'valid').andReturn(false);

                        runs(function() {
                            setAjaxMock({ data : 'Message', success : true, redirectTo : '' });
                            action.internalExecute(state);
                        });

                        waits(500);

                        runs(function() {
                            expect($.fn.valid).toHaveBeenCalled();
                            expect(fakeSuccess.execute).not.toHaveBeenCalled();
                            expect(fakeComplete.execute).not.toHaveBeenCalled();
                            expect(fakeError.execute).not.toHaveBeenCalled();
                        });

                    });

                });

            });

            describe('Executables', function() {

                describe('When ExecutableInsert', function() {

                    var insert;
                    beforeEach(function() {
                        spyOn(IncodingEngine.Current, 'parse');
                        spyOnEvent(document, IncSpecialBinds.IncInsert);

                        insert = new ExecutableInsert();
                        insert.self = instanceSandBox;
                        insert.target = instanceSandBox;
                    });

                    describe('When with data', function() {

                        beforeEach(function() {
                            insert.jsonData = $.parseJSON($('#ExecutableInsert').val());
                        });

                        it('Should be insert undifiend', function() {
                            insert.internalExecute(undefined);
                            expect(instanceSandBox).toHaveHtml('');
                        });

                        it('Should be insert html', function() {
                            insert.internalExecute('Content');
                            expect(instanceSandBox).toHaveHtml('Content');
                        });

                        it('Should be insert string object', function() {
                            insert.internalExecute(new String('Content'));
                            expect(instanceSandBox).toHaveHtml('Content');
                        });

                        it('Should be insert object', function() {
                            insert.internalExecute(1);
                            expect(instanceSandBox).toHaveHtml('1');
                        });

                    });

                    describe('When with property', function() {

                        beforeEach(function() {
                            insert.jsonData = $.parseJSON($('#ExecutableInsertWithProperty').val());
                        });

                        it('Should be insert object', function() {
                            insert.internalExecute({ prop1 : 1, Prop2 : 2 });

                            expect(instanceSandBox).toHaveHtml('2');
                        });

                        it('Should be insert array', function() {
                            insert.internalExecute([{ prop1 : 1, Prop2 : 2 }]);

                            expect(instanceSandBox).toHaveHtml('2');
                        });

                        it('Should be insert big array', function() {
                            insert.internalExecute([{ Prop2 : 1 }, { Prop2 : 2 }, { Prop2 : 3 }]);

                            expect(instanceSandBox).toHaveHtml('1');
                        });

                        it('Should be insert empty object', function() {
                            insert.internalExecute({});

                            expect(instanceSandBox).toHaveHtml({});
                        });

                        it('Should be insert empty array', function() {
                            insert.internalExecute([]);

                            expect(instanceSandBox).toHaveHtml({});
                        });

                    });

                    describe('When with prepare', function() {

                        var isValue1, isValue2;

                        beforeEach(function() {
                            isValue1 = false;
                            isValue2 = false;
                            insert.tryGetVal = function(value) {
                                if (value === 1) {
                                    isValue1 = true;
                                }
                                if (value === 2) {
                                    isValue2 = true;
                                }
                            };

                            insert.jsonData = $.parseJSON($('#ExecutableInsertWithPrepare').val());
                        });

                        it('Should be insert array', function() {
                            insert.internalExecute([{ prop1 : 1 }, { prop1 : 2 }]);

                            expect(isValue1).toBeTruthy();
                            expect(isValue2).toBeTruthy();
                        });

                        it('Should be insert object', function() {
                            insert.internalExecute({ prop1 : 1, prop2 : 2 });

                            expect(isValue1).toBeTruthy();
                            expect(isValue2).toBeTruthy();
                        });

                    });

                    describe('When insert with template', function() {

                        var incTemplate;

                        beforeEach(function() {
                            incTemplate = {
                                render : function() {
                                }
                            };
                            spyOn(incTemplate, 'render').andReturn('renderContent');
                            spyOn(TemplateFactory, 'Create').andReturn(incTemplate);
                        });

                        it('Should be template selector', function() {

                            insert.tryGetVal = function(value) {
                                if (value == 'selectorId') {
                                    return value;
                                }

                                throw 'Incorect value {0}'.f(value);
                            };

                            insert.jsonData = $.parseJSON($('#ExecutableInsertWithTemplateSelector').val());
                            insert.internalExecute([{ item : 1 }, { item : 2 }]);

                            expect(instanceSandBox).toHaveHtml('renderContent');
                            expect(TemplateFactory.Create).toHaveBeenCalledWith('Mustache', [{ item : 1 }, { item : 2 }], 'selectorId');
                        });

                    });

                    afterEach(function() {
                        expect(IncodingEngine.Current.parse).toHaveBeenCalledWith(instanceSandBox);
                        expect(IncSpecialBinds.IncInsert).toHaveBeenTriggeredOn(document);
                    });

                });

                describe('When ExecutableTrigger', function() {

                    var trigger;

                    beforeEach(function() {

                        trigger = new ExecutableTrigger();
                        trigger.jsonData = $.parseJSON($('#ExecutableTrigger').val());
                        ;
                        trigger.self = instanceSandBox;
                        trigger.target = instanceSandBox;

                        spyOnEvent(instanceSandBox, trigger.jsonData.trigger);
                    });

                    it('Should be trigger with data as undefined', function() {
                        trigger.internalExecute(undefined);
                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : undefined, redirectTo : '' }));
                    });

                    it('Should be trigger with data as object', function() {
                        trigger.internalExecute({ is : true });
                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : { is : true }, redirectTo : '' }));
                    });

                    it('Should be trigger with data as string', function() {
                        trigger.internalExecute('content');
                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : 'content', redirectTo : '' }));
                    });

                    it('Should be trigger with data as int', function() {
                        trigger.internalExecute(5);
                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : 5, redirectTo : '' }));
                    });

                    it('Should be trigger with data as int object', function() {
                        trigger.internalExecute(Number(5));
                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : 5, redirectTo : '' }));
                    });

                    it('Should be trigger with data as bool', function() {
                        trigger.internalExecute(true);
                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : true, redirectTo : '' }));
                    });

                    it('Should be trigger with data as bool object', function() {
                        trigger.internalExecute(Boolean(true));
                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : true, redirectTo : '' }));
                    });

                    it('Should be trigger data as  array', function() {
                        trigger.internalExecute([5, 6]);
                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : [5, 6], redirectTo : '' }));
                    });

                    it('Should be trigger data as  date time', function() {
                        var currentTime = new Date().now;
                        trigger.internalExecute(currentTime);
                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : currentTime, redirectTo : '' }));
                    });

                    it('Should be trigger with property data', function() {
                        trigger.jsonData = $.parseJSON($('#ExecutableTriggerWithProperty').val());
                        ;
                        trigger.internalExecute({ is : true });

                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : true, redirectTo : '' }));
                    });

                    it('Should be trigger with property empty data', function() {
                        trigger.jsonData = $.parseJSON($('#ExecutableTriggerWithProperty').val());
                        ;
                        trigger.internalExecute({});

                        expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : '', redirectTo : '' }));
                    });

                });

                describe('When ExecutableValidationParse', function() {

                    var form, element, submitButton, validation;

                    beforeEach(function() {

                        form = $('<form>');
                        element = $('<input>')
                            .attr({ id : 'inputId', type : 'textbox', name : 'inputName' })
                            .attr('data-val', true)
                            .attr('data-val-required', 'message');

                        submitButton = $('<input>');

                        $(form).append(element).append(submitButton);
                        $('body').append(form);

                        validation = new ExecutableValidationParse();
                        validation.self = submitButton;
                        validation.target = form;
                    });

                    it('Should be not valid', function() {
                        validation.internalExecute();
                        $(element).val('');

                        expect($(form).valid()).toBeFalsy();
                    });

                    it('Should be valid', function() {
                        validation.internalExecute();
                        $(element).val('text');

                        expect($(form).valid()).toBeTruthy();
                    });

                    it('Should be found form if form wrap another tag', function() {

                        validation.target = $(form).wrap('<div>');
                        validation.internalExecute();
                        $(element).val('');

                        expect($(form).valid()).toBeFalsy();
                    });

                    afterEach(function() {
                        $(form).remove();
                    });
                });

                describe('When ExecutableValidationRefresh', function() {

                    var inputErrorClass = 'input-validation-error';
                    var messageErrorClass = 'field-validation-error';
                    var messageValidClass = 'field-validation-valid';
                    var input, span, form, validationRefresh;

                    beforeEach(function() {
                        input = $('<input>').attr({ name : 'Input.Email' });
                        span = $('<span>').attr('data-valmsg-for', 'input.Email');

                        form = $('<from>')
                            .append(input)
                            .append(span);

                        appendSetFixtures(form);

                        validationRefresh = new ExecutableValidationRefresh();
                        validationRefresh.self = form;
                        validationRefresh.target = form;

                    });

                    it('Should be is not valid', function() {

                        $(span).addClass(messageValidClass);

                        validationRefresh.internalExecute([{ name : 'input.Email', errorMessage : 'errroMessage', isValid : false }]);

                        expect(input).toHaveClass(inputErrorClass);
                        expect(span).toHaveClass(messageErrorClass);
                        expect(span).toHaveText('errroMessage');

                        expect(span).not.toHaveClass(messageValidClass);
                    });

                    it('Should be is valid', function() {

                        $(span).addClass(messageErrorClass);
                        $(input).addClass(inputErrorClass);

                        validationRefresh.internalExecute([{ name : input.prop('name'), errorMessage : 'errroMessage', isValid : true }]);

                        expect(input).not.toHaveClass(inputErrorClass);
                        expect(span).not.toHaveClass(messageErrorClass);
                        expect(span).toHaveClass(messageValidClass);

                    });

                    afterEach(function() {
                        $(form).remove();
                    });

                });

                describe('When ExecutableRedirect', function() {
                    var redirect;
                    beforeEach(function() {
                        redirect = new ExecutableRedirect();
                        redirect.self = instanceSandBox;
                        redirect.target = instanceSandBox;

                        spyOn(ExecutableHelper, 'RedirectTo');
                    });

                    it('Should be redirect to', function() {
                        redirect.jsonData = $.parseJSON($('#ExecutableRedirect').val());

                        redirect.internalExecute();

                        expect(ExecutableHelper.RedirectTo).toHaveBeenCalledWith('testUrl?Id=#idValue&Id2=#idValue2');
                    });

                    it('Should be redirect to self', function() {
                        redirect.jsonData = $.parseJSON($('#ExecutableRedirectToSelf').val());

                        redirect.internalExecute();

                        expect(ExecutableHelper.RedirectTo).toHaveBeenCalledWith(document.location.href);
                    });

                });

                describe('When ExecutableEval', function() {
                    var evalEx;
                    beforeEach(function() {
                        evalEx = new ExecutableEval();
                        evalEx.jsonData = $.parseJSON($('#ExecutableEval').val());
                        evalEx.self = instanceSandBox;
                        evalEx.target = instanceSandBox;
                    });

                    it('Should be eval', function() {
                        evalEx.internalExecute('data');
                        expect(newFakeEvalVariable).toEqual('data');
                    });
                });

                describe('When ExecutableBreak', function() {

                    var breakEx;
                    beforeEach(function() {

                        breakEx = new ExecutableBreak();
                        breakEx.jsonData = { ands : [] };
                        breakEx.self = instanceSandBox;
                        breakEx.target = instanceSandBox;
                    });

                    it('Should be break', function() {
                        expect(function() {
                            breakEx.internalExecute();
                        }).toThrow(new IncClientException());
                    });

                });

                describe('When ExecutableStoreInsert hash', function() {

                    var hashInsert;

                    beforeEach(function() {
                        hashInsert = new ExecutableStoreInsert();
                        hashInsert.jsonData = { replace : 'false', prefix : 'root' };
                        hashInsert.self = instanceSandBox;
                        hashInsert.target = instanceSandBox;
                    });

                    describe('When in container', function() {

                        var valueInput, valueSelect;

                        beforeEach(function() {

                            $(instanceSandBox).append(TestHelper.Instance.SandboxTextBox(),
                                TestHelper.Instance.SandboxSelect(),
                                TestHelper.Instance.SandboxSubmit(),
                                TestHelper.Instance.SandboxInputButton(),
                                TestHelper.Instance.SandboxReset(),
                                TestHelper.Instance.SandboxInput());

                            valueInput = '@@input';
                            valueSelect = 'select';

                            spyOn(ExecutableHelper, 'RedirectTo');
                            spyOn(ExecutableHelper.Instance, 'TryGetVal').andCallFake(function(selector) {
                                if (selector === '[name=sandboxTextBox]') {
                                    return valueInput;
                                }
                                if (selector === '[name=sandboxSelect]') {
                                    return valueSelect;
                                }

                                return undefined;
                            });

                            window.document.location.hash = 'aws=param';
                        });

                        it('Should be insert', function() {

                            hashInsert.internalExecute();

                            var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                            expect(redirectUrl.fparam('aws', 'root')).toEqual('param');
                            expect(redirectUrl.fparam().hasOwnProperty('sandboxSubmit__root')).toBeFalsy();
                            expect(redirectUrl.fparam('sandboxTextBox', 'root')).toEqual('@@input');
                            expect(redirectUrl.fparam('sandboxSelect', 'root')).toEqual(valueSelect);
                        });

                        it('Should be update by prefix', function() {

                            hashInsert.jsonData.prefix = 'search';
                            hashInsert.internalExecute();

                            var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                            expect(redirectUrl.fparam().hasOwnProperty('aws__search')).toBeFalsy();
                            expect(redirectUrl.fparam('aws', 'root')).toEqual('param');
                            expect(redirectUrl.fparam('sandboxTextBox', 'search')).toEqual('@@input');
                            expect(redirectUrl.fparam('sandboxSelect', 'search')).toEqual(valueSelect);

                        });

                        it('Should be update with replace', function() {

                            hashInsert.jsonData.replace = 'true';
                            hashInsert.internalExecute();

                            var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                            expect(redirectUrl.fparam().hasOwnProperty('aws__root')).toBeFalsy();
                            expect(redirectUrl.fparam().hasOwnProperty('sandboxSubmit__root')).toBeFalsy();
                            expect(redirectUrl.fparam('sandboxTextBox', 'root')).toEqual('@@input');
                            expect(redirectUrl.fparam('sandboxSelect', 'root')).toEqual(valueSelect);
                        });

                        afterEach(function() {
                            expect(ExecutableHelper.Instance.TryGetVal.callCount).toEqual(2);
                        });

                    });

                    describe('When self ', function() {

                        beforeEach(function() {
                            spyOn(ExecutableHelper, 'RedirectTo');
                            var textBox = TestHelper.Instance.SandboxTextBox();
                            hashInsert.self = textBox;
                            hashInsert.target = textBox;
                        });

                        it('Should be update self', function() {
                            spyOn(ExecutableHelper.Instance, 'TryGetVal').andReturn('value');

                            hashInsert.internalExecute();

                            var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                            expect(redirectUrl.fparam('sandboxTextBox', 'root')).toEqual('value');
                        });

                        it('Should be update with ignore empty', function() {
                            spyOn(ExecutableHelper.Instance, 'TryGetVal').andReturn('');

                            hashInsert.internalExecute();

                            var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                            expect(redirectUrl.fparam().hasOwnProperty('sandboxTextBox__root')).toBeFalsy();
                        });

                        it('Should be remove hash if empty', function() {
                            window.location.hash = "!sandboxTextBox=aws";
                            spyOn(ExecutableHelper.Instance, 'TryGetVal').andReturn('');

                            hashInsert.internalExecute();

                            var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                            expect(redirectUrl.fparam().hasOwnProperty('sandboxTextBox__root')).toBeFalsy();
                        });

                        afterEach(function() {
                            expect(ExecutableHelper.Instance.TryGetVal.callCount).toEqual(1);
                        });

                    });

                });

                describe('When ExecutableStoreFetch hash', function() {

                    var element, hashFetch;

                    beforeEach(function() {
                        spyOn(ExecutableHelper.Instance, 'TrySetValue');
                        element = TestHelper.Instance.SandboxTextBox();

                        hashFetch = new ExecutableStoreFetch();
                        hashFetch.jsonData = $.parseJSON($('#ExecutableStoreFetch').val());
                        hashFetch.self = instanceSandBox;
                        hashFetch.target = element;
                    });

                    it('Should be fetch', function() {
                        window.location.hash = "sandboxTextBox=@Test";

                        hashFetch.internalExecute();

                        expect(ExecutableHelper.Instance.TrySetValue).toHaveBeenCalledWith($(element).get(0), '@Test');
                    });

                    it('Should be fetch  with decode', function() {
                        window.location.hash = "sandboxTextBox=%26TestÐóññêèå Áóêâû";

                        hashFetch.internalExecute();

                        expect(ExecutableHelper.Instance.TrySetValue).toHaveBeenCalledWith($(element).get(0), '&TestÐóññêèå Áóêâû');
                    });

                    it('Should be fetch by prefix', function() {
                        window.location.hash = "sandboxTextBox=Test&search:sandboxTextBox=ValueFromPrefix";
                        hashFetch.jsonData = $.parseJSON($('#ExecutableStoreFetchWithPrefix').val());

                        hashFetch.internalExecute();

                        expect(ExecutableHelper.Instance.TrySetValue).toHaveBeenCalledWith($(element).get(0), 'ValueFromPrefix');
                    });

                    it('Should be fetch container', function() {

                        $(instanceSandBox).append(element);
                        hashFetch.target = instanceSandBox;
                        window.location.hash = "sandboxTextBox=Test";

                        hashFetch.internalExecute();

                        expect($(ExecutableHelper.Instance.TrySetValue.argsForCall[0][0]).attr('name')).toEqual($(element).attr('name'));
                        expect(ExecutableHelper.Instance.TrySetValue.argsForCall[0][1]).toEqual('Test');
                    });

                    it('Should be fetch selector', function() {
                        var textBox = TestHelper.Instance.SandboxTextBox().addClass('fetch');
                        var select = TestHelper.Instance.SandboxSelect([{ value : 'selectValue', selected : true }]).addClass('fetch');
                        $(instanceSandBox).append(textBox).append(select);

                        hashFetch.target = $('.fetch');
                        window.location.hash = "sandboxTextBox=TextBoxValue&sandboxSelect=SelectValue";

                        hashFetch.internalExecute();

                        expect($(ExecutableHelper.Instance.TrySetValue.argsForCall[0][0]).attr('name')).toEqual($(textBox).attr('name'));
                        expect(ExecutableHelper.Instance.TrySetValue.argsForCall[0][1]).toEqual('TextBoxValue');

                        expect($(ExecutableHelper.Instance.TrySetValue.argsForCall[1][0]).attr('name')).toEqual($(select).attr('name'));
                        expect(ExecutableHelper.Instance.TrySetValue.argsForCall[1][1]).toEqual('SelectValue');
                    });

                });

                describe('When ExecutableStoreManipulate', function() {

                    var executable;

                    beforeEach(function() {
                        executable = new ExecutableStoreManipulate();
                        executable.jsonData = $('#');
                    });

                    it('Should be ', function() {

                    });

                });

                describe('When ExecutableBind', function() {

                    var executable;

                    beforeEach(function() {
                        executable = new ExecutableBind();
                        executable.target = instanceSandBox;
                    });

                    it('Should be attach', function() {
                        spyOn(IncodingEngine.Current, 'parse');
                        $(instanceSandBox).data('incoding-runner', 'runner');
                        executable.jsonData = $.parseJSON($('#ExecutableBindAttach').val());

                        executable.internalExecute();

                        expect(executable.target).toHaveAttr('incoding', 'meta');
                        expect(executable.target).not.toHaveData('incoding-runner');
                        expect(IncodingEngine.Current.parse).toHaveBeenCalledWith(instanceSandBox);
                    });

                    it('Should be detach', function() {
                        executable.jsonData = $.parseJSON($('#ExecutableBindDetach').val());
                        var hasClick = false;
                        $(executable.target).bind('click', function() {
                            hasClick = true;
                        });

                        executable.internalExecute();
                        $(executable.target).trigger('click');

                        expect(hasClick).toBeFalsy();
                    });

                    it('Should be detach all', function() {
                        executable.jsonData = $.parseJSON($('#ExecutableBindDetachAll').val());
                        var hashAny = false;
                        $(executable.target).bind('click dbclick', function() {
                            hashAny = true;
                        });

                        executable.internalExecute();
                        $(executable.target).trigger('click');
                        $(executable.target).trigger('dbclick');

                        expect(hashAny).toBeFalsy();
                    });

                });

                describe('When ExecutableForm', function() {

                    var executable, form;

                    beforeEach(function() {
                        form = $('<form>').attr({ id : 'formId' });

                        executable = new ExecutableForm();
                        executable.target = form;
                    });

                    it('Should be clear', function() {
                        spyOn($.fn, 'clearForm');

                        executable.jsonData = $.parseJSON($('#ExecutableFormClear').val());
                        executable.internalExecute();

                        expect($.fn.clearForm).toHaveBeenCalled();
                        expect($.fn.clearForm.calls[0].object).toEqual(form);
                    });

                    it('Should be target element in', function() {
                        var instanceIn = TestHelper.Instance.SandboxInput();
                        $(form).append(instanceIn);
                        executable.target = instanceIn;

                        spyOn($.fn, 'clearForm');

                        executable.jsonData = $.parseJSON($('#ExecutableFormClear').val());
                        executable.internalExecute();

                        expect($.fn.clearForm).toHaveBeenCalled();
                        expect($($.fn.clearForm.calls[0].object).get(0)).toEqual($(form).get(0));

                    });

                    it('Should be reset', function() {
                        spyOn($.fn, 'resetForm');

                        executable.jsonData = $.parseJSON($('#ExecutableFormReset').val());
                        executable.internalExecute();

                        expect($.fn.resetForm).toHaveBeenCalled();
                        expect($.fn.resetForm.calls[0].object).toEqual(form);
                    });

                    afterEach(function() {
                        $(form).remove();
                    });

                });

            });

            describe('Conditionals', function() {

                describe('When ConditionalFactory', function() {

                    it('Should be create by type', function() {
                        $('input', '#supportedConditional').each(function() {
                            var type = $(this).val();
                            var conditional = ConditionalFactory.Create({ type : type }, new ExecutableBase());
                            expect(conditional).not.toBeNull();
                        });
                    });

                    it('Should be create', function() {
                        var executable = new ExecutableBreak();
                        var conditional = ConditionalFactory.Create({ type : 'Eval', code : 'true;', inverse : false }, executable);

                        expect(conditional.jsonData).toEqual({ type : 'Eval', code : 'true;', inverse : false });
                        expect(conditional.executable).toEqual(executable);
                    });

                });

                describe('When isSatisfied', function() {
                    var executable, conditional;

                    beforeEach(function() {
                        executable = new ExecutableBase();
                        executable.self = 'self';
                        spyOn(executable, 'getTarget').andReturn('target');

                        conditional = new ConditionalBase();
                        conditional.executable = executable;
                        conditional.jsonData = { inverse : false };
                    });

                    it('Should be isSatisfied true', function() {
                        conditional.isInternalSatisfied = function(data) {
                            if (data != 'aws') {
                                throw 'Incorrect data';
                            }

                            return true;
                        };
                        var isSatisfied = conditional.isSatisfied('aws');

                        expect(isSatisfied).toBeTruthy();
                        expect(conditional.target).toEqual('target');
                        expect(conditional.self).toEqual('self');
                        expect(executable.getTarget).toHaveBeenCalled();
                    });

                    it('Should be isSatisfied false', function() {
                        conditional.isInternalSatisfied = function(data) {
                            if (data != 'aws') {
                                throw 'Incorrect data';
                            }

                            return false;
                        };
                        var isSatisfied = conditional.isSatisfied('aws');

                        expect(isSatisfied).toBeFalsy();
                    });

                    it('Should be isSatisfied inverse', function() {
                        conditional.jsonData = { inverse : true };
                        conditional.isInternalSatisfied = function(data) {
                            if (data != 'aws') {
                                throw 'Incorrect data';
                            }

                            return false;
                        };
                        var isSatisfied = conditional.isSatisfied('aws');

                        expect(isSatisfied).toBeTruthy();
                    });

                });

                describe('When tryGetVal', function() {
                    var executable, conditional;

                    beforeEach(function() {
                        executable = new ExecutableBase();
                        spyOn(executable, 'tryGetVal').andReturn('value');

                        conditional = new ConditionalEval();
                        conditional.executable = executable;
                    });

                    it('Should be tryGetVal exectuable', function() {
                        var tryGetVal = conditional.tryGetVal('aws');

                        expect(tryGetVal).toEqual('value');
                        expect(executable.tryGetVal).toHaveBeenCalledWith('aws');
                    });

                });

                describe('When ConditionalEval', function() {

                    var evalConditional;

                    beforeEach(function() {
                        evalConditional = new ConditionalEval();
                        evalConditional.jsonData = { inverse : false, code : 'false' };
                    });

                    describe('When Eval', function() {

                        it('Should be satisfied', function() {
                            var isSatisfied = evalConditional.isInternalSatisfied();
                            expect(isSatisfied).toBeFalsy();
                        });

                        it('Should be with data', function() {
                            evalConditional.jsonData.code = 'data';
                            var isSatisfied = evalConditional.isInternalSatisfied(true);
                            expect(isSatisfied).toBeTruthy();
                        });

                    });

                });

                describe('When ConditionalData', function() {

                    var existsItem, dataConditional;

                    beforeEach(function() {

                        existsItem = { prop1 : 1, prop2 : 2 };

                        dataConditional = new ConditionalData();
                        dataConditional.jsonData = { property : "prop2", value : "123", method : 1, inverse : false };
                        dataConditional.tryGetVal = function() {
                            return dataConditional.jsonData.value;
                        };

                        spyOn(ExecutableHelper, 'Compare').andReturn(true);
                    });

                    it('Should be satisfied object', function() {
                        var satisfied = dataConditional.isInternalSatisfied(existsItem);

                        expect(satisfied).toBeTruthy();
                        expect(ExecutableHelper.Compare).toHaveBeenCalledWith(existsItem.prop2, dataConditional.jsonData.value, dataConditional.jsonData.method);
                    });

                    it('Should be satisfied object without property', function() {
                        dataConditional.jsonData.property = '';

                        var satisfied = dataConditional.isInternalSatisfied(existsItem);

                        expect(satisfied).toBeTruthy();
                        expect(ExecutableHelper.Compare).toHaveBeenCalledWith(existsItem, dataConditional.jsonData.value, dataConditional.jsonData.method);
                    });

                    it('Should be satisfied array', function() {
                        var satisfied = dataConditional.isInternalSatisfied([existsItem, { prop1 : 8, prop2 : 18 }]);

                        expect(satisfied).toBeTruthy();
                        expect(ExecutableHelper.Compare).toHaveBeenCalledWith(existsItem.prop2, dataConditional.jsonData.value, dataConditional.jsonData.method);
                    });

                    it('Should be satisfied array without property', function() {
                        dataConditional.jsonData.property = '';
                        var data = [existsItem, { prop1 : 8, prop2 : 18 }];

                        var satisfied = dataConditional.isInternalSatisfied([existsItem, { prop1 : 8, prop2 : 18 }]);

                        expect(satisfied).toBeTruthy();
                        expect(ExecutableHelper.Compare).toHaveBeenCalledWith(data, dataConditional.jsonData.value, dataConditional.jsonData.method);
                    });

                });

                describe('When ConditionalDataIsId', function() {

                    var existsItem, existsElement, property, conditionalDataIds;

                    beforeEach(function() {

                        existsItem = { prop1 : 1, prop2 : 'valueFromField' };
                        property = "prop2";
                        existsElement = $('<div>').attr({
                            id : 'valueFromField'
                        });
                        appendSetFixtures(existsElement);

                        conditionalDataIds = new ConditionalDataIsId();
                        conditionalDataIds.jsonData = { property : property, inverse : false };
                        conditionalDataIds.self = instanceSandBox;
                        conditionalDataIds.target = instanceSandBox;

                    });

                    it('Should be satisfied', function() {
                        var satisfied = conditionalDataIds.isInternalSatisfied(existsItem);
                        expect(satisfied).toBeTruthy();
                    });

                    it('Should be satisfied with array', function() {
                        var satisfied = conditionalDataIds.isInternalSatisfied([existsItem, { prop1 : 8, prop2 : 18 }]);
                        expect(satisfied).toBeTruthy();
                    });

                });

            });

        });

        describe('When JavaScriptCodeTemplate', function() {

            describe('Target', function() {

                it('Should be Target_Increment', function() {
                    var incValue = TestHelper.Instance.SandboxTextBox();
                    $(incValue).val('5');

                    this.target = incValue;
                    eval($('#Target_Increment').val());

                    expect($(incValue).val()).toEqual('6');
                });

                it('Should be Target_Decrement', function() {
                    var incValue = TestHelper.Instance.SandboxTextBox();
                    $(incValue).val('5');

                    this.target = incValue;
                    eval($('#Target_Decrement').val());

                    expect($(incValue).val()).toEqual('4');
                });

                it('Should be Target_Wrap', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn("<div class=\"wrap\" />");
                    this.target = instanceSandBox;

                    eval($('#Target_Wrap').val().f("'value'"));

                    expect(instanceSandBox.parent()).toHaveClass('wrap');
                    expect(this.tryGetVal).toHaveBeenCalledWith('value');
                });

                it('Should be Target_UnBind', function() {

                    var isCall = false;
                    this.target = instanceSandBox;
                    $(this.target).bind('click', function() {
                        isCall = true;
                    });

                    eval($('#Target_UnBind').val().f("'click'"));
                    $(this.target).trigger('click');

                    expect(isCall).toBeFalsy();
                });

                it('Should be Target_WrapAll', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn("<div class=\"wrap\" />");
                    this.target = instanceSandBox;

                    eval($('#Target_WrapAll').val().f("'value'"));

                    expect(instanceSandBox.parent()).toHaveClass('wrap');
                    expect(this.tryGetVal).toHaveBeenCalledWith('value');
                });

                it('Should be Target_Remove', function() {

                    this.target = instanceSandBox;
                    eval($('#Target_Remove').val());

                    expect($('#sandbox', 'body').length).toEqual(0);
                });

                it('Should be Target_Empty', function() {

                    this.target = instanceSandBox;
                    $(this.target).html('content');

                    eval($('#Target_Empty').val());

                    expect(this.target).not.toHaveHtml('content');
                });

                it('Should be Target_RemoveClass', function() {
                    $(instanceSandBox).addClass('inc');

                    this.target = instanceSandBox;
                    eval($('#Target_RemoveClass').val().f('inc'));

                    expect(instanceSandBox).not.toHaveClass('inc');
                });

                it('Should be Target_ToggleClass', function() {

                    this.target = instanceSandBox;
                    var code = $('#Target_ToggleClass').val().f('inc');

                    eval(code);
                    expect(instanceSandBox).toHaveClass('inc');

                    eval(code);
                    expect(instanceSandBox).not.toHaveClass('inc');
                });

                it('Should be Target_AddClass', function() {
                    this.target = instanceSandBox;

                    eval($('#Target_AddClass').val().f('inc'));
                    expect(instanceSandBox).toHaveClass('inc');
                });

                it('Should be Target_ToggleProp', function() {

                    this.target = instanceSandBox;
                    var code = $('#Target_ToggleProp').val().f('readonly');

                    eval(code);
                    expect(instanceSandBox).toHaveProp('readonly', true);

                    eval(code);
                    expect(instanceSandBox).toHaveProp('readonly', false);

                });

                it('Should be Target_Val', function() {

                    this.target = TestHelper.Instance.SandboxTextBox();
                    eval($('#Target_Val').val().f("'aws'"));

                    expect(this.target).toHaveValue('aws');
                });

                it('Should be Target_Insert', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn('content');
                    this.target = instanceSandBox;

                    eval($('#Target_Insert').val().f("html", "'content'"));

                    expect(this.target).toHaveHtml('content');
                    expect(this.tryGetVal).toHaveBeenCalledWith('content');
                });

                it('Should be Target_ValFromSelector', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn('newAws');

                    this.target = TestHelper.Instance.SandboxTextBox();
                    eval($('#Target_ValFromSelector').val().f("'aws'"));

                    expect(this.target).toHaveValue('newAws');
                    expect(this.tryGetVal).toHaveBeenCalledWith('aws');
                });

                it('Should be Target_SetAttr', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn('newAws');

                    this.target = TestHelper.Instance.SandboxTextBox();
                    eval($('#Target_SetAttr').val().f("attr1", "'aws'"));

                    expect(this.target).toHaveAttr('attr1', 'newAws');
                    expect(this.tryGetVal).toHaveBeenCalledWith('aws');
                });

                it('Should be Target_RemoveAttr', function() {
                    this.target = instanceSandBox;
                    $(instanceSandBox).attr('attr1', 'aws');

                    eval($('#Target_RemoveAttr').val().f("attr1"));

                    expect(instanceSandBox).not.toHaveAttr("attr1");
                });

                it('Should be Target_RemoveProp', function() {
                    this.target = instanceSandBox;
                    $(instanceSandBox).prop('attr1', 'aws');

                    eval($('#Target_RemoveProp').val().f("attr1"));

                    expect(instanceSandBox).not.toHaveProp("attr1");
                });

                it('Should be Target_SetProp', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn('newAws');

                    this.target = TestHelper.Instance.SandboxTextBox();
                    eval($('#Target_SetProp').val().f("prop1", "'aws'"));

                    expect(this.target).toHaveProp('prop1', 'newAws');
                    expect(this.tryGetVal).toHaveBeenCalledWith('aws');
                });

                it('Should be Target_SetCss', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn('10');
                    this.target = TestHelper.Instance.SandboxTextBox();

                    eval($('#Target_SetCss').val().f("width", "'aws'"));

                    if (jQuery.browser.msie && jQuery.browser.version <= 8) {
                        expect($(this.target).attr('style')).toEqual('width: 10px');
                    }
                    else if ($.browser.safari) {
                        expect($(this.target).attr('style')).toEqual('width: 10px; ');
                    }
                    else {
                        expect(this.target).toHaveAttr('style', 'width: 10px;');
                    }

                    expect(this.tryGetVal).toHaveBeenCalledWith('aws');
                });

                it('Should be Target_ScrollLeft', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn('300');
                    this.target = instanceSandBox;
                    $(this.target).css({ height : "100px", width : "100px", overflow : "auto" });
                    $(this.target).html($("<div>").css({ height : "500px", width : "500px", border : "3px solid #666666" }));

                    eval($('#Target_ScrollLeft').val().f("'aws'"));

                    expect($(this.target).scrollLeft()).toEqual(300);
                    expect(this.tryGetVal).toHaveBeenCalledWith('aws');
                });

                it('Should be Target_ScrollTop', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn('10');
                    this.target = instanceSandBox;
                    $(this.target).css({ height : "100px", width : "100px", overflow : "auto" });
                    $(this.target).html($("<div>").css({ height : "500px", width : "500px", border : "3px solid #666666" }));

                    eval($('#Target_ScrollTop').val().f("'aws'"));

                    expect($(this.target).scrollTop()).toEqual(10);
                    expect(this.tryGetVal).toHaveBeenCalledWith('aws');
                });

                it('Should be Target_Width', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn('10');
                    this.target = instanceSandBox;

                    eval($('#Target_Width').val().f("'aws'"));

                    expect($(this.target).width()).toEqual(10);
                    expect(this.tryGetVal).toHaveBeenCalledWith('aws');
                });

                it('Should be Target_Height', function() {
                    this.tryGetVal = function() {
                    };
                    spyOn(this, 'tryGetVal').andReturn('10');
                    this.target = instanceSandBox;

                    eval($('#Target_Height').val().f("'aws'"));

                    expect($(this.target).height()).toEqual(10);
                    expect(this.tryGetVal).toHaveBeenCalledWith('aws');
                });

            });

            describe('Conditional', function() {

                describe('Exists jquery selector', function() {

                    it('Should be exist', function() {
                        var code = $('#Conditional_Exists_Jquery_Selector').val().f("$('body')");
                        expect(eval(code)).toBeTruthy();
                    });

                    it('Should not be exist', function() {
                        var code = $('#Conditional_Exists_Jquery_Selector').val().f("$('.specialNoClass')");
                        expect(eval(code)).toBeFalsy();
                    });

                });

                describe('Exists incoding selector', function() {

                    it('Should be exist', function() {
                        spyOn(ExecutableHelper, 'IsNullOrEmpty').andReturn(false);
                        this.tryGetVal = function(value) {
                            return value;
                        };

                        var code = $('#Conditional_Exists_Incoding_Selector').val().f("'inc'");

                        expect(eval(code)).toBeTruthy();
                        expect(ExecutableHelper.IsNullOrEmpty).toHaveBeenCalledWith('inc');
                    });

                    it('Should not be exist', function() {
                        spyOn(ExecutableHelper, 'IsNullOrEmpty').andReturn(true);
                        this.tryGetVal = function(value) {
                            return value;
                        };

                        var code = $('#Conditional_Exists_Incoding_Selector').val().f("'inc'");

                        expect(eval(code)).toBeFalsy();
                        expect(ExecutableHelper.IsNullOrEmpty).toHaveBeenCalledWith('inc');
                    });

                });

                describe('Exists incoding selector', function() {

                    it('Should be exist', function() {
                        var code = $('#Conditional_Exists_Jquery_Selector').val().f("$('body')");
                        expect(eval(code)).toBeTruthy();
                    });

                    it('Should not be exist', function() {
                        var code = $('#Conditional_Exists_Jquery_Selector').val().f("$('.specialNoClass')");
                        expect(eval(code)).toBeFalsy();
                    });

                });

                describe('Support modernizr', function() {

                    it('Should be support', function() {
                        $('html').addClass('super');
                        var code = $('#Conditional_ModernizrSupport').val().f("super");
                        expect(eval(code)).toBeTruthy();
                    });

                    it('Should not be support', function() {
                        var code = $('#Conditional_ModernizrSupport').val().f("no-super");
                        expect(eval(code)).toBeFalsy();
                    });

                });

                describe('Compare', function() {

                    var actualVal, expectVal;

                    beforeEach(function() {

                        actualVal = 'actualVal';
                        expectVal = 'expectedVal';

                        spyOn(ExecutableHelper, "Compare").andReturn(true);
                    });

                    it('Should be satisfied', function() {
                        this.tryGetVal = function(arg) {
                            if (arg == 'element') {
                                return actualVal;
                            }
                            if (arg == 'value') {
                                return expectVal;
                            }
                        };

                        var code = $('#Conditional_Value').val().f("'element'", "'value'", "equal");

                        expect(eval(code)).toBeTruthy();
                        expect(ExecutableHelper.Compare).toHaveBeenCalledWith(actualVal, expectVal, 'equal');
                    });

                });

                describe('Confirm', function() {

                    var textMessage;
                    beforeEach(function() {
                        textMessage = "Message";
                        spyOn(window, 'confirm').andReturn(true);
                    });

                    it('Should be satisfied', function() {
                        this.tryGetVal = function() {
                            return textMessage;
                        };
                        var code = $('#Conditional_Confirm').val().f("'element'");

                        expect(eval(code)).toBeTruthy();
                        expect(window.confirm).toHaveBeenCalledWith(textMessage);
                    });

                });

            });

            it('Should be Window_Alert', function() {
                var message = 'Message 2';
                spyOn(window, 'alert');

                eval($('#Window_Alert').val().f(message));

                expect(window.alert).toHaveBeenCalledWith(message);
            });

            it('Should be Window_Clear_Interval', function() {
                var intervalId = 'inervalId';
                ExecutableBase.IntervalIds[intervalId] = 1;
                spyOn(window, 'clearInterval');

                eval($('#Window_Clear_Interval').val().f(intervalId));

                expect(window.clearInterval).toHaveBeenCalledWith(1);
            });

            it('Should be Document_SetTitle', function() {

                var code = $('#Document_SetTitle').val().f("vlad");
                eval(code);

                expect(document.title).toEqual('vlad');
            });

        });

    });

    afterEach(function() {
        $('#jasmine-fixtures').remove();
        $.mockjaxClear();
    });

});

function FakeMapEngine() {
    this.wasTarget = null;
    this.wasOptions = null;
    this.init = function(target, options) {
        this.wasTarget = target;
        this.wasOptions = options;
    };
}