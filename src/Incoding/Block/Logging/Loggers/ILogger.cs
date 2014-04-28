namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;

    #endregion

    public interface ILogger
    {
        void Log(LogMessage logMessage);

        ILogger WithTemplate(Func<LogMessage, string> func);
    }
}