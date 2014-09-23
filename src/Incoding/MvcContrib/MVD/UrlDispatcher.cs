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

        public UrlQueryDispatcher<TQuery> Query<TQuery>(object routes = null)
        {
            VerifySchema<TQuery>(routes);
            return new UrlQueryDispatcher<TQuery>(this.urlHelper, routes);
        }

        public UrlQueryDispatcher<TQuery> Query<TQuery>(TQuery routes)
        {
            return Query<TQuery>(routes as object);
        }

        public UrlPushDispatcher Push<TCommand>(TCommand routes)
        {
            return Push<TCommand>(routes as object);
        }

        public UrlPushDispatcher Push<TCommand>(object routes = null)
        {
            VerifySchema<TCommand>(routes);
            var res = new UrlPushDispatcher(this.urlHelper);
            return res.Push<TCommand>(routes);
        }

        public string AsView([PathReference] string incView)
        {
            return this.urlHelper.Action("Render", "Dispatcher", new
                                                                     {
                                                                             incView = incView,
                                                                     });
        }

        public UrlModelDispatcher<TModel> Model<TModel>()
        {
            return new UrlModelDispatcher<TModel>(this.urlHelper, null);
        }

        public UrlModelDispatcher<TModel> Model<TModel>(object routes)
        {
            VerifySchema<TModel>(routes);
            return new UrlModelDispatcher<TModel>(this.urlHelper, routes);
        }

        public UrlModelDispatcher<TModel> Model<TModel>(TModel routes) where TModel : class
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
                this.defaultRoutes = new RouteValueDictionary
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
                this.defaultRoutes.Add("incView", incView);
                return this.urlHelper.Action("Render", "Dispatcher", this.defaultRoutes)
                           .AppendToQueryString(this.model);
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
                this.defaultRoutes = new RouteValueDictionary();
                this.defaultRoutes.Add("incType", GetTypeName(typeof(TQuery)));
                if (typeof(TQuery).IsGenericType)
                {
                    this.defaultRoutes.Add("incGeneric", typeof(TQuery).GetGenericArguments().Select(r => GetTypeName(r))
                                                                       .AsString(","));
                }

                this.urlHelper = urlHelper;
                this.query = AnonymousHelper.ToDictionary(query);
            }

            #endregion

            #region Api Methods

            public UrlQueryDispatcher<TQuery> EnableValidate()
            {
                this.defaultRoutes.Add("incValidate", true);
                return this;
            }

            public string AsFile(string incContentType = "", string incFileDownloadName = "")
            {
                if (!string.IsNullOrWhiteSpace(incContentType))
                    this.defaultRoutes.Add("incContentType", incContentType);
                if (!string.IsNullOrWhiteSpace(incFileDownloadName))
                    this.defaultRoutes.Add("incFileDownloadName", incFileDownloadName);
                return this.urlHelper.Action("QueryToFile", "Dispatcher", this.defaultRoutes)
                           .AppendToQueryString(this.query);
            }

            public string AsJson()
            {
                return this.urlHelper.Action("Query", "Dispatcher", this.defaultRoutes)
                           .AppendToQueryString(this.query);
            }

            public string AsView([PathReference] string incView)
            {
                this.defaultRoutes.Add("incView", incView);
                return this.urlHelper.Action("Render", "Dispatcher", this.defaultRoutes)
                           .AppendToQueryString(this.query);
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
                if (this.dictionary.ContainsKey(type))
                    this.dictionary[type].Add(routes);
                else
                    this.dictionary.Add(type, new List<object> { routes });

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
                if (this.dictionary.Count == 1 && this.dictionary.Values.First().Count == 1)
                {
                    var pair = this.dictionary.First();
                    var commandType = pair.Key;
                    baseUrl = this.urlHelper.Action("Push", "Dispatcher", new RouteValueDictionary
                                                                              {
                                                                                      { "incType", GetTypeName(commandType) },
                                                                                      { "incGeneric", commandType.IsGenericType ? GetTypeName(commandType.GetGenericArguments()[0]) : string.Empty }
                                                                              });
                }
                else
                {
                    baseUrl = this.urlHelper.Action("Composite", "Dispatcher", new RouteValueDictionary
                                                                                   {
                                                                                           {
                                                                                                   "incType", this.dictionary.Select(r => GetTypeName(r.Key))
                                                                                                                  .Distinct()
                                                                                                                  .AsString(",")
                                                                                           }
                                                                                   });
                }

                var routes = new RouteValueDictionary();
                foreach (var pair in this.dictionary)
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