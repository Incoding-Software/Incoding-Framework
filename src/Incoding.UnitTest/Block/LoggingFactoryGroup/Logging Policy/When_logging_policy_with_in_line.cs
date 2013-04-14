namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Logging;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(LoggingPolicy))]
    public class When_logging_policy_with_in_line : Context_Logging_Policy
    {
        #region Estabilish value

        static bool result;

        #endregion

        Establish establish = () =>
                                  {
                                      loggingPolicy = LoggingPolicy
                                              .For(LogType.Debug)
                                              .UseInLine(context => { result = true; });
                                  };

        Because of = () => loggingPolicy.Log(new LogMessage(Pleasure.Generator.String(), null, null));

        It should_be_in_line = () => result.ShouldBeTrue();
    }
}