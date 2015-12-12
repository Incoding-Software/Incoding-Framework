namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.ComponentModel;
    using System.Linq;
    using Incoding.Extensions;

    #endregion

    public static class BExtensions
    {
        #region Static Fields

        static readonly B[] all = Enum.GetValues(typeof(B))
                                      .Cast<B>()
                                      .ToArray();

        #endregion

        #region Factory constructors

        public static string AsClass(this B b)
        {
            return all.Where(r => b.HasFlag(r))
                      .Select(r => r.ToLocalization())
                      .AsString(" ");
        }

        #endregion
    }

    /// <summary>
    ///     Bootstrap classes
    /// </summary>
    [Flags]
    public enum B : long
    {
        /// <summary>
        ///     Bootstrap requires a containing element to wrap site contents and house our grid system. You may choose one of two
        ///     containers to use in your projects. Note that, due to padding and more, neither container is nestable.
        ///     Use .container for a responsive fixed width container.
        /// </summary>
        [Description("container")]
        Container = 1,

        /// <summary>
        ///     Use .container-fluid for a full width container, spanning the entire width of your viewport.
        /// </summary>
        [Description("container-fluid")]
        Container_fluid = 2,

        /// <summary>
        ///     Active state Buttons will appear pressed (with a darker background, darker border, and inset shadow) when active.
        ///     For      button   elements, this is done via :active.
        ///     For a elements, it's done with .active. However, you may use .active on buttons (and include the
        ///     aria-pressed="true" attribute) should you need to replicate the active state programmatically.
        /// </summary>
        [Description("active")]
        Active = 4,

        /// <summary>
        ///     Make buttons look unclickable by fading them back with opacity.
        /// </summary>
        [Description("disabled")]
        Disabled = 8,

        /// <summary>
        ///     Force an element to be shown or hidden (including for screen readers) with the use of .show and .hidden classes.
        ///     These classes use !important to avoid specificity conflicts, just like the quick floats. They are only available
        ///     for block level toggling. They can also be used as mixins.
        ///     .hide is available, but it does not always affect screen readers and is deprecated as of v3.0.1. Use .hidden or
        ///     .sr-only instead.
        ///     Furthermore, .invisible can be used to toggle only the visibility of an element, meaning its display is not
        ///     modified and the element can still affect the flow of the document.
        /// </summary>
        [Description("hide"), Obsolete("Please use B.Hidden")]
        Hide = 16,

        /// <summary>
        ///     Force an element to be shown or hidden (including for screen readers) with the use of .show and .hidden classes.
        ///     These classes use !important to avoid specificity conflicts, just like the quick floats. They are only available
        ///     for block level toggling. They can also be used as mixins.
        ///     .hide is available, but it does not always affect screen readers and is deprecated as of v3.0.1. Use .hidden or
        ///     .sr-only instead.
        ///     Furthermore, .invisible can be used to toggle only the visibility of an element, meaning its display is not
        ///     modified and the element can still affect the flow of the document.
        /// </summary>
        [Description("hidden"),]
        Hidden = 32,

        /// <summary>
        ///     Use Bootstrap's predefined grid classes to align labels and groups of form controls in a horizontal layout by
        ///     adding .form-horizontal to the form (which doesn't have to be a
        ///     <form>). Doing so changes .form-groups to behave as grid rows, so no need for .row.
        /// </summary>
        [Description("form-horizontal")]
        Form_horizontal = 64,

        [Description("form-group")]
        Form_group = 128,

        [Description("col-xs-1")]
        Col_xs_1 = 256,

        [Description("col-xs-2")]
        Col_xs_2 = 512,

        [Description("col-xs-3")]
        Col_xs_3 = 1024,

        [Description("col-xs-4")]
        Col_xs_4 = 2048,

        [Description("col-xs-5")]
        Col_xs_5 = 4096,

        [Description("col-xs-6")]
        Col_xs_6 = 8912,

        [Description("col-xs-7")]
        Col_xs_7 = 16384,

        [Description("col-xs-8")]
        Col_xs_8 = 32768,

        [Description("col-xs-9")]
        Col_xs_9 = 65536,

        [Description("col-xs-11")]
        Col_xs_11 = 142592,

        [Description("col-xs-12")]
        Col_xs_12 = 131072,

        [Description("form-control")]
        Form_control = 262144,

        [Description("form-control-static")]
        Form_static_control = 524288,

        [Description("control-label")]
        Control_label = 1048576,

        [Description("col-xs-offset-1")]
        Col_xs_offset_1 = 2097152,

        [Description("col-xs-offset-2")]
        Col_xs_offset_2 = 4194304,

        [Description("col-xs-offset-3")]
        Col_xs_offset_3 = 8388608,

        [Description("col-xs-offset-4")]
        Col_xs_offset_4 = 16777216,

        [Description("col-xs-offset-5")]
        Col_xs_offset_5 = 33554432,

        [Description("col-xs-offset-6")]
        Col_xs_offset_6 = 67108864,

        [Description("col-xs-offset-7")]
        Col_xs_offset_7 = 134217728,

        [Description("col-xs-offset-8")]
        Col_xs_offset_8 = 268435456,

        [Description("col-xs-offset-9")]
        Col_xs_offset_9 = 536870912,

        [Description("col-xs-offset-11")]
        Col_xs_offset_11 = 1073741824,

        [Description("col-xs-offset-12")]
        Col_xs_offset_12 = 2147483648,

        [Description("checkbox")]
        Checkbox = 4294967296,

        [Description("checkbox-inline")]
        Checkbox_inline = 8589934592,

        [Description("radio")]
        Radio = 17179869184,

        [Description("radio-inline")]
        Radio_inline = 34359738368,

        [Description("col-xs-10")]
        Col_xs_10 = 68719476736,

        
    }
}