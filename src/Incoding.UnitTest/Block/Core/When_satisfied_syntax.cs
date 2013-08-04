namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.Core;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IsSatisfied<>))]
    public class When_satisfied_syntax
    {
        It should_be_all = () => SatisfiedSyntax
                                         .All<Exception>()
                                         .IsSatisfied(new ApplicationException())
                                         .ShouldBeTrue();

        It should_be_and_also = () => SatisfiedSyntax
                                              .For<Exception, ArgumentException>()
                                              .Also<Exception, InvalidCastException>()
                                              .IsSatisfied(new InvalidCastException())
                                              .ShouldBeTrue();

        It should_be_exclude = () => SatisfiedSyntax
                                             .ForDeepDerived<Exception, Exception>()
                                             .Exclude<Exception, ArgumentException>()
                                             .IsSatisfied(new ArgumentException())
                                             .ShouldBeFalse();

        It should_be_filter = () => SatisfiedSyntax
                                            .Filter<Exception>(exception => ((ArgumentException)exception).ParamName.Equals(Pleasure.Generator.TheSameString()))
                                            .IsSatisfied(new ArgumentException(string.Empty, Pleasure.Generator.TheSameString()))
                                            .ShouldBeTrue();

        It should_be_first_derived_with_wrong_instance = () => SatisfiedSyntax
                                                                       .ForFirstDerived<Exception, Exception>()
                                                                       .IsSatisfied(new InvalidCastException())
                                                                       .ShouldBeFalse();

        It should_be_first_derived = () => SatisfiedSyntax
                                                   .ForFirstDerived<Exception, SystemException>()
                                                   .IsSatisfied(new ArgumentException())
                                                   .ShouldBeTrue();
    }
}