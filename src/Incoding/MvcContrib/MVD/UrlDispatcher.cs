namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using JetBrains.Annotations;

    #endregion

    public class UrlDispatcher
    {
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

        #region Api Methods

        public UrlQueryDispatcher<TQuery> Query<TQuery>(object routes = null) where TQuery : new()
        {
            VerifySchema<TQuery>(routes);
            return new UrlQueryDispatcher<TQuery>(urlHelper, routes);
        }

        public UrlQueryDispatcher<TQuery> Query<TQuery>(TQuery routes) where TQuery : new()
        {
            return Query<TQuery>(routes as object);
        }

        public UrlPushDispatcher Push<TCommand>(TCommand routes) where TCommand : new()
        {
            return Push<TCommand>(routes as object);
        }

        public UrlPushDispatcher Push<TCommand>(object routes = null) where TCommand : new()
        {
            VerifySchema<TCommand>(routes);
            var res = new UrlPushDispatcher(urlHelper);
            return res.Push<TCommand>(routes);
        }

        public string AsView([PathReference] string incView)
        {
            return urlHelper.Action("Render", "Dispatcher", new
                                                            {
                                                                    incView = incView, 
                                                            });
        }

        public UrlModelDispatcher<TModel> Model<TModel>() where TModel : new()
        {
            return new UrlModelDispatcher<TModel>(urlHelper, null);
        }

        public UrlModelDispatcher<TModel> Model<TModel>(object routes) where TModel : new()
        {
            VerifySchema<TModel>(routes);
            return new UrlModelDispatcher<TModel>(urlHelper, routes);
        }

        public UrlModelDispatcher<TModel> Model<TModel>(TModel routes) where TModel : class, new()
        {
            return Model<TModel>(routes as object);
        }

        #endregion

        #region Nested classes

        public class UrlModelDispatcher<TModel>
        {
            #region Fields

            readonly UrlHelper urlHelper;

            readonly object model;

            readonly RouteValueDictionary defaultRoutes;

            #endregion

            #region Constructors

            public UrlModelDispatcher(UrlHelper urlHelper, object model)
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

            public string AsView([PathReference] string incView)
            {
                defaultRoutes.Add("incView", incView);
                return urlHelper.Action("Render", "Dispatcher", defaultRoutes)
                                .AppendToQueryString(model);
            }

            #endregion
        }

        public class UrlQueryDispatcher<TQuery>
        {
            #region Fields

            readonly UrlHelper urlHelper;

            readonly RouteValueDictionary query;

            readonly RouteValueDictionary defaultRoutes;

            #endregion

            #region Constructors

            public UrlQueryDispatcher(UrlHelper urlHelper, object query)
            {
                defaultRoutes = new RouteValueDictionary();
                defaultRoutes.Add("incType", GetTypeName(typeof(TQuery)));
                if (typeof(TQuery).IsGenericType)
                {
                    defaultRoutes.Add("incGeneric", typeof(TQuery).GetGenericArguments().Select(r => GetTypeName(r))
                                                                  .AsString(","));
                }

                this.urlHelper = urlHelper;
                this.query = AnonymousHelper.ToDictionary(query);
            }

            #endregion

            #region Api Methods

            public UrlQueryDispatcher<TQuery> EnableValidate()
            {
                defaultRoutes.Add("incValidate", true);
                return this;
            }

            public string AsFile(string incContentType = "", string incFileDownloadName = "")
            {
                if (!string.IsNullOrWhiteSpace(incContentType))
                    defaultRoutes.Add("incContentType", incContentType);
                if (!string.IsNullOrWhiteSpace(incFileDownloadName))
                    defaultRoutes.Add("incFileDownloadName", incFileDownloadName);
                return urlHelper.Action("QueryToFile", "Dispatcher", defaultRoutes)
                                .AppendToQueryString(query);
            }

            public string AsJson()
            {
                return urlHelper.Action("Query", "Dispatcher", defaultRoutes)
                                .AppendToQueryString(query);
            }

            public string AsView([PathReference] string incView)
            {
                defaultRoutes.Add("incView", incView);
                return urlHelper.Action("Render", "Dispatcher", defaultRoutes)
                                .AppendToQueryString(query);
            }

            #endregion

            public override string ToString()
            {
                return AsJson();
            }
        }

        public class UrlPushDispatcher
        {
            #region Fields

            readonly UrlHelper urlHelper;

            readonly Dictionary<Type, List<object>> dictionary = new Dictionary<Type, List<object>>();

            #endregion

            #region Constructors

            public UrlPushDispatcher(UrlHelper urlHelper)
            {
                this.urlHelper = urlHelper;
            }

            #endregion

            #region Api Methods

            public UrlPushDispatcher Push<TCommand>(object routes)
            {
                var type = typeof(TCommand);
                if (dictionary.ContainsKey(type))
                    dictionary[type].Add(routes);
                else
                    dictionary.Add(type, new List<object> { routes });

                return this;
            }

            public UrlPushDispatcher Push<TCommand>(TCommand command)
            {
                return Push<TCommand>(command as object);
            }

            #endregion

            public override string ToString()
            {
                string baseUrl;
                if (dictionary.Count == 1 && dictionary.Values.First().Count == 1)
                {
                    var pair = dictionary.First();
                    var commandType = pair.Key;
                    baseUrl = urlHelper.Action("Push", "Dispatcher", new RouteValueDictionary
                                                                     {
                                                                             { "incType", GetTypeName(commandType) }, 
                                                                             { "incGeneric", commandType.IsGenericType ? GetTypeName(commandType.GetGenericArguments()[0]) : string.Empty }
                                                                     });
                }
                else
                {
                    baseUrl = urlHelper.Action("Composite", "Dispatcher", new RouteValueDictionary
                                                                          {
                                                                                  {
                                                                                          "incTypes", dictionary.Select(r => GetTypeName(r.Key))
                                                                                                               .Distinct()
                                                                                                               .AsString(",")
                                                                                  }
                                                                          });
                }

                var routes = new RouteValueDictionary();
                foreach (var pair in dictionary)
                {
                    var valueAsRoutes = pair.Value.Select(AnonymousHelper.ToDictionary)
                                            .ToList();

                    if (valueAsRoutes.Count() > 1)
                    {
                        for (int i = 0; i < valueAsRoutes.Count; i++)
                        {
                            foreach (var keyValue in valueAsRoutes[i].Where(valueDictionary => !string.IsNullOrWhiteSpace(valueDictionary.Value.With(r => r.ToString()))))
                                routes.Add("[{0}].{1}".F(i, keyValue.Key), keyValue.Value.ToString());
                        }
                    }
                    else
                    {
                        foreach (var keyValue in valueAsRoutes[0].Where(valueDictionary => !string.IsNullOrWhiteSpace(valueDictionary.Value.With(r => r.ToString()))))
                            routes.Add(keyValue.Key, keyValue.Value.ToString());
                    }
                }

                return baseUrl.AppendToQueryString(routes);
            }

            public static implicit operator string(UrlPushDispatcher s)
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
            return DispatcherControllerBase.duplicates.Any(r => r == type)
                           ? type.FullName
                           : type.Name;
        }
    }
}