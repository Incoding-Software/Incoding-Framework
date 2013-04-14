namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    public static class IncodingMetaLanguageDslExtensions
    {
        #region Factory constructors

        public static void Behaviors(this IIncodingMetaLanguagePlugInDsl plugInDsl, Action<IIncodingMetaLanguagePlugInDsl> action)
        {
            action(plugInDsl);
        }

        public static IncodingMetaLanguageCoreDsl Core(this IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            return new IncodingMetaLanguageCoreDsl(plugInDsl);
        }

        #endregion
    }
}