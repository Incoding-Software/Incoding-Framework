namespace Incoding.Block.Core
{
    public delegate bool IsSatisfied<in TInstance>(TInstance instance);
}