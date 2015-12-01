namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class UrlDispatcher : IUrlDispatcher
    {
        #region Constants

        internal const string separatorByGeneric = "/";

        internal const string separatorByPair = "|";

        internal const string separatorByType = "&";

        #endregion

        #region Static Fields

        public static bool IsVerifySchema;

        #endregion

        #region Fields

        readonly UrlHelper urlHelper;

        #endregion

        #region Constructors

        public UrlDispatcher(UrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        #endregion

        #region IUrlDispatcher Members

        public IUrlQuery<TQuery> Query<TQuery>(object routes = null) where TQuery : new()
        {
            VerifySchema<TQuery>(routes);
            return new UrlQuery<TQuery>(urlHelper, routes);
        }

        public IUrlQuery<TQuery> Query<TQuery>([NotNull] TQuery routes) where TQuery : new()
        {
            return Query<TQuery>(routes: routes as object);
        }

        public UrlPush Push<TCommand>([NotNull] TCommand routes) where TCommand : new()
        {
            return Push<TCommand>(routes: routes as object);
        }

        public UrlPush Push<TCommand>(object routes = null) where TCommand : new()
        {
            VerifySchema<TCommand>(routes);
            var res = new UrlPush(urlHelper);
            return res.Push<TCommand>(routes);
        }

        public string AsView([PathReference] string incView)
        {
            // ReSharper disable once Mvc.ActionNotResolved
            // ReSharper disable once Mvc.ControllerNotResolved
            return urlHelper.Action("Render", "Dispatcher", new
            {
                incView = incView,
            });
        }

        public UrlModel<TModel> Model<TModel>(object routes = null)
        {
            VerifySchema<TModel>(routes);
            var type = routes == null ? typeof(TModel) : routes.GetType();
            return new UrlModel<TModel>(urlHelper, type.IsTypicalType() ? new { incValue = routes } : routes);
        }

        public UrlModel<TModel> Model<TModel>(TModel routes)
        {
            return Model<TModel>(routes as object);
        }

        #endregion

        #region Nested classes

        public class UrlModel<TModel>
        {
            #region Fields

            readonly UrlHelper urlHelper;

            readonly object model;

            readonly RouteValueDictionary defaultRoutes;

            #endregion

            #region Constructors

            public UrlModel(UrlHelper urlHelper, object model)
            {
                defaultRoutes = new RouteValueDictionary
                                {
                                        { "incType", GetTypeName(typeof(TModel)) },
                                        { "incIsModel", true },
                                };

                this.urlHelper = urlHelper;
                this.model = model;
            }

            #endregion

            #region Api Methods

            public string AsView([PathReference, NotNull] string incView)
            {
                defaultRoutes.Add("incView", incView);
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
                return urlHelper.Action("Render", "Dispatcher", defaultRoutes)
                                .AppendToQueryString(model);
            }

            #endregion
        }

        public class UrlQuery<TQuery> : IUrlQuery<TQuery>
        {
            #region Fields

            readonly UrlHelper urlHelper;

            readonly RouteValueDictionary query;

            readonly RouteValueDictionary defaultRoutes;

            #endregion

            #region Constructors

            public UrlQuery(UrlHelper urlHelper, object query)
            {
                defaultRoutes = new RouteValueDictionary();
                defaultRoutes.Add("incType", GetTypeName(typeof(TQuery)));
                this.urlHelper = urlHelper;
                this.query = AnonymousHelper.ToDictionary(query);
            }

            #endregion

            #region IUrlQuery<TQuery> Members

            public UrlQuery<TQuery> ValidateOnly()
            {
                defaultRoutes.Add("incOnlyValidate", true);
                return this;
            }

            public UrlQuery<TQuery> EnableValidate()
            {
                defaultRoutes.Add("incValidate", true);
                return this;
            }

            public string AsFile(string incContentType = "", string incFileDownloadName = "")
            {
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
                return urlHelper.Action("QueryToFile", "Dispatcher", defaultRoutes)
                                .AppendToQueryString(new
                                {
                                    incContentType = incContentType,
                                    incFileDownloadName = incFileDownloadName
                                })
                                .AppendToQueryString(query);
            }

            public string AsJson()
            {
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
                return urlHelper.Action("Query", "Dispatcher", defaultRoutes)
                                .AppendToQueryString(query);
            }

            public string AsView([PathReference, NotNull] string incView)
            {
                defaultRoutes.Add("incView", incView);
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
                return urlHelper.Action("Render", "Dispatcher", defaultRoutes)
                                .AppendToQueryString(query);
            }

            #endregion

            public override string ToString()
            {
                return AsJson();
            }
        }

        public class UrlPush
        {
            #region Fields

            readonly UrlHelper urlHelper;

            readonly Dictionary<Type, List<object>> dictionary = new Dictionary<Type, List<object>>();

            bool onlyValidate;

            bool isCompositeAsArray;

            #endregion

            #region Constructors

            [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, true), ExcludeFromCodeCoverage]
            public UrlPush() { }

            public UrlPush(UrlHelper urlHelper)
            {
                this.urlHelper = urlHelper;
            }

            #endregion

            #region IUrlPush Members

            public UrlPush OnlyValidate()
            {
                onlyValidate = true;
                return this;
            }

            public UrlPush Push<TCommand>(object routes)
            {
                var type = typeof(TCommand);
                bool isContains = dictionary.ContainsKey(type);
                if (isContains)
                    isCompositeAsArray = true;

                if (isContains)
                    dictionary[type].Add(routes);
                else
                    dictionary.Add(type, new List<object> { routes });

                return this;
            }

            public UrlPush Push<TCommand>(TCommand command)
            {
                return Push<TCommand>(command as object);
            }

            #endregion

            public override string ToString()
            {
                var routeValues = new RouteValueDictionary { { "incTypes", dictionary.Select(r => GetTypeName(r.Key)).AsString(separatorByType) } };
                if (isCompositeAsArray)
                    routeValues.Add("incIsCompositeAsArray", true);
                if (onlyValidate)
                    routeValues.Add("incOnlyValidate", true);
                // ReSharper disable once Mvc.ActionNotResolved
                // ReSharper disable once Mvc.ControllerNotResolved
                return urlHelper.Action("Push", "Dispatcher", routeValues)
                                .AppendToQueryString(GetQueryString());
            }

            RouteValueDictionary GetQueryString()
            {
                var query = new RouteValueDictionary();
                foreach (var pair in dictionary)
                {
                    var valueAsRoutes = pair.Value.Select(AnonymousHelper.ToDictionary)
                                            .ToList();

                    if (valueAsRoutes.Count() > 1)
                    {
                        for (int i = 0; i < valueAsRoutes.Count; i++)
                        {
                            foreach (var keyValue in valueAsRoutes[i].Where(valueDictionary => !string.IsNullOrWhiteSpace(valueDictionary.Value.With(r => r.ToString()))))
                                query.Add("[{0}].{1}".F(i, keyValue.Key), keyValue.Value.ToString());
                        }
                    }
                    else
                    {
                        foreach (var keyValue in valueAsRoutes[0].Where(valueDictionary => !string.IsNullOrWhiteSpace(valueDictionary.Value.With(r => r.ToString()))))
                            query.Add(keyValue.Key, keyValue.Value.ToString());
                    }
                }

                return query;
            }

            public static implicit operator string (UrlPush s)
            {
                return s.ToString();
            }
        }

        #endregion


        void VerifySchema<TOriginal>(object routes)
        {
            if (!IsVerifySchema || routes == null)
                return;

            foreach (var property in routes.GetType().GetProperties())
            {
                var type = typeof(TOriginal);
                if (type.GetProperties().All(r => r.Name != property.Name))
                    throw new ArgumentOutOfRangeException("routes", "Can't found property {0} on {1}".F(property.Name, type.Name));
            }
        }

        static string GetTypeName(Type type)
        {
            string mainName = DispatcherControllerBase.duplicates.Any(r => r == type)
                                      ? type.FullName
                                      : type.Name;
            return type.IsGenericType ? "{0}{1}{2}".F(mainName, separatorByPair, type.GetGenericArguments().Select(GetTypeName).AsString(separatorByGeneric)) : mainName;
        }

        public interface IUrlQuery<TQuery>
        {
            UrlQuery<TQuery> ValidateOnly();

            UrlQuery<TQuery> EnableValidate();

            string AsFile(string incContentType = "", string incFileDownloadName = "");

            string AsJson();

            string AsView([PathReference, NotNull] string incView);
        }
    }
}