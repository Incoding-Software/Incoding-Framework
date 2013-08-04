namespace Incoding.MvcContrib
{
    #region << Using >>

    using JetBrains.Annotations;

    #endregion

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