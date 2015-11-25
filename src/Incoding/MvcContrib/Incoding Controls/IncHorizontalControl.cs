namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;
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
            Func<IncControlBase, bool> isForDefClass = @base => !@base.GetAttr(HtmlAttribute.Class).With(r => r.Contains("col-"));
            bool isV3orMore = IncodingHtmlHelper.BootstrapVersion == BootstrapOfVersion.v3;
            bool isStatic = Input.GetType().Name.Contains("IncStaticControl");

            Label.AddClass(B.Control_label);
            if (isV3orMore && isForDefClass(Label))
                Label.AddClass(IncodingHtmlHelper.Def_Label_Class.AsClass());

            AddClass(isV3orMore
                             ? "form-group"
                             : isStatic ? string.Empty : "control-group");
            if (isV3orMore && isForDefClass(this))
                AddClass(IncodingHtmlHelper.Def_Group_Class.AsClass());

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Label.ToHtmlString());
            var containerForControl = new RouteValueDictionary(new
                                                               {
                                                                       @class = isV3orMore
                                                                                        ? isForDefClass(Input) ? IncodingHtmlHelper.Def_Input_Class.AsClass() : Input.GetAttr(HtmlAttribute.Class)
                                                                                        : "controls"
                                                               });
            if (isV3orMore)
                Input.AddClass(isStatic ? B.Form_static_control.AsClass() : B.Form_control.AsClass());
            var controlContainer = IncodingHtmlHelper.CreateTag(HtmlTag.Div, Input.ToHtmlString(), containerForControl);

            stringBuilder.Append(controlContainer);

            stringBuilder.Append(Validation.ToHtmlString());
            stringBuilder.Append(HelpBlock.ToHtmlString());

            var controlGroup = IncodingHtmlHelper.CreateTag(HtmlTag.Div, new MvcHtmlString(stringBuilder.ToString()), AnonymousHelper.ToDictionary(attributes));
            return new MvcHtmlString(controlGroup.ToString());
        }
    }
}