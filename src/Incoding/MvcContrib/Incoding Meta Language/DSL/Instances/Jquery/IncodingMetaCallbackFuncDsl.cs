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

        public IExecutableSetting IncrementVal(Selector step)
        {
            return this.plugInDsl.Core().JQuery.Call("increment", step);
        }

        public IExecutableSetting IncrementVal()
        {
            return IncrementVal(1);
        }

        public IExecutableSetting DecrementVal()
        {
            return IncrementVal(-1);
        }

        #endregion
    }
}