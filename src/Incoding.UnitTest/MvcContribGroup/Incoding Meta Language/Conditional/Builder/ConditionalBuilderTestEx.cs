namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;
    using Incoding.MvcContrib;

    #endregion

    public static class ConditionalBuilderTestEx
    {
        #region Factory constructors

        public static ConditionalBase GetByIndex(this IConditionalBinaryBuilder builder, int index)
        {
            var all = builder.TryGetValue("conditionals") as List<ConditionalBase>;
            return all[index];
        }

        public static ConditionalBase GetFirst(this IConditionalBinaryBuilder builder)
        {
            return builder.GetByIndex(0);
        }

        #endregion
    }
}