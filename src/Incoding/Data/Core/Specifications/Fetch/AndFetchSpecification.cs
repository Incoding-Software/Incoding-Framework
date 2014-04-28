namespace Incoding.Data
{
    #region << Using >>

    using System;

    #endregion

    public class AndFetchSpecification<TEntity> : FetchSpecification<TEntity>
    {
        #region Fields

        readonly FetchSpecification<TEntity> first;

        readonly FetchSpecification<TEntity> second;

        #endregion

        #region Constructors

        public AndFetchSpecification(FetchSpecification<TEntity> first, FetchSpecification<TEntity> second)
        {
            this.first = first;
            this.second = second;
        }

        #endregion

        public override Action<AdHocFetchSpecificationBase<TEntity>> FetchedBy()
        {
            return r =>
                       {
                           this.first.FetchedBy()(r);
                           this.second.FetchedBy()(r);
                       };
        }
    }
}