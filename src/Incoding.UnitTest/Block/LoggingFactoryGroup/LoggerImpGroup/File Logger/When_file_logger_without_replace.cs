namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System.Linq;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(FileLogger))]
    public class When_file_logger_without_replace : Context_file_logger
    {
        #region Establish value

        static string logMessage;

        static string fileName = typeof(When_file_logger_without_replace).Name;

        #endregion

        Establish establish = () =>
                                  {
                                      logger = FileLogger.WithoutReplace(folderPath, () => fileName);
                                      logger.WithTemplate(r => r.Message);

                                      logMessage = Pleasure.Generator.String();
                                  };

        Because of = () => Pleasure.Do((i) => logger.Log(new LogMessage(logMessage, null, null)), 3);

        It should_be_append_in_file_message = () => GetContent(fileName).ShouldEqual(new[] { logMessage, logMessage, logMessage }.Aggregate(string.Empty, (res, current) => res += current));
    }
}