namespace Incoding.MvcContrib
{
    public class IncodingMetaCallbackStoreDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaCallbackStoreDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Properties

        public IncodingMetaCallbackHashDsl Hash
        {
            get { return new IncodingMetaCallbackHashDsl(this.plugIn); }
        }

        #endregion
    }
}