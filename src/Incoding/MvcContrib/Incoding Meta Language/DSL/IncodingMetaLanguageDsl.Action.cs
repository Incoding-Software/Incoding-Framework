namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public partial class IncodingMetaLanguageDsl : IIncodingMetaLanguageActionDsl
    {
        #region IIncodingMetaLanguageActionDsl Members

        public IIncodingMetaLanguageEventBuilderDsl Event()
        {
            this.meta.Add(new ExecutableEventAction());
            return this;
        }

        public IIncodingMetaLanguageEventBuilderDsl Direct()
        {
            this.meta.Add(new ExecutableDirectAction(string.Empty));
            return this;
        }

        public IIncodingMetaLanguageEventBuilderDsl Direct(IncodingResult result)
        {
            this.meta.Add(new ExecutableDirectAction(result.Data.ToJsonString()));
            return this;
        }

        public IIncodingMetaLanguageEventBuilderDsl Direct(object result)
        {
            return Direct(IncodingResult.Success(result));
        }

        public IIncodingMetaLanguageEventBuilderDsl Submit(Action<JqueryAjaxFormOptions> configuration = null)
        {
            return SubmitOn(selector => selector.Self(), configuration);
        }

        public IIncodingMetaLanguageEventBuilderDsl SubmitOn(Func<JquerySelector, JquerySelector> action, Action<JqueryAjaxFormOptions> configuration = null)
        {
            var options = new JqueryAjaxFormOptions(JqueryAjaxFormOptions.Default);
            configuration.Do(r => r(options));
            this.meta.Add(new ExecutableSubmitAction(action(Selector.Jquery), options));
            return this;
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxHashGet(string url = "", string prefix = "root")
        {
            return AjaxHash(options =>
                            {
                                if (!string.IsNullOrWhiteSpace(url))
                                    options.Url = url;
                                options.Type = HttpVerbs.Get;
                            }, prefix);
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxHashPost(string url = "", string prefix = "root")
        {
            return AjaxHash(options =>
                            {
                                if (!string.IsNullOrWhiteSpace(url))
                                    options.Url = url;
                                options.Type = HttpVerbs.Post;
                            }, prefix);
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxGet(string url)
        {
            return Ajax(options =>
                        {
                            options.Url = url;
                            options.Type = HttpVerbs.Get;
                        });
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxPost(string url)
        {
            return Ajax(options =>
                        {
                            options.Url = url;
                            options.Type = HttpVerbs.Post;
                        });
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax(Action<JqueryAjaxOptions> configuration)
        {
            var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
            configuration(options);
            this.meta.Add(new ExecutableAjaxAction(false, string.Empty, options));
            return this;
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxHash(Action<JqueryAjaxOptions> configuration, string prefix = "root")
        {
            var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
            configuration(options);
            this.meta.Add(new ExecutableAjaxAction(true, prefix, options));
            return this;
        }

        #endregion
    }
}