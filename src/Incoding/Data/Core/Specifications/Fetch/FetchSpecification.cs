namespace Incoding.Data
{
    #region << Using >>

    using System;
    using Incoding.Extensions;

    #endregion

    public abstract class FetchSpecification<TEntity>
    {
        #region Api Methods
        
        public abstract Action<AdHocFetchSpecificationBase<TEntity>> FetchedBy();

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            return Equals(obj as FetchSpecification<TEntity>);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        protected bool Equals(FetchSpecification<TEntity> other)
        {
            ////ncrunch: no coverage start
            if (!this.IsReferenceEquals(other))
                return false;

            ////ncrunch: no coverage end
            /// 
            var fetchLeft = new AdHocFetchNhibernateSpecification<TEntity>();
            FetchedBy()(fetchLeft);

            var fetchRight = new AdHocFetchNhibernateSpecification<TEntity>();
            other.FetchedBy()(fetchRight);

            return fetchLeft.Equals(fetchRight);
        }

        #endregion
    }
}