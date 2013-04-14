namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.ExceptionHandling;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(ExceptionPolicy))]
    public class When_exception_policy
    {
        It should_be_mute = () => ExceptionPolicy
                                          .ForAll()
                                          .Mute()
                                          .Handle(new ArgumentException())
                                          .ShouldBeNull();

        It should_be_catch = () =>
                                 {
                                     bool isCatch = false;

                                     ExceptionPolicy
                                             .ForAll()
                                             .Catch(exception =>
                                                        {
                                                            exception.ShouldBeOfType<ArgumentException>();
                                                            isCatch = true;
                                                        })
                                             .Handle(new ArgumentException())
                                             .ShouldBeNull();

                                     isCatch.ShouldBeTrue();
                                 };

        It should_be_wrap = () => ExceptionPolicy
                                          .ForAll()
                                          .Wrap(exception => new ApplicationException(string.Empty, exception))
                                          .Handle(new ArgumentException())
                                          .ShouldBeOfType<ApplicationException>();

        It should_be_re_throw = () => ExceptionPolicy
                                              .ForAll()
                                              .ReThrow()
                                              .Handle(new ArgumentException());
    }
}