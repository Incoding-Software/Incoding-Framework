namespace Incoding.MvcContrib
{
    public class JSCoordsSelector : Selector, IJavaScriptSelector
    {
        #region Constructors

        public JSCoordsSelector(Selector selector)
                : base(selector) { }

        #endregion

        #region Properties

        public ISelector Latitude
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("latitude");
                return res;
            }
        }

        public ISelector Longitude
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("longitude");
                return res;
            }
        }

        public ISelector Accuracy
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("accuracy");
                return res;
            }
        }

        public ISelector AltitudeAccuracy
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("altitudeAccuracy");
                return res;
            }
        }

        public ISelector Heading
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("heading");
                return res;
            }
        }

        public ISelector Speed
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("speed");
                return res;
            }
        }

        #endregion
    }
}