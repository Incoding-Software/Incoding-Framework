namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(InventFactory<>))]
    public class When_invent_factory_create : Context_invent_factory
    {
        It should_be_byte = () => new InventFactory<FakeGenerateObject>().Create().ByteValue.ShouldBeGreaterThan(0);

        It should_be_byte_nullable = () => new InventFactory<FakeGenerateObject>().Create().ByteValueNullable.ShouldBeGreaterThan(0);

        It should_be_bytes = () => new InventFactory<FakeGenerateObject>().Create().ByteArray.ShouldNotBeEmpty();

        It should_be_callback = () =>
                                {
                                    var inventFactory = new InventFactory<FakeGenerateObject>();
                                    string callbackValue = Pleasure.Generator.String();
                                    inventFactory.Callback(o => o.ValueSetInCtor = callbackValue);
                                    inventFactory.Create().ValueSetInCtor.ShouldEqual(callbackValue);
                                };

        It should_be_char = () => new InventFactory<FakeGenerateObject>().Create().CharValue.ShouldNotBeNull();

        It should_be_char_nullable = () => new InventFactory<FakeGenerateObject>().Create().CharValueNullable.ShouldNotBeNull();

        It should_be_create_byte = () => new InventFactory<byte>()
                                                 .Create()
                                                 .ShouldBeGreaterThan(0);

        It should_be_create_enum = () => ((int)new InventFactory<DayOfWeek>()
                                                       .Create())
                                                 .ShouldBeGreaterThan(0);

        It should_be_create_enum_as_nullable = () => ((int?)new InventFactory<DayOfWeek?>()
                                                                    .Create())
                                                             .ShouldBeGreaterThan(0);

        It should_be_create_int = () => new InventFactory<int>()
                                                .Create()
                                                .ShouldBeGreaterThan(0);

        It should_be_create_int_nullable = () => new InventFactory<int?>()
                                                         .Create()
                                                         .ShouldBeGreaterThan(0);

        It should_be_create_primitive_string = () => new InventFactory<string>()
                                                             .Create()
                                                             .ShouldNotBeEmpty();

        It should_be_date_time = () => new InventFactory<FakeGenerateObject>().Create().DateTimeValue.ShouldNotEqual(DateTime.Now);

        It should_be_date_time_nullable = () => new InventFactory<FakeGenerateObject>().Create().DateTimeValueNullable.ShouldNotBeNull();

        It should_be_decimal = () => new InventFactory<FakeGenerateObject>().Create().DecimalValue.ShouldBeGreaterThan(0);

        It should_be_decimal_nullable = () => new InventFactory<FakeGenerateObject>().Create().DecimalValueNullable.ShouldBeGreaterThan(0);

        It should_be_dictionary = () => new InventFactory<FakeGenerateObject>().Create().DictionaryValue.ShouldNotBeEmpty();

        It should_be_dictionary_object = () => new InventFactory<FakeGenerateObject>().Create().DictionaryObjectValue.ShouldNotBeEmpty();

        It should_be_double = () => new InventFactory<FakeGenerateObject>().Create().DoubleValue.ShouldBeGreaterThan(0);

        It should_be_double_nullable = () => new InventFactory<FakeGenerateObject>().Create().DoubleValueNullable.ShouldBeGreaterThan(0);

        It should_be_enum = () => ((int)new InventFactory<FakeGenerateObject>().Create().EnumValue).ShouldBeGreaterThan(0);

        It should_be_enum_as_nullable = () => ((int)new InventFactory<FakeGenerateObject>().Create().EnumAsNullableValue).ShouldBeGreaterThan(0);

        It should_be_float = () => new InventFactory<FakeGenerateObject>().Create().FloatValue.ShouldBeGreaterThan(0);

        It should_be_float_nullable = () => new InventFactory<FakeGenerateObject>().Create().FloatValueNullable.ShouldBeGreaterThan(0);

        It should_be_generate_bytes = () => new InventFactory<byte[]>()
                                                    .Create()
                                                    .ShouldNotBeEmpty();

        It should_be_generate_guids = () => new InventFactory<Guid[]>()
                                                    .Create()
                                                    .ShouldNotBeEmpty();

        It should_be_generate_to = () =>
                                   {
                                       var inventFactory = new InventFactory<FakeGenerateObject>();
                                       inventFactory.GenerateTo(r => r.Fake);
                                       inventFactory.Create().Fake.ShouldNotBeNull();
                                   };

        It should_be_generate_to_array = () => new InventFactory<FakeGenerateObject[]>()
                                                       .Create()
                                                       .ShouldNotBeEmpty();

        It should_be_generate_to_array_as_datetime = () => new InventFactory<DateTime[]>()
                                                                   .Create()
                                                                   .ShouldNotBeEmpty();

        It should_be_generate_to_list = () => new InventFactory<List<FakeGenerateObject>>()
                                                      .Create()
                                                      .ShouldNotBeEmpty();

        It should_be_generate_to_read_only_list = () => new InventFactory<ReadOnlyCollection<FakeGenerateObject>>()
                                                                .Create()
                                                                .ShouldNotBeEmpty();

        It should_be_generate_to_with_generic_class_as_class = () =>
                                                               {
                                                                   var inventFactory = new InventFactory<FakeGenerateGeneric<FakeGenerateObject>>();
                                                                   inventFactory.GenerateTo(r => r.Result);
                                                                   inventFactory.Create().Result.ShouldNotBeNull();
                                                               };

        It should_be_generate_with_dsl = () =>
                                         {
                                             var inventFactory = new InventFactory<FakeGenerateObject>();
                                             inventFactory.GenerateTo(r => r.Fake, dsl => dsl.Tuning(r => r.FloatValue, 5));
                                             inventFactory.Create().Fake.FloatValue.ShouldEqual(5);
                                         };   
        
        It should_be_generate_list_with_dsl = () =>
                                              {
                                                  var inventFactory = new InventFactory<FakeGenerateObject>();
                                                  inventFactory.GenerateTo(r => r.Fakes, dsl => dsl.Tuning(r => r.FloatValue, 5));
                                                  inventFactory.Create().Fakes.Any().ShouldBeTrue();
                                                  inventFactory.Create().Fakes.All(r => r.FloatValue == 5).ShouldBeTrue();
                                              };

        It should_be_generic_class_as_class = () =>
                                              {
                                                  var inventFactory = new InventFactory<FakeGenerateGeneric<FakeGenerateObject>>();
                                                  inventFactory.Create().Result.ShouldBeNull();
                                              };

        It should_be_generic_class_as_string = () =>
                                               {
                                                   var inventFactory = new InventFactory<FakeGenerateGeneric<string>>();
                                                   inventFactory.Create().Result.ShouldNotBeNull();
                                               };

        It should_be_guid = () => new InventFactory<FakeGenerateObject>().Create().GuidValue.ShouldNotEqual(Guid.Empty);

        It should_be_guid_nullable = () => new InventFactory<FakeGenerateObject>().Create().GuidValueNullable.ShouldNotEqual(Guid.Empty);

        It should_be_http_post_file = () => new HttpMemoryPostedFile(new InventFactory<FakeGenerateObject>().Create().HttpPostFileValue).ContentAsBytes.ShouldNotBeEmpty();

        It should_be_ignore_by_attribute = () =>
                                           {
                                               var inventFactory = new InventFactory<FakeGenerateObject>();
                                               inventFactory.Create().IgnoreValueByAttr.ShouldBeNull();
                                           };

        It should_be_int = () => new InventFactory<FakeGenerateObject>().Create().IntValue.ShouldBeGreaterThan(0);

        It should_be_int_array = () => new InventFactory<FakeGenerateObject>().Create().IntArray.ShouldNotBeEmpty();

        It should_be_int_nullable = () => new InventFactory<FakeGenerateObject>().Create().IntValueNullable.ShouldBeGreaterThan(0);

        It should_be_long = () => new InventFactory<FakeGenerateObject>().Create().LongValue.ShouldBeGreaterThan(0);

        It should_be_long_nullable = () => new InventFactory<FakeGenerateObject>().Create().LongValueNullable.ShouldBeGreaterThan(0);

        It should_be_object_as_string = () => new InventFactory<FakeGenerateObject>().Create().ObjValue.ShouldBeOfType<string>();

        It should_be_stream = () => new InventFactory<FakeGenerateObject>().Create().StreamValue.ShouldNotBeNull();

        It should_be_string = () => new InventFactory<FakeGenerateObject>().Create().StrValue.ShouldNotBeEmpty();

        It should_be_string_array = () => new InventFactory<FakeGenerateObject>().Create().StrArray.ShouldNotBeEmpty();

        It should_be_time_span = () => new InventFactory<FakeGenerateObject>().Create().TimeSpanValue.ShouldBeGreaterThan(new TimeSpan(0, 0, 0));

        It should_be_tuning = () =>
                              {
                                  var inventFactory = new InventFactory<FakeGenerateObject>();
                                  string tuningValue = Pleasure.Generator.String();
                                  inventFactory.Tuning(r => r.StrValue, tuningValue);
                                  inventFactory.Create().StrValue.ShouldEqual(tuningValue);
                              };

        It should_be_tuning_cover_ignore_by_attribute = () =>
                                                        {
                                                            var inventFactory = new InventFactory<FakeGenerateObject>();
                                                            inventFactory.Tuning(r => r.IgnoreValueByAttr, Pleasure.Generator.TheSameString());
                                                            inventFactory.Create().IgnoreValueByAttr.ShouldBeTheSameString();
                                                        };

        It should_be_tuning_default = () =>
                                      {
                                          var inventFactory = new InventFactory<FakeGenerateObject>();
                                          inventFactory.Tuning(r => r.StrValue, default(string));
                                          inventFactory.Create().StrValue.ShouldBeNull();
                                      };

        It should_be_tuning_for_only_get_with_exception = () =>
                                                          {
                                                              var inventFactory = new InventFactory<FakeGenerateObject>();
                                                              Catch.Exception(() => inventFactory.Tuning(r => r.OnlyGet, "")).ShouldNotBeNull();
                                                          };

        It should_be_tuning_null = () =>
                                   {
                                       var inventFactory = new InventFactory<FakeGenerateObject>();
                                       inventFactory.Tuning(r => r.StrValue, null);
                                       inventFactory.Create().StrValue.ShouldBeNull();
                                   };

        It should_be_tuning_private = () =>
                                      {
                                          var inventFactory = new InventFactory<FakeGenerateObject>();
                                          inventFactory.Tuning("privateValue", Pleasure.Generator.TheSameString());
                                          inventFactory.Create().GetPrivateValue().ShouldBeTheSameString();
                                      };

        It should_be_tunings = () =>
                               {
                                   var inventFactory = new InventFactory<FakeGenerateObject>();
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

        It should_be_value_on_ctor = () => new InventFactory<FakeGenerateObject>()
                                                   .Create()
                                                   .ValueSetInCtor.ShouldBeTheSameString();

        It should_be_value_on_ctor_with_empty = () =>
                                                {
                                                    var inventFactory = new InventFactory<FakeGenerateObject>();
                                                    inventFactory.Empty(r => r.ValueSetInCtor);
                                                    inventFactory.Create()
                                                                 .ValueSetInCtor.ShouldBeEmpty();
                                                };

        It should_be_value_on_ctor_with_tuning = () =>
                                                 {
                                                     var inventFactory = new InventFactory<FakeGenerateObject>();
                                                     string value = Pleasure.Generator.String();
                                                     inventFactory.Tuning(r => r.ValueSetInCtor, value);
                                                     inventFactory.Create()
                                                                  .ValueSetInCtor.ShouldEqual(value);
                                                 };

        It should_be_value_on_mute_ctor = () =>
                                          {
                                              var inventFactory = new InventFactory<FakeGenerateObject>();
                                              inventFactory.MuteCtor();
                                              inventFactory.Create()
                                                           .ValueSetInCtor.ShouldNotEqual(Pleasure.Generator.TheSameString());
                                          };
    }
}