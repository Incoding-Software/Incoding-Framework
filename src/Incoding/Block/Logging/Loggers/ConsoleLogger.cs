#region copyright

// @incoding 2011
#endregion

namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;

    #endregion

    /// <summary>
    ///     Imp <see cref="ILogger" /> for console.
    /// </summary>
    public class ConsoleLogger : LoggerBase
    {
        public override void Log(LogMessage logMessage)
        {
            Console.Write(this.template(logMessage));
        }
    }
}