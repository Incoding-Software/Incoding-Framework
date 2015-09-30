namespace Incoding.MvcContrib
{
    public interface IIncodingMetaLanguageBehaviorDsl : IIncodingMetaLanguagePlugInDsl
    {
        void Lock();

        void UnLock();
    }
}