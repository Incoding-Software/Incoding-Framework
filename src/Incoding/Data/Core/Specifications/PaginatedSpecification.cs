namespace Incoding.Data
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class PaginatedSpecification
    {
        #region Constructors

        public PaginatedSpecification(int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        #endregion

        #region Properties

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            return Equals(obj as PaginatedSpecification);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (CurrentPage * 397) ^ PageSize;
            }
        }

        protected bool Equals(PaginatedSpecification other)
        {
            return this.IsReferenceEquals(other) && CurrentPage == other.CurrentPage && PageSize == other.PageSize;
        }

        #endregion
    }
}