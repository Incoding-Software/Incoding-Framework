namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class IncodingMetaCallbackInsertDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        string insertProperty = string.Empty;

        string insertTemplateSelector = string.Empty;

        bool prepare;

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

        public IncodingMetaCallbackInsertDsl WithTemplate(Selector selector)
        {
            this.insertTemplateSelector = selector;
            return this;
        }

        public IncodingMetaCallbackInsertDsl Prepare()
        {
            this.prepare = true;
            return this;
        }

        public IncodingMetaCallbackInsertDsl For<TModel>(Expression<Func<TModel, object>> property)
        {
            this.insertProperty = property.GetMemberName();
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
            return this.plugIn.Registry(new ExecutableInsert(method, this.insertProperty, this.insertTemplateSelector, this.prepare));
        }
    }
}