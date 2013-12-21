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
            Label.AddClass("control-label");
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

        public override MvcHtmlString ToHtmlString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Label.ToHtmlString());

            var controlContainer = IncodingHtmlHelper.CreateTag(HtmlTag.Div, Input.ToHtmlString(), new RouteValueDictionary(new { @class = "controls" }));
            stringBuilder.Append(controlContainer);

            stringBuilder.Append(Validation.ToHtmlString());
            stringBuilder.Append(HelpBlock.ToHtmlString());

            var controlGroup = IncodingHtmlHelper.CreateTag(HtmlTag.Div, new MvcHtmlString(stringBuilder.ToString()), AnonymousHelper.ToDictionary(this.attributes));
            return new MvcHtmlString(controlGroup.ToString());
        }
    }
}