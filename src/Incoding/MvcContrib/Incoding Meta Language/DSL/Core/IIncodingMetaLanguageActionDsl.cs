namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    public interface IIncodingMetaLanguageActionDsl
    {
        IIncodingMetaLanguageEventBuilderDsl Ajax(Action<JqueryAjaxOptions> configuration);

        IIncodingMetaLanguageEventBuilderDsl AjaxHash(Action<JqueryAjaxOptions> configuration, string prefix = "root");

        IIncodingMetaLanguageEventBuilderDsl AjaxGet(string url);

        IIncodingMetaLanguageEventBuilderDsl AjaxPost(string url);

        [Obsolete("Please use Selector.Event.Result")]
        IIncodingMetaLanguageEventBuilderDsl Event();

        IIncodingMetaLanguageEventBuilderDsl Direct();

        [Obsolete(@"Please use method 'Use' with IncodingResult")]
        IIncodingMetaLanguageEventBuilderDsl Direct(IncodingResult result);

        [Obsolete("Deprecated method")]
        IIncodingMetaLanguageEventBuilderDsl Direct(object data);

        IIncodingMetaLanguageEventBuilderDsl Submit(Action<JqueryAjaxFormOptions> configuration = null);

        IIncodingMetaLanguageEventBuilderDsl SubmitOn(Func<JquerySelector, JquerySelector> action, Action<JqueryAjaxFormOptions> configuration = null);

        IIncodingMetaLanguageEventBuilderDsl AjaxHashGet(string url = "", string prefix = "root");

        IIncodingMetaLanguageEventBuilderDsl AjaxHashPost(string url = "", string prefix = "root");

        IIncodingMetaLanguageActionDsl PreventDefault();

        IIncodingMetaLanguageActionDsl StopPropagation();
    }
}