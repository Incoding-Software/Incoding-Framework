namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Logging;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(LoggingPolicy))]
    public class When_logging_policy_for : Context_Logging_Policy
    {
        #region Estabilish value

        static bool result;

        #endregion

        Establish establish = () => { loggingPolicy = LoggingPolicy.For(Pleasure.Generator.TheSameString()); };

        Because of = () => { result = loggingPolicy.IsSatisfied(Pleasure.Generator.TheSameString()); };

        It should_be_satisfied = () => result.ShouldBeTrue();
    }
}