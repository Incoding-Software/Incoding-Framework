namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    public class IncodingMetaLanguageCoreDsl : IIncodingMetaLanguageCoreDsl
    {
        #region Fields

        protected IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaLanguageCoreDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Properties

        public IncodingMetaCallbackBindDsl Bind { get { return new IncodingMetaCallbackBindDsl(this.plugIn); } }

        public IncodingMetaCallbackInsertDsl Insert { get { return new IncodingMetaCallbackInsertDsl(this.plugIn); } }

        public IExecutableSetting Break { get { return this.plugIn.Registry(new ExecutableBreak()); } }

        public IncodingMetaCallbackJqueryDsl JQuery { get { return new IncodingMetaCallbackJqueryDsl(this.plugIn); } }

        public IncodingMetaCallbackFuncDsl Func { get { return new IncodingMetaCallbackFuncDsl(this.plugIn); } }

        public IncodingMetaCallbackStoreApiDsl Store { get { return new IncodingMetaCallbackStoreApiDsl(this.plugIn); } }

        public IncodingMetaCallbackTriggerDsl Trigger { get { return new IncodingMetaCallbackTriggerDsl(this.plugIn); } }

        public IncodingMetaCallbackFormApiDsl Form { get { return new IncodingMetaCallbackFormApiDsl(this.plugIn); } }

        #endregion

        #region Api Methods

        [Obsolete("Will be remove on next version then use method Call instead")]
        public IExecutableSetting Eval(string code)
        {
            return this.plugIn.Registry(new ExecutableEval(code));
        }

        public IExecutableSetting Call(string funcName, params object[] args)
        {
            return this.plugIn.Registry(new ExecutableEvalMethod(funcName, args, string.Empty));
        }

        #endregion
    }
}