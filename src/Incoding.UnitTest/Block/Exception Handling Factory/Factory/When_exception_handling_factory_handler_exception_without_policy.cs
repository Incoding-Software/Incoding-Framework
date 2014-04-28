namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.ExceptionHandling;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ExceptionHandlingFactory))]
    public class When_exception_handling_factory_handler_exception_without_policy
    {
        #region Establish value

        static Exception exception;

        static ExceptionHandlingFactory exceptionHandling;

        #endregion

        Establish establish = () =>
                                  {
                                      exceptionHandling = new ExceptionHandlingFactory();
                                      exceptionHandling.Initialize();
                                  };

        Because of = () => { exception = Catch.Exception(() => exceptionHandling.Handler(new ArgumentException())); };

        It should_be_re_throw_same_exception = () => exception.ShouldBeOfType<ArgumentException>();
    }
}