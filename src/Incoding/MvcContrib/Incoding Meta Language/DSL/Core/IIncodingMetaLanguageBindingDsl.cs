namespace Incoding.MvcContrib
{
    using JetBrains.Annotations;

    public interface IIncodingMetaLanguageBindingDsl
    {
        IIncodingMetaLanguageActionDsl Do();

        [UsedImplicitly]
        IIncodingMetaLanguageActionDsl DoWithPreventDefault();

        [UsedImplicitly]
        IIncodingMetaLanguageActionDsl DoWithStopPropagation();

        [UsedImplicitly]
        IIncodingMetaLanguageActionDsl DoWithPreventDefaultAndStopPropagation();
    }
}