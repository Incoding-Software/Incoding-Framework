namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(LoggingPolicy))]
    public class When_logging_policy_with_in_line : Context_Logging_Policy
    {
        #region Establish value

        static bool result;

        #endregion

        Establish establish = () =>
                                  {
                                      loggingPolicy = new LoggingPolicy();
                                      loggingPolicy.For(LogType.Debug)
                                                   .UseInLine(context => { result = true; });
                                  };

        Because of = () => loggingPolicy.Log(LogType.Debug,new LogMessage(Pleasure.Generator.String(), null, null));

        It should_be_in_line = () => result.ShouldBeTrue();
    }
}