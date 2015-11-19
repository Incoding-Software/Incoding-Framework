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
            Label.AddClass(IncodingHtmlHelper.BootstrapVersion == BootstrapOfVersion.v3 ? "control-label " : "control-label");
            Label.AddClass(GetOffsetAsClass(LabelOffset.GetValueOrDefault(IncodingHtmlHelper.Def_Label_Offset)));
            Input = input;
            if (IncodingHtmlHelper.BootstrapVersion == BootstrapOfVersion.v3)
                Input.AddClass("form-control");
            Validation = validation;
            HelpBlock = new IncHelpBlockControl();
            AddClass(IncodingHtmlHelper.BootstrapVersion == BootstrapOfVersion.v3 ? "form-group" : "control-group");
            if (GroupOffset.HasValue)
                AddClass(GetOffsetAsClass(GroupOffset.Value));
        }

        #endregion

        #region Properties

        public Offset? GroupOffset { get; set; }

        public Offset? LabelOffset { get; set; }

        public Offset? InputOffset { get; set; }

        public IncLabelControl Label { get; set; }

        public TInput Input { get; set; }

        public IncControlBase Validation { get; set; }

        public IncHelpBlockControl HelpBlock { get; set; }

        #endregion

        string GetOffsetAsClass(Offset set)
        {
            return set.ToString().Replace("_", "-").ToLower();
        }

        public override MvcHtmlString ToHtmlString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Label.ToHtmlString());

            var controlContainer = IncodingHtmlHelper.CreateTag(HtmlTag.Div, Input.ToHtmlString(), new RouteValueDictionary(new
                                                                                                                            {
                                                                                                                                    @class = IncodingHtmlHelper.BootstrapVersion == BootstrapOfVersion.v3
                                                                                                                                                     ? GetOffsetAsClass(InputOffset.GetValueOrDefault(IncodingHtmlHelper.Def_Input_Offset))
                                                                                                                                                     : "controls"
                                                                                                                            }));
            stringBuilder.Append(controlContainer);

            stringBuilder.Append(Validation.ToHtmlString());
            stringBuilder.Append(HelpBlock.ToHtmlString());

            var controlGroup = IncodingHtmlHelper.CreateTag(HtmlTag.Div, new MvcHtmlString(stringBuilder.ToString()), AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(controlGroup.ToString());
        }
    }
}