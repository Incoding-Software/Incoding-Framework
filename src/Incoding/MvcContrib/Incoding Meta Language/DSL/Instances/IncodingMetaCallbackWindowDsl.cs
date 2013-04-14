namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class IncodingMetaCallbackWindowDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaCallbackWindowDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Api Methods

        public IExecutableSetting Alert(string message)
        {
            return this.plugIn.Registry(new ExecutableEval(JavaScriptCodeTemplate.Window_Alert.F(message)));
        }

        public IExecutableSetting ClearInterval(string intervalId)
        {
            return this.plugIn.Registry(new ExecutableEval(JavaScriptCodeTemplate.Window_Clear_Interval.F(intervalId)));
        }

        #endregion
    }
}