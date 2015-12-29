namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.WebPages;
    using Incoding.Extensions;
    using Incoding.MvcContrib.MVD;
    using JetBrains.Annotations;

    #endregion

    public class IncodingMetaCallbackInsertDsl
    {
        #region Constructors

        public IncodingMetaCallbackInsertDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

#if DEBUG
        [Obsolete("Please use selector as argument", false), ExcludeFromCodeCoverage]
#endif
#if !DEBUG
        [Obsolete("Please use selector as argument", true), UsedImplicitly, ExcludeFromCodeCoverage]
#endif
        public IncodingMetaCallbackInsertDsl WithTemplate(string selector)
        {
            throw new ArgumentException("Argument should be type of Selector", "selector");
        }

        [Obsolete("Please use WithTemplateByUrl/WithTemplateByView")]
        public IncodingMetaCallbackInsertDsl WithTemplate([NotNull] Selector selector)
        {
            this.insertTemplateSelector = selector;
            return this;
        }

        [Obsolete("Please use WithTemplateByUrl/WithTemplateByView")]
        public IncodingMetaCallbackInsertDsl WithTemplate([NotNull] JquerySelectorExtend selector)
        {
            return WithTemplate(selector as Selector);
        }

        [Obsolete("Suggest use ONLY WithTemplateByUrl")]
        public IncodingMetaCallbackInsertDsl WithTemplateById([NotNull, HtmlAttributeValue("id")] string id)
        {
            return WithTemplate(id.ToId() as Selector);
        }

        public IncodingMetaCallbackInsertDsl WithTemplateByUrl([NotNull] string url)
        {
            if (url.StartsWith("||"))
                throw new ArgumentException("Please use Url instead of Selector", "url");

            if (url.StartsWith("~"))
                throw new ArgumentException("Please use Url instead of path to View", "url");

            return WithTemplate(url.ToAjaxGet());
        }

        [ExcludeFromCodeCoverage]
        public IncodingMetaCallbackInsertDsl WithTemplateByUrl(Func<UrlDispatcher, string> evaluated)
        {
            var dispatcher = new UrlDispatcher(new UrlHelper(HttpContext.Current.Request.RequestContext));
            return WithTemplateByUrl(evaluated(dispatcher));
        }

        [ExcludeFromCodeCoverage]
        public IncodingMetaCallbackInsertDsl WithTemplateByView([PathReference, NotNull] string view)
        {
            return WithTemplateByUrl(r => r.AsView(view));
        }

        public IncodingMetaCallbackInsertDsl Prepare()
        {
            prepare = true;
            return this;
        }

        /// <summary>
        ///     Remove the set of matched elements from the DOM.
        /// </summary>
        public IExecutableSetting Remove()
        {
            return this.plugIn.Core().JQuery.Call("remove");
        }

        /// <summary>
        ///     Remove all child nodes of the set of matched elements from the DOM.
        /// </summary>
        public IExecutableSetting Empty()
        {
            return this.plugIn.Core().JQuery.Call("empty");
        }

        /// <summary>
        ///     Remove the set of matched elements from the DOM.
        /// </summary>
        public IExecutableSetting Detach()
        {
            return this.plugIn.Core().JQuery.Call("detach");
        }

        /// <summary>
        ///     Wrap an HTML structure around each element in the set of matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Wrap()
        {
            return this.plugIn.Core().JQuery.Call("wrap", this.content);
        }

        /// <summary>
        ///     Wrap an HTML structure around all elements in the set of matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting WrapAll()
        {
            return this.plugIn.Core().JQuery.Call(this.content);
        }

        [Obsolete("Please use On with Selector.Result.For<T>(r=>r.Prop)", false)]
        public IncodingMetaCallbackInsertDsl For<TModel>(Expression<Func<TModel, object>> property)
        {
            insertProperty = property.GetMemberName();
            return this;
        }

        public IncodingMetaCallbackInsertDsl Use(Func<object, HelperResult> text)
        {
            return Use(new ValueSelector(Selector.FromHelperResult(text)));
        }

        public IncodingMetaCallbackInsertDsl Use(Selector setContent)
        {
            content = setContent;
            return this;
        }

        /// <summary>
        ///     Set the data of every matched element through After.
        /// </summary>
        public IExecutableSetting After()
        {
            return InternalInsert("after");
        }

        public IExecutableSetting Val()
        {
            return InternalInsert("val");
        }

        public IExecutableSetting Before()
        {
            return InternalInsert("before");
        }

        /// <summary>
        ///     Insert content, specified by the parameter, to the end of each element in the set of matched elements.
        /// </summary>
        public IExecutableSetting Append()
        {
            return InternalInsert("append");
        }

        public IExecutableSetting Prepend()
        {
            return InternalInsert("prepend");
        }

        /// <summary>
        ///     Set the data of every matched element through Html.
        /// </summary>
        public IExecutableSetting Html()
        {
            return InternalInsert("html");
        }

        /// <summary>
        ///     Set the data of every matched element through Text.
        /// </summary>
        public IExecutableSetting Text()
        {
            return InternalInsert("text");
        }

        IExecutableSetting InternalInsert(string method)
        {
            return plugIn.Registry(new ExecutableInsert(method, insertProperty, insertTemplateSelector, prepare, content));
        }

        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        string insertProperty = string.Empty;

        string insertTemplateSelector = string.Empty;

        bool prepare;

        string content = Selector.Result;

        #endregion
    }
}