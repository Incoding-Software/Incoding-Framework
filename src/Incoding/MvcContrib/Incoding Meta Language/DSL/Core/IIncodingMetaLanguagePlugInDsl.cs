namespace Incoding.MvcContrib
{
    public interface IIncodingMetaLanguageBehaviorDsl : IIncodingMetaLanguagePlugInDsl
    {
        void Lock();

        void UnLock();
    }

    public interface IIncodingMetaLanguagePlugInDsl
    {
        IExecutableSetting Registry(ExecutableBase callback);
    }
}