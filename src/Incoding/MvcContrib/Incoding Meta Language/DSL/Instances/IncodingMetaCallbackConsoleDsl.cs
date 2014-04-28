namespace Incoding.MvcContrib
{
    public class IncodingMetaCallbackConsoleDsl
    {
        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        public IncodingMetaCallbackConsoleDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        public IExecutableSetting Log(string logType, Selector message)
        {
            return this.plugIn.Registry(new ExecutableEvalMethod("log", new object[] { logType, message }, "window.console"));
        }
    }
}