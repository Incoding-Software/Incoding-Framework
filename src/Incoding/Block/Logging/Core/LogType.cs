namespace Incoding.Block.Logging
{
    #region << Using >>

    using JetBrains.Annotations;

    #endregion

    public class LogType
    {
        #region Constants

        public const string Debug = "Debug";

        [UsedImplicitly]
        public const string Release = "Release";

        public const string Trace = "Trace";

        [UsedImplicitly]
        public const string Fatal = "Fatal";

        [UsedImplicitly]
        public const string Info = "Info";

        #endregion
    }
}