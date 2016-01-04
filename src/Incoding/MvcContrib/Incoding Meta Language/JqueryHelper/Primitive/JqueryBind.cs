namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    // ReSharper disable UnusedMember.Global
    /// <summary>
    ///     All support event jquery. Name bind should be in lower cases.
    /// </summary>
    [Flags]
    public enum JqueryBind : long
    {
        None = 1,

        Click = 2,

        Change = 4,

        DblClick = 8,

        Blur = 16,

        Focus = 32,

        FocusIn = 64,

        FocusOut = 128,

        Select = 256,

        Submit = 512,

        KeyPress = 1024,

        KeyDown = 2048,

        KeyUp = 4096,

        Scroll = 8192,

        MouseDown = 16384,

        MouseOver = 32768,

        MouseMove = 65536,

        MouseUp = 131072,

        MouseOut = 262144,

        MouseEnter = 524288,

        MouseLeave = 1048576,

        Load = 2097152,

        ReSize = 4194304,

        UnLoad = 8388608,

        Error = 16777216,

        /// <summary>
        ///     Universal bind
        /// </summary>
        Incoding = 33554432,

        /// <summary>
        ///     First parse element
        /// </summary>
        InitIncoding = 67108864,

        /// <summary>
        ///     Change url.
        /// </summary>
        IncChangeUrl = 134217728,

        /// <summary>
        ///     A pre-request callback
        /// </summary>
        IncAjaxBefore = 268435456,

        /// <summary>
        ///     A function to be called if the request fails.
        ///     Are "timeout", "error", "abort", and "parsererror".
        /// </summary>
        IncAjaxError = 536870912,

        /// <summary>
        ///     A function to be called when the request finishes (after error callback are executed).
        /// </summary>
        IncAjaxComplete = 1073741824,

        /// <summary>
        ///     A function to be called when the request finishes (after error callback are executed).
        /// </summary>
        IncAjaxSuccess = 2147483648,

        IncInsert = 4294967296,

        IncGlobalError = 4294967296 * 2,

        IncError = 8589934592 * 2
    }

    // ReSharper restore UnusedMember.Global
}