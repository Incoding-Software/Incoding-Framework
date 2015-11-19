namespace Incoding.MvcContrib
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Bootstrap classes
    /// </summary>
    [Flags]
    public enum B
    {
        /// <summary>
        ///     Bootstrap requires a containing element to wrap site contents and house our grid system. You may choose one of two
        ///     containers to use in your projects. Note that, due to padding and more, neither container is nestable.
        ///     Use .container for a responsive fixed width container.
        /// </summary>
        Container = 1,

        /// <summary>
        ///     Use .container-fluid for a full width container, spanning the entire width of your viewport.
        /// </summary>
        [Description("container-fluid")]
        ContainerFluid = 2,

        /// <summary>
        ///     Active state Buttons will appear pressed (with a darker background, darker border, and inset shadow) when active.
        ///     For      button   elements, this is done via :active.
        ///     For a elements, it's done with .active. However, you may use .active on buttons (and include the
        ///     aria-pressed="true" attribute) should you need to replicate the active state programmatically.
        /// </summary>
        Active = 4,

        /// <summary>
        ///     Make buttons look unclickable by fading them back with opacity.
        /// </summary>
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
        Hidden = 32,

        /// <summary>
        /// Use Bootstrap's predefined grid classes to align labels and groups of form controls in a horizontal layout by adding .form-horizontal to the form (which doesn't have to be a <form>). Doing so changes .form-groups to behave as grid rows, so no need for .row.
        /// </summary>
        [Description("form-horizontal")]
        FormHorizontal = 64
    }
}