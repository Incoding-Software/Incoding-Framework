namespace Incoding.MvcContrib
{
    using JetBrains.Annotations;

    public interface ITemplateOnServerSide
    {
        string Render<T>([PathReference] string pathToView, T data);

        string Render<T>([PathReference] string pathToView, object modelForView, T data);
    }
}