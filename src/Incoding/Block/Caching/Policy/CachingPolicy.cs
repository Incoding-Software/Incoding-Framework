namespace Incoding.Block.Caching
{
    #region << Using >>

    using Incoding.Block.Core;

    #endregion

    public class CachingPolicy
    {
        #region Fields

        readonly IsCacheExpires cacheExpires;

        readonly IsSatisfied<ICacheKey> satisfied;

        #endregion

        #region Constructors

        public CachingPolicy() { }

        internal CachingPolicy(IsSatisfied<ICacheKey> satisfied, IsCacheExpires cacheExpires)
        {
            this.satisfied = satisfied;
            this.cacheExpires = cacheExpires;
        }

        #endregion

        #region Factory constructors

        public static IsSatisfied<ICacheKey> For<TCacheKey>()
                where TCacheKey : class, ICacheKey
        {
            return SatisfiedSyntax.For<ICacheKey, TCacheKey>();
        }

        #endregion

        #region Api Methods

        public IsSatisfied<ICacheKey> ForAll()
        {
            return SatisfiedSyntax.Filter<ICacheKey>(key => true);
        }

        public IsSatisfied<ICacheKey> ForFirstDerived<TCacheKey>()
                where TCacheKey : ICacheKey
        {
            return SatisfiedSyntax.ForFirstDerived<ICacheKey, TCacheKey>();
        }

        public IsSatisfied<ICacheKey> ForDeepDerived<TCacheKey>()
                where TCacheKey : class, ICacheKey
        {
            return SatisfiedSyntax.ForDeepDerived<ICacheKey, TCacheKey>();
        }

        public bool IsExpires(ICacheKey cacheKey)
        {
            return this.cacheExpires(cacheKey);
        }

        #endregion

        internal bool IsSatisfied(ICacheKey key)
        {
            return this.satisfied.IsSatisfied(key);
        }
    }
}