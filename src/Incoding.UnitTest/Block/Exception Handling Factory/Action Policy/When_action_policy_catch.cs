namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.ExceptionHandling;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(ActionPolicy))]
    public class When_action_policy_catch
    {
        #region Estabilish value

        static Mock<ISpy> spy;

        static ActionPolicy actionPolicy;

        static Exception exception;

        #endregion

        Establish establish = () =>
                                  {
                                      spy = Pleasure.Spy();
                                      actionPolicy = ActionPolicy
                                              .Catch(exception => spy.Object.Is(exception));
                                  };

        Because of = () => { exception = Catch.Exception(() => actionPolicy.Do(() => { throw new ArgumentException(); })); };

        It should_be_catch_at_once = () => spy.Verify(r => r.Is(Pleasure.MockIt.Is<object[]>(objects => objects[0].ShouldBeOfType<ArgumentException>())));

        It should_be_catch_exception = () => exception.ShouldBeNull();
    }
}