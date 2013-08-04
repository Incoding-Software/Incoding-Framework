namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.ExceptionHandling;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ExceptionHandlingFactory))]
    public class When_exception_handling_factory_handler_exception_with_nothing
    {
        #region Estabilish value

        static Exception exception;

        static ExceptionHandlingFactory exceptionHandling;

        #endregion

        Establish establish = () =>
                                  {
                                      exceptionHandling = new ExceptionHandlingFactory();
                                      exceptionHandling.Initialize(handling => handling.WithPolicy(r=>r.ForAll().Mute()));
                                  };

        Because of = () => { exception = Catch.Exception(() => exceptionHandling.Handler(new ArgumentException())); };

        It should_be_without_exception = () => exception.ShouldBeNull();
    }
}