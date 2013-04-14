namespace Incoding.MvcContrib
{
    public class IncodingMetaCallbackJqueryFuncDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugInDsl;

        #endregion

        #region Constructors

        public IncodingMetaCallbackJqueryFuncDsl(IIncodingMetaLanguagePlugInDsl plugInDsl)
        {
            this.plugInDsl = plugInDsl;
        }

        #endregion

        #region Api Methods

        public IExecutableSetting IncrementVal()
        {
            return this.plugInDsl.Registry(new ExecutableEval(JavaScriptCodeTemplate.Target_Increment));
        }

        public IExecutableSetting DecrementVal()
        {
            return this.plugInDsl.Registry(new ExecutableEval(JavaScriptCodeTemplate.Target_Decrement));
        }

        #endregion
    }
}