namespace Incoding.MvcContrib.MVD
{
    using JetBrains.Annotations;

    public interface IUrlDispatcher
    {
        UrlDispatcher.IUrlQuery<TQuery> Query<TQuery>(object routes = null) where TQuery : new();

        UrlDispatcher.IUrlQuery<TQuery> Query<TQuery>([NotNull] TQuery routes) where TQuery : new();

        UrlDispatcher.UrlPush Push<TCommand>([NotNull] TCommand routes) where TCommand : new();

        UrlDispatcher.UrlPush Push<TCommand>(object routes = null) where TCommand : new();
            
        string AsView([PathReference, NotNull] string incView);

        UrlDispatcher.UrlModel<TModel> Model<TModel>(object routes = null);

        UrlDispatcher.UrlModel<TModel> Model<TModel>([NotNull] TModel routes);
    }
}