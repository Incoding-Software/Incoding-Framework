namespace Incoding.Block.Caching
{
    #region << Using >>

    using System;
    using Incoding.Block.Core;

    #endregion

    public static class CachingPolicyFluentEx
    {
        #region Factory constructors

        /// <summary>
        ///     Reset cache each get
        /// </summary>
        public static CachingPolicy AlwaysExpires(this IsSatisfied<ICacheKey> satisfied)
        {
            return new CachingPolicy(satisfied, key => true);
        }

        /// <summary>
        ///     Set expires to <paramref name="generateEndDt" />
        /// </summary>
        /// <param name="satisfied">
        ///     See <see cref="IsSatisfied{TInstance}" />
        /// </param>
        /// <param name="generateEndDt"> Date end caching key </param>
        public static CachingPolicy EndAbsolute(this IsSatisfied<ICacheKey> satisfied, Func<DateTime> generateEndDt)
        {
            Guard.NotNull("generateEndDt", generateEndDt);

            var endDt = generateEndDt.Invoke();
            IsCacheExpires ret = key =>
                                     {
                                         bool isExpires = DateTime.Now > endDt;
                                         if (isExpires) // Generate next end date time if expires
                                             endDt = generateEndDt.Invoke();
                                         return isExpires;
                                     };
            return new CachingPolicy(satisfied, ret);
        }

        /// <summary>
        ///     Set expires to <paramref name="generateEndDt" /> and reset each try get value by caching key
        /// </summary>
        /// <param name="satisfied">
        ///     See <see cref="IsSatisfied{TInstance}" />
        /// </param>
        /// <param name="generateEndDt"> Date end caching key </param>
        public static CachingPolicy EndSliding(this IsSatisfied<ICacheKey> satisfied, Func<DateTime> generateEndDt)
        {
            Guard.NotNull("generateEndDt", generateEndDt);

            var endDt = generateEndDt.Invoke();
            IsCacheExpires ret = key =>
                                     {
                                         bool isExpires = DateTime.Now > endDt;
                                         endDt = generateEndDt.Invoke(); // Generate next end date time.
                                         return isExpires;
                                     };
            return new CachingPolicy(satisfied, ret);
        }

        /// <summary>
        ///     Set always expires
        /// </summary>
        /// <param name="satisfied">
        ///     See <see cref="IsSatisfied{TInstance}" />
        /// </param>
        public static CachingPolicy NeverExpires(this IsSatisfied<ICacheKey> satisfied)
        {
            return new CachingPolicy(satisfied, key => false);
        }

        /// <summary>
        ///     Set custom<c>expires</c>s
        /// </summary>
        /// <param name="satisfied"> Filter for caching key </param>
        /// <param name="expires">
        ///     Predicate for <c>expires</c>
        /// </param>
        /// <returns> Instance caching policy </returns>
        public static CachingPolicy SetExpires(this IsSatisfied<ICacheKey> satisfied, IsCacheExpires expires)
        {
            return new CachingPolicy(satisfied, expires);
        }

        #endregion
    }
}