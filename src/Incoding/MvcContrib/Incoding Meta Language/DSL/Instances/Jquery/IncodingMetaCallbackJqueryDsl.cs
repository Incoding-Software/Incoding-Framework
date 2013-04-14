namespace Incoding.MvcContrib
{
    public class IncodingMetaCallbackJqueryDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugInDsl;

        #endregion

        #region Constructors

        public IncodingMetaCallbackJqueryDsl(IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            this.plugInDsl = plugInDsl;
        }

        #endregion

        #region Properties

        public IncodingMetaCallbackJqueryAttributesDsl Attributes
        {
            get { return new IncodingMetaCallbackJqueryAttributesDsl(this.plugInDsl); }
        }

        public IncodingMetaCallbackJqueryCssDsl Css
        {
            get { return new IncodingMetaCallbackJqueryCssDsl(this.plugInDsl); }
        }

        public IncodingMetaCallbackJqueryManipulationDsl Manipulation
        {
            get { return new IncodingMetaCallbackJqueryManipulationDsl(this.plugInDsl); }
        }

        public IncodingMetaCallbackJqueryFuncDsl Func
        {
            get { return new IncodingMetaCallbackJqueryFuncDsl(this.plugInDsl); }
        }

        #endregion
    }
}