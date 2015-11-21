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
            bool isV3orMore = IncodingHtmlHelper.BootstrapVersion == BootstrapOfVersion.v3;
            bool isStatic = Input.GetType().Name.Contains("IncStaticControl");

            Label.AddClass("control-label");
            if (isV3orMore)
                Label.AddClass(GetOffsetAsClass(LabelOffset.GetValueOrDefault(IncodingHtmlHelper.Def_Label_Offset)));
            if (isV3orMore)
                Input.AddClass(isStatic ? "form-control-static" : "form-control");

            AddClass(isV3orMore
                             ? "form-group"
                             : isStatic ? string.Empty : "control-group");
            if (GroupOffset.HasValue)
                AddClass(GetOffsetAsClass(GroupOffset.Value));

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Label.ToHtmlString());

            var controlContainer = IncodingHtmlHelper.CreateTag(HtmlTag.Div, Input.ToHtmlString(), new RouteValueDictionary(new
                                                                                                                            {
                                                                                                                                    @class = isV3orMore
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