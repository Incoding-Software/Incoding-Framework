namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.WebPages;
    using Incoding.Extensions;
    using Incoding.MvcContrib.MVD;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class IncodingMetaCallbackInsertDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        string insertProperty = string.Empty;

        string insertTemplateSelector = string.Empty;

        bool prepare;

        string content = Selector.Result;

        #endregion

        #region Constructors

        public IncodingMetaCallbackInsertDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Api Methods

        /// <summary>
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

#if DEBUG
        [Obsolete("Please use selector as argument", false)]
#endif
#if !DEBUG
        [Obsolete("Please use selector as argument", true)]
#endif
        [UsedImplicitly, ExcludeFromCodeCoverage]
        public IncodingMetaCallbackInsertDsl WithTemplate(string selector)
        {
            throw new ArgumentException("Argument should be type of Selector", "selector");
        }

        [Obsolete("Please use WithTemplateByUrl")]
        public IncodingMetaCallbackInsertDsl WithTemplate(Selector selector)
        {
            this.insertTemplateSelector = selector;
            return this;
        }

        [Obsolete("Please use WithTemplateByUrl")]
        public IncodingMetaCallbackInsertDsl WithTemplate(JquerySelectorExtend selector)
        {
            this.insertTemplateSelector = selector;
            return this;
        }

        [Obsolete("Suggest use ONLY WithTemplateByUrl")]
        public IncodingMetaCallbackInsertDsl WithTemplateById(string id)
        {
            return WithTemplate(id.ToId() as Selector);
        }

        public IncodingMetaCallbackInsertDsl WithTemplateByUrl(string url)
        {
            return WithTemplate(url.ToAjaxGet());
        }

        [ExcludeFromCodeCoverage]
        public IncodingMetaCallbackInsertDsl WithTemplateByUrl(Func<UrlDispatcher,string> evaluated)
        {
            var dispatcher = new UrlDispatcher(new UrlHelper(HttpContext.Current.Request.RequestContext));           
            return WithTemplateByUrl(evaluated(dispatcher));
        }


        [ExcludeFromCodeCoverage]
        public IncodingMetaCallbackInsertDsl WithTemplateByView([PathReference]string view)
        {
            return WithTemplateByUrl(r => r.AsView(view));
        }

        public IncodingMetaCallbackInsertDsl Prepare()
        {
            this.prepare = true;
            return this;
        }

        [Obsolete("Please use On with Selector.Result.For<T>(r=>r.Prop)", false)]
        public IncodingMetaCallbackInsertDsl For<TModel>(Expression<Func<TModel, object>> property)
        {
            this.insertProperty = property.GetMemberName();
            return this;
        }

        public IncodingMetaCallbackInsertDsl Use(Func<object, HelperResult> text)
        {
            this.content = new ValueSelector(Selector.FromHelperResult(text));
            return this;
        }
        public IncodingMetaCallbackInsertDsl Use(Selector setContent)
        {
            this.content = setContent;
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

        #endregion

        IExecutableSetting InternalInsert(string method)
        {
            return this.plugIn.Registry(new ExecutableInsert(method, this.insertProperty, this.insertTemplateSelector, this.prepare,this.content));
        }
    }
}