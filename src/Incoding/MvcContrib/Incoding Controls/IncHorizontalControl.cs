namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Extensions;

    #endregion

    public class IncHorizontalControl<TInput> : IncControlBase where TInput : IncControlBase
    {
        #region Constructors

        public IncHorizontalControl(IncLabelControl label, TInput input, IncControlBase validation)
        {
            Label = label;
            Input = input;
            Validation = validation;
            HelpBlock = new IncHelpBlockControl();
            AddClass("control-group");
        }

        #endregion

        #region Properties

        public IncLabelControl Label { get; set; }

        public TInput Input { get; set; }

        public IncControlBase Validation { get; set; }

        public IncHelpBlockControl HelpBlock { get; set; }

        #endregion

        public override MvcHtmlString Render()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Label.Render());

            var controlContainer = IncodingHtmlHelper.CreateTag(HtmlTag.Div, Input.Render(), new RouteValueDictionary(new { @class = "controls" }));
            stringBuilder.Append(controlContainer);

            stringBuilder.Append(Validation.Render());
            stringBuilder.Append(HelpBlock.Render());

            var controlGroup = IncodingHtmlHelper.CreateTag(HtmlTag.Div, new MvcHtmlString(stringBuilder.ToString()), AnonymousHelper.ToDictionary(this.attributes));
            return new MvcHtmlString(controlGroup.ToString());
        }
    }
}