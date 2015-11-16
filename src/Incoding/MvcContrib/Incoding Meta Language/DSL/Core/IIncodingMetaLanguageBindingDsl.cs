namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using JetBrains.Annotations;

    #endregion


    public interface IIncodingMetaLanguageBindingDsl : IIncodingMetaLanguageActionDsl, IIncodingMetaLanguageEventBuilderDsl
    {
        [Obsolete("Not use because is default")]
        IIncodingMetaLanguageActionDsl Do();

        [UsedImplicitly, Obsolete("Use PreventDefault()")]
        IIncodingMetaLanguageActionDsl DoWithPreventDefault();

        [UsedImplicitly, Obsolete("Use StopPropagation()")]
        IIncodingMetaLanguageActionDsl DoWithStopPropagation();

        [UsedImplicitly, Obsolete("Use combine PreventDefault and StopPropagation()")]
        IIncodingMetaLanguageActionDsl DoWithPreventDefaultAndStopPropagation();
    }
}