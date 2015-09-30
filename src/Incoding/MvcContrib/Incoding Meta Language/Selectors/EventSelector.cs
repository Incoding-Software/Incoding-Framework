namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class EventSelector : Selector
    {
        #region Constructors

        internal EventSelector()
                : base("this.event") { }

        #endregion

        Selector Get(string prop)
        {
            var js = new JavaScriptSelector(selector);
            js.AddProperty(prop);
            return js;
        }

        #region Properties

        /// <summary>
        ///     Returns the name of the event
        /// </summary>
        public Selector Type { get { return Get("type"); } }

        /// <summary>
        ///     Returns indicates whether the META key was pressed when the event fired.
        /// </summary>
        public Selector MetaKey { get { return Get("metaKey"); } }

        /// <summary>
        ///     Returns which keyboard key or mouse button was pressed for the event.
        /// </summary>
        public Selector Which { get { return Get("which"); } }

        /// <summary>
        ///     Returns the horizontal coordinate of the mouse pointer, relative to the current window, when an event was triggered
        /// </summary>
        public Selector PageX { get { return Get("pageX"); } }

        /// <summary>
        ///     Returns the vertical coordinate of the mouse pointer, relative to the current window, when an event was triggered
        /// </summary>
        public Selector PageY { get { return Get("pageY"); } }

        /// <summary>
        ///     Returns the horizontal coordinate of the mouse pointer, relative to the screen, when an event was triggered
        /// </summary>
        public Selector ScreenX { get { return Get("screenX"); } }

        /// <summary>
        ///     Returns the vertical coordinate of the mouse pointer, relative to the screen, when an event was triggered
        /// </summary>
        public Selector ScreenY { get { return Get("screenY"); } }

        /// <summary>
        ///     An optional object of data passed to an event method when the current executing handler is bound.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <returns></returns>
        public Selector Data<T>(Expression<Func<T, object>> prop)
        {
            return Get(prop.GetMemberName());
        }

        public Selector Data(string prop)
        {
            return Get(prop);
        }

        #endregion
    }
}