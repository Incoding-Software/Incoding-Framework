namespace Incoding.MvcContrib
{
    public class JSGeolocationSelector : Selector, IJavaScriptSelector
    {
        #region Constructors

        public JSGeolocationSelector(Selector selector)
                : base(selector) { }

        #endregion

        #region Properties

        public JSCoordsSelector CurrentPosition
        {
            get
            {
                AddProperty("geolocation");
                AddProperty("currentPosition");
                return new JSCoordsSelector(this);
            }
        }

        #endregion
    }
}