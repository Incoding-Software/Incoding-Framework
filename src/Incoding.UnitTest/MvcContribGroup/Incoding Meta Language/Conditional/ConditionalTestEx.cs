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

        public static ConditionalBase GetFirst(this IConditionalBinaryBuilder builder)
        {
            return builder.GetByIndex(0);
        }

        public static void ShouldEqualConditionalIs(this object conditionalIs, string left, string right, string method, bool inverse = false)
        {
            conditionalIs.ShouldEqualWeak(new
                                              {
                                                      type = ConditionalOfType.Is.ToString(), 
                                                      inverse = inverse, 
                                                      left = left, 
                                                      right = right, 
                                                      method = method, 
                                                      and = true
                                              }, dsl => dsl.IncludeAllFields());
        }

        #endregion
    }
}