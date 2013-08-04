namespace Incoding.Block.Core
{
    #region << Using >>

    using System;
    using Incoding.Extensions;

    #endregion

    public static class SatisfiedSyntax
    {
        #region Factory constructors

        public static IsSatisfied<TInstance> All<TInstance>()
        {
            return Filter<TInstance>(instance => true);
        }

        public static IsSatisfied<TInstance> Filter<TInstance>(Func<TInstance, bool> conditional)
        {
            return instance => conditional(instance);
        }

        public static IsSatisfied<TInstance> For<TInstance, TFound>()
                where TFound : class
        {
            return instance => instance is TFound;
        }

        public static IsSatisfied<TInstance> ForDeepDerived<TInstance, TFound>()
                where TFound : class
        {
            return instance => instance.GetType().IsImplement<TFound>();
        }

        public static IsSatisfied<TInstance> ForFirstDerived<TInstance, TFound>()
        {
            return instance => instance.GetType().BaseType == typeof(TFound);
        }

        public static bool IsSatisfied<TInstance>(this IsSatisfied<TInstance> conditional, TInstance instance)
        {            
            return conditional(instance);
        }

        #endregion
    }
}