namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    // ReSharper disable UnusedMember.Global
    [Flags]
    public enum HtmlAttribute : long
    {
        /// <summary>
        ///     Specifies an alternate text for images (only for type="image")
        /// </summary>
        Alt = 1, 

        /// <summary>
        ///     Specifies the types of files that the server accepts (only for type="file")
        /// </summary>
        Accept = 2, 

        /// <summary>
        ///     Specifies a unique id for an element
        /// </summary>
        Id = 4, 

        /// <summary>
        ///     Specifies one or more class names for an element (refers to a class in a style sheet)
        /// </summary>
        Class = 8, 

        /// <summary>
        ///     Specifies whether the content of an element is editable or not
        /// </summary>
        ContentEditable = 16, 

        /// <summary>
        ///     Specifies a context menu for an element. The context menu appears when a user right-clicks on the element
        /// </summary>
        ContextMenu = 32, 

        /// <summary>
        ///     Specifies whether an element is drag gable or not
        /// </summary>
        DragGable = 64, 

        /// <summary>
        ///     Specifies whether the dragged data is copied, moved, or linked, when dropped
        /// </summary>
        DropZone = 128, 

        /// <summary>
        ///     Specifies that an element is not yet, or is no longer, relevant
        /// </summary>
        Hidden = 256, 

        /// <summary>
        ///     Specifies the language of the element's content
        /// </summary>
        Lang = 512, 

        /// <summary>
        ///     Specifies whether the element is to have its spelling and grammar checked or not
        /// </summary>
        SpellCheck = 1024, 

        /// <summary>
        ///     Specifies whether an element's value are to be translated when the page is localized, or not.
        /// </summary>
        Translate = 2048, 

        /// <summary>
        ///     Specifies the type of input element
        /// </summary>
        Type = 4096, 

        Name = 8192, 

        /// <summary>
        ///     The href attribute specifies the URL of the page the link goes to.
        /// </summary>
        Href = 16384, 

        /// <summary>
        ///     The rel attribute specifies the relationship between the current document and the linked document.
        /// </summary>
        Rel = 32768, 

        /// <summary>
        ///     Specifies the URL of the image to use as a submit button (only for type="image")
        /// </summary>
        Src = 65536, 

        /// <summary>
        ///     Specifies the tabbing order of an element
        /// </summary>
        TabIndex = 262144, 

        /// <summary>
        ///     Specifies a shortcut key to activate/focus an element
        /// </summary>
        AccessKey = 524288, 

        /// <summary>
        ///     Specifies extra information about an element
        /// </summary>
        Title = 1048576, 

        /// <summary>
        ///     Specifies the text direction for the content in an element
        /// </summary>
        Dir = 2097152, 

        /// <summary>
        ///     The disabled attribute specifies that an input element should be disabled.
        /// </summary>
        Disabled = 4194304, 

        /// <summary>
        ///     Specifies the maximum number of characters allowed in an input element
        /// </summary>
        MaxLength = 8388608, 

        /// <summary>
        ///     Specifies that an input field should be read-only
        /// </summary>
        Readonly = 16777216, 

        /// <summary>
        ///     Specifies that an input element should be preselected when the page loads (for type="checkbox" or type="radio")
        /// </summary>
        Checked = 33554432, 

        /// <summary>
        ///     Specifies the width, in characters, of an input element
        /// </summary>
        Size = 67108864, 

        /// <summary>
        ///     Specifies that multiple options can be selected at once
        /// </summary>
        Multiple = 134217728, 

        /// <summary>
        ///     The value attribute specifies the value of an input element.
        /// </summary>
        Value = 268435456, 

        /// <summary>
        ///     The placeholder attribute specifies a short hint that describes the expected value of an input field (e.g. a sample value or a short description of the expected format).
        /// </summary>
        Placeholder = 536870912, 

        /// <summary>
        ///     The autocomplete attribute specifies whether a form or input field should have autocomplete on or off.
        /// </summary>
        AutoComplete = 1073741824, 

        /// <summary>
        ///     The cols attribute specifies the visible width of a text area.
        /// </summary>
        Cols = 2147483648, 

        /// <summary>
        ///     The rows property sets or returns the height (in rows) of a text area.
        /// </summary>
        Rows = 4294967296, 

        /// <summary>
        ///     Specifies an inline CSS style for an element
        /// </summary>
        Style = 8589934592, 

        /// <summary>
        ///     Specifies that an input element should automatically get focus when the page loads
        /// </summary>
        AutoFocus = 17179869184, 

        /// <summary>
        ///     Specifies the URL of the file that will process the input control when the form is submitted (for type="submit" and type="image")
        /// </summary>
        Action = 34359738368, 

        /// <summary>
        ///     Specifies how the form-data should be encoded when submitting it to the server (for type="submit" and type="image")
        /// </summary>
        Enctype = 68719476736, 

        /// <summary>
        ///     Defines the HTTP method for sending data to the action URL (for type="submit" and type="image")
        /// </summary>
        Method = 137438953472, 

        /// <summary>
        ///     Defines that form elements should not be validated when submitted
        /// </summary>
        NoValidate = 274877906944, 

        /// <summary>
        ///     Specifies where to display the response that is received after submitting the form (for type="submit" and type="image")
        /// </summary>
        Target = 549755813888, 

        /// <summary>
        ///     Specifies the height of an input element (only for type="image")
        /// </summary>
        Height = 1099511627776, 

        /// <summary>
        ///     The hreflang attribute specifies the language of the linked document.
        /// </summary>
        HrefLang = 2199023255552, 
    }

    // ReSharper restore UnusedMember.Global
}