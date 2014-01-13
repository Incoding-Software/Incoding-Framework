namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.ComponentModel;

    #endregion

    // ReSharper disable UnusedMember.Global
    public enum Display
    {
        /// <summary>
        ///     Default. Displays an element as an inline element
        /// </summary>
        Inline, 

        /// <summary>
        ///     Displays an element as a block element
        /// </summary>
        Block, 

        /// <summary>
        ///     The element will not be displayed at all (has no effect on layout)
        /// </summary>
        None, 

        /// <summary>
        ///     The value of the display property will be inherited from the parent element
        /// </summary>
        Inherit, 

        /// <summary>
        ///     Displays an element as an inline-level block container. The inside of this block is formatted as block-level box, and the element itself is formatted as an inline-level box
        /// </summary>
        [Description("inline-block")]
        InlineBlock, 

        /// <summary>
        ///     The element is displayed as an inline-level table
        /// </summary>
        [Description("inline-table")]
        InlineTable, 

        /// <summary>
        ///     Let the element behave like a li element
        /// </summary>
        [Description("list-item")]
        ListItem, 

        /// <summary>
        ///     Displays an element as either block or inline, depending on context
        /// </summary>
        [Description("run-in")]
        RunIn, 

        /// <summary>
        ///     Let the element behave like a table element
        /// </summary>
        Table, 

        /// <summary>
        ///     Let the element behave like a caption element
        /// </summary>
        [Description("table-caption")]
        TableCaption, 

        /// <summary>
        ///     Let the element behave like a colgroup element
        /// </summary>
        [Description("table-column-group")]
        TableColumnGroup, 

        /// <summary>
        ///     Let the element behave like a thead element
        /// </summary>
        [Description("table-header-group")]
        TableHeaderGroup, 

        /// <summary>
        ///     Let the element behave like a tfoot element
        /// </summary>
        [Description("table-footer-group")]
        TableFooterGroup, 

        /// <summary>
        ///     Let the element behave like a tbody element
        /// </summary>
        [Description("table-row-group")]
        TableRowGroup, 

        /// <summary>
        ///     Let the element behave like a td element
        /// </summary>
        [Description("table-cell")]
        TableCell, 

        /// <summary>
        ///     Let the element behave like a col element
        /// </summary>
        [Description("table-column")]
        TableColumn, 

        /// <summary>
        ///     Let the element behave like a tr element
        /// </summary>
        [Description("table-row")]
        TableRow, 
    }
}