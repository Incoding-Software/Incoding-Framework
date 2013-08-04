namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(InventFactory<>))]
    public class When_invent_factory_empty : Context_invent_factory
    {
        #region Estabilish value

        static InventFactory<FakeGenerateObject> inventFactory;

        static FakeGenerateObject inventEmpty;

        #endregion

        Establish establish = () => { inventFactory = new InventFactory<FakeGenerateObject>(); };

        Because of = () => { inventEmpty = inventFactory.CreateEmpty(); };

        It should_be_empty_string = () => inventEmpty.StrValue.ShouldBeEmpty();

        It should_be_empty_int = () => inventEmpty.IntValue.ShouldEqual(0);

        It should_be_empty_float = () => inventEmpty.FloatValue.ShouldEqual(0);

        It should_be_empty_decimal = () => inventEmpty.DecimalValue.ShouldEqual(0);

        It should_be_empty_long = () => inventEmpty.LongValue.ShouldEqual(0);

        It should_be_empty_bool = () => inventEmpty.BoolValue.ShouldBeFalse();
        
        It should_be_empty_byte = () => inventEmpty.ByteValue.ShouldEqual(new byte());

        It should_be_empty_byte_array = () => inventEmpty.ByteArray.ShouldBeEmpty();

        It should_be_empty_string_array = () => inventEmpty.StrArray.ShouldBeEmpty();

        It should_be_empty_enum = () => ((int)inventEmpty.EnumValue).ShouldEqual(0);

        It should_be_not_found_empty = () =>
                                           {
                                               var invent = new InventFactory<FakeGenerateObject>();
                                               invent.Empty<FakeGenerateObject>(r => r.Fake);
                                               Catch.Exception(() => invent.Create()).ShouldBeOfType<ArgumentException>();
                                           };

        It should_be_empty = () =>
                                 {
                                     var invent = new InventFactory<FakeGenerateObject>();
                                     invent.Empty<string>(r => r.StrValue);
                                     invent.Create().StrValue.ShouldBeEmpty();
                                 };
    }
}