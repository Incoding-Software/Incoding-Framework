namespace Incoding.Block.Caching
{
    #region << Using >>

    using System;
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

        /// <summary>
        ///     See <see cref="SatisfiedSyntax" />
        /// </summary>
        public static IsSatisfied<ICacheKey> Filter(Func<ICacheKey, bool> conditional)
        {
            return SatisfiedSyntax.Filter(conditional);
        }

        /// <summary>
        ///     See <see cref="SatisfiedSyntax" />
        /// </summary>
        public static IsSatisfied<ICacheKey> For<TCacheKey>()
                where TCacheKey : class, ICacheKey
        {
            return SatisfiedSyntax.For<ICacheKey, TCacheKey>();
        }

        /// <summary>
        ///     See <see cref="SatisfiedSyntax" />
        /// </summary>
        public IsSatisfied<ICacheKey> ForAll()
        {
            return SatisfiedSyntax.Filter<ICacheKey>(key => true);
        }

        /// <summary>
        ///     See <see cref="SatisfiedSyntax" />
        /// </summary>
        public IsSatisfied<ICacheKey> ForFirstDerived<TCacheKey>()
                where TCacheKey : ICacheKey
        {
            return SatisfiedSyntax.ForFirstDerived<ICacheKey, TCacheKey>();
        }

        #endregion

        #region Api Methods

        /// <summary>
        ///     See <see cref="SatisfiedSyntax" />
        /// </summary>
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