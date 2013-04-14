namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System;
    using System.ComponentModel;
    using Incoding.Extensions;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(EnumExtensions))]
    public class When_enum_extensions
    {
        #region Estabilish value

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
            Value, 

            Value2, 

            Value3
        }

        #endregion

        It should_be_to_string_int = () => FakeEnum.Value2
                                                   .ToStringInt()
                                                   .ShouldEqual("2");

        It should_be_to_string_lower = () => FakeEnum.Value2
                                                     .ToStringLower()
                                                     .ShouldEqual("value2");

        It should_be_to_jquery_string = () => (FlagFakeEnum.Value2 | FlagFakeEnum.Value3)
                                                      .ToJqueryString()
                                                      .ShouldEqual("value2 value3");

        It should_be_to_localization_without_description = () => FakeEnum.Value2
                                                                         .ToLocalization()
                                                                         .ShouldEqual("Value2");    
        
        It should_be_to_localization_with_wrong_enum = () => ((FakeEnum)15)
                                                                         .ToLocalization()
                                                                         .ShouldBeEmpty();

        It should_be_to_localization_with_description = () => FakeEnum.Value3
                                                                      .ToLocalization()
                                                                      .ShouldEqual("Custom");

        It should_be_to_array_with_ignore_0 = () => typeof(FakeEnum)
                                                            .ToArrayEnum<FakeEnum>()
                                                            .ShouldEqualWeak(Pleasure.ToArray(FakeEnum.Value, FakeEnum.Value2, FakeEnum.Value3));
    }
}