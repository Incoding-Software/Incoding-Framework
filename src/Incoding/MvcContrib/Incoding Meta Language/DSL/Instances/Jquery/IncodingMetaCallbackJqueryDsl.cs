namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

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

        public IncodingMetaCallbackJqueryAttributesDsl Attributes { get { return new IncodingMetaCallbackJqueryAttributesDsl(this.plugInDsl); } }

        public IncodingMetaCallbackJqueryCssDsl Css { get { return new IncodingMetaCallbackJqueryCssDsl(this.plugInDsl); } }

        public IncodingMetaCallbackJqueryManipulationDsl Manipulation { get { return new IncodingMetaCallbackJqueryManipulationDsl(this.plugInDsl); } }

        #endregion

        #region Api Methods

        public IExecutableSetting PlugIn(string name, object options = null)
        {
            return this.plugInDsl.Registry(new ExecutableEval(JavaScriptCodeTemplate.Target_Jquery_Plug_In.F(name, options.ReturnOrDefault(r => r.ToJsonString(), string.Empty))));
        }

        public IExecutableSetting Call(string name, params object[] args)
        {
            return this.plugInDsl.Registry(new ExecutableEvalMethod(name, args, "$(this.target)"));
        }

        #endregion
    }
}