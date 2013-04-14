namespace Incoding.Quality
{
    #region << Using >>

    using JetBrains.Annotations;

    #endregion

    public static class ObsoleteMessage
    {
        #region Constants

        [UsedImplicitly]
        public const string UsageFactory = "Please usage factory method from static method";

        [UsedImplicitly]
        public const string SerializeConstructor = "This constructor usage only for serialize and deserialize";

        [UsedImplicitly]
        public const string ClassNotForDirectUsage = "This class usage for background or shadow target . ( sample:Nhibernate mapping)";

        [UsedImplicitly]
        public const string NotSupportForThisImplement = "Not support for this implement";

        #endregion
    }
}