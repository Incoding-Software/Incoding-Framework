namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.ExceptionHandling;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(ActionPolicy))]
    public class When_action_policy
    {
        #region Establish value

        static void Run(ActionPolicy policy, Action<Mock<ISpy>> verify)
        {
            var spy = Pleasure.Spy();
            spy.Setup(r => r.Is()).Throws<ArgumentException>();
            var exception = Catch.Exception(() => policy.Do(() => spy.Object.Is()));
            exception.ShouldNotBeNull();
            verify(spy);
        }

        #endregion

        It should_be_direct = () => Run(ActionPolicy.Direct(), mock => mock.Exactly(1));

        It should_be_repeat_once = () => Run(ActionPolicy.Repeat(1), mock => mock.Exactly(2));

        It should_be_repeat = () => Run(ActionPolicy.Repeat(5), mock => mock.Exactly(6));

        It should_be_reperat_with_interval = () => Run(ActionPolicy.Repeat(5).Interval(1.Seconds()), mock => mock.Exactly(6));
    }
}