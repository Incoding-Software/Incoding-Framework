namespace Incoding.Data
{
    #region << Using >>

    using System.Collections.Generic;

    #endregion

    public class IncPaginatedResult<TItem>
    {
        #region Fields

        readonly List<TItem> items;

        readonly int totalCount;

        #endregion

        #region Constructors

        public IncPaginatedResult(List<TItem> items, int totalCount)
        {
            this.items = items;
            this.totalCount = totalCount;
        }

        #endregion

        #region Properties

        public List<TItem> Items
        {
            get { return this.items; }
        }

        public int TotalCount
        {
            get { return this.totalCount; }
        }

        #endregion
    }
}