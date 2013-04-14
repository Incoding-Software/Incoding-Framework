namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(Manipulate))]
    public class When_pleasure_manipulate
    {
        #region Estabilish value

        enum Fake2Enum
        {
            Value1, 

            Value2
        }

        enum FakeEnum
        {
            [UsedImplicitly]
            Email, 

            [UsedImplicitly]
            Twitter, 

            [UsedImplicitly]
            Facebook, 

            [UsedImplicitly]
            Mcf, 

            [UsedImplicitly]
            Csv, 

            [UsedImplicitly]
            Google, 

            Linkedin, 

            [UsedImplicitly]
            QuickBooks
        }

        static string sourceValue;

        #endregion

        Establish establish = () => { sourceValue = Pleasure.Generator.String(); };

        It should_be_inverse = () => sourceValue.Inverse().ShouldNotEqual(sourceValue);

        It should_be_inverse_bool = () => true.Inverse().ShouldBeFalse();

        It should_be_inverse_int = () => 5.Inverse().ShouldNotEqual(5);

        It should_be_inverse_enum = () => Pleasure.Do((i) => FakeEnum.Email
                                                                       .Inverse<FakeEnum>()
                                                                       .ShouldNotEqual(FakeEnum.Email),50);

        It should_be_inverse_case = () =>
                                        {
                                            string afterInverseCase = sourceValue.InverseCase();
                                            afterInverseCase.Equals(sourceValue).ShouldBeFalse();
                                            afterInverseCase.Equals(sourceValue, StringComparison.InvariantCultureIgnoreCase).ShouldBeTrue();
                                        };

        It should_be_cut_part = () =>
                                    {
                                        string afterCutPart = sourceValue.CutPart();
                                        sourceValue.Equals(afterCutPart, StringComparison.InvariantCultureIgnoreCase).ShouldBeFalse();
                                        sourceValue.ShouldContain(afterCutPart);
                                    };
    }
}