namespace Incoding.MvcContrib
{
    public class JSLocationSelector : Selector, IJavaScriptSelector
    {
        #region Constructors

        internal JSLocationSelector(Selector selector)
                : base(selector) { }

        #endregion

        #region Properties

        public ISelector Host
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("host");
                return res;
            }
        }

        public ISelector Href
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("href");
                return res;
            }
        }

        public ISelector HostName
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("hostname");
                return res;
            }
        }

        public ISelector PathName
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("pathname");
                return res;
            }
        }

        public ISelector Port
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("port");
                return res;
            }
        }

        public ISelector Protocol
        {
            get
            {
                var res = new JSLocationSelector(this);
                res.AddProperty("protocol");
                return res;
            }
        }

        public ISelector Search
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