namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class IncodingMetaCallbackJqueryAttributesDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugInDsl;

        #endregion

        #region Constructors

        public IncodingMetaCallbackJqueryAttributesDsl(IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            this.plugInDsl = plugInDsl;
        }

        #endregion

        #region Api Methods

        /// <summary>
        ///     Add or remove attribute
        /// </summary>
        public IExecutableSetting Toggle(HtmlAttribute attribute)
        {
            return Toggle(attribute.ToJqueryString());
        }

        /// <summary>
        ///     Add or remove attribute
        /// </summary>
        public IExecutableSetting Toggle(string attribute)
        {
            return this.plugInDsl.Core().JQuery.Call("toggleProp", attribute);
        }

        /// <summary>
        ///     Add or remove attribute <see cref="HtmlAttribute.Checked" />
        /// </summary>
        public IExecutableSetting ToggleChecked()
        {
            return Toggle(HtmlAttribute.Checked);
        }

        /// <summary>
        ///     Add or remove attribute <see cref="HtmlAttribute.Disabled" />
        /// </summary>
        public IExecutableSetting ToggleDisabled()
        {
            return Toggle(HtmlAttribute.Disabled);
        }

        /// <summary>
        ///     Add or remove attribute <see cref="HtmlAttribute.Readonly" />
        /// </summary>
        public IExecutableSetting ToggleReadonly()
        {
            return Toggle(HtmlAttribute.Readonly);
        }

        /// <summary>
        ///     Add or remove one or more classes from each element in the set of matched elements, depending on either the class's presence or the value of the switch argument.
        /// </summary>
        /// <param name="class">
        ///     One or more <c>class</c> names (separated by spaces) to be toggled for each element in the matched set.
        /// </param>
        public IExecutableSetting ToggleClass(string @class)
        {
            return this.plugInDsl.Core().JQuery.Call("toggleClass", @class);
        }

        /// <summary>
        ///     <c>Remove</c> a single <c>class</c>, multiple classes, or all classes from each element in the set of matched elements.
        /// </summary>
        /// <param name="class"> One or more space-separated classes to be removed from the class attribute of each matched element. </param>
        public IExecutableSetting RemoveClass(string @class)
        {
            return this.plugInDsl.Core().JQuery.Call("removeClass", @class);
        }

        /// <summary>
        ///     Remove an attribute from each element in the set of matched elements.
        /// </summary>
        /// <param name="key">Html attributes</param>
        public IExecutableSetting RemoveAttr(string key)
        {
            return this.plugInDsl.Core().JQuery.Call("removeAttr", key.ToLower());
        }

        /// <summary>
        ///     Remove an attribute from each element in the set of matched elements.
        /// </summary>
        /// <param name="key">Html attributes</param>
        public IExecutableSetting RemoveAttr(HtmlAttribute key)
        {
            return RemoveAttr(key.ToJqueryString());
        }

        /// <summary>
        ///     Set one attributes for every matched element in context.
        /// </summary>
        /// <param name="key">Html attributes</param>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting SetAttr(string key, Selector value = null)
        {
            return this.plugInDsl.Core().JQuery.Call("attr", key, value ?? key);
        }

        /// <summary>
        ///     Set one attributes for every matched element in context.
        /// </summary>
        /// <param name="key">Html attributes</param>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting SetAttr(HtmlAttribute key, Selector value = null)
        {
            return SetAttr(key.ToJqueryString(), value);
        }

        /// <summary>
        ///     Remove a property for the set of matched elements.
        /// </summary>
        /// <param name="key">Html attributes</param>
        public IExecutableSetting RemoveProp(string key)
        {
            return this.plugInDsl.Core().JQuery.Call("removeProp", key);
        }

        /// <summary>
        ///     Remove a property for the set of matched elements.
        /// </summary>
        /// <param name="key">Html attributes</param>
        public IExecutableSetting RemoveProp(HtmlAttribute key)
        {
            return RemoveProp(key.ToJqueryString());
        }

        /// <summary>
        ///     Set one properties for every matched element in context.
        /// </summary>
        /// <param name="key">Html attributes</param>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting SetProp(string key, Selector value = null)
        {
            return this.plugInDsl.Core().JQuery.Call("prop", key, value ?? key);
        }

        /// <summary>
        ///     Set one properties for every matched element in context.
        /// </summary>
        /// <param name="key">Html attributes</param>
        /// <param name="value">
        ///     <see cref="Selector" />
        /// </param>
        public IExecutableSetting SetProp(HtmlAttribute key, Selector value = null)
        {
            return SetProp(key.ToJqueryString(), value);
        }

        /// <summary>
        ///     Set the <paramref name="value" /> of each element in the set of matched elements.
        /// </summary>
        /// <param name="value"> A string of text or an array of strings corresponding to the value of each matched element to set as selected </param>
        public IExecutableSetting Val(object value)
        {
            if (value is IEnumerable)
                return this.plugInDsl.Registry(new ExecutableEval(JavaScriptCodeTemplate.Target_Val.F(value.ToJsonString())));

            return this.plugInDsl.Core().JQuery.Call("val", value.Recovery(string.Empty));
        }

        /// <summary>
        ///     Set the <paramref name="value" /> of each element in the set of matched elements.
        /// </summary>
        /// <param name="value"> A string of text or an array of strings corresponding to the value of each matched element to set as selected </param>
        public IExecutableSetting Val(Func<JquerySelector, JquerySelectorExtend> value)
        {
            if (value == null)
                return Val((object)null);

            return this.plugInDsl.Core().JQuery.Call("val", value(Selector.Jquery));
        }

        /// <summary>
        ///     Adds the specified <c>class</c> to each of the set of matched elements
        /// </summary>
        /// <param name="class">
        ///     One or more <c>class</c> names to be added to the class attribute of each matched element.
        /// </param>
        public IExecutableSetting AddClass(string @class)
        {
            return this.plugInDsl.Core().JQuery.Call("addClass", @class);
        }

        #endregion
    }
}