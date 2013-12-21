namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Web.WebPages;

    #endregion

    public class IncodingMetaCallbackJqueryManipulationDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugInDsl;

        #endregion

        #region Constructors

        public IncodingMetaCallbackJqueryManipulationDsl(IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            this.plugInDsl = plugInDsl;
        }

        #endregion

        #region Api Methods

        /// <summary>
        ///     Wrap an HTML structure around each element in the set of matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Wrap(Selector selector)
        {
            return this.plugInDsl.Core().JQuery.Call("wrap", selector);
        }

        /// <summary>
        ///     Wrap an HTML structure around each element in the set of matched elements.
        /// </summary>
        public IExecutableSetting Wrap(Func<object, HelperResult> text)
        {
            return Wrap(Selector.FromHelperResult(text));
        }

        /// <summary>
        ///     Wrap an HTML structure around all elements in the set of matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting WrapAll(Selector selector)
        {
            return this.plugInDsl.Core().JQuery.Call("wrapAll", selector);
        }

        /// <summary>
        ///     Wrap an HTML structure around all elements in the set of matched elements.
        /// </summary>
        public IExecutableSetting WrapAll(Func<object, HelperResult> text)
        {
            return WrapAll(Selector.FromHelperResult(text));
        }

        /// <summary>
        ///     Set of matched elements or set the HTML contents of every matched element.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Html(Selector selector)
        {
            return Insert("html", selector);
        }

        /// <summary>
        ///     Set of matched elements or set the HTML contents of every matched element.
        /// </summary>
        public IExecutableSetting Html(Func<object, HelperResult> text)
        {
            return Html(Selector.FromHelperResult(text));
        }

        /// <summary>
        ///     Set the text contents of the matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Text(Selector selector)
        {
            return Insert("text", selector);
        }

        /// <summary>
        ///     Set the text contents of the matched elements.
        /// </summary>
        public IExecutableSetting Text(Func<object, HelperResult> text)
        {
            return Text(Selector.FromHelperResult(text));
        }

        /// <summary>
        ///     Insert content, specified by the parameter, to the end of each element in the set of matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Append(Selector selector)
        {
            return Insert("append", selector);
        }

        /// <summary>
        ///     Insert content, specified by the parameter, to the end of each element in the set of matched elements.
        /// </summary>
        public IExecutableSetting Append(Func<object, HelperResult> text)
        {
            return Append(Selector.FromHelperResult(text));
        }

        /// <summary>
        ///     Insert content, specified by the parameter, to the beginning of each element in the set of matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Prepend(Selector selector)
        {
            return Insert("prepend", selector);
        }

        /// <summary>
        ///     Insert content, specified by the parameter, to the beginning of each element in the set of matched elements.
        /// </summary>
        public IExecutableSetting Prepend(Func<object, HelperResult> text)
        {
            return Prepend(Selector.FromHelperResult(text));
        }

        /// <summary>
        ///     Insert content, specified by the parameter, after each element in the set of matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting After(Selector selector)
        {
            return Insert("after", selector);
        }

        /// <summary>
        ///     Insert content, specified by the parameter, after each element in the set of matched elements.
        /// </summary>
        public IExecutableSetting After(Func<object, HelperResult> text)
        {
            return After(Selector.FromHelperResult(text));
        }

        /// <summary>
        ///     Insert content, specified by the parameter, before each element in the set of matched elements.
        /// </summary>
        /// <param name="selector">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting Before(Selector selector)
        {
            return Insert("before", selector);
        }

        /// <summary>
        ///     Insert content, specified by the parameter, before each element in the set of matched elements.
        /// </summary>
        public IExecutableSetting Before(Func<object, HelperResult> text)
        {
            return Before(Selector.FromHelperResult(text));
        }

        /// <summary>
        ///     Replace each element in the set of matched elements with the provided new content
        /// </summary>
        /// <param name="newContent">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting ReplaceWith(Selector newContent)
        {
            return this.plugInDsl.Core().JQuery.Call("replaceWith", newContent);
        }

        /// <summary>
        ///     Replace each element in the set of matched elements with the provided new content
        /// </summary>
        public IExecutableSetting ReplaceWith(Func<object, HelperResult> text)
        {
            return ReplaceWith(Selector.FromHelperResult(text));
        }

        /// <summary>
        ///     Remove the set of matched elements from the DOM.
        /// </summary>
        public IExecutableSetting Remove()
        {
            return this.plugInDsl.Core().JQuery.Call("remove");
        }

        /// <summary>
        ///     Remove all child nodes of the set of matched elements from the DOM.
        /// </summary>
        public IExecutableSetting Empty()
        {
            return this.plugInDsl.Core().JQuery.Call("empty");
        }

        /// <summary>
        ///     Remove the set of matched elements from the DOM.
        /// </summary>
        public IExecutableSetting Detach()
        {
            return this.plugInDsl.Core().JQuery.Call("detach");
        }

        #endregion

        IExecutableSetting Insert(string method, Selector selector)
        {
            return this.plugInDsl.Core().JQuery.Call(method, selector);
        }
    }
}