namespace Incoding.Block.Core
{
    #region << Using >>

    using System;
    using System.Diagnostics;
    using Incoding.Extensions;

    #endregion

    public static class SatisfiedSyntax
    {
        #region Factory constructors

        /// <summary>
        ///     <c>For</c> all <see cref="TInstance" />
        /// </summary>
        [DebuggerStepThrough]
        public static IsSatisfied<TInstance> All<TInstance>()
        {
            return Filter<TInstance>(instance => true);
        }

        /// <summary>
        ///     <c>For</c> all <see cref="TInstance" /> after check conditional
        /// </summary>
        /// <param name="conditional"> Predicate for filter </param>
        [DebuggerStepThrough]
        public static IsSatisfied<TInstance> Filter<TInstance>(Func<TInstance, bool> conditional)
        {
            Guard.NotNull("conditional", conditional);
            return instance => conditional(instance);
        }

        /// <summary>
        ///     For select type instance expected <see cref="TInstance" />
        /// </summary>
        [DebuggerStepThrough]
        public static IsSatisfied<TInstance> For<TInstance, TFound>()
                where TFound : class
        {
            return instance => instance is TFound;
        }

        /// <summary>
        ///     <c>For</c> all child from <typeparamref name="TInstance" /> .
        /// </summary>
        [DebuggerStepThrough]
        public static IsSatisfied<TInstance> ForDeepDerived<TInstance, TFound>()
                where TFound : class
        {
            return instance => instance.GetType().IsImplement<TFound>();
        }

        /// <summary>
        ///     <c>For</c> all first child from <typeparamref name="TInstance" />
        /// </summary>
        [DebuggerStepThrough]
        public static IsSatisfied<TInstance> ForFirstDerived<TInstance, TFound>()
        {
            return instance => instance.GetType().BaseType == typeof(TFound);
        }

        #endregion

        public static bool IsSatisfied<TInstance>(this IsSatisfied<TInstance> conditional, TInstance instance)
        {
            Guard.NotNull("conditional", conditional);
            return conditional(instance);
        }
    }
}