namespace Incoding.Data
{
    #region << Using >>

    

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
    }
}