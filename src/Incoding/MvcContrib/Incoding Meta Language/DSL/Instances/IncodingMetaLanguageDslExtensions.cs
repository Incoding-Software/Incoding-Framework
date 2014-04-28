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
            var behaviorDsl = (IIncodingMetaLanguageBehaviorDsl)plugInDsl;
            behaviorDsl.Lock();
            action(plugInDsl);
            behaviorDsl.UnLock();
        }

        public static IncodingMetaLanguageCoreDsl Core(this IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            return new IncodingMetaLanguageCoreDsl(plugInDsl);
        }

        #endregion
    }
}