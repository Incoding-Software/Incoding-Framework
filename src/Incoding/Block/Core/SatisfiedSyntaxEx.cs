namespace Incoding.Block.Core
{
    #region << Using >>

    using System.Diagnostics;

    #endregion

    public static class SatisfiedSyntaxEx
    {
        #region Factory constructors

        [DebuggerStepThrough]
        public static IsSatisfied<TInstance> Also<TInstance, TFound>(this IsSatisfied<TInstance> source)
        {
            return instance => source(instance) || instance is TFound;
        }

        [DebuggerStepThrough]
        public static IsSatisfied<TInstance> Exclude<TInstance, TFound>(this IsSatisfied<TInstance> source)
        {
            return instance => source(instance) && !(instance is TFound);
        }

        #endregion
    }
}