namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.MvcContrib.MVD;
    using JetBrains.Annotations;
    using NHibernate.Properties;
    using Ninject.Infrastructure.Language;

    #endregion

    public partial class IncodingMetaLanguageDsl 
    {
        #region IIncodingMetaLanguageActionDsl Members

        IIncodingMetaLanguageEventBuilderDsl AddAction(ExecutableActionBase action)
        {
            isEmptyAction = false;
            meta.Add(action);
            return this;
        }

        [Obsolete("Please use Selector.Event.Result")]
        public IIncodingMetaLanguageEventBuilderDsl Event()
        {
           return AddAction(new ExecutableEventAction());
        }

        [Obsolete(@"Method not more require")]
        public IIncodingMetaLanguageEventBuilderDsl Direct()
        {
            return AddAction(new ExecutableDirectAction(string.Empty));
        }

        public IIncodingMetaLanguageEventBuilderDsl Direct(IncodingResult result)
        {
            return AddAction(new ExecutableDirectAction(result.Data.ToJsonString()));
        }

        public IIncodingMetaLanguageEventBuilderDsl Direct(object result)
        {
            return Direct(IncodingResult.Success(result));
        }

        public IIncodingMetaLanguageEventBuilderDsl Submit(Action<JqueryAjaxFormOptions> configuration = null)
        {
            return SubmitOn(selector => selector.Self(), configuration);
        }

        [Obsolete(@"Use Submit with option.Selector = selector ")]
        public IIncodingMetaLanguageEventBuilderDsl SubmitOn(Func<JquerySelector, JquerySelector> action, Action<JqueryAjaxFormOptions> configuration = null)
        {
            var options = new JqueryAjaxFormOptions(JqueryAjaxFormOptions.Default);
            configuration.Do(r => r(options));
            return AddAction(new ExecutableSubmitAction(action(Selector.Jquery), options));
        }

        public IIncodingMetaLanguageEventBuilderDsl Hash(string url = "", string prefix = "root")
        {
            return Hash(r =>
                        {
                            r.Url = url;
                            r.Type = HttpVerbs.Get;
                        }, prefix);
        }

        [Obsolete("Use Hash")]
        public IIncodingMetaLanguageEventBuilderDsl AjaxHashGet(string url = "", string prefix = "root")
        {
            return Hash(url, prefix);
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxHashPost(string url = "", string prefix = "root")
        {
            return HashPost(url, prefix);
        }

        public IIncodingMetaLanguageEventBuilderDsl HashPost(string url = "", string prefix = "root")
        {
            return AjaxHash(options =>
                            {
                                if (!string.IsNullOrWhiteSpace(url))
                                    options.Url = url;
                                options.Type = HttpVerbs.Post;
                            }, prefix);
        }

        public IIncodingMetaLanguageEventBuilderDsl Hash(Action<JqueryAjaxOptions> configuration, string prefix = "root")
        {
            var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
            configuration(options);
            return AddAction(new ExecutableAjaxAction(true, prefix, options));
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxGet(string url)
        {
            return Ajax(url);
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxPost(string url)
        {
            return Ajax(options =>
                        {
                            options.Url = url;
                            options.Type = HttpVerbs.Post;
                        });
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxPost(MvcHtmlString url)
        {
            return AjaxPost(url.ToHtmlString());
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax([NotNull] Action<JqueryAjaxOptions> configuration)
        {
            var options = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
            configuration(options);
            Guard.NotNullOrWhiteSpace("url", options.Url);
            return AddAction(new ExecutableAjaxAction(false, string.Empty, options));
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax([NotNull] string url)
        {
            return Ajax(options =>
                        {
                            options.Url = url;
                            options.Type = HttpVerbs.Get;
                        });
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax<TMessage>([NotNull] TMessage message) where TMessage : IMessage, new()
        {
            return Ajax<TMessage>(message as object);
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax<TMessage>(object message = null) where TMessage : IMessage, new()
        {
            var baseType = typeof(TMessage).BaseType;
            while (baseType != typeof(object))
            {
                if (baseType == typeof(CommandBase))
                    return AjaxPost(r => r.Push<TMessage>(message));
                if (baseType.Name == "QueryBase`1")
                    return Ajax(r => r.Query<TMessage>(message).AsJson());

                baseType = baseType.BaseType;
            }
            throw new ArgumentException("Use Command or Query", "message");
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax(MvcHtmlString url)
        {
            return Ajax(url.ToHtmlString());
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxHash(Action<JqueryAjaxOptions> configuration, string prefix = "root")
        {
            return Hash(configuration, prefix);
        }

        public IIncodingMetaLanguageEventBuilderDsl Ajax([NotNull] Func<IUrlDispatcher, string> url)
        {
            var urlDispatcher = HtmlExtensions.UrlDispatcher;
            return Ajax(url(urlDispatcher));
        }

        public IIncodingMetaLanguageEventBuilderDsl AjaxPost([NotNull] Func<IUrlDispatcher, string> url)
        {
            var urlDispatcher = HtmlExtensions.UrlDispatcher;
            return AjaxPost(url(urlDispatcher));
        }

        #endregion
    }
}