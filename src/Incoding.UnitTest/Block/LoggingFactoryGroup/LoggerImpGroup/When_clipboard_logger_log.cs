namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using System.Windows.Forms;
    using Incoding.Block.Logging;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ClipboardLogger))]
    public class When_clipboard_logger_log : Context_Logger
    {
        #region Establish value

        [STAThread]
        static void Log()
        {
            messageToLog = !string.IsNullOrWhiteSpace(Clipboard.GetText()) ? Clipboard.GetText() : Pleasure.Generator.String();
            logger.Log(new LogMessage(messageToLog, null, null));
        }

        static string messageToLog;

        #endregion

        Establish establish = () =>
                                  {
                                      logger = new ClipboardLogger();
                                      logger.WithTemplate(r => r.Message);
                                  };

        Because of = Log;

        It should_be_insert_text_in_clipboard = () => Clipboard.GetText().ShouldEqual(messageToLog);
    }
}