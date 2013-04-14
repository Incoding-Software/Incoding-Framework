namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.ExceptionHandling;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(ActionPolicy))]
    public class When_action_policy_direct
    {
        #region Estabilish value

        static ActionPolicy actionPolicy;

        static Exception exception;

        #endregion

        Establish establish = () => { actionPolicy = ActionPolicy.Direct(); };

        Because of = () => { exception = Catch.Exception(() => actionPolicy.Do(() => { throw new InvalidCastException(); })); };

        It should_be_exception = () => exception.ShouldBeOfType<InvalidCastException>();
    }
}