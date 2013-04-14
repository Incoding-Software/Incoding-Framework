namespace Incoding.Block.Logging
{
    #region << Using >>

    using System.Windows.Forms;

    #endregion

    /// <summary>
    ///     Imp <see cref="ILogger" /> for clipboard windows
    /// </summary>
    public class ClipboardLogger : LoggerBase
    {
        public override void Log(LogMessage logMessage)
        {
            Clipboard.SetText(this.template(logMessage));
        }
    }
}