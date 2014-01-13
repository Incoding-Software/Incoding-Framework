namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using Incoding.Block.IoC;

    #endregion

    public class MvcTemplate<TModel> : IDisposable
    {
        #region Static Fields

        readonly Lazy<ITemplateFactory> factory = new Lazy<ITemplateFactory>(() => IoCFactory.Instance.TryResolve<ITemplateFactory>() ?? new TemplateHandlebarsFactory());

        #endregion

        #region Fields

        readonly HtmlHelper htmlHelper;

        #endregion

        #region Constructors

        public MvcTemplate(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        #endregion

        #region Api Methods

        public ITemplateSyntax<TModel> ForEach()
        {
            return factory.Value.ForEach<TModel>(this.htmlHelper);
        }

        public ITemplateSyntax<TModel> NotEach()
        {
            return factory.Value.NotEach<TModel>(this.htmlHelper);
        }

        #endregion

        #region Disposable

        public virtual void Dispose() { }

        #endregion
    }
}