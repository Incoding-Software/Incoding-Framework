namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.Quality;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(CompareFactory<FakeCompare, FakeCompare>))]
    public class When_compare_factory
    {
        #region Fake classes

        class FakeCompareWithPrivateField
        {
            #region Fields

            [UsedImplicitly]
            string privateField;

            #endregion

            #region Api Methods

            public void SetPrivateField(string value)
            {
                this.privateField = value;
            }

            #endregion
        }

        class FakeCompareWithPrivateFieldWithIEnumerable
        {
            #region Fields

            [UsedImplicitly]
            IEnumerable<string> privateField;

            #endregion

            #region Api Methods

            public void SetPrivateField(IEnumerable<string> value)
            {
                this.privateField = value;
            }

            #endregion
        }

        class FakeCompareWithIgnore
        {
            #region Properties

            [IgnoreCompare("Design fixed"), UsedImplicitly]
            public string Result { get; set; }

            #endregion
        }

        class FakeCompare
        {
            #region Properties

            public string Result { get; set; }

            [IgnoreCompare("Test")]
            public string IgnoreValueByAttr { get; set; }

            #endregion
        }

        class FakeCompareWithStructure
        {
            #region Properties

            public int Result { get; set; }

            #endregion
        }

        class FakeCompare2
        {
            #region Properties

            public string Result2 { get; set; }

            #endregion
        }

        class FakeCompare3
        {
            #region Properties

            [UsedImplicitly]
            public string Result { get; set; }

            [UsedImplicitly]
            public string Result2 { get; set; }

            #endregion
        }

        class FakeCompareWithEnumerable
        {
            #region Properties

            public IEnumerable<byte> Bytes { get; set; }

            #endregion
        }

        // ReSharper disable UnusedTypeParameter
        class FakeCompareGeneric<TFake> { }

        // ReSharper restore UnusedTypeParameter
        class FakeCompareWithClass
        {
            #region Properties

            public FakeCompare Fake { get; set; }

            #endregion
        }

        class FakeCompareWithSameClass
        {
            #region Properties

            [UsedImplicitly]
            public FakeCompareWithSameClass Fake { get; set; }

            #endregion
        }

        class FakeCompareWithoutField { }

        class FakeNextCompareWithoutField { }

        class FakeCompareWithGuid
        {
            #region Properties

            public Guid Result { get; set; }

            #endregion
        }

        #endregion

        It should_be_compare = () =>
                                   {
                                       var compare = new CompareFactory<FakeCompare, FakeCompare>();
                                       compare.Compare(new FakeCompare { Result = Pleasure.Generator.TheSameString() }, new FakeCompare { Result = Pleasure.Generator.TheSameString() });
                                       compare.IsCompare().ShouldBeTrue();
                                       compare.GetDifferencesAsString().ShouldBeEmpty();
                                   };

        It should_be_compare_generic = () =>
                                           {
                                               var compare = new CompareFactory<FakeCompareGeneric<string>, FakeCompareGeneric<string>>();
                                               compare.Compare(new FakeCompareGeneric<string>(), new FakeCompareGeneric<string>());
                                               compare.IsCompare().ShouldBeTrue();
                                           };

        It should_be_compare_generic_false = () =>
                                                 {
                                                     var compare = new CompareFactory<FakeCompareGeneric<string>, FakeCompareGeneric<int>>();
                                                     compare.Compare(new FakeCompareGeneric<string>(), new FakeCompareGeneric<int>());
                                                     compare.IsCompare().ShouldBeFalse();
                                                 };

        It should_not_be_compare_with_private_field = () =>
                                                          {
                                                              var compare = new CompareFactory<FakeCompareWithPrivateField, FakeCompareWithPrivateField>();
                                                              compare.IncludeAllFields();
                                                              var expected = new FakeCompareWithPrivateField();
                                                              expected.SetPrivateField(Pleasure.Generator.String());
                                                              compare.Compare(new FakeCompareWithPrivateField(), expected);
                                                              compare.IsCompare().ShouldBeFalse();
                                                          };

        It should_be_compare_with_private_field_ienumerable = () =>
                                                                  {
                                                                      var compare = new CompareFactory<FakeCompareWithPrivateFieldWithIEnumerable, FakeCompareWithPrivateFieldWithIEnumerable>();
                                                                      compare.IncludeAllFields();
                                                                      var expected = new FakeCompareWithPrivateFieldWithIEnumerable();
                                                                      expected.SetPrivateField(Pleasure.ToEnumerable(Pleasure.Generator.String()));
                                                                      compare.Compare(expected, expected);
                                                                      compare.IsCompare().ShouldBeTrue();
                                                                  };

        It should_be_compare_with_private_field_ienumerable_with_false = () =>
                                                                             {
                                                                                 var compare = new CompareFactory<FakeCompareWithPrivateFieldWithIEnumerable, FakeCompareWithPrivateFieldWithIEnumerable>();
                                                                                 compare.IncludeAllFields();
                                                                                 var expected = new FakeCompareWithPrivateFieldWithIEnumerable();
                                                                                 expected.SetPrivateField(Pleasure.ToEnumerable(Pleasure.Generator.String()));
                                                                                 var actual = new FakeCompareWithPrivateFieldWithIEnumerable();
                                                                                 actual.SetPrivateField(Pleasure.ToEnumerable(Pleasure.Generator.String()));
                                                                                 compare.Compare(expected, actual);
                                                                                 compare.IsCompare().ShouldBeFalse();
                                                                             };

        It should_be_compare_with_private_field = () =>
                                                      {
                                                          var compare = new CompareFactory<FakeCompareWithPrivateField, FakeCompareWithPrivateField>();
                                                          compare.IncludeAllFields();
                                                          var expected = new FakeCompareWithPrivateField();
                                                          expected.SetPrivateField(Pleasure.Generator.String());
                                                          compare.Compare(expected, expected);
                                                          compare.IsCompare().ShouldBeTrue();
                                                      };

        It should_be_compare_with_private_field_with_wrong = () =>
                                                                 {
                                                                     var compare = new CompareFactory<FakeCompareWithPrivateField, FakeCompareWithPrivateField>();
                                                                     compare.IncludeAllFields();

                                                                     var expected = new FakeCompareWithPrivateField();
                                                                     expected.SetPrivateField(Pleasure.Generator.String());
                                                                     var actual = new FakeCompareWithPrivateField();
                                                                     actual.SetPrivateField(Pleasure.Generator.String());
                                                                     compare.Compare(expected, actual);
                                                                     compare.IsCompare().ShouldBeFalse();
                                                                 };

        It should_be_compare_with_different = () =>
                                                  {
                                                      var compare = new CompareFactory<string, int>();
                                                      compare.Compare(Pleasure.Generator.TheSameString(), 5);
                                                      compare.IsCompare().ShouldBeFalse();
                                                      compare.GetDifferencesAsString().ShouldNotBeEmpty();
                                                  };

        It should_be_compare_with_default = () =>
                                                {
                                                    var compare = new CompareFactory<FakeCompare, FakeCompare>();
                                                    compare.Compare(new FakeCompare(), new FakeCompare());
                                                    compare.IsCompare().ShouldBeTrue();
                                                    compare.GetDifferencesAsString().ShouldBeEmpty();
                                                };

        It should_be_compare_with_enumerable = () =>
                                                   {
                                                       var compare = new CompareFactory<FakeCompareWithEnumerable, FakeCompareWithEnumerable>();
                                                       var fakeCompareWithEnumerable = new FakeCompareWithEnumerable { Bytes = Pleasure.Generator.Bytes() };
                                                       compare.Compare(fakeCompareWithEnumerable, fakeCompareWithEnumerable);
                                                       compare.IsCompare().ShouldBeTrue();
                                                   };

        It should_be_compare_class_with_enumerable_left_different_type = () =>
                                                                             {
                                                                                 var compare = new CompareFactory<FakeCompareWithEnumerable, FakeCompareWithEnumerable>();
                                                                                 var bytes = Pleasure.Generator.Bytes();
                                                                                 compare.Compare(new FakeCompareWithEnumerable
                                                                                                     {
                                                                                                             Bytes = bytes.ToArray()
                                                                                                     }, new FakeCompareWithEnumerable
                                                                                                            {
                                                                                                                    Bytes = bytes
                                                                                                            });
                                                                                 compare.IsCompare().ShouldBeTrue();
                                                                             };

        It should_be_compare_class_with_enumerable_right_different_type = () =>
                                                                              {
                                                                                  var compare = new CompareFactory<FakeCompareWithEnumerable, FakeCompareWithEnumerable>();
                                                                                  var bytes = Pleasure.Generator.Bytes();
                                                                                  compare.Compare(new FakeCompareWithEnumerable
                                                                                                      {
                                                                                                              Bytes = bytes
                                                                                                      }, new FakeCompareWithEnumerable
                                                                                                             {
                                                                                                                     Bytes = bytes.ToArray()
                                                                                                             });
                                                                                  compare.IsCompare().ShouldBeTrue();
                                                                              };

        It should_be_not_compare_with_enumerable = () =>
                                                       {
                                                           var compare = new CompareFactory<FakeCompareWithEnumerable, FakeCompareWithEnumerable>();
                                                           compare.Compare(new FakeCompareWithEnumerable { Bytes = Pleasure.Generator.Bytes() }, new FakeCompareWithEnumerable { Bytes = Pleasure.Generator.Bytes() });
                                                           compare.IsCompare().ShouldBeFalse();
                                                       };

        It should_be_not_compare_with_enumerable_by_index = () =>
                                                                {
                                                                    var compare = new CompareFactory<FakeCompareWithEnumerable, FakeCompareWithEnumerable>();
                                                                    byte existsByte = (byte)Pleasure.Generator.PositiveNumber();
                                                                    compare.Compare(new FakeCompareWithEnumerable { Bytes = new[] { existsByte, (byte)Pleasure.Generator.PositiveNumber() } }, new FakeCompareWithEnumerable { Bytes = new[] { existsByte, (byte)Pleasure.Generator.PositiveNumber() } });
                                                                    compare.IsCompare().ShouldBeFalse();
                                                                    compare.GetDifferencesAsString().ShouldContain("Compare  Item 1 from Bytes with Item 1 from Bytes");
                                                                };

        It should_be_not_compare_with_enumerable_different_count = () =>
                                                                       {
                                                                           var compare = new CompareFactory<FakeCompareWithEnumerable, FakeCompareWithEnumerable>();
                                                                           compare.Compare(new FakeCompareWithEnumerable { Bytes = Pleasure.Generator.Bytes(20) }, new FakeCompareWithEnumerable { Bytes = Pleasure.Generator.Bytes(30) });
                                                                           compare.IsCompare().ShouldBeFalse();
                                                                           compare.GetDifferencesAsString().ShouldContain("Count from Bytes with Count from Bytes");
                                                                       };

        It should_be_compare_with_differences = () =>
                                                    {
                                                        var compare = new CompareFactory<FakeCompare3, FakeCompare>();
                                                        compare.Compare(Pleasure.Generator.Invent<FakeCompare3>(), Pleasure.Generator.Invent<FakeCompare>());
                                                        compare.IsCompare().ShouldBeFalse();
                                                        ((List<string>)compare.TryGetValue("differences")).Count.ShouldEqual(2);
                                                        compare.GetDifferencesAsString().Length.ShouldEqual(290);
                                                    };

        It should_be_compare_with_ignore_by_attribute = () =>
                                                            {
                                                                var compare = new CompareFactory<FakeCompareWithIgnore, FakeCompareWithIgnore>();
                                                                compare.Compare(Pleasure.Generator.Invent<FakeCompareWithIgnore>(), Pleasure.Generator.Invent<FakeCompareWithIgnore>());
                                                                compare.IsCompare().ShouldBeTrue();
                                                                compare.GetDifferencesAsString().ShouldBeEmpty();
                                                            };

        It should_be_compare_with_both_null = () =>
                                                  {
                                                      var compare = new CompareFactory<FakeCompare, FakeCompare>();
                                                      compare.Compare(null, null);
                                                      compare.IsCompare().ShouldBeTrue();
                                                      compare.GetDifferencesAsString().ShouldBeEmpty();
                                                  };

        It should_be_compare_with_left_null = () =>
                                                  {
                                                      var compare = new CompareFactory<FakeCompare, FakeCompare>();
                                                      compare.Compare(null, new FakeCompare { Result = Pleasure.Generator.TheSameString() });
                                                      compare.IsCompare().ShouldBeFalse();
                                                      compare.GetDifferencesAsString().ShouldContain("Actual is null but Expected not null");
                                                  };

        It should_be_compare_with_wrong_schema = () =>
                                                     {
                                                         var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                         compare.Compare(new FakeCompare { Result = Pleasure.Generator.TheSameString() }, new FakeCompare2 { Result2 = Pleasure.Generator.TheSameString() });
                                                         compare.IsCompare().ShouldBeFalse();
                                                         compare.GetDifferencesAsString().ShouldContain("Not found property Result in class FakeCompare2");
                                                     };

        It should_be_compare_with_class = () =>
                                              {
                                                  var compare = new CompareFactory<FakeCompareWithClass, FakeCompareWithClass>();
                                                  var fakeCompareWithClass = new FakeCompareWithClass { Fake = new FakeCompare { Result = Pleasure.Generator.TheSameString() } };
                                                  compare.Compare(fakeCompareWithClass, fakeCompareWithClass);
                                                  compare.IsCompare().ShouldBeTrue();
                                              };

        It should_not_be_compare_with_class = () =>
                                                  {
                                                      var compare = new CompareFactory<FakeCompareWithClass, FakeCompareWithClass>();

                                                      compare.Compare(new FakeCompareWithClass { Fake = Pleasure.Generator.Invent<FakeCompare>() }, new FakeCompareWithClass { Fake = Pleasure.Generator.Invent<FakeCompare>() });
                                                      compare.IsCompare().ShouldBeFalse();
                                                  };

        It should_be_compare_class_without_field = () =>
                                                       {
                                                           var compare = new CompareFactory<FakeCompareWithoutField, FakeCompareWithoutField>();
                                                           compare.Compare(new FakeCompareWithoutField(), new FakeCompareWithoutField());
                                                           compare.IsCompare().ShouldBeTrue();
                                                       };

        It should_be_compare_class_without_field_false = () =>
                                                             {
                                                                 var compare = new CompareFactory<FakeCompareWithoutField, FakeNextCompareWithoutField>();
                                                                 compare.Compare(new FakeCompareWithoutField(), new FakeNextCompareWithoutField());
                                                                 compare.IsCompare().ShouldBeFalse();
                                                             };

        It should_be_compare_class_without_field_and_include_all_fields = () =>
                                                                              {
                                                                                  var compare = new CompareFactory<FakeCompareWithoutField, FakeCompareWithoutField>();
                                                                                  compare.IncludeAllFields();
                                                                                  compare.Compare(new FakeCompareWithoutField(), new FakeCompareWithoutField());
                                                                                  compare.IsCompare().ShouldBeTrue();
                                                                              };

        #region Primitive, Collection, Special

        It should_be_not_compare_string_with_string = () =>
                                                          {
                                                              var compare = new CompareFactory<string, string>();
                                                              compare.Compare(Pleasure.Generator.String(), Pleasure.Generator.String());
                                                              compare.IsCompare().ShouldBeFalse();
                                                              compare.GetDifferencesAsString().ShouldContain("Actual with Expected");
                                                          };

        It should_be_not_compare_collection_string = () =>
                                                         {
                                                             var compare = new CompareFactory<string[], string[]>();
                                                             compare.Compare(new[] { Pleasure.Generator.String() }, new[] { Pleasure.Generator.String() });
                                                             compare.IsCompare().ShouldBeFalse();
                                                             compare.GetDifferencesAsString().ShouldContain("Compare  Item 0 from Actual with Item 0 from Expected");
                                                         };

        It should_be_not_compare_collection_by_count = () =>
                                                           {
                                                               var compare = new CompareFactory<string[], string[]>();
                                                               compare.Compare(new string[0], new[] { Pleasure.Generator.String() });
                                                               compare.IsCompare().ShouldBeFalse();
                                                               compare.GetDifferencesAsString().ShouldContain("Count from Actual with Count from Expected");
                                                           };

        It should_be_not_compare_dictionary_by_count = () =>
                                                           {
                                                               var compare = new CompareFactory<IDictionary<string, string>, IDictionary<string, string>>();
                                                               compare.Compare(Pleasure.ToDictionary<string, string>(), Pleasure.ToDictionary(Pleasure.Generator.KeyValuePair()));
                                                               compare.IsCompare().ShouldBeFalse();
                                                               compare.GetDifferencesAsString().ShouldContain("Count from Actual with Count from Expected");
                                                           };

        It should_be_not_compare_dictionary = () =>
                                                  {
                                                      var compare = new CompareFactory<IDictionary<string, string>, IDictionary<string, string>>();
                                                      compare.Compare(Pleasure.ToDictionary(Pleasure.Generator.KeyValuePair()), Pleasure.ToDictionary(Pleasure.Generator.KeyValuePair()));
                                                      compare.IsCompare().ShouldBeFalse();
                                                      compare.GetDifferencesAsString().ShouldContain("Compare  Item 0 from Actual with Item 0 from Expected");
                                                  };

        It should_be_compare_complex_dictionary = () =>
                                                      {
                                                          var compare = new CompareFactory<Dictionary<string, List<string>>, Dictionary<string, List<string>>>();
                                                          var dictionary = Pleasure.ToDictionary(new KeyValuePair<string, List<string>>(Pleasure.Generator.String(), Pleasure.ToList(Pleasure.Generator.String())));
                                                          compare.Compare(dictionary, dictionary);
                                                          compare.IsCompare().ShouldBeTrue();
                                                      };

        It should_be_not_compare_complex_dictionary = () =>
                                                          {
                                                              var compare = new CompareFactory<Dictionary<string, List<string>>, Dictionary<string, List<string>>>();
                                                              var dictionary = Pleasure.ToDictionary(new KeyValuePair<string, List<string>>(Pleasure.Generator.String(), Pleasure.ToList(Pleasure.Generator.String())));
                                                              var dictionary2 = Pleasure.ToDictionary(new KeyValuePair<string, List<string>>(Pleasure.Generator.String(), Pleasure.ToList(Pleasure.Generator.String())));
                                                              compare.Compare(dictionary, dictionary2);
                                                              compare.IsCompare().ShouldBeFalse();
                                                          };

        It should_be_not_compare_with_decimal = () =>
                                                    {
                                                        var compare = new CompareFactory<decimal, decimal>();
                                                        compare.Compare(Pleasure.Generator.PositiveDecimal(), Pleasure.Generator.PositiveDecimal());
                                                        compare.IsCompare().ShouldBeFalse();
                                                        compare.GetDifferencesAsString().ShouldContain("Actual with Expected");
                                                    };

        It should_be_not_compare_string_with_date_time = () =>
                                                             {
                                                                 var compare = new CompareFactory<DateTime, DateTime>();
                                                                 compare.Compare(Pleasure.Generator.DateTime(), Pleasure.Generator.DateTime());
                                                                 compare.IsCompare().ShouldBeFalse();
                                                                 compare.GetDifferencesAsString().ShouldContain("Actual with Expected");
                                                             };

        It should_be_compare_select_list = () =>
                                               {
                                                   var compare = new CompareFactory<SelectList, SelectList>();
                                                   var selectList = new SelectList(new[] { Pleasure.Generator.String() });
                                                   compare.Compare(selectList, selectList);
                                                   compare.IsCompare().ShouldBeTrue();
                                               };

        It should_be_compare_db_connection = () =>
                                                 {
                                                     var compare = new CompareFactory<IDbConnection, IDbConnection>();
                                                     var connection = new SqlConnection(@"Data Source=Work\SQLEXPRESS;Database=IncRealDb;Integrated Security=true;");
                                                     compare.Compare(connection, connection);
                                                     compare.IsCompare().ShouldBeTrue();
                                                 };

        It should_not_be_compare_db_connection = () =>
                                                     {
                                                         var compare = new CompareFactory<IDbConnection, IDbConnection>();
                                                         compare.Compare(new SqlConnection(@"Data Source=Work\SQLEXPRESS;Database=OtherDb;Integrated Security=true;"), new SqlConnection(@"Data Source=Work\SQLEXPRESS;Database=IncRealDb;Integrated Security=true;"));
                                                         compare.IsCompare().ShouldBeFalse();
                                                     };

        #endregion

        #region Forward and Ignore

        It should_be_compare_with_ignore_private_field = () =>
                                                             {
                                                                 var compare = new CompareFactory<FakeCompareWithPrivateField, FakeCompareWithPrivateField>();
                                                                 var expected = new FakeCompareWithPrivateField();
                                                                 expected.SetPrivateField(Pleasure.Generator.String());
                                                                 compare.Compare(expected, new FakeCompareWithPrivateField());
                                                                 compare.IsCompare().ShouldBeTrue();
                                                             };

        It should_be_compare_with_ignore_dynamic_field_field = () =>
                                                                   {
                                                                       var compare = new CompareFactory<object, object>();
                                                                       compare.Ignore("type", "dynamic");
                                                                       compare.Compare(new { type = Pleasure.Generator.String() }, new { type = Pleasure.Generator.String() });
                                                                       compare.IsCompare().ShouldBeTrue();
                                                                   };

        It should_be_compare_with_ignore_attribute = () =>
                                                         {
                                                             var compare = new CompareFactory<FakeCompare, FakeCompare>();
                                                             compare.Compare(new FakeCompare { IgnoreValueByAttr = Pleasure.Generator.String() }, new FakeCompare { IgnoreValueByAttr = Pleasure.Generator.String() });
                                                             compare.IsCompare().ShouldBeTrue();
                                                         };

        It should_be_compare_with_forward = () =>
                                                {
                                                    var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                    compare.Forward(r => r.Result, r => r.Result2);
                                                    compare.Compare(new FakeCompare { Result = Pleasure.Generator.TheSameString() }, new FakeCompare2 { Result2 = Pleasure.Generator.TheSameString() });
                                                    compare.IsCompare().ShouldBeTrue();
                                                };

        It should_be_compare_with_forward_cover_ignore_attribute = () =>
                                                                       {
                                                                           var compare = new CompareFactory<FakeCompare, FakeCompare>();
                                                                           compare.Forward(r => r.IgnoreValueByAttr, r => r.IgnoreValueByAttr);
                                                                           compare.Compare(new FakeCompare { IgnoreValueByAttr = Pleasure.Generator.String() }, new FakeCompare { IgnoreValueByAttr = Pleasure.Generator.TheSameString() });
                                                                           compare.IsCompare().ShouldBeFalse();
                                                                       };

        It should_be_compare_with_forward_to_value = () =>
                                                         {
                                                             var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                             compare.ForwardToValue(r => r.Result, Pleasure.Generator.TheSameString());
                                                             compare.Compare(new FakeCompare { Result = Pleasure.Generator.TheSameString() }, new FakeCompare2 { Result2 = Pleasure.Generator.TheSameString() });
                                                             compare.IsCompare().ShouldBeTrue();
                                                         };

        It should_be_compare_with_forward_to_value_cover_ignore_attribute = () =>
                                                                                {
                                                                                    var compare = new CompareFactory<FakeCompare, FakeCompare>();
                                                                                    compare.ForwardToValue(r => r.Result, Pleasure.Generator.String());
                                                                                    compare.Compare(new FakeCompare { Result = Pleasure.Generator.TheSameString() }, new FakeCompare { Result = Pleasure.Generator.TheSameString() });
                                                                                    compare.IsCompare().ShouldBeFalse();
                                                                                };

        It should_be_compare_with_forward_to_string = () =>
                                                          {
                                                              var compare = new CompareFactory<FakeCompare, FakeCompareWithGuid>();
                                                              compare.ForwardToString(r => r.Result);
                                                              var theSameGuid = Pleasure.Generator.TheSameGuid();
                                                              compare.Compare(new FakeCompare { Result = theSameGuid.ToString() }, new FakeCompareWithGuid { Result = theSameGuid });
                                                              compare.IsCompare().ShouldBeTrue();
                                                          };

        It should_be_compare_with_forward_to_default_class = () =>
                                                                 {
                                                                     var compare = new CompareFactory<FakeCompare, FakeCompare>();
                                                                     compare.ForwardToDefault(r => r.Result);
                                                                     compare.Compare(new FakeCompare(), new FakeCompare { Result = Pleasure.Generator.String() });
                                                                     compare.IsCompare().ShouldBeTrue();
                                                                 };

        It should_be_compare_with_forward_to_default_structure = () =>
                                                                     {
                                                                         var compare = new CompareFactory<FakeCompareWithStructure, FakeCompareWithStructure>();
                                                                         compare.ForwardToDefault(r => r.Result);
                                                                         compare.Compare(new FakeCompareWithStructure(), new FakeCompareWithStructure { Result = Pleasure.Generator.PositiveNumber() });
                                                                         compare.IsCompare().ShouldBeTrue();
                                                                     };

        It should_be_compare_with_forward_to_action = () =>
                                                          {
                                                              var spy = Pleasure.Spy();

                                                              var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                              compare.ForwardToAction(r => r.Result, fakeCompare => spy.Object.Is(fakeCompare));
                                                              compare.Compare(new FakeCompare(), new FakeCompare2());
                                                              compare.IsCompare().ShouldBeTrue();

                                                              spy.Verify(r => r.Is(Pleasure.MockIt.IsAny<FakeCompare>()));
                                                          };

        It should_be_compare_with_ignore = () =>
                                               {
                                                   var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                   compare.Ignore(r => r.Result, "Test");
                                                   compare.Compare(Pleasure.Generator.Invent<FakeCompare>(), Pleasure.Generator.Invent<FakeCompare2>());
                                                   compare.IsCompare().ShouldBeTrue();
                                               };

        It should_be_compare_with_ignore_because_calculate = () =>
                                                                 {
                                                                     var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                                     compare.IgnoreBecauseCalculate(r => r.Result);
                                                                     compare.Compare(Pleasure.Generator.Invent<FakeCompare>(), Pleasure.Generator.Invent<FakeCompare2>());
                                                                     compare.IsCompare().ShouldBeTrue();
                                                                 };

        It should_be_compare_with_ignore_because_not_use = () =>
                                                               {
                                                                   var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                                   compare.IgnoreBecauseNotUse(r => r.Result);
                                                                   compare.Compare(Pleasure.Generator.Invent<FakeCompare>(), Pleasure.Generator.Invent<FakeCompare2>());
                                                                   compare.IsCompare().ShouldBeTrue();
                                                               };

        It should_be_compare_with_ignore_because_root = () =>
                                                            {
                                                                var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                                compare.IgnoreBecauseRoot(r => r.Result);
                                                                compare.Compare(Pleasure.Generator.Invent<FakeCompare>(), Pleasure.Generator.Invent<FakeCompare2>());
                                                                compare.IsCompare().ShouldBeTrue();
                                                            };

        #endregion

        #region Exceptions 

        It should_be_throw_exception_if_property_duplicate_in_ignore = () =>
                                                                           {
                                                                               var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                                               compare.Ignore(r => r.Result, "Test");
                                                                               Catch
                                                                                       .Exception(() => compare.Ignore(r => r.Result, "Test"))
                                                                                       .ShouldBeOfType<SpecificationException>();
                                                                           };

        It should_be_throw_exception_if_property_duplicate_in_forward_to_value = () =>
                                                                                     {
                                                                                         var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                                                         compare.ForwardToValue(r => r.Result, "Test");
                                                                                         Catch
                                                                                                 .Exception(() => compare.ForwardToValue(r => r.Result, "Test"))
                                                                                                 .ShouldBeOfType<SpecificationException>();
                                                                                     };

        It should_be_throw_exception_if_property_duplicate_in_forward = () =>
                                                                            {
                                                                                var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                                                compare.Forward(r => r.Result, r => r.Result2);
                                                                                Catch
                                                                                        .Exception(() => compare.Forward(r => r.Result, r => r.Result2))
                                                                                        .ShouldBeOfType<SpecificationException>();
                                                                            };

        It should_be_throw_exception_if_property_duplicate_in_forward_to_action = () =>
                                                                                      {
                                                                                          var compare = new CompareFactory<FakeCompare, FakeCompare2>();
                                                                                          compare.ForwardToAction(r => r.Result, fakeCompare => { });
                                                                                          Catch
                                                                                                  .Exception(() => compare.ForwardToAction(r => r.Result, fakeCompare => { }))
                                                                                                  .ShouldBeOfType<SpecificationException>();
                                                                                      };

        #endregion
    }
}