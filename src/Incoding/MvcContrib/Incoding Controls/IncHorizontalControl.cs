namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Text;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using Incoding.Maybe;

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
            Control = new IncDivControl();
        }

        #endregion

        #region Properties

        public IncLabelControl Label { get; set; }

        public TInput Input { get; set; }

        public IncDivControl Control { get; set; }

        public IncControlBase Validation { get; set; }

        public IncHelpBlockControl HelpBlock { get; set; }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            Func<IncControlBase, bool> isForDefClass = @base => !@base.GetAttr(HtmlAttribute.Class).With(r => r.Contains("col-"));
            bool isV3orMore = IncodingHtmlHelper.BootstrapVersion == BootstrapOfVersion.v3;
            bool isStatic = Input.GetType().Name.Contains("IncStaticControl");

            
            AddClass(isV3orMore ? B.Form_group.ToLocalization() : "control-group");

            Label.AddClass(B.Control_label);
            if (isV3orMore && isForDefClass(Label))
                Label.AddClass(IncodingHtmlHelper.Def_Label_Class.ToLocalization());

            if(!isV3orMore)
                Control.AddClass("controls");

            if (string.IsNullOrWhiteSpace(Control.GetAttr(HtmlAttribute.Class)))
            {
                Control.AddClass(isV3orMore
                                         ? IncodingHtmlHelper.Def_Control_Class.ToLocalization()
                                         : isStatic ? string.Empty : "control-group");
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Label.ToHtmlString());

            if (isV3orMore)
                Input.AddClass(isStatic ? B.Form_static_control.ToLocalization() : B.Form_control.ToLocalization());
            Control.Content = Input.ToHtmlString();

            stringBuilder.Append(Control.ToHtmlString());
            stringBuilder.Append(Validation.ToHtmlString());
            stringBuilder.Append(HelpBlock.ToHtmlString());

            var controlGroup = IncodingHtmlHelper.CreateTag(HtmlTag.Div, new MvcHtmlString(stringBuilder.ToString()), GetAttributes());
            return new MvcHtmlString(controlGroup.ToString());
        }
    }
}