namespace Incoding.Block.Logging
{
    #region << Using >>

    using System.Diagnostics;

    #endregion

    /// <summary>
    ///     Imp <see cref="ILogger" /> for <see cref="Trace" />
    /// </summary>
    public class TraceLogger : ActionLogger
    {
        #region Constructors

        public TraceLogger(string category)
                : base(context => Trace.Write(context, category)) { }

        #endregion
    }
}