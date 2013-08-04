namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(FileLogger))]
    public class When_file_logger_each_replace : Context_file_logger
    {
        #region Estabilish value

        static string logMessage;

        static string fileName = typeof(When_file_logger_each_replace).Name;

        #endregion

        Establish establish = () =>
                                  {
                                      logger = FileLogger.WithEachReplace(folderPath, () => fileName);
                                      logger.WithTemplate(r => r.Message);

                                      logMessage = Pleasure.Generator.String();
                                  };

        Because of = () => Pleasure.Do((i) => logger.Log(new LogMessage(logMessage, null, null)), 3);

        It should_be_each_clear_file_before_write_message = () => GetContent(fileName).ShouldEqual(logMessage);
    }
}