namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.ExceptionHandling;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ExceptionPolicy))]
    public class When_exception_policy
    {
        It should_be_mute = () => new ExceptionPolicy()
                                          .ForAll()
                                          .Mute()
                                          .Handle(new ArgumentException())
                                          .ShouldBeNull();

        It should_be_catch = () =>
                                 {
                                     bool isCatch = false;

                                     new ExceptionPolicy()
                                             .ForAll()
                                             .Catch(exception =>
                                                        {
                                                            exception.ShouldBeAssignableTo<ArgumentException>();
                                                            isCatch = true;
                                                        })
                                             .Handle(new ArgumentException())
                                             .ShouldBeNull();

                                     isCatch.ShouldBeTrue();
                                 };

        It should_be_wrap = () => new ExceptionPolicy()
                                          .ForAll()
                                          .Wrap(exception => new ApplicationException(string.Empty, exception))
                                          .Handle(new ArgumentException())
                                          .ShouldBeAssignableTo<ApplicationException>();

        It should_be_re_throw = () => new ExceptionPolicy()
                                              .ForAll()
                                              .ReThrow()
                                              .Handle(new ArgumentException());
    }
}