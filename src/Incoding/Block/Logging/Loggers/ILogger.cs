namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;

    #endregion

    public interface ILogger
    {
        /// <summary>
        ///     Log message.
        /// </summary>
        /// <param name="logMessage">
        ///     See <see cref="LogMessage" />
        /// </param>
        void Log(LogMessage logMessage);

        ILogger WithTemplate(Func<LogMessage, string> func);
    }
}