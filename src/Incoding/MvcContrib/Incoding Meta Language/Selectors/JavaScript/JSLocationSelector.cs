namespace Incoding.MvcContrib
{
    public class JSLocationSelector : Selector, IJavaScriptSelector
    {
        #region Constructors

        internal JSLocationSelector(Selector selector)
                : base(selector) { }

        #endregion

        #region Properties

        public Selector Host
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("host");
                return res;
            }
        }


        public Selector Hash
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("hash");
                return res;
            }
        }

        public Selector Href
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("href");
                return res;
            }
        }

        public Selector HostName
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("hostname");
                return res;
            }
        }

        public Selector PathName
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("pathname");
                return res;
            }
        }

        public Selector Port
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("port");
                return res;
            }
        }

        public Selector Protocol
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("protocol");
                return res;
            }
        }

        public Selector Search
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("search");
                return res;
            }
        }

        #endregion
    }
}