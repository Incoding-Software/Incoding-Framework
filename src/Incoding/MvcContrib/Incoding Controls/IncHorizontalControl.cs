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

        public override MvcHtmlString ToHtmlString()
        {
            bool isV3orMore = IncodingHtmlHelper.BootstrapVersion == BootstrapOfVersion.v3;
            bool isStatic = Input.GetType().Name.Contains("IncStaticControl");

            Label.AddClass("control-label");
            if (isV3orMore)
                Label.AddClass(IncodingHtmlHelper.Def_Label_Class.AsClass());
            if (isV3orMore)
                Input.AddClass(isStatic ? B.Form_static_control.AsClass() : B.Form_control.AsClass());

            AddClass(isV3orMore
                             ? "form-group"
                             : isStatic ? string.Empty : "control-group");
            if (isV3orMore)
                AddClass(IncodingHtmlHelper.Def_Group_Class.AsClass());

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Label.ToHtmlString());

            var controlContainer = IncodingHtmlHelper.CreateTag(HtmlTag.Div, Input.ToHtmlString(), new RouteValueDictionary(new
                                                                                                                            {
                                                                                                                                    @class = isV3orMore
                                                                                                                                                     ? IncodingHtmlHelper.Def_Input_Class.AsClass()
                                                                                                                                                     : "controls"
                                                                                                                            }));
            stringBuilder.Append(controlContainer);

            stringBuilder.Append(Validation.ToHtmlString());
            stringBuilder.Append(HelpBlock.ToHtmlString());

            var controlGroup = IncodingHtmlHelper.CreateTag(HtmlTag.Div, new MvcHtmlString(stringBuilder.ToString()), AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(controlGroup.ToString());
        }

        #region Properties

        public IncLabelControl Label { get; set; }

        public TInput Input { get; set; }

        public IncControlBase Validation { get; set; }

        public IncHelpBlockControl HelpBlock { get; set; }

        #endregion
    }
}