namespace Incoding.MvcContrib.MVD
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;

    public class IsUniqueTypeByNameQuery : QueryBase<bool>
    {
        #region Static Fields

        internal static readonly ConcurrentDictionary<Type, bool> cache = new ConcurrentDictionary<Type, bool>();

        #endregion

        public Type Type { get; set; }

        protected override bool ExecuteResult()
        {
            return cache.GetOrAdd(Type, s =>
                                        {
                                            return AppDomain.CurrentDomain.GetAssemblies()
                                                            .Select(r => ReflectionExtensions.GetLoadableTypes(r))
                                                            .SelectMany(r => r)
                                                            .Count(r => r.Name.IsAnyEqualsIgnoreCase(s.Name)) == 1;
                                        });
        }
    }
}