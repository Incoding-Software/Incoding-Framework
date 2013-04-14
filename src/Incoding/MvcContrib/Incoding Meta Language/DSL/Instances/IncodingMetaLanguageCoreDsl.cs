namespace Incoding.MvcContrib
{
    public class IncodingMetaLanguageCoreDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaLanguageCoreDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Properties

        public IncodingMetaCallbackBindDsl Bind
        {
            get { return new IncodingMetaCallbackBindDsl(this.plugIn); }
        }

        public IncodingMetaCallbackInsertDsl Insert
        {
            get { return new IncodingMetaCallbackInsertDsl(this.plugIn); }
        }

        public IExecutableSetting Break
        {
            get { return this.plugIn.Registry(new ExecutableBreak()); }
        }

        public IncodingMetaCallbackJqueryDsl JQuery
        {
            get { return new IncodingMetaCallbackJqueryDsl(this.plugIn); }
        }

        public IncodingMetaCallbackStoreApiDsl Store
        {
            get { return new IncodingMetaCallbackStoreApiDsl(this.plugIn); }
        }

        public IncodingMetaCallbackTriggerDsl Trigger
        {
            get { return new IncodingMetaCallbackTriggerDsl(this.plugIn); }
        }

        public IncodingMetaCallbackFormApiDsl Form
        {
            get { return new IncodingMetaCallbackFormApiDsl(this.plugIn); }
        }

        #endregion

        #region Api Methods

        public IExecutableSetting Eval(string code)
        {
            return this.plugIn.Registry(new ExecutableEval(code));
        }

        #endregion
    }
}