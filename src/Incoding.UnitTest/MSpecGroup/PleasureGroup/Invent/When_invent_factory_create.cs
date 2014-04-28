namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(InventFactory<>))]
    public class When_invent_factory_create : Context_invent_factory
    {
        #region Establish value

        static InventFactory<FakeGenerateObject> inventFactory;

        #endregion

        Because of = () => { inventFactory = new InventFactory<FakeGenerateObject>(); };

        It should_be_tuning = () =>
                                  {
                                      inventFactory = new InventFactory<FakeGenerateObject>();
                                      string tuningValue = Pleasure.Generator.String();
                                      inventFactory.Tuning(r => r.StrValue, tuningValue);
                                      inventFactory.Create().StrValue.ShouldEqual(tuningValue);
                                  };

        It should_be_generate_to = () =>
                                       {
                                           inventFactory = new InventFactory<FakeGenerateObject>();
                                           inventFactory.GenerateTo(r => r.Fake);
                                           inventFactory.Create().Fake.ShouldNotBeNull();
                                       };

        It should_be_generate_to_list = () => new InventFactory<List<FakeGenerateObject>>()
                                                      .Create()
                                                      .ShouldNotBeEmpty();

        It should_be_generate_to_read_only_list = () => new InventFactory<ReadOnlyCollection<FakeGenerateObject>>()
                                                                .Create()
                                                                .ShouldNotBeEmpty();

        It should_be_create_primitive = () => new InventFactory<int>()
                                                      .Create()
                                                      .ShouldBeGreaterThan(0);

        It should_be_create_primitive_nullable = () => new InventFactory<int?>()
                                                               .Create()
                                                               .ShouldBeGreaterThan(0);

        It should_be_create_primitive_string = () => new InventFactory<string>()
                                                             .Create()
                                                             .ShouldNotBeEmpty();

        It should_be_generate_to_dsl = () =>
                                           {
                                               inventFactory = new InventFactory<FakeGenerateObject>();
                                               inventFactory.GenerateTo(r => r.Fake, dsl => dsl.Tuning(r => r.FloatValue, 5));
                                               inventFactory.Create().Fake.FloatValue.ShouldEqual(5);
                                           };

        It should_be_ignore_by_attribute = () =>
                                               {
                                                   inventFactory = new InventFactory<FakeGenerateObject>();
                                                   inventFactory.Create().IgnoreValueByAttr.ShouldBeNull();
                                               };

        It should_be_tuning_null = () =>
                                       {
                                           inventFactory = new InventFactory<FakeGenerateObject>();
                                           inventFactory.Tuning(r => r.StrValue, null);
                                           inventFactory.Create().StrValue.ShouldBeNull();
                                       };

        It should_be_tuning_default = () =>
                                          {
                                              inventFactory = new InventFactory<FakeGenerateObject>();
                                              inventFactory.Tuning(r => r.StrValue, default(string));
                                              inventFactory.Create().StrValue.ShouldBeNull();
                                          };

        It should_be_tuning_cover_ignore_by_attribute = () =>
                                                            {
                                                                inventFactory = new InventFactory<FakeGenerateObject>();
                                                                inventFactory.Tuning(r => r.IgnoreValueByAttr, Pleasure.Generator.TheSameString());
                                                                inventFactory.Create().IgnoreValueByAttr.ShouldBeTheSameString();
                                                            };

        It should_be_tunings = () =>
                                   {
                                       inventFactory = new InventFactory<FakeGenerateObject>();
                                       int tuningValue = Pleasure.Generator.PositiveNumber();
                                       inventFactory.Tunings(new
                                                                 {
                                                                         StrValue = tuningValue.ToString(),
                                                                         IntValue = tuningValue
                                                                 });

                                       var fakeGenerateObject = inventFactory.Create();
                                       fakeGenerateObject.StrValue.ShouldEqual(tuningValue.ToString());
                                       fakeGenerateObject.IntValue.ShouldEqual(tuningValue);
                                   };

        It should_be_callback = () =>
                                    {
                                        inventFactory = new InventFactory<FakeGenerateObject>();
                                        string callbackValue = Pleasure.Generator.String();
                                        inventFactory.Callback(o => o.ValueSetInCtor = callbackValue);
                                        inventFactory.Create().ValueSetInCtor.ShouldEqual(callbackValue);
                                    };

        It should_be_value_on_ctor = () => new InventFactory<FakeGenerateObject>()
                                                   .Create()
                                                   .ValueSetInCtor.ShouldBeTheSameString();

        It should_be_value_on_mute_ctor = () =>
                                              {
                                                  inventFactory = new InventFactory<FakeGenerateObject>();
                                                  inventFactory.MuteCtor();
                                                  inventFactory.Create()
                                                               .ValueSetInCtor.ShouldNotEqual(Pleasure.Generator.TheSameString());
                                              };

        It should_be_value_on_ctor_with_tuning = () =>
                                                     {
                                                         inventFactory = new InventFactory<FakeGenerateObject>();
                                                         string value = Pleasure.Generator.String();
                                                         inventFactory.Tuning(r => r.ValueSetInCtor, value);
                                                         inventFactory.Create()
                                                                      .ValueSetInCtor.ShouldEqual(value);
                                                     };

        It should_be_value_on_ctor_with_empty = () =>
                                                    {
                                                        inventFactory = new InventFactory<FakeGenerateObject>();
                                                        inventFactory.Empty(r => r.ValueSetInCtor);
                                                        inventFactory.Create()
                                                                     .ValueSetInCtor.ShouldBeEmpty();
                                                    };

        It should_be_string = () => inventFactory.Create().StrValue.ShouldNotBeEmpty();

        It should_be_char = () => inventFactory.Create().CharValue.ShouldNotBeNull();

        It should_be_char_nullable = () => inventFactory.Create().CharValueNullable.ShouldNotBeNull();

        It should_be_int = () => inventFactory.Create().IntValue.ShouldBeGreaterThan(0);

        It should_be_int_nullable = () => inventFactory.Create().IntValueNullable.ShouldBeGreaterThan(0);

        It should_be_object_as_string = () => inventFactory.Create().ObjValue.ShouldBeOfType<string>();

        It should_be_double = () => inventFactory.Create().DoubleValue.ShouldBeGreaterThan(0);

        It should_be_double_nullable = () => inventFactory.Create().DoubleValueNullable.ShouldBeGreaterThan(0);

        It should_be_float = () => inventFactory.Create().FloatValue.ShouldBeGreaterThan(0);

        It should_be_float_nullable = () => inventFactory.Create().FloatValueNullable.ShouldBeGreaterThan(0);

        It should_be_decimal = () => inventFactory.Create().DecimalValue.ShouldBeGreaterThan(0);

        It should_be_decimal_nullable = () => inventFactory.Create().DecimalValueNullable.ShouldBeGreaterThan(0);

        It should_be_long = () => inventFactory.Create().LongValue.ShouldBeGreaterThan(0);

        It should_be_long_nullable = () => inventFactory.Create().LongValueNullable.ShouldBeGreaterThan(0);

        It should_be_byte = () => inventFactory.Create().ByteValue.ShouldBeGreaterThan(0);

        It should_be_http_post_file = () => new HttpMemoryPostedFile(inventFactory.Create().HttpPostFileValue).ContentAsBytes.ShouldNotBeEmpty();

        It should_be_byte_nullable = () => inventFactory.Create().ByteValueNullable.ShouldBeGreaterThan(0);

        It should_be_bytes = () => inventFactory.Create().ByteArray.ShouldNotBeEmpty();

        It should_be_int_array = () => inventFactory.Create().IntArray.ShouldNotBeEmpty();

        It should_be_string_array = () => inventFactory.Create().StrArray.ShouldNotBeEmpty();

        It should_be_stream = () => inventFactory.Create().StreamValue.ShouldNotBeNull();

        It should_be_dictionary = () => inventFactory.Create().DictionaryValue.ShouldNotBeEmpty();

        It should_be_dictionary_object = () => inventFactory.Create().DictionaryObjectValue.ShouldNotBeEmpty();

        It should_be_date_time = () => inventFactory.Create().DateTimeValue.ShouldNotEqual(DateTime.Now);

        It should_be_date_time_nullable = () => inventFactory.Create().DateTimeValueNullable.ShouldNotBeNull();

        It should_be_time_span = () => inventFactory.Create().TimeSpanValue.ShouldBeGreaterThan(new TimeSpan(0, 0, 0));

        It should_be_enum = () => ((int)inventFactory.Create().DayOfWeek).ShouldBeGreaterThan(0);

        It should_be_guid = () => inventFactory.Create().GuidValue.ShouldNotEqual(Guid.Empty);

        It should_be_guid_nullable = () => inventFactory.Create().GuidValueNullable.ShouldNotEqual(Guid.Empty);
    }
}