namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class IncodingMetaCallbackDocumentDsl
    {
        #region Fields

        readonly IIncodingMetaLanguagePlugInDsl plugIn;

        #endregion

        #region Constructors

        public IncodingMetaCallbackDocumentDsl(IIncodingMetaLanguagePlugInDsl plugIn)
        {
            this.plugIn = plugIn;
        }

        #endregion

        #region Api Methods

        public IExecutableSetting HistoryGo(int countGo)
        {
            return this.plugIn.Registry(new ExecutableEval(JavaScriptCodeTemplate.Document_HistoryGo.F(countGo)));
        }

        public IExecutableSetting Back()
        {
            return HistoryGo(-1);
        }

        public IExecutableSetting Forward()
        {
            return HistoryGo(1);
        }

        public IExecutableSetting RedirectTo(string url)
        {
            return this.plugIn.Registry(new ExecutableRedirect(url));
        }

        public IExecutableSetting RedirectToSelf()
        {
            return RedirectTo(string.Empty);
        }

        public IExecutableSetting Reload(bool force = false)
        {
            return this.plugIn.Registry(new ExecutableEval(JavaScriptCodeTemplate.Window_Location_Reload.F(force.ToString().ToLower())));
        }

        public IExecutableSetting Title(string title)
        {
            return this.plugIn.Registry(new ExecutableEval(JavaScriptCodeTemplate.Document_SetTitle.F(title)));
        }

        #endregion
    }
}