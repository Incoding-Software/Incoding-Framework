namespace Incoding.MvcContrib
{
    public class IncodingMetaCallbackFuncDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugInDsl;

        #endregion

        #region Constructors

        public IncodingMetaCallbackFuncDsl(IIncodingMetaLanguagePlugInDsl plugInDsl)
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