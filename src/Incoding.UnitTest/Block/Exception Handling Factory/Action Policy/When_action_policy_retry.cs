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
    public class When_action_policy_retry
    {
        #region Estabilish value

        protected static ActionPolicy actionPolicy;

        static Exception exception;

        static Mock<ISpy> spy;

        static int retryCount;

        #endregion

        Establish establish = () =>
                                  {
                                      retryCount = 5;
                                      actionPolicy = ActionPolicy.Retry(retryCount);
                                  };

        Because because = () =>
                              {
                                  spy = Pleasure.Spy();
                                  exception = Catch.Exception(() => actionPolicy.Do(() =>
                                                                                        {
                                                                                            spy.Object.Is();
                                                                                            throw new ArgumentException();
                                                                                        }));
                              };

        It should_be_with_exception = () => exception.ShouldNotBeNull();

        It should_be_retry = () => spy.Exactly(retryCount);
    }
}