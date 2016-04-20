namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System;
    using System.ComponentModel;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(EnumExtensions))]
    public class When_enum_extensions
    {
        It should_be_equal_name_to_name_with_nestead_enum = () => typeof(FakeClass1.FakeEnum).Name.ShouldEqual(typeof(FakeClass2.FakeEnum).Name);

        It should_be_to_array_with_ignore_0 = () => typeof(FakeEnum)
                                                            .ToArrayEnum<FakeEnum>()
                                                            .ShouldEqualWeak(Pleasure.ToArray(FakeEnum.Value, FakeEnum.Value2, FakeEnum.Value3));

        It should_be_to_jquery_string = () => (FlagFakeEnum.Value2 | FlagFakeEnum.Value3)
                                                      .ToJqueryString()
                                                      .ShouldEqual("value2 value 3");

        It should_be_to_localization_flag = () => FlagFakeEnum.Value2
                                                              .ToLocalization()
                                                              .ShouldEqual("Value2");

        It should_be_to_localization_flag_multiple = () => (FlagFakeEnum.Value2 | FlagFakeEnum.Value3)
                                                                   .ToLocalization()
                                                                   .ShouldEqual("Value2 Value 3");

        It should_be_to_localization_nestead = () =>
                                               {
                                                   FakeClass1.FakeEnum.Value.ToLocalization().ShouldEqual("Value From FakeClass1");
                                                   FakeClass2.FakeEnum.Value.ToLocalization().ShouldEqual("Value From FakeClass2");
                                               };

        It should_be_to_localization_with_description = () => FakeEnum.Value3
                                                                      .ToLocalization()
                                                                      .ShouldEqual("Custom");

        It should_be_to_localization_with_wrong_enum = () => ((FakeEnum)15)
                                                                     .ToLocalization()
                                                                     .ShouldBeEmpty();

        It should_be_to_localization_without_description = () => FakeEnum.Value2
                                                                         .ToLocalization()
                                                                         .ShouldEqual("Value2");

        It should_be_to_localization_as_pefromance = () => Pleasure.Do(i => (FlagFakeEnum.Value2 | FlagFakeEnum.Value3)
                                                                    .ToLocalization(), 100000)
                                                   .ShouldBeLessThan(160);

        It should_be_to_string_int = () => FakeEnum.Value2
                                                   .ToStringInt()
                                                   .ShouldEqual("2");

        It should_be_to_string_lower = () => FakeEnum.Value2
                                                     .ToStringLower()
                                                     .ShouldEqual("value2");

        public class FakeClass1
        {
            public enum FakeEnum
            {
                [Description("Value From FakeClass1")]
                Value
            }
        }

        public class FakeClass2
        {
            public enum FakeEnum
            {
                [Description("Value From FakeClass2")]
                Value
            }
        }

        #region Establish value

        enum FakeEnum
        {
            [UsedImplicitly]
            Zero = 0,

            Value = 1,

            Value2 = 2,

            [Description("Custom")]
            Value3 = 3
        }

        [Flags]
        enum FlagFakeEnum
        {
            Value = 1,

            Value2 = 2,

            [Description("Value 3")]
            Value3 = 4
        }

        #endregion
    }
}