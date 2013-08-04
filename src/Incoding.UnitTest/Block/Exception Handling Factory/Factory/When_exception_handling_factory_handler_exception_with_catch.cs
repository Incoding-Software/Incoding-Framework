namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.ExceptionHandling;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(ExceptionHandlingFactory))]
    public class When_exception_handling_factory_handler_exception_with_catch
    {
        #region Estabilish value

        static Mock<ISpy> spy;

        static Exception exception;

        static ExceptionHandlingFactory exceptionHandling;

        #endregion

        Establish establish = () =>
                                  {
                                      exceptionHandling = new ExceptionHandlingFactory();
                                      exceptionHandling.Initialize(handling =>
                                                                       {
                                                                           spy = Pleasure.Spy();
                                                                           handling.WithPolicy(r=>r.ForAll().Catch(exception => spy.Object.Is(exception)));
                                                                       });
                                  };

        Because of = () => { exception = Catch.Exception(() => exceptionHandling.Handler(new ArgumentException())); };

        It should_be_mute = () => exception.ShouldBeNull();

        It should_be_fire_mute = () => spy.Verify(r => r.Is(Pleasure.MockIt.IsStrong(new ArgumentException())), Times.Once());
    }
}