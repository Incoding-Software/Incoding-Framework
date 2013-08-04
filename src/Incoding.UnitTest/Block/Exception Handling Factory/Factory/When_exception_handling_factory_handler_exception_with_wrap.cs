namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.ExceptionHandling;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ExceptionHandlingFactory))]
    public class When_exception_handling_factory_handler_exception_with_wrap
    {
        #region Estabilish value

        static Exception exception;

        static ExceptionHandlingFactory exceptionHandling;

        #endregion

        Establish establish = () =>
                                  {
                                      exceptionHandling = new ExceptionHandlingFactory();
                                      exceptionHandling.Initialize(handling => handling.WithPolicy(policy => policy
                                                                                                                     .ForAll()
                                                                                                                     .Wrap(r => new ApplicationException(Pleasure.Generator.TheSameString(), r))));
                                  };

        Because of = () => { exception = Catch.Exception(() => exceptionHandling.Handler(new ArgumentException())); };

        It should_be_wrap = () =>
                                {
                                    exception.ShouldBeOfType<ApplicationException>();
                                    var applicationException = (ApplicationException)exception;
                                    applicationException.InnerException.ShouldBeOfType<ArgumentException>();
                                    applicationException.Message.ShouldEqual(Pleasure.Generator.TheSameString());
                                };
    }
}