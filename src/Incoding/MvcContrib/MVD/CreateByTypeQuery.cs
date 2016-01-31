namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class CreateByTypeQuery : QueryBase<object>
    {
        public ControllerContext ControllerContext { get; set; }

        protected override object ExecuteResult()
        {
            var byPair = Type.Split(UrlDispatcher.separatorByPair.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string genericType = byPair.ElementAtOrDefault(1);

            var inst = Dispatcher.Query(new FindTypeByName()
                                        {
                                                Type = byPair[0],
                                        });
            var formCollection = Dispatcher.Query(new GetFormCollectionsQuery());
            var instanceType = IsGroup ? typeof(List<>).MakeGenericType(inst) : inst;
            if (instanceType.IsTypicalType() && IsModel)
            {
                string str = formCollection["incValue"];
                if (instanceType == typeof(string))
                    return str;
                if (instanceType == typeof(bool))
                    return bool.Parse(str);
                if (instanceType == typeof(DateTime))
                    return DateTime.Parse(str);
                if (instanceType == typeof(int))
                    return int.Parse(str);
                if (instanceType.IsEnum)
                    return Enum.Parse(instanceType, str);
            }
            else if (!string.IsNullOrWhiteSpace(genericType))
            {
                instanceType = instanceType.MakeGenericType(genericType.Split(UrlDispatcher.separatorByGeneric.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                                                       .Select(name => Dispatcher.Query(new FindTypeByName()
                                                                                                        {
                                                                                                                Type = name,
                                                                                                        }))
                                                                       .ToArray());
            }

            var instance = Activator.CreateInstance(instanceType);

            new DefaultModelBinder().BindModel(ControllerContext ?? new ControllerContext(), new ModelBindingContext()
                                                                                             {
                                                                                                     ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => instance, instanceType),
                                                                                                     ModelState = ModelState ?? new ModelStateDictionary(),
                                                                                                     PropertyFilter = propertyName => true,
                                                                                                     ValueProvider = formCollection
                                                                                             });
            return instance;
        }

        #region Nested classes

        public class FindTypeByName : QueryBase<Type>
        {
            #region Static Fields

            static readonly Dictionary<string, Type> cache = new Dictionary<string, Type>();

            #endregion

            #region Fields

            public string Type { get; set; }

            #endregion

            protected override Type ExecuteResult()
            {
                string name = HttpUtility.UrlDecode(Type).Replace(" ", "+");
                return cache.GetOrAdd(name, () =>
                                            {
                                                var allSatisfied = AppDomain.CurrentDomain.GetAssemblies()
                                                                            .Select(r => r.GetLoadableTypes())
                                                                            .SelectMany(r => r)
                                                                            .Where(type => type.Name.IsAnyEqualsIgnoreCase(name) || type.FullName.IsAnyEqualsIgnoreCase(name));

                                                if (!allSatisfied.Any())
                                                    throw new IncMvdException("Not found any type {0}".F(name));

                                                if (allSatisfied.Count() > 1)
                                                    throw new IncMvdException("Ambiguous type {0}".F(name));

                                                return allSatisfied.Single();
                                            });
            }
        }

        #endregion

        public class GetFormCollectionsQuery : QueryBase<FormCollection>
        {
            protected override FormCollection ExecuteResult()
            {
                var request = HttpContext.Current.Request;
                var formAndQuery = new FormCollection(request.Form);
                formAndQuery.Add(request.QueryString);
                return formAndQuery;
            }
        }

        #region Properties

        public string Type { get; set; }

        public bool IsGroup { get; set; }

        public bool IsModel { get; set; }

        public ModelStateDictionary ModelState { get; set; }

        #endregion
    }
}