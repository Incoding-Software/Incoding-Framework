namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;

    #endregion

    public static class ConditionalTestEx
    {
        #region Factory constructors

        public static ConditionalBase GetByIndex(this IConditionalBinaryBuilder builder, int index)
        {
            var all = builder.TryGetValue("conditionals") as List<ConditionalBase>;
            return all[index];
        }

        public static void ShouldEqualConditionalIs(this object conditionalIs, string left, string right, string method, bool inverse = false, bool and = true)
        {
            conditionalIs.ShouldEqualWeak(new
                                              {
                                                      type = ConditionalOfType.Is.ToString(),
                                                      inverse = inverse,
                                                      left = left,
                                                      right = right,
                                                      method = method,
                                                      and = and
                                              }, dsl => dsl.IncludeAllFields());
        }

        #endregion
    }
}