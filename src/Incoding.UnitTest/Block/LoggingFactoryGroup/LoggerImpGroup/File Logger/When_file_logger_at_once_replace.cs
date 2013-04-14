namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System.IO;
    using Incoding.Block.Logging;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(FileLogger))]
    public class When_file_logger_at_once_replace : Context_file_logger
    {
        #region Estabilish value

        static string logMessage;

        static string fileName = typeof(When_file_logger_at_once_replace).Name;

        #endregion

        Establish establish = () =>
                                  {
                                      logger = FileLogger.WithAtOnceReplace(folderPath, () => fileName);
                                      logger.WithTemplate(r => r.Message);

                                      using (var stream = new StreamWriter(Path.Combine(folderPath, fileName), false))
                                      {
                                          stream.Write("This establish message");
                                          stream.Flush();
                                      }

                                      logMessage = Pleasure.Generator.String();
                                  };

        Because of = () => logger.Log(new LogMessage(logMessage, null, null));

        It should_be_clear_only_first_write = () => GetContent(fileName).ShouldContain(logMessage);
    }
}