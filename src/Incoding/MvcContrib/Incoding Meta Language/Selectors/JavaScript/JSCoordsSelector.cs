namespace Incoding.MvcContrib
{
    public class JSCoordsSelector : Selector, IJavaScriptSelector
    {
        #region Constructors

        public JSCoordsSelector(Selector selector)
                : base(selector) { }

        #endregion

        #region Properties

        public Selector Latitude
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("latitude");
                return res;
            }
        }

        public Selector Longitude
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("longitude");
                return res;
            }
        }

        public Selector Accuracy
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("accuracy");
                return res;
            }
        }

        public Selector AltitudeAccuracy
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("altitudeAccuracy");
                return res;
            }
        }

        public Selector Heading
        {
            get
            {
                var res = new JSCoordsSelector(this);
                res.AddProperty("heading");
                return res;
            }
        }

        public Selector Speed
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