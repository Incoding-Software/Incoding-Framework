namespace Incoding.Block.Core
{
    public static class SatisfiedSyntaxEx
    {
        #region Factory constructors

        public static IsSatisfied<TInstance> Also<TInstance, TFound>(this IsSatisfied<TInstance> source)
        {
            return instance => source(instance) || instance is TFound;
        }

        public static IsSatisfied<TInstance> Exclude<TInstance, TFound>(this IsSatisfied<TInstance> source)
        {
            return instance => source(instance) && !(instance is TFound);
        }

        #endregion
    }
}