"use strict";

var testEvalResult;

function testEvalMethod(arg1, arg2, arg3) {
    testEvalResult = {
        arg1 : arg1,
        arg2 : arg2,
        arg3 : arg3
    };
}

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

    this.SandboxHidden = function(attr) {
        var res = $('<input>').attr({ id : 'sandboxHidden', type : 'hidden', name : 'sandboxHidden' });
        if (attr) {
            $(res).attr(attr);
        }
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

    this.SandboxTag = function(tag, content) {
        var span = $('<{0}>'.f(tag)).html(content);
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

                    it('Should be remove', function() {
                        var expected = [5, 10, 25];
                        expected.remove(1, 1);
                        expect(expected).toEqual([5, 25]);
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

                        it('Should be fparam without key', function() {
                            var url = $.url('http://sample.com/Home/Index');
                            expect(url.fparam('key', 'root')).toEqual('');
                        });

                        it('Should be parse absolute url', function() {
                            var href = 'http://sample.com/Home/Index?param=value&param2=value2#!Manager/Office?param=fragmentValue/param2=fragmentValue2&SearchUrl:Search/Index?param=value/param2=value2';
                            var url = $.url(href);
                            verify(url, {
                                base : 'http://sample.com/Home/Index',                                
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

                        it('Should be parse with hash url', function() {
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

                        it('Should be parse with empty hash', function() {
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

                        it('Should be parse hash query decode fparam', function() {
                            var href = 'Home/Index#!value=12%2F12%2F12';

                            var url = $.url(href);
                            expect(url.fparam('value', 'root')).toEqual('12/12/12');
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


                        it('Should be to href with hash url (pazar issue)', function () {
                            var href = 'http://localhost:9435/mk/Dispatcher/Render?incType=GetAdvertiserIndexPageQuery&incView=~%2FViews%2FAdvertiser%2FIndex.cshtml#!/mk/Dispatcher/Render?incType=GetAccountSavedAdNotificationQry/incView=~%2FViews%2FAdvertiser%2FSaved.cshtml';
                            var url = $.url(href);
                            expect(url.furl('root')).toEqual('/mk/Dispatcher/Render');
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

                            expect(url.fparam('param2', 'root')).toEqual('');
                        });

                        it('Should be set new hash parameter', function() {
                            var url = $.url('http://sample.com/Home/Index?param=value&param2=value2#!Manager/Office');

                            var newFragmentValue = 'newFragmentValue';
                            url.setFparam('newParam', newFragmentValue, 'root');

                            expect(url.fparam('newParam', 'root')).toEqual(newFragmentValue);
                        });

                        it('Should be set new hash parameter without hash', function() {
                            var url = $.url('http://sample.com/Home');

                            var newFragmentValue = 'newFragmentValue';
                            url.setFparam('newParam', newFragmentValue, 'root');

                            expect(url.fparam('newParam', 'root')).toEqual(newFragmentValue);
                        });

                        it('Should be set new hash parameter with decode', function() {
                            var url = $.url('http://sample.com/Home#!Value=1');

                            var newFragmentValue = '12/12/12/';
                            url.setFparam('value', newFragmentValue, 'root');

                            expect(url.fparam('value', 'root')).toEqual(newFragmentValue);
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

                    describe('When increment', function() {

                        it('Should be with value nan', function() {

                            var input = TestHelper.Instance.SandboxHidden();
                            $(input).increment(1);
                            expect(input).toHaveValue(1);
                        });

                        it('Should be with value not numeric', function() {

                            var input = TestHelper.Instance.SandboxHidden();
                            $(input).val('test');
                            $(input).increment(1);
                            expect(input).toHaveValue(1);
                        });

                        it('Should be with value', function() {

                            var input = TestHelper.Instance.SandboxHidden();
                            $(input).val(2);
                            $(input).increment(1);
                            expect(input).toHaveValue(3);
                        });

                        it('Should be with value under collection', function() {

                            var inputs = $(TestHelper.Instance.SandboxHidden())
                                .add(TestHelper.Instance.SandboxHidden());
                            $(inputs).val(2);
                            $(inputs).increment(1);
                            $(inputs).each(function() {
                                expect(this).toHaveValue(3);
                            });
                        });

                        it('Should be with negative value', function() {

                            var input = TestHelper.Instance.SandboxHidden();
                            $(input).val(2);
                            $(input).increment(-1);
                            expect(input).toHaveValue(1);
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

                it('Should be element', function() {
                    expect(new IncodingMetaElement(instanceSandBox).element).toEqual(instanceSandBox);
                });

                it('Should be executables', function() {                    
                    instanceSandBox.attr({ "incoding" : '[{ "type" : "Action" }, { "type" : "Action2" }]' });

                    var callbacks = new IncodingMetaElement(instanceSandBox).getExecutables();

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

                    it('Should be action', function() {
                        runner.Registry('Action', undefined, { is : true });
                        expect(runner.actions[0].is).toBeTruthy();
                    });

                    it('Should be before', function() {
                        runner.Registry('Executable', 1, { is : true });
                        expect(runner.before[0].is).toBeTruthy();
                    });

                    it('Should be success', function() {
                        runner.Registry('Executable', 2, { is : true });
                        expect(runner.success[0].is).toBeTruthy();
                    });

                    it('Should be error', function() {
                        runner.Registry('Executable', 3, { is : true });
                        expect(runner.error[0].is).toBeTruthy();
                    });

                    it('Should be complete', function() {
                        runner.Registry('Executable', 4, { is : true });
                        expect(runner.complete[0].is).toBeTruthy();
                    });

                    it('Should be break', function() {
                        runner.Registry('Executable', 5, { is : true });
                        expect(runner.breakes[0].is).toBeTruthy();
                    });

                    it('Should be undefined', function() {
                        expect(function() {
                            runner.Registry('Executable', 6, { is : true });
                        }).toThrow('Not found status 6');
                    });

                });

                describe('When doIt', function() {

                    var event, action;

                    var createCallback = function(bind, execute) {
                        var callback = new ExecutableBase();
                        callback.onBind = bind;

                        spyOn(callback, 'execute').andCallFake(function() {
                            if (execute) {
                                execute();
                            }
                        });
                        return callback;
                    };
                    beforeEach(function() {
                        event = $.Event('click');
                        action = createCallback(event.type);
                        runner.actions.push(action);
                    });

                    it('Should be break on before', function() {

                        runner.before.push(createCallback(event.type, function() {
                            throw new IncClientException();
                        }));
                        runner.breakes.push(createCallback(event.type));

                        runner.DoIt(event);

                        expect(runner.before[0].event).toEqual(event);
                        expect(runner.before[0].execute).toHaveBeenCalled();

                        expect(runner.breakes[0].event).toEqual(event);
                        expect(runner.breakes[0].execute).toHaveBeenCalledWith(new IncClientException());

                        expect(action.execute).not.toHaveBeenCalled();
                    });

                    it('Should be throw on before', function() {
                        runner.before.push(createCallback(event.type, function() {
                            throw 'SpecificationException';
                        }));

                        expect(function() {
                            runner.DoIt(event);
                        }).toThrow('SpecificationException');

                        expect(action.execute).not.toHaveBeenCalled();
                    });

                    it('Should be doIt', function() {
                        runner.before.push(createCallback(event.type));
                        runner.breakes.push(createCallback(event.type));

                        runner.DoIt(event, IncodingResult.Empty);

                        expect(runner.before[0].event).toEqual(event);
                        expect(runner.before[0].execute).toHaveBeenCalled();
                        expect(action.event).toEqual(event);
                        expect(action.execute).toHaveBeenCalledWith({
                            success : runner.success,
                            error : runner.error,
                            complete : runner.complete,
                            breakes : runner.breakes,

                            eventResult : IncodingResult.Empty,
                            event : event
                        });

                        expect(runner.breakes[0].execute).not.toHaveBeenCalled();
                    });

                    it('Should be doIt multiple', function() {
                        runner.before.push(createCallback(event.type));
                        runner.before.push(createCallback('noclick'));
                        runner.before.push(createCallback(event.type));

                        runner.DoIt(event, IncodingResult.Empty);

                        expect(runner.before[0].event).toEqual(event);
                        expect(runner.before[0].execute).toHaveBeenCalled();
                        expect(runner.before[1].event).toEqual('');
                        expect(runner.before[1].execute).not.toHaveBeenCalled();
                        expect(runner.before[2].event).toEqual(event);
                        expect(runner.before[2].execute).toHaveBeenCalled();
                    });

                    it('Should be DoIt filter by event type', function() {

                        var noClickAction = createCallback('noclick');
                        runner.actions.push(noClickAction);
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
                            eventResult : IncodingResult.Empty,
                            event : event
                        });
                    });

                    it('Should be DoIt filter contains event type', function () {
                        var multipleAction = createCallback('click otherevent');
                        var otherEvent = $.Event('otherevent');

                        runner.actions.push(multipleAction);
                        runner.success.push(multipleAction);
                        runner.error.push(multipleAction);
                        runner.breakes.push(multipleAction);
                        runner.complete.push(multipleAction);
                        

                        runner.DoIt(otherEvent, IncodingResult.Empty);
                        
                        expect(multipleAction.execute).toHaveBeenCalledWith({
                            success: [multipleAction],
                            error: [multipleAction],
                            complete: [multipleAction],
                            breakes: [multipleAction],
                            eventResult : IncodingResult.Empty,
                            event: otherEvent
                        });
                    });

                    it('Should be DoIt filter by action', function() {
                        var mutlipleAction = createCallback('click changes');
                        runner.actions.push(mutlipleAction);
                        var singleExeuctable = createCallback('click');
                        var multipleExeuctable = createCallback('click changes');

                        runner.success.push(singleExeuctable);
                        runner.success.push(multipleExeuctable);

                        runner.DoIt(event, IncodingResult.Empty);

                        expect(action.execute).toHaveBeenCalledWith({
                            success : [singleExeuctable],
                            error : [],
                            complete : [],
                            breakes : [],
                            eventResult : IncodingResult.Empty,
                            event : event
                        });
                        expect(mutlipleAction.execute).toHaveBeenCalledWith({
                            success : [multipleExeuctable],
                            error : [],
                            complete : [],
                            breakes : [],
                            eventResult : IncodingResult.Empty,
                            event : event
                        });

                    });

                });

            });

            describe('When Value', function() {
                
                it('Should be string', function() {
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Value_String').val());
                    expect(res).toEqual('aws');
                });

            });

            describe('When Result', function() {

                it('Should be for', function() {
                    ExecutableHelper.Instance.result = 'message';
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Result').val());
                    expect(res).toEqual(ExecutableHelper.Instance.result.Text);
                });

                it('Should be for property', function() {
                    ExecutableHelper.Instance.result = { Text : 'message' };
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Result_For_Property').val());
                    expect(res).toEqual(ExecutableHelper.Instance.result.Text);
                });

                it('Should be for complexity', function() {
                    var value = 'value';
                    ExecutableHelper.Instance.result = { Inner : { Value : value } };
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Result_For_Complexity').val());
                    expect(res).toEqual(value);
                });

                it('Should be for array string', function() {
                    ExecutableHelper.Instance.result = { Strings : ['message', 'message2'] };
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Result_For_Array_String').val());
                    expect(res).toEqual(['message', 'message2']);
                });

                it('Should be for array by index', function() {
                    ExecutableHelper.Instance.result = { Inners : [{ Value : 1 }, { Value : 2 }] };
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Result_For_Array_By_Index').val());
                    expect(res).toEqual(2);
                });

                it('Should be for array select', function() {
                    ExecutableHelper.Instance.result = { Inners : [{ Value : 1 }, { Value : 2 }] };
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Result_For_Array_Select').val());
                    expect(res).toEqual([1, 2]);
                });

                it('Should be for array by index as self', function() {
                    ExecutableHelper.Instance.result = [{ Value : 1 }, { Value : 2 }];
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Result_For_Array_By_Index_As_Self').val());
                    expect(res).toEqual(1);
                });

                it('Should be for array by index as self string', function() {
                    ExecutableHelper.Instance.result = ['1', '2'];
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Result_For_Array_By_Index_As_Self_String').val());
                    expect(res).toEqual('1');
                });

                it('Should be for array any true', function() {
                    TestHelper.Instance.SandboxTextBox({ value : 3 });
                    ExecutableHelper.Instance.result = { Inners : [{ Value : 1 }, { Value : 3 }] };
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Result_For_Array_Any').val());
                    expect(res).toEqual(true);
                });

                it('Should be for array any false', function() {
                    TestHelper.Instance.SandboxTextBox({ value : 5 });
                    ExecutableHelper.Instance.result = { Inners : [{ Value : 1 }, { Value : 3 }] };
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Result_For_Array_Any').val());
                    expect(res).toEqual(false);
                });

            });

            describe('When Event', function() {

                var event, originalEvent;

                beforeEach(function() {

                    originalEvent = jQuery.Event('keydown', {
                        which : 64,
                        metaKey : 25,
                        screenX : 15,
                        screenY : 2014,
                        pageX : 45,
                        pageY : 87,
                    });

                    $($('div').get(0)).keydown(function(e) {
                            event = e;
                        })
                        .trigger(originalEvent);
                    ExecutableHelper.Instance.event = event;
                });

                it('Should be sceen x', function() {
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Event_ScreenX').val());
                    expect(res).toEqual(originalEvent.screenX);
                });

                it('Should be sceen y', function() {
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Event_ScreenY').val());
                    expect(res).toEqual(originalEvent.screenY);
                });

                it('Should be type', function() {
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Event_Type').val());
                    expect(res).toEqual('keydown');
                });

                it('Should be page x', function() {
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Event_PageX').val());
                    expect(res).toEqual(originalEvent.pageX);
                });

                it('Should be page y', function() {
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Event_PageY').val());
                    expect(res).toEqual(originalEvent.pageY);
                });

                it('Should be which', function() {
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Event_Which').val());
                    expect(res).toEqual(originalEvent.which);
                });

                it('Should be meta key', function() {
                    var res = ExecutableHelper.Instance.TryGetVal($('#Selector_Event_MetaKey').val());
                    expect(res).toEqual(originalEvent.metaKey);
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
                    var executable = ExecutableFactory.Create('ExecutableInsert', $.parseJSON('{ "onBind":"Value","timeOut":5, "interval":10,"intervalId":"id","target":"$(\'#sandboxSubmit\')","ands":[1,2] }'), instanceSandBox);

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
                    var executable = ExecutableFactory.Create('ExecutableInsert', $.parseJSON('{ "onBind":"Value","timeOut":5,"target":"$(this.self)" }'), instanceSandBox);

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

                    it('Should be ExecutableFactory.Create', function() {

                        var incData = '[{ "type" : "DirectAction", "data" : { "innerText" : "innerText", "onBind" : "click", "onEventStatus":1 } }]';
                        $(instanceSandBox).attr('incoding', incData);

                        spyOn(ExecutableFactory, 'Create');

                        engine.parse(instanceSandBox);

                        expect(ExecutableFactory.Create).toHaveBeenCalledWith('DirectAction', { "innerText" : "innerText", "onBind" : "click", "onEventStatus" : 1 }, jasmine.any(Object));
                    });

                    it('Should be only once', function() {

                        $(instanceSandBox).attr('incoding', '[{ "type" : "Action", "data" : { "onBind" : "click blur","onEventStatus":1  } }]');

                        spyOn(ExecutableFactory, 'Create');

                        engine.parse(instanceSandBox);
                        engine.parse(instanceSandBox);

                        expect(ExecutableFactory.Create.callCount).toEqual(1);

                    });

                    describe('When bind', function() {

                        var fakeExecute;

                        beforeEach(function() {
                            $(instanceSandBox).removeData('incoding-runner');
                            fakeExecute = new ExecutableDirectAction();
                            spyOn(fakeExecute, 'execute');

                            spyOn(ExecutableFactory, 'Create').andReturn(fakeExecute);
                        });

                        var stubMetaBind = function(bind) {
                            instanceSandBox = $('#sandbox').attr('incoding', JSON.stringify([{ type: 'ExecutableDirectAction', data: { onBind: bind, onEventStatus: 1 } }]));
                            fakeExecute.onBind = bind;
                        };

                        it('Should be bind', function() {
                            stubMetaBind('click');

                            engine.parse(instanceSandBox);

                            $(instanceSandBox).trigger('click');
                            expect(fakeExecute.execute).toHaveBeenCalled();
                        });

                        it('Should be bind with trim', function() {
                            stubMetaBind(' click ');

                            engine.parse(instanceSandBox);

                            $(instanceSandBox).trigger('click');
                            expect(fakeExecute.execute).toHaveBeenCalled();
                        });

                        it('Should be bind once', function() {
                            instanceSandBox = $('#sandbox').attr('incoding', JSON.stringify([
                                { type : 'ExecutableDirectAction', data : { onBind : 'click', onEventStatus : 1 } },
                                { type : 'ExecutableDirectAction', data : { onBind : 'click', onEventStatus : 1 } }
                            ]));
                            fakeExecute.onBind = 'click';

                            engine.parse(instanceSandBox);

                            $(instanceSandBox).trigger('click');
                            expect(fakeExecute.execute).toHaveBeenCalled();
                        });

                        it('Should be at once trigger init incoding', function() {
                            stubMetaBind('initincoding click');
                            engine.parse(instanceSandBox);
                            expect(fakeExecute.execute).toHaveBeenCalled();
                        });

                        it('Should be bind init incoding', function() {
                            stubMetaBind('initincoding click');

                            engine.parse(instanceSandBox);
                            $(instanceSandBox).trigger('initincoding');

                            expect(fakeExecute.execute.callCount).toEqual(2);
                        });

                        it('Should be init incoding with left whitespace', function() {
                            stubMetaBind('click initincoding');

                            engine.parse(instanceSandBox);
                            expect(fakeExecute.execute).toHaveBeenCalled();
                        });

                        it('Should be bind init incoding with right whitespace', function() {
                            stubMetaBind('initincoding click');

                            engine.parse(instanceSandBox);
                            expect(fakeExecute.execute).toHaveBeenCalled();
                        });

                        it('Should be bind init incoding with both whitespace', function() {
                            stubMetaBind('change initincoding click');

                            engine.parse(instanceSandBox);
                            expect(fakeExecute.execute).toHaveBeenCalled();
                        });

                        it('Should be bind inc change url', function() {
                            stubMetaBind('incchangeurl');

                            engine.parse(instanceSandBox);

                            runs(function() {
                                document.location.hash = '!'+Date.now().toString();
                            });

                            waits(500);

                            runs(function() {
                                expect(fakeExecute.execute).toHaveBeenCalled();
                            });

                        });
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
                    expect(IncSpecialBinds.DocumentBinds.contains(IncSpecialBinds.IncGlobalError)).toBeTruthy();

                    expect(IncSpecialBinds.DocumentBinds.length).toEqual(7);

                });
            });

            describe('When AjaxAdapter', function() {

                var adapter;

                beforeEach(function() {
                    adapter = new AjaxAdapter();
                });

                describe('When request', function() {

                    var fakeUrl, ajaxOptions;

                    beforeEach(function() {

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
                        var isCallback = false;
                        runs(function() {
                            adapter.request($.extend({}, ajaxOptions, true), function(data) {
                                expect(data instanceof IncodingResult).toBeTruthy();
                                isCallback = true;
                            });
                        });

                        waits(500);

                        runs(function() {
                            expect(isCallback).toBeTruthy();
                        });
                    });

                    describe('When special events', function() {
                        beforeEach(function() {
                            spyOnEvent(document, IncSpecialBinds.IncAjaxBefore);
                            spyOnEvent(document, IncSpecialBinds.IncAjaxSuccess);
                            spyOnEvent(document, IncSpecialBinds.IncAjaxComplete);
                            spyOnEvent(document, IncSpecialBinds.IncAjaxError);
                        });

                        it('Should be success', function() {

                            runs(function() {
                                adapter.request($.extend({}, ajaxOptions, true), function() {
                                });
                            });
                            waits(500);
                            runs(function() {
                                expect(IncSpecialBinds.IncAjaxBefore).toHaveBeenTriggeredOn(document);
                                +expect(IncSpecialBinds.IncAjaxComplete).toHaveBeenTriggeredOn(document);
                                +expect(IncSpecialBinds.IncAjaxSuccess).toHaveBeenTriggeredOn(document);
                                +expect(IncSpecialBinds.IncAjaxError).not.toHaveBeenTriggeredOn(document);
                            });
                        });

                        it('Should be error', function() {
                            runs(function() {
                                var extend = $.extend({}, ajaxOptions, true);
                                extend.url = "bad url";
                                adapter.request(extend, function() {
                                });
                            });
                            waits(500);
                            runs(function() {
                                expect(IncSpecialBinds.IncAjaxBefore).toHaveBeenTriggeredOn(document);
                                expect(IncSpecialBinds.IncAjaxComplete).toHaveBeenTriggeredOn(document);
                                expect(IncSpecialBinds.IncAjaxSuccess).not.toHaveBeenTriggeredOn(document);
                                expect(IncSpecialBinds.IncAjaxError).toHaveBeenTriggeredOn(document);
                            });
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
                        catch (e) {
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
                        var element = TestHelper.Instance.SandboxTag('span');
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

                    it('Should be set val to hidden', function() {
                        var element = TestHelper.Instance.SandboxHidden();
                        var data = 'Data';

                        ExecutableHelper.Instance.TrySetValue(element, data);

                        expect($(element).val()).toEqual(data);
                    });

                    it('Should be set val to hidden with checkbox', function() {
                        appendSetFixtures($('<input>').attr({
                            type : 'checkbox',
                            name : 'sandboxHidden'
                        }));
                        var element = TestHelper.Instance.SandboxHidden();
                        var data = 'Data';

                        ExecutableHelper.Instance.TrySetValue(element, data);

                        expect($(element).val()).toEqual(data);
                    });

                    it('Should be set radio', function() {
                        var data = 'aws';
                        var elementWithData = TestHelper.Instance.SandboxRadioButton(false, data);
                        var elementWithoutData = TestHelper.Instance.SandboxRadioButton(true, '123');

                        ExecutableHelper.Instance.TrySetValue(elementWithData, data);

                        expect(elementWithData).toHaveProp('checked', true);
                        expect(elementWithoutData).toHaveProp('checked', false);
                    });

                    it('Should be set radio with decode name', function() {
                        var data = 'aws';
                        var elementWithData = $('<input>')
                            .attr({ name : 'customer[1].IsChecked', type : 'radio', value : data })
                            .prop('checked', false);
                        var elementWithoutData = $('<input>').attr({ name : 'customer[1].IsChecked', type : 'radio', value : '123' })
                            .prop('checked', true);
                        appendSetFixtures(elementWithData.add(elementWithoutData));

                        ExecutableHelper.Instance.TrySetValue(elementWithData, data);

                        expect(elementWithData).toHaveProp('checked', true);
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

                        it('Should be set in textbox with multiple attribute', function () {

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

                    describe('When common', function() {

                        it('Should be empty', function() {
                            var val = ExecutableHelper.Instance.TryGetVal('');
                            expect(val).toEqual('');
                        });

                        it('Should be null object', function() {
                            var val = ExecutableHelper.Instance.TryGetVal({});
                            expect(val).toEqual({});
                        });

                        it('Should be value', function() {
                            var selector = '{{item}},{value]}}}{{{';
                            var val = ExecutableHelper.Instance.TryGetVal(selector);

                            expect(val).toEqual(selector);
                        });

                        it('Should be value as tag', function() {
                            var selector = 'select';
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

                    describe('When Jquery', function() {

                        it('Should be script', function() {
                            var val = ExecutableHelper.Instance.TryGetVal($("#sandboxScript"));

                            var length = 362;
                            if (jQuery.browser.msie) {
                                if (jQuery.browser.version <= 8) {
                                    length = 390;
                                }
                            }
                            expect(val.length).toEqual(length);

                        });

                        it('Should be content tag', function() {
                            $(['span', 'div', 'h3']).each(function() {
                                var content = 'value';
                                var element = TestHelper.Instance.SandboxTag(this, content);
                                var val = ExecutableHelper.Instance.TryGetVal(element);
                                expect(content).toEqual(val);
                            });                            
                        });

                        it('Should be not found element', function() {
                            var val = ExecutableHelper.Instance.TryGetVal('$("[name=123sandboxTextBox123]")');
                            expect(val).toEqual('');
                        });

                        it('Should be textbox with comma', function() {
                            var original = 'aws,qwerty';
                            TestHelper.Instance.SandboxTextBox({ value : original });

                            var val = ExecutableHelper.Instance.TryGetVal('$("[name=sandboxTextBox]")');
                            expect(val).toEqual(original);
                        });

                        it('Should be checkbox true', function() {
                            appendSetFixtures($('<input>').attr({
                                id : 'sandboxCheckbox',
                                name : 'sandboxCheckbox',
                                value : false,
                                type : 'hidden'
                            }));
                            TestHelper.Instance.SandboxCheckBox(true, true);

                            var val = ExecutableHelper.Instance.TryGetVal('$("[name=sandboxCheckbox]")');
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

          
                        it('Should be more select', function() {
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

                        it('Should be multiple but not select', function() {
                            var selector = $(TestHelper.Instance.SandboxTextBox({ multiple : true, value : '1' }))
                                .append(TestHelper.Instance.CreateOption(1, true), TestHelper.Instance.CreateOption(2, true), TestHelper.Instance.CreateOption(3, false));

                            var val = ExecutableHelper.Instance.TryGetVal(selector);
                            expect(val).toEqual('1');
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

                        it('Should be property', function() {
                            var val = ExecutableHelper.Instance.TryGetVal("$('#my-fixture').length");
                            expect(val).toEqual(1);
                        });

                        it('Should be method', function() {
                            var val = ExecutableHelper.Instance.TryGetVal("$('#my-fixture').is('div')");
                            expect(val).toEqual(true);
                        });

                    });

                describe('When special', function() {

                    it('Should be double special', function() {
                        var cookiesVal = 'cookiesVal';
                        $.cookie('*abc', cookiesVal);
                        var val = ExecutableHelper.Instance.TryGetVal('||cookie**abc||');
                        expect(val).toEqual(cookiesVal);
                    });

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
                        var actualvalue = 'actualValue';
                        TestHelper.Instance.SandboxTextBox({ value : actualvalue });

                        var val = ExecutableHelper.Instance.TryGetVal($('#Selector_Incoding_Ajax').val());
                        expect(val).toEqual(actualvalue);
                    });

                    it('Should be build url', function() {
                        var actualvalue = 'actualValue';
                        var hashValue = 'hashValue';
                        TestHelper.Instance.SandboxTextBox({ value : actualvalue });
                        TestHelper.Instance.SandboxHidden({ value : hashValue });

                        var val = ExecutableHelper.Instance.TryGetVal($('#Selector_Incoding_Build_Url').val());
                        expect(val).toEqual('/Jasmine/GetValue?Value=actualValue#!Hash=hashValue');
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
                        expect(ExecutableHelper.Compare('Vlad', 'ad', 'iscontains')).toBeTruthy();
                    });

                    it('Should be contains false', function() {
                        expect(ExecutableHelper.Compare('Vlad', 'sd', 'iscontains')).toBeFalsy();
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
                        document.location.hash = '!aws';
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
                        document.location.hash = '!~/Areas/Admin/Views/Category/Index.cshtml'; //set encode value
                        var selfEncode = '{0}#!{1}'.f(document.location.href.split("#")[0], '~%2FAreas%2FAdmin%2FViews%2FCategory%2FIndex.cshtml');
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

    describe('When TemplateFactory', function() {

        var builder, evaluatedSelector, compile, item, selectorKey;

        beforeEach(function() {
            evaluatedSelector = function() {
                return 'selector';
            };
            compile = 'compile';
            item = { id : 1 };
            selectorKey = 'selectorKey';
            builder = new IncMustacheTemplate();
            spyOn(builder, 'render');
            spyOn(builder, 'compile').andReturn(compile);

            navigator.Ie8 = false;
            TemplateFactory.Version = '';
            localStorage.removeItem(selectorKey);
        });

        it('Should be ToHtml item', function() {
            TemplateFactory.ToHtml(builder, selectorKey, evaluatedSelector, item);
            expect(builder.render).toHaveBeenCalledWith(compile, { data : [item] });
        });

        it('Should be ToHtml empty', function() {
            TemplateFactory.ToHtml(builder, selectorKey, evaluatedSelector, '');
            expect(builder.render).toHaveBeenCalledWith(compile, { data : '' });
        });

        it('Should be ToHtml null', function() {
            TemplateFactory.ToHtml(builder, selectorKey, evaluatedSelector, {});
            expect(builder.render).toHaveBeenCalledWith(compile, { data : {} });
        });

        it('Should be ToHtml items', function() {
            TemplateFactory.ToHtml(builder, selectorKey, evaluatedSelector, [item]);
            expect(builder.render).toHaveBeenCalledWith(compile, { data : [item] });
        });

        it('Should be ToHtml compile from local storage', function() {
            localStorage.setItem(selectorKey, 1);
            TemplateFactory.ToHtml(builder, selectorKey, evaluatedSelector, [{ id : 1 }]);
            expect(builder.render).toHaveBeenCalledWith('1', { data : [item] });
        });

        it('Should be ToHtml compile from local storage with version', function() {
            localStorage.setItem(selectorKey + 'aws', 1);
            TemplateFactory.Version = 'aws';
            TemplateFactory.ToHtml(builder, selectorKey, evaluatedSelector, [{ id : 1 }]);
            expect(builder.render).toHaveBeenCalledWith('1', { data : [item] });
        });

        it('Should be ToHtml ie8', function() {
            navigator.Ie8 = true;
            spyOn(localStorage, 'getItem').andReturn('1');
            spyOn(localStorage, 'setItem');

            TemplateFactory.ToHtml(builder, selectorKey, evaluatedSelector, [{ id : 1 }]);

            expect(builder.render).toHaveBeenCalledWith('1', { data : [item] });
            expect(localStorage.getItem).toHaveBeenCalledWith(selectorKey + 'ie8');
            expect(localStorage.setItem).not.toHaveBeenCalled();
        });

        it('Should be ToHtml local storage get with throw', function() {
            spyOn(localStorage, 'getItem').andThrow();
            spyOn(localStorage, 'setItem');

            TemplateFactory.ToHtml(builder, selectorKey, evaluatedSelector, [{ id : 1 }]);

            expect(builder.render).toHaveBeenCalledWith(compile, { data : [item] });
            expect(localStorage.setItem).toHaveBeenCalledWith(selectorKey, compile);
        });

        it('Should be ToHtml local storage set with empty evaluated selector', function() {
            spyOn(localStorage, 'getItem').andReturn('');

            expect(function() {
                TemplateFactory.ToHtml(builder, selectorKey, function() {
                    return '';
                }, { data : [item] });
            }).toThrow('Template is empty');

        });

        it('Should be ToHtml local storage set with throw quota', function() {
            spyOn(localStorage, 'setItem').andThrow({ name : 'QUOTA_EXCEEDED_ERR' });
            spyOn(localStorage, 'clear');

            expect(function() {
                TemplateFactory.ToHtml(builder, selectorKey, evaluatedSelector, [{ id : 1 }]);
            }).not.toThrow();

            expect(builder.render).toHaveBeenCalledWith(compile, { data : [item] });
            expect(localStorage.clear).toHaveBeenCalled();
        });

        it('Should be ToHtml local storage set with throw', function() {
            spyOn(localStorage, 'setItem').andThrow('SpecificationException');
            spyOn(localStorage, 'clear');

            expect(function() {
                TemplateFactory.ToHtml(builder, selectorKey, evaluatedSelector, [{ id : 1 }]);
            }).not.toThrow();

            expect(builder.render).toHaveBeenCalledWith(compile, { data : [item] });
            expect(localStorage.clear).not.toHaveBeenCalled();
        });

    });

    describe('Template', function() {

        var item, items, compileTemplate, engine;

        beforeEach(function() {
            item = { data : { title : "Joe" } };
            items = { data : [{ title : "Joe" }, { title : "Joe" }] };
        });

        describe('When Mustaches', function() {

            beforeEach(function() {
                engine = new IncMustacheTemplate();
                compileTemplate = engine.compile('{{#data}} {{title}} "spends" {{/data}}');
            });

            it('Should be render from localStorage', function() {

                localStorage.setItem('tempMustache', compileTemplate);
                var view = engine.render(localStorage.getItem('tempMustache'), item);
                expect(view).toEqual(' Joe "spends" ');

            });

            it('Should be render item', function() {

                var view = engine.render(compileTemplate, item);
                expect(view).toEqual(' Joe "spends" ');

            });

            it('Should be render array item', function() {

                var view = engine.render(compileTemplate, items);
                expect(view).toEqual(' Joe "spends"  Joe "spends" ');
            });

        });

        describe('When Handlebars', function() {

            beforeEach(function() {
                engine = new IncHandlerbarsTemplate();
                compileTemplate = engine.compile('{{#data}} {{title}} "spends" {{/data}}');
            });

            it('Should be render item', function() {

                var view = engine.render(compileTemplate, item);
                expect(view).toEqual(' Joe "spends" ');

            });

            it('Should be render from localStorage', function() {

                localStorage.setItem('tempHanldebars', compileTemplate);
                var view = engine.render(localStorage.getItem('tempHanldebars'), item);
                expect(view).toEqual(' Joe "spends" ');

            });

            it('Should be render array item', function() {

                var view = engine.render(compileTemplate, items);
                expect(view).toEqual(' Joe "spends"  Joe "spends" ');
            });

        });

    });

    describe('Executable', function() {

        describe('When ExecutableBase', function() {

            it('Should be initialize default', function() {
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

                var executable, expectedState;

                beforeEach(function() {
                    expectedState = 'aws';
                    executable = new ExecutableBase();
                    executable.timeOut = 0;
                    executable.isValid = function() {
                        return true;
                    };
                    executable.getTarget = function() {
                        return 5;
                    };
                });

                it('Should be execute', function() {
                    var callExecute = false;
                    executable.internalExecute = function(state) {
                        callExecute = state == expectedState;
                    };

                    executable.execute(expectedState);

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

                    executable.execute(expectedState);

                    expect(callExecute).toBeFalsy();
                    expect(executable.target).toEqual(5);
                });

                it('Should be execute time out', function() {
                    ExecutableHelper.Compare;
                    var callCount = 0;
                    executable.internalExecute = function(data) {
                        if (data == expectedState) {
                            callCount++;
                        }
                    };
                    executable.timeOut = 500;

                    runs(function() {
                        executable.execute(expectedState);
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
                        if (data == expectedState) {
                            callCount++;
                        }
                    };
                    executable.interval = 500;
                    executable.intervalId = '156EE3CE_9799_4BCB_9EE7_7FA61FC277B8';

                    runs(function() {
                        executable.execute(expectedState);
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
                        executable.result = expectedData;
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

                        expect(executable.isValid()).toBeTruthy();

                        expect(trueConditional.isSatisfied.callCount).toEqual(2);
                        expect(trueConditional.isSatisfied).toHaveBeenCalledWith(expectedData);
                        expect(falseConditional.isSatisfied).not.toHaveBeenCalled();

                    });

                    it('Should be last group all true', function() {
                        executable.ands = [[{ type : false }], [{ type : true }, { type : true }]];

                        expect(executable.isValid()).toBeTruthy();
                        expect(trueConditional.isSatisfied.callCount).toEqual(2);
                        expect(falseConditional.isSatisfied.callCount).toEqual(1);
                        expect(falseConditional.isSatisfied).toHaveBeenCalledWith(expectedData);
                    });

                    it('Should be group has false', function() {
                        executable.ands = [[{ type : true }, { type : false }]];

                        expect(executable.isValid()).toBeFalsy();
                        expect(trueConditional.isSatisfied).toHaveBeenCalledWith(expectedData);
                        expect(falseConditional.isSatisfied).toHaveBeenCalledWith(expectedData);

                    });

                    it('Should be group false', function() {
                        executable.ands = [[{ type : false }, { type : false }]];

                        expect(executable.isValid()).toBeFalsy();
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
                    success : [jasmine.createSpyObj('callback', ['execute'])],
                    error : [jasmine.createSpyObj('callback', ['execute'])],
                    complete : [jasmine.createSpyObj('callback', ['execute'])],
                    breakes : [jasmine.createSpyObj('callback', ['execute'])],
                };
                result = { redirectTo : '', success : true, data : 'data' };

                action = new ExecutableActionBase();
                action.jsonData = {};
                spyOn(ExecutableHelper, "RedirectTo");
            });

            it('Should be throw', function() {
                state.success[0] = {
                    execute : function() {
                    }
                };
                spyOn(state.success[0], 'execute').andThrow('SpecificationException');
                navigator.Ie8 = false;

                expect(function() {
                    action.complete(result, state);
                }).toThrow('SpecificationException');
            });

            it('Should be mute throw for ie 8', function() {
                state.success[0] = {
                    execute : function() {
                    }
                };
                spyOn(state.success[0], 'execute').andThrow('SpecificationException');
                spyOn(console, 'log');
                navigator.Ie8 = true;

                expect(function() {
                    action.complete(result, state);
                }).not.toThrow();
                expect(console.log).toHaveBeenCalledWith('Incoding exception: SpecificationException');
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
                expect(state.success[0].execute).toHaveBeenCalled();
                expect(state.complete[0].execute).toHaveBeenCalled();
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

                expect(state.success[0].execute).toHaveBeenCalled();
                expect(state.complete[0].execute).toHaveBeenCalled();
                expect(state.breakes[0].execute).toHaveBeenCalled();
                expect(state.error[0].execute).not.toHaveBeenCalled();
            });

            it('Should be error', function() {
                result.success = false;

                action.complete(result, state);

                expect(ExecutableHelper.RedirectTo).not.toHaveBeenCalled();
                expect(state.error[0].execute).toHaveBeenCalled();
                expect(state.complete[0].execute).toHaveBeenCalled();
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
                expect(state.error[0].execute).toHaveBeenCalled();
                expect(state.complete[0].execute).toHaveBeenCalled();
                expect(state.breakes[0].execute).toHaveBeenCalled();
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
                expect(state.complete[0].execute).toHaveBeenCalled();
                expect(state.breakes[0].execute).toHaveBeenCalled();
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

                    expect(fakeComplete.result).toEqual(IncodingResult.Empty.data);
                    expect(fakeComplete.execute).toHaveBeenCalled();
                    expect(fakeError.execute).not.toHaveBeenCalled();
                });

                it('Should be execute result success', function() {

                    action.jsonData = $.parseJSON($('#ExecutableDirectActionWithSucces').val());

                    action.internalExecute(state);

                    expect(fakeSuccess.result).toEqual($.parseJSON(action.jsonData.result).data);
                    expect(fakeSuccess.execute).toHaveBeenCalled();
                    expect(fakeComplete.execute).toHaveBeenCalled();
                    expect(fakeError.execute).not.toHaveBeenCalled();
                });

                it('Should be execute result error', function() {

                    action.jsonData = $.parseJSON($('#ExecutableDirectActionWithError').val());

                    action.internalExecute(state);

                    expect(fakeError.result).toEqual($.parseJSON(action.jsonData.result).data);
                    expect(fakeError.execute).toHaveBeenCalled();
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

                    expect(fakeSuccess.result).toEqual(jsonData.data);
                    expect(fakeSuccess.execute).toHaveBeenCalled();
                    expect(fakeComplete.execute).toHaveBeenCalled();
                    expect(fakeError.execute).not.toHaveBeenCalled();
                });

            });

            describe('When ExecutableAjaxAction', function() {

                var fakeUrl, action;

                beforeEach(function() {

                    $.mockjaxClear();

                    fakeUrl = 'Url/Test';

                    action = new ExecutableAjaxAction();
                    action.self = instanceSandBox;
                    action.target = instanceSandBox;
                });

                it('Should be execute without data', function() {

                    action.jsonData = $.parseJSON($('#ExecutableAjaxActionWithoutData').val());
                    $.mockjax({
                        url : fakeUrl,
                        data : [],
                        type : 'GET',
                        responseText : { data : 'Success', success : true, redirectTo : '' }
                    });

                    runs(function() {
                        action.jsonData.ajax.data = [];
                        action.internalExecute(state);
                    });

                    waits(500);

                    runs(function() {
                        expect(fakeSuccess.result).toEqual('Success');
                        expect(fakeSuccess.execute).toHaveBeenCalled();
                    });
                });

                it('Should be execute', function() {
                    var valBy = 'Value';
                    spyOn(ExecutableHelper.Instance, 'TryGetVal').andCallFake(function(value) {
                        if (value == valBy) {
                            return valBy;
                        }
                    });
                    action.jsonData = $.parseJSON($('#ExecutableAjaxAction').val());
                    $.mockjax({
                        url : fakeUrl,
                        data : [{ Name : valBy }],
                        type : 'GET',
                        responseText : { data : 'Success', success : true, redirectTo : '' }
                    });

                    runs(function() {
                        action.internalExecute(state);
                    });

                    waits(500);

                    runs(function() {
                        expect(fakeSuccess.result).toEqual('Success');
                        expect(fakeSuccess.execute).toHaveBeenCalled();
                        expect(fakeComplete.execute).toHaveBeenCalled();
                        expect(fakeError.execute).not.toHaveBeenCalled();
                        expect(ExecutableHelper.Instance.TryGetVal).toHaveBeenCalledWith(valBy);
                    });
                });

                //it('Should be execute with data', function() {

                //    spyOn(ExecutableHelper.Instance, 'TryGetVal').andCallFake(function(value) {
                //        return value;
                //    });
                //    action.jsonData = $.parseJSON($('#ExecutableAjaxActionWithData').val());
                //    $.mockjax({
                //        url : fakeUrl,
                //        data : [{ Name : 'Value' }, { Name : 'Value2' }],
                //        type : 'GET',
                //        responseText : { data : 'Success', success : true, redirectTo : '' }
                //    });

                //    runs(function() {
                //        action.internalExecute(state);
                //    });

                //    waits(500);

                //    runs(function() {
                //        expect(fakeSuccess.result).toEqual('Success');

                //        expect(ExecutableHelper.Instance.TryGetVal).toHaveBeenCalledWith('Value');
                //        expect(ExecutableHelper.Instance.TryGetVal).toHaveBeenCalledWith('Value2');
                //    });
                //});

                it('Should be execute with hash', function() {
                    var valBy = 'Value';
                    spyOn(ExecutableHelper.Instance, 'TryGetVal').andCallFake(function(value) {
                        if (value == valBy) {
                            return valBy;
                        }
                    });
                    action.jsonData = $.parseJSON($('#ExecutableAjaxActionWithHash').val());
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

                    runs(function() {
                        window.location.hash = '!hash/fakeUrl?Name2=Value2/Name3=Value3';
                        action.internalExecute(state);
                    });

                    waits(500);

                    runs(function() {
                        expect(fakeSuccess.result).toEqual('WithUrlFromHash');
                        expect(fakeSuccess.execute).toHaveBeenCalled();
                        expect(fakeComplete.execute).toHaveBeenCalled();
                        expect(fakeError.execute).not.toHaveBeenCalled();
                        expect(ExecutableHelper.Instance.TryGetVal).toHaveBeenCalledWith(valBy);
                    });

                });

                it('Should be execute with only query string hash', function() {
                    action.jsonData = $.parseJSON($('#ExecutableAjaxActionWithOnlyQueryStringHash').val());
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
                    runs(function() {

                        window.location.hash = '!Name2=Value2/Name3=Value3';
                        action.internalExecute(state);
                    });

                    waits(500);

                    runs(function() {
                        expect(fakeSuccess.result).toEqual('WithQueryStringHash');
                        expect(fakeSuccess.execute).toHaveBeenCalled();
                        expect(fakeComplete.execute).toHaveBeenCalled();
                        expect(fakeError.execute).not.toHaveBeenCalled();
                    });

                });

            });

            describe('When ExecutableSubmitAction', function() {

                var form, inputField, submitButton, action;

                var setAjaxMock = function(response) {
                    $.mockjax({
                        url : 'http://localhost:64225/Url/Test',
                        data : 'name=value',
                        type : 'POST',
                        responseText : response
                    });

                };

                beforeEach(function() {

                    form = $('<form>').attr({ action : 'http://localhost:64225/Url/Test', method : 'POST', id : 'formSubmit' });
                    inputField = $('<input>').attr({ type : 'textbox', id : 'id', name : 'name', value : 'value' });
                    submitButton = $('<input>').attr({ type : 'submit' });
                    form.append(inputField).append(submitButton);
                    appendSetFixtures(form);

                    action = new ExecutableSubmitAction();
                    action.self = submitButton;
                    action.target = submitButton;
                });

                it('Should be success', function() {
                    spyOnEvent(document, IncSpecialBinds.IncAjaxComplete);
                    spyOnEvent(document, IncSpecialBinds.IncAjaxSuccess);
                    spyOnEvent(document, IncSpecialBinds.IncAjaxBefore);
                    spyOnEvent(document, IncSpecialBinds.IncAjaxError);
                    action.jsonData = $.parseJSON($('#ExecutableSubmitAction').val());
                    
                    var response = { data: 'Message', success: true, redirectTo: '' };
                    runs(function() {                        
                        setAjaxMock(response);
                        action.internalExecute(state);
                    });

                    waits(500);

                    runs(function() {
                        expect(fakeSuccess.execute).toHaveBeenCalled();
                        expect(fakeComplete.execute).toHaveBeenCalled();
                        expect(fakeError.execute).not.toHaveBeenCalled();

                        var xhr = IncodingResult.Success(IncAjaxEvent.Create({ success: true, data: 'Message', redirectTo: '' }));
                        expect(IncSpecialBinds.IncAjaxSuccess).toHaveBeenTriggeredOn(document, xhr);
                        expect(IncSpecialBinds.IncAjaxComplete).toHaveBeenTriggeredOn(document, xhr);
                        expect(IncSpecialBinds.IncAjaxBefore).toHaveBeenTriggeredOn(document);
                        expect(IncSpecialBinds.IncAjaxError).not.toHaveBeenTriggeredOn(document);
                    });

                });

                it('Should be error', function() {
                    spyOnEvent(document, IncSpecialBinds.IncAjaxError);
                    spyOnEvent(document, IncSpecialBinds.IncAjaxComplete);
                    action.jsonData = $.parseJSON($('#ExecutableSubmitAction').val());
                    runs(function () {                        
                        action.internalExecute(state);
                    });

                    waits(3000);

                    runs(function() {
                        expect(IncSpecialBinds.IncAjaxError).toHaveBeenTriggeredOn(document);
                        expect(IncSpecialBinds.IncAjaxComplete).toHaveBeenTriggeredOn(document);
                    });

                });

                it('Should be success with url', function() {
                    action.jsonData = $.parseJSON($('#ExecutableSubmitActionWithUrl').val());
                    
                    runs(function() {
                        $.mockjax({
                            url : 'http://localhost:64225/Submit/Get?name=value&CustomParam=123',
                            type : 'GET',
                            responseText : { data : 'Message', success : true, redirectTo : '' }
                        });

                        action.internalExecute(state);
                    });

                    waits(500);

                    runs(function() {
                        expect(fakeSuccess.result).toEqual('Message');
                        expect(fakeSuccess.execute).toHaveBeenCalled();
                    });

                });

                it('Should be success with form', function() {
                    action.jsonData = $.parseJSON($('#ExecutableSubmitActionWithForm').val());
                    spyOnEvent(document, IncSpecialBinds.IncAjaxBefore);

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

                it('Should be broken validate', function() {
                    action.jsonData = $.parseJSON($('#ExecutableSubmitAction').val());
                    spyOn($.fn, 'valid').andReturn(false);
                    var validate = jasmine.createSpyObj('validate', ['focusInvalid']);
                    spyOn($.fn, 'validate').andReturn(validate);

                    runs(function() {
                        setAjaxMock({ data : 'Message', success : true, redirectTo : '' });
                        action.internalExecute(state);
                    });

                    waits(500);

                    runs(function() {
                        expect($.fn.valid).toHaveBeenCalled();
                        expect(validate.focusInvalid).toHaveBeenCalled();

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

                describe('When result', function() {

                    beforeEach(function() {
                        insert.jsonData = $.parseJSON($('#ExecutableInsert').val());
                    });

                    it('Should be insert undifiend', function() {
                        insert.result = undefined;
                        insert.internalExecute();
                        expect(instanceSandBox).toHaveHtml('');
                    });

                    it('Should be insert html', function () {
                        insert.result = 'Content';
                        insert.internalExecute();
                        expect(instanceSandBox).toHaveHtml('Content');
                    });

                    it('Should be insert string object', function () {
                        insert.result = new String('Content');
                        insert.internalExecute();
                        expect(instanceSandBox).toHaveHtml('Content');
                    });

                    it('Should be insert object', function () {
                        insert.result = 1;
                        insert.internalExecute();
                        expect(instanceSandBox).toHaveHtml('1');
                    });

                });

                describe('When result from attribute', function() {

                    beforeEach(function() {
                        insert.jsonData = $.parseJSON($('#ExecutableInsertWithResult').val());
                    });

                    it('Should be insert string', function () {
                        insert.internalExecute();
                        expect(instanceSandBox).toHaveHtml('Content');
                    });

                });

                describe('When result with property', function() {

                    beforeEach(function() {
                        insert.jsonData = $.parseJSON($('#ExecutableInsertWithProperty').val());
                    });

                    it('Should be insert object', function () {
                        insert.result = { prop1 : 1, Prop2 : 2 };
                        insert.internalExecute();

                        expect(instanceSandBox).toHaveHtml('2');
                    });

                    it('Should be insert array', function () {
                        insert.result = [{ prop1: 1, Prop2: 2 }];
                        insert.internalExecute();

                        expect(instanceSandBox).toHaveHtml('2');
                    });

                    it('Should be insert big array', function () {
                        insert.result = [{ Prop2 : 1 }, { Prop2 : 2 }, { Prop2 : 3 }];
                        insert.internalExecute();

                        expect(instanceSandBox).toHaveHtml('1');
                    });

                    it('Should be insert empty object', function () {
                        insert.result = {};
                        insert.internalExecute();

                        expect(instanceSandBox).toHaveHtml({});
                    });

                    it('Should be insert empty array', function () {
                        insert.result = [];
                        insert.internalExecute();

                        expect(instanceSandBox).toHaveHtml({});
                    });

                });

                describe('When result with prepare', function() {

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

                    it('Should be insert array', function () {
                        insert.result = [{ prop1 : 1 }, { prop1 : 2 }];
                        insert.internalExecute();

                        expect(isValue1).toBeTruthy();
                        expect(isValue2).toBeTruthy();
                    });

                    it('Should be insert object', function () {
                        insert.result = { prop1 : 1, prop2 : 2 };
                        insert.internalExecute();

                        expect(isValue1).toBeTruthy();
                        expect(isValue2).toBeTruthy();
                    });

                });

                describe('When insert result with template', function() {

                    var incTemplate;

                    beforeEach(function() {
                        incTemplate = 'renderContent';
                        insert.result = { item: 1 };
                        spyOn(TemplateFactory, 'ToHtml').andReturn(incTemplate);
                    });

                    it('Should be template selector', function() {
                        insert.tryGetVal = function(value) {
                            return value;
                        };
                        insert.jsonData = $.parseJSON($('#ExecutableInsertWithTemplateSelector').val());
                        
                        insert.internalExecute();

                        expect(instanceSandBox).toHaveHtml(incTemplate);
                        expect(TemplateFactory.ToHtml.mostRecentCall.args[0]).toEqual(ExecutableInsert.Template);
                        expect(TemplateFactory.ToHtml.mostRecentCall.args[1]).toEqual('selectorId');
                        expect(TemplateFactory.ToHtml.mostRecentCall.args[2]()).toEqual('selectorId');
                        expect(TemplateFactory.ToHtml.mostRecentCall.args[3]).toEqual({ item : 1 });
                    });

                    it('Should be template ajax', function() {
                        insert.tryGetVal = function(value) {
                            if (value.startsWith('||buildurl*')) {
                                return 'isKey';
                            }
                            else {
                                return value;
                            }
                        };
                        insert.jsonData = $.parseJSON($('#ExecutableInsertWithTemplateAjax').val());
                        insert.internalExecute();

                        expect(instanceSandBox).toHaveHtml(incTemplate);
                        expect(TemplateFactory.ToHtml.mostRecentCall.args[0]).toEqual(ExecutableInsert.Template);
                        expect(TemplateFactory.ToHtml.mostRecentCall.args[1]).toEqual('isKey');
                        expect(TemplateFactory.ToHtml.mostRecentCall.args[2]()).toEqual('||ajax*{"url":"/Jasmine/GetValue?Value=%24(\'%23sandboxTextBox\')","type":"GET","async":false}||');
                        expect(TemplateFactory.ToHtml.mostRecentCall.args[3]).toEqual({ item : 1 });
                    });

                });

                afterEach(function() {
                    expect(IncodingEngine.Current.parse).toHaveBeenCalledWith(instanceSandBox);
                    expect(IncSpecialBinds.IncInsert).toHaveBeenTriggeredOn(document);
                });

            });

            describe('When ExecutableInsert Before and After', function() {

                var insert, div;
                beforeEach(function() {
                    spyOn(IncodingEngine.Current, 'parse');
                    div = $('<div>');
                    $('body').append(div);
                    insert = new ExecutableInsert();                    
                    insert.target = div;
                    insert.result = '<p/>';
                });

                it('Should be insert after', function() {
                    insert.jsonData = $.parseJSON($('#ExecutableInsertAfter').val());
                    insert.internalExecute();
                    expect(IncodingEngine.Current.parse).toHaveBeenCalledWith(div.nextAll());
                });

                it('Should be insert before', function() {
                    insert.jsonData = $.parseJSON($('#ExecutableInsertBefore').val());
                    insert.internalExecute();
                    expect(IncodingEngine.Current.parse).toHaveBeenCalledWith(div.prevAll());
                });

            });

            describe('When ExecutableTrigger', function() {

                var trigger;

                beforeEach(function() {

                    trigger = new ExecutableTrigger();
                    trigger.jsonData = $.parseJSON($('#ExecutableTrigger').val());;
                    trigger.self = instanceSandBox;
                    trigger.target = instanceSandBox;

                    spyOnEvent(instanceSandBox, trigger.jsonData.trigger);
                });

                it('Should be trigger with data as undefined', function() {
                    trigger.result = undefined;
                    trigger.internalExecute();
                    expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : undefined, redirectTo : '' }));
                });

                it('Should be trigger with data as object', function () {
                    trigger.result = { is : true };
                    trigger.internalExecute();
                    expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : { is : true }, redirectTo : '' }));
                });

                it('Should be trigger with data as string', function () {
                    trigger.result = 'content';
                    trigger.internalExecute();
                    expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : 'content', redirectTo : '' }));
                });

                it('Should be trigger with data as int', function () {
                    trigger.result = 5;
                    trigger.internalExecute();
                    expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : 5, redirectTo : '' }));
                });

                it('Should be trigger with data as int object', function () {
                    trigger.result = Number(5);
                    trigger.internalExecute();
                    expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : 5, redirectTo : '' }));
                });

                it('Should be trigger with data as bool', function () {
                    trigger.result = true;
                    trigger.internalExecute();
                    expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : true, redirectTo : '' }));
                });

                it('Should be trigger with data as bool object', function () {
                    trigger.result = Boolean(true);
                    trigger.internalExecute();
                    expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : true, redirectTo : '' }));
                });

                it('Should be trigger data as  array', function () {
                    trigger.result = [5, 6];
                    trigger.internalExecute();
                    expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : [5, 6], redirectTo : '' }));
                });

                it('Should be trigger data as  date time', function() {
                    var currentTime = new Date().now;
                    trigger.result = currentTime;
                    trigger.internalExecute();
                    expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : currentTime, redirectTo : '' }));
                });

                it('Should be trigger with property data', function() {
                    trigger.jsonData = $.parseJSON($('#ExecutableTriggerWithProperty').val());
                    trigger.result = { is: true };
                    trigger.internalExecute();

                    expect(trigger.jsonData.trigger).toHaveBeenTriggeredOn(instanceSandBox, new IncodingResult({ success : true, data : true, redirectTo : '' }));
                });

                it('Should be trigger with property empty data', function() {
                    trigger.jsonData = $.parseJSON($('#ExecutableTriggerWithProperty').val());;
                    trigger.result = {};
                    trigger.internalExecute();

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

                    form = $('<form>')
                        .append(input)
                        .append(span);

                    appendSetFixtures(form);

                    validationRefresh = new ExecutableValidationRefresh();
                    validationRefresh.target = form;

                });

                it('Should be is not valid', function() {
                    var validate = jasmine.createSpyObj('validate', ['focusInvalid']);
                    spyOn($.fn, 'validate').andReturn(validate);
                    $(span).addClass(messageValidClass);

                    validationRefresh.result = [{ name : 'input.Email', errorMessage : '<b>errroMessage</b>', isValid : false }];
                    validationRefresh.internalExecute();

                    expect(input).toHaveClass(inputErrorClass);

                    expect(span).toHaveClass(messageErrorClass);
                    expect(span).toHaveHtml('<span for="input.Email" generated="true"><b>errroMessage</b></span>');
                    expect(span).not.toHaveClass(messageValidClass);
                    expect(validate.focusInvalid).toHaveBeenCalled();
                });

                it('Should be is valid', function() {

                    $(span).addClass(messageErrorClass).html('message');
                    $(input).addClass(inputErrorClass);

                    validationRefresh.result = [{ name: 'input.Email', errorMessage: 'errroMessage', isValid: true }];
                    validationRefresh.internalExecute();

                    expect(input).not.toHaveClass(inputErrorClass);
                    expect(span).not.toHaveClass(messageErrorClass);
                    expect(span).toHaveClass(messageValidClass);
                    expect(span).toHaveHtml('');

                });

                it('Should be is empty', function () {

                    $(span).addClass(messageErrorClass).html('message');
                    $(input).addClass(inputErrorClass);

                    validationRefresh.result = [];
                    validationRefresh.internalExecute();

                    expect(input).not.toHaveClass(inputErrorClass);
                    expect(span).not.toHaveClass(messageErrorClass);
                    expect(span).toHaveClass(messageValidClass);
                    expect(span).toHaveHtml("");

                });

                it('Should be is wrong result (not model state)', function () {

                    $(span).addClass(messageErrorClass).html('message');
                    $(input).addClass(inputErrorClass);

                    validationRefresh.result = {name:'',value:''};
                    validationRefresh.internalExecute();

                    expect(input).not.toHaveClass(inputErrorClass);
                    expect(span).not.toHaveClass(messageErrorClass);
                    expect(span).toHaveClass(messageValidClass);
                    expect(span).toHaveHtml("");

                });

                it('Should be is undefined', function () {

                    $(span).addClass(messageErrorClass).html('message');
                    $(input).addClass(inputErrorClass);

                    validationRefresh.result = undefined;
                    validationRefresh.internalExecute();

                    expect(input).not.toHaveClass(inputErrorClass);
                    expect(span).not.toHaveClass(messageErrorClass);
                    expect(span).toHaveClass(messageValidClass);
                    expect(span).toHaveHtml('');
                });

                afterEach(function() {
                    $(form).remove();
                });

            });

            describe('When ExecutableEval', function() {
                var evalEx;
                beforeEach(function() {
                    evalEx = new ExecutableEval();
                    evalEx.jsonData = $.parseJSON($('#ExecutableEval').val());
                    evalEx.target = instanceSandBox;
                });

                it('Should be eval', function () {
                    evalEx.result = 'data';
                    evalEx.internalExecute();
                    expect(newFakeEvalVariable).toEqual('data');
                });
            });

            describe('When ExecutableEvalMethod', function() {
                var evalEx;
                beforeEach(function() {
                    evalEx = new ExecutableEvalMethod();
                    evalEx.jsonData = $.parseJSON($('#ExecutableEvalMethod').val());
                    evalEx.self = instanceSandBox;
                    evalEx.target = instanceSandBox;
                });

                it('Should be eval', function() {
                    evalEx.internalExecute('data');

                    expect(testEvalResult.arg1).toEqual('arg1');
                    expect(testEvalResult.arg2).toEqual('arg2');
                    expect(testEvalResult.arg3).toEqual('arg3');
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
                });

                describe('When container', function() {

                    var valueInput, valueSelect;

                    beforeEach(function() {

                        $(instanceSandBox).append(TestHelper.Instance.SandboxTextBox(),
                            TestHelper.Instance.SandboxSelect(),
                            TestHelper.Instance.SandboxSubmit(),
                            TestHelper.Instance.SandboxInputButton(),
                            TestHelper.Instance.SandboxReset(),
                            TestHelper.Instance.SandboxInput());

                        valueInput = "sandboxTextBox";
                        valueSelect = 'sandboxSelect';
                        spyOn(ExecutableHelper.Instance, "TryGetVal").andCallFake(function(jObject) {
                            return jObject.attr("name");
                        });
                        spyOn(ExecutableHelper, "RedirectTo");

                        window.document.location.hash = "!aws=param";
                    });

                    it('Should be insert', function () {                   
                        hashInsert.jsonData = $.parseJSON($("#ExecutableStoreInsert").val());
                        hashInsert.target = instanceSandBox;

                        hashInsert.internalExecute();

                        var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                        expect(redirectUrl.fparam('aws', 'root')).toEqual('param');
                        expect(redirectUrl.fparam().hasOwnProperty('sandboxSubmit__root')).toBeFalsy();
                        expect(redirectUrl.fparam('sandboxTextBox', 'root')).toEqual(valueInput);
                        expect(redirectUrl.fparam('sandboxSelect', 'root')).toEqual(valueSelect);
                    });

                    it('Should be insert with decode name', function() {
                        hashInsert.jsonData = $.parseJSON($('#ExecutableStoreInsert').val());
                        $(instanceSandBox)
                            .empty()
                            .append(TestHelper.Instance.SandboxTextBox({ name : 'sandboxTextBox[1].Value', value : valueInput }))
                            .append(TestHelper.Instance.SandboxTextBox({ name : 'sandboxTextBox[2].Value', value : valueInput }));
                        hashInsert.target = instanceSandBox;

                        hashInsert.internalExecute();

                        var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                        expect(redirectUrl.fparam('sandboxTextBox[1].Value', 'root')).toEqual('sandboxTextBox[1].Value');
                        expect(redirectUrl.fparam('sandboxTextBox[2].Value', 'root')).toEqual('sandboxTextBox[2].Value');
                    });

                    it('Should be insert with multiple', function() {
                    
                        hashInsert.jsonData = $.parseJSON($('#ExecutableStoreInsert').val());
                        hashInsert.target = $('[name=sandboxTextBox], [name=sandboxSelect]');

                        hashInsert.internalExecute();

                        var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                        expect(redirectUrl.fparam('aws', 'root')).toEqual('param');
                        expect(redirectUrl.fparam().hasOwnProperty('sandboxSubmit__root')).toBeFalsy();
                        expect(redirectUrl.fparam('sandboxTextBox', 'root')).toEqual("sandboxTextBox");
                        expect(redirectUrl.fparam('sandboxSelect', 'root')).toEqual("sandboxSelect");
                    });

                    it('Should be insert by prefix', function() {
                        hashInsert.jsonData = $.parseJSON($('#ExecutableStoreInsertWithPrefix').val());
                        hashInsert.target = instanceSandBox;

                        hashInsert.internalExecute();

                        var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                        expect(redirectUrl.fparam().hasOwnProperty('aws__search')).toBeFalsy();
                        expect(redirectUrl.fparam('aws', 'root')).toEqual('param');
                        expect(redirectUrl.fparam('sandboxTextBox', 'search')).toEqual(valueInput);
                        expect(redirectUrl.fparam('sandboxSelect', 'search')).toEqual(valueSelect);

                    });

                    it('Should be insert with replace', function() {
                        hashInsert.jsonData = $.parseJSON($('#ExecutableStoreInsertWithReplace').val());
                        hashInsert.target = instanceSandBox;

                        hashInsert.internalExecute();

                        var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                        expect(redirectUrl.fparam().hasOwnProperty('aws__root')).toBeFalsy();
                        expect(redirectUrl.fparam().hasOwnProperty('sandboxSubmit__root')).toBeFalsy();
                        expect(redirectUrl.fparam('sandboxTextBox', 'root')).toEqual(valueInput);
                        expect(redirectUrl.fparam('sandboxSelect', 'root')).toEqual(valueSelect);
                    });

                    afterEach(function() {
                        expect(ExecutableHelper.Instance.TryGetVal.callCount).toEqual(2);
                    });

                });

                describe('When element ', function() {

                    beforeEach(function() {
                        spyOn(ExecutableHelper, 'RedirectTo');
                        hashInsert.jsonData = $.parseJSON($('#ExecutableStoreInsert').val());
                        var textBox = TestHelper.Instance.SandboxTextBox();
                        hashInsert.target = textBox;
                    });

                    it('Should be insert', function() {
                        spyOn(ExecutableHelper.Instance, 'TryGetVal').andReturn('value');

                        hashInsert.internalExecute();

                        var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                        expect(redirectUrl.fparam('sandboxTextBox', 'root')).toEqual('value');
                    });

                    it('Should be insert with ignore empty', function() {
                        spyOn(ExecutableHelper.Instance, 'TryGetVal').andReturn('');

                        hashInsert.internalExecute();

                        var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                        expect(redirectUrl.fparam('sandboxTextBox', 'root')).toEqual('');
                    });

                    it('Should be remove hash if empty', function() {
                        window.location.hash = "!sandboxTextBox=aws";
                        spyOn(ExecutableHelper.Instance, 'TryGetVal').andReturn('');

                        hashInsert.internalExecute();

                        var redirectUrl = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                        expect(redirectUrl.fparam('sandboxTextBox', 'root')).toEqual('');
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
                });

                it('Should be fetch element', function() {
                    hashFetch.target = element;
                    window.location.hash = "!sandboxTextBox=@Test";

                    hashFetch.internalExecute();

                    expect(ExecutableHelper.Instance.TrySetValue).toHaveBeenCalledWith(element, '@Test');
                });


                it('Should be fetch element without value', function() {
                    hashFetch.target = element;
                    window.location.hash = "";

                    hashFetch.internalExecute();

                    expect(ExecutableHelper.Instance.TrySetValue).toHaveBeenCalledWith(element, '');
                });

                it('Should be fetch element with decode', function() {
                    hashFetch.target = element;
                    window.location.hash = "!sandboxTextBox=%26TestÐóññêèå Áóêâû";

                    hashFetch.internalExecute();

                    expect(ExecutableHelper.Instance.TrySetValue).toHaveBeenCalledWith(element, '&TestÐóññêèå Áóêâû');
                });

                it('Should be fetch element by prefix', function() {
                    window.location.hash = "!sandboxTextBox=Test&search:sandboxTextBox=ValueFromPrefix";
                    hashFetch.jsonData = $.parseJSON($('#ExecutableStoreFetchWithPrefix').val());
                    hashFetch.target = element;

                    hashFetch.internalExecute();

                    expect(ExecutableHelper.Instance.TrySetValue).toHaveBeenCalledWith(element, 'ValueFromPrefix');
                });

                it('Should be fetch container', function() {
                    $(instanceSandBox).append(element);
                    hashFetch.target = instanceSandBox;
                    window.location.hash = "!sandboxTextBox=Test";

                    hashFetch.internalExecute();

                    expect($(ExecutableHelper.Instance.TrySetValue.argsForCall[0][0]).attr('name')).toEqual('sandboxTextBox');
                    expect(ExecutableHelper.Instance.TrySetValue.argsForCall[0][1]).toEqual('Test');
                    expect(ExecutableHelper.Instance.TrySetValue.callCount).toEqual(1);
                });

                it('Should be fetch container with multiple', function() {
                    var textBox = TestHelper.Instance.SandboxTextBox();
                    var button = TestHelper.Instance.SandboxInputButton();
                    var radioButton = TestHelper.Instance.SandboxRadioButton(true, 'value');
                    $(instanceSandBox)
                        .append(textBox)
                        .append(button)
                        .append(radioButton);
                    hashFetch.target = instanceSandBox;
                    window.location.hash = "!sandboxTextBox=Test&sandboxRadiobutton=value";

                    hashFetch.internalExecute();

                    expect(ExecutableHelper.Instance.TrySetValue).toHaveBeenCalledWith(instanceSandBox.find(textBox.prop("name").toSelectorAsName()), 'Test');
                    expect(ExecutableHelper.Instance.TrySetValue).toHaveBeenCalledWith(instanceSandBox.find(radioButton.prop("name").toSelectorAsName()), 'value');
                    expect(ExecutableHelper.Instance.TrySetValue.callCount).toEqual(2);
                });

            });

            describe('When ExecutableStoreManipulate', function() {

                var executable;

                beforeEach(function() {
                    spyOn(ExecutableHelper, 'RedirectTo');
                    executable = new ExecutableStoreManipulate();
                });

                it('Should be remove', function() {
                    window.location.hash = "!IncHash=Test";
                    executable.jsonData = $.parseJSON($('#ExecutableStoreManipulateRemove').val());

                    executable.internalExecute();

                    var url = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                    expect(url.fparam('IncHash', 'root')).toEqual('');
                });

                it('Should be remove with prefix', function() {
                    window.location.hash = "!search:IncHash=ValueFromPrefix";
                    executable.jsonData = $.parseJSON($('#ExecutableStoreManipulateRemoveWithPrefix').val());

                    executable.internalExecute();

                    var url = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                    expect(url.fparam('IncHash', 'search')).toEqual('');
                });

                it('Should be set', function() {
                    executable.tryGetVal = function(value) {
                        if (value === "$('#incSelector')") {
                            return 5;
                        }
                    };
                    executable.jsonData = $.parseJSON($('#ExecutableStoreManipulateSet').val());

                    executable.internalExecute();

                    var url = $.url(ExecutableHelper.RedirectTo.mostRecentCall.args[0]);
                    expect(url.fparam('IncHash', 'root')).toEqual('5');
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
                });

                it('Should be satisfied', function() {
                    evalConditional.jsonData = $.parseJSON($('#ConditionalEval').val());
                    var isSatisfied = evalConditional.isInternalSatisfied();
                    expect(isSatisfied).toBeFalsy();
                });

                it('Should be with data', function() {
                    evalConditional.jsonData = $.parseJSON($('#ConditionalEvalData').val());
                    var isSatisfied = evalConditional.isInternalSatisfied(true);
                    expect(isSatisfied).toBeTruthy();
                });
            });

            describe('When ConditionalIs', function() {

                var evalConditional;

                beforeEach(function() {
                    evalConditional = new ConditionalIs();
                    evalConditional.jsonData = $.parseJSON($('#ConditionalIs').val());
                    evalConditional.tryGetVal = function(original) {
                        return original;
                    };
                });

                it('Should be satisfied', function() {
                    spyOn(ExecutableHelper, 'Compare').andReturn(true);

                    var isSatisfied = evalConditional.isInternalSatisfied();

                    expect(isSatisfied).toBeTruthy();
                    expect(ExecutableHelper.Compare).toHaveBeenCalledWith("$('#id')", '||value*5||', 'equal');
                });

            });

            describe('When ConditionalData', function() {

                var existsItem, dataConditional, expectedValue;

                beforeEach(function() {

                    existsItem = { prop1: 1, Text: 2 };
                    expectedValue = '123';
                    dataConditional = new ConditionalData();
                    dataConditional.tryGetVal = function() {
                        return expectedValue;
                    };

                    spyOn(ExecutableHelper, 'Compare').andReturn(true);
                });

                it('Should be satisfied object', function() {
                    dataConditional.jsonData = $.parseJSON($('#ConditionalData').val());

                    var satisfied = dataConditional.isInternalSatisfied(existsItem);

                    expect(satisfied).toBeTruthy();
                    expect(ExecutableHelper.Compare).toHaveBeenCalledWith(existsItem.Text, expectedValue, dataConditional.jsonData.method);
                });


                it('Should be satisfied object with bool value', function() {
                    dataConditional.jsonData = $.parseJSON($('#ConditionalData').val());

                    var satisfied = dataConditional.isInternalSatisfied({ Text: false });

                    expect(satisfied).toBeTruthy();
                    expect(ExecutableHelper.Compare).toHaveBeenCalledWith(false, expectedValue, dataConditional.jsonData.method);
                });

                it('Should be satisfied object with undefined property', function() {
                    dataConditional.jsonData = $.parseJSON($('#ConditionalData').val());

                    var satisfied = dataConditional.isInternalSatisfied({});

                    expect(satisfied).toBeTruthy();
                    expect(ExecutableHelper.Compare).toHaveBeenCalledWith('', expectedValue, dataConditional.jsonData.method);
                });

                it('Should be satisfied object without property', function() {
                    dataConditional.jsonData = $.parseJSON($('#ConditionalDataWihtoutProperty').val());

                    var satisfied = dataConditional.isInternalSatisfied(existsItem);

                    expect(satisfied).toBeTruthy();
                    expect(ExecutableHelper.Compare).toHaveBeenCalledWith(existsItem, expectedValue, dataConditional.jsonData.method);
                });

                it('Should be satisfied array', function() {
                    dataConditional.jsonData = $.parseJSON($('#ConditionalData').val());
                    var satisfied = dataConditional.isInternalSatisfied([existsItem, { prop1: 8, Text: 18 }]);

                    expect(satisfied).toBeTruthy();
                    expect(ExecutableHelper.Compare).toHaveBeenCalledWith(existsItem.Text, dataConditional.jsonData.value, dataConditional.jsonData.method);
                });

                it('Should be satisfied array with bool value', function () {
                    dataConditional.jsonData = $.parseJSON($('#ConditionalData').val());
                    var satisfied = dataConditional.isInternalSatisfied([{ Text: false }, { prop1: 8, Text: 18 }]);

                    expect(satisfied).toBeTruthy();
                    expect(ExecutableHelper.Compare).toHaveBeenCalledWith(false, dataConditional.jsonData.value, dataConditional.jsonData.method);
                });

                it('Should be satisfied array without property', function() {
                    dataConditional.jsonData = $.parseJSON($('#ConditionalDataWihtoutProperty').val());
                    var data = [existsItem, { prop1: 8, Text: 18 }];

                    var satisfied = dataConditional.isInternalSatisfied([existsItem, { prop1: 8, Text: 18 }]);

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

            it('Should be Target_Val', function() {

                this.target = TestHelper.Instance.SandboxTextBox();
                eval($('#Target_Val').val().f("'aws'"));

                expect(this.target).toHaveValue('aws');
            });

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
    $(document).unbind();
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