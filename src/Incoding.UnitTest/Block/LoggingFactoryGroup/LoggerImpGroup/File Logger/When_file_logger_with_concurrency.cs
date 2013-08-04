namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System.Linq;
    using System.Threading;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(FileLogger)), Ignore("Need fixed")]
    public class When_file_logger_with_concurrency : Context_file_logger
    {
        #region Estabilish value

        static string logMessage;

        static string fileName = typeof(When_file_logger_with_concurrency).Name;

        static ManualResetEvent manualResetEvent;

        #endregion

        Establish establish = () =>
                                  {
                                      logger = FileLogger.WithoutReplace(folderPath, () => fileName);
                                      logMessage = Pleasure.Generator.String();
                                  };

        Because of = () => { manualResetEvent = Pleasure.MultiThread.Do10(() => logger.Log(new LogMessage(logMessage, null, null))); };

        It should_be_contact_content = () =>
                                           {
                                               manualResetEvent.Reset();
                                               GetContent(fileName).ShouldEqual(new[]
                                                                                    {
                                                                                            logMessage, logMessage, logMessage, logMessage, logMessage, 
                                                                                            logMessage, logMessage, logMessage, logMessage, logMessage
                                                                                    }.Aggregate(string.Empty, (res, current) => res += current));
                                           };
    }
}