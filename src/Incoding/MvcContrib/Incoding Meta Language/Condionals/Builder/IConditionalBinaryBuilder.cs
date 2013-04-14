namespace Incoding.MvcContrib
{
    public interface IConditionalBinaryBuilder
    {
        IConditionalBuilder And { get; }

        IConditionalBuilder Or { get; }
    }
}