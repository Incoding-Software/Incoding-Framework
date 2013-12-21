namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    // ReSharper disable UnusedMember.Global
    [Flags]
    public enum JqueryExpression
    {
        /// <summary>
        ///     Select all elements that are in the progress of an animation at the time the selector is run.
        /// </summary>
        Animated = 1, 

        /// <summary>
        ///     Matches all elements that are checked.
        /// </summary>
        Checked = 2, 

        /// <summary>
        ///     Selects all elements that are disabled.
        /// </summary>
        Disabled = 4, 

        /// <summary>
        ///     Select all elements that have no children (including text nodes).
        /// </summary>
        Empty = 8, 

        /// <summary>
        ///     Select all input elements
        /// </summary>
        Input = 16, 

        /// <summary>
        ///     Select all input elements with type="radio"
        /// </summary>
        Radio = 32, 

        /// <summary>
        ///     Select all input elements with type="checkbox"
        /// </summary>
        Checkbox = 64, 

        /// <summary>
        ///     Select all input elements with type="reset"
        /// </summary>
        Reset = 128, 

        /// <summary>
        ///     Select all input elements with type="submit"
        /// </summary>
        Submit = 256, 

        /// <summary>
        ///     Select all input elements with type="button"
        /// </summary>
        Button = 512, 

        /// <summary>
        ///     Select all input elements with type="image"
        /// </summary>
        Image = 1024, 

        /// <summary>
        ///     Select all input elements with type="file"
        /// </summary>
        File = 2048, 

        /// <summary>
        ///     Select all input elements with type="text"
        /// </summary>
        Text = 4096, 

        /// <summary>
        ///     Select all input elements with type="password"
        /// </summary>
        Password = 8192, 

        /// <summary>
        ///     Selects all elements that are enabled.
        /// </summary>
        Enabled = 16384, 

        /// <summary>
        ///     Selects even elements, zero-indexed. See also odd.
        /// </summary>
        Even = 32768, 

        /// <summary>
        ///     Selects the first matched element.
        /// </summary>
        First = 65536, 

        /// <summary>
        ///     Selects element if it is currently focused.
        /// </summary>
        Focus = 131072, 

        /// <summary>
        ///     Selects all elements that are headers, like h1, h2, h3 and so on.
        /// </summary>
        Header = 262144, 

        /// <summary>
        ///     Selects all elements that are hidden.
        /// </summary>
        Hidden = 524288, 

        /// <summary>
        ///     Selects the last matched element.
        /// </summary>
        Last = 1048576, 

        /// <summary>
        ///     Selects odd elements, zero-indexed
        /// </summary>
        Odd = 2097152, 

        /// <summary>
        ///     Select all elements that are the parent of another element, including text nodes.
        /// </summary>
        Parent = 4194304, 

        /// <summary>
        ///     Selects all elements that are selected.
        /// </summary>
        Selected = 8388608, 

        /// <summary>
        ///     Selects all elements that are visible.
        /// </summary>
        Visible = 16777216, 

        /// <summary>
        ///     Select first child of their parent
        /// </summary>
        First_Child = 33554432, 

        /// <summary>
        ///     Select last child of their parent
        /// </summary>
        Last_Child = 67108864, 

        /// <summary>
        ///     Select  only child of their parent
        /// </summary>
        Only_Child = 134217728, 
    }

    // ReSharper restore UnusedMember.Global
}