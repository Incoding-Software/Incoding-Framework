namespace Incoding.MvcContrib
{
    public class IncodingMetaCallbackFormApiDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaCallbackFormApiDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Properties

        public IncodingMetaCallbackValidationDsl Validation { get { return new IncodingMetaCallbackValidationDsl(this.plugIn); } }

        #endregion

        #region Nested classes

        public class IncodingMetaCallbackValidationDsl
        {
            #region Fields

            readonly IIncodingMetaLanguagePlugInDsl plugIn;

            #endregion

            #region Constructors

            public IncodingMetaCallbackValidationDsl(IIncodingMetaLanguagePlugInDsl plugIn)
            {
                this.plugIn = plugIn;
            }

            #endregion

            #region Api Methods

            public IExecutableSetting Parse()
            {
                return this.plugIn.Registry(new ExecutableValidationParse());
            }

            public IExecutableSetting Refresh(string prefix = "")
            {
                return this.plugIn.Registry(new ExecutableValidationRefresh(prefix));
            }

            #endregion
        }

        #endregion

        #region Api Methods

        public IExecutableSetting Reset()
        {
            return this.plugIn.Registry(new ExecutableForm("reset"));
        }

        public IExecutableSetting Clear()
        {
            return this.plugIn.Registry(new ExecutableForm("clear"));
        }

        #endregion
    }
}