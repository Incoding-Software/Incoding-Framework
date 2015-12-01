namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class CreateByTypeQuery : QueryBase<object>
    {
        #region Properties

        public HttpRequestBase Request { get; set; }

        public string Type { get; set; }

        public bool IsGroup { get; set; }

        public bool IsModel { get; set; }

        #endregion

        #region Nested classes

        public class FindTypeByName : QueryBase<Type>
        {
            #region Static Fields

            internal static readonly ConcurrentDictionary<string, Type> cache = new ConcurrentDictionary<string, Type>();

            #endregion

            #region Fields

            readonly string type;

            readonly bool isGeneric;

            #endregion

            #region Constructors

            public FindTypeByName(string type, bool isGeneric)
            {
                this.type = type;
                this.isGeneric = isGeneric;
            }

            #endregion

            protected override Type ExecuteResult()
            {
                string name = HttpUtility.UrlDecode(type).With(s => s.Replace(" ", "+"));
                return cache.GetOrAdd(name, s =>
                                            {
                                                var allSatisfied = AppDomain.CurrentDomain.GetAssemblies()
                                                                            .Select(r => r.GetLoadableTypes())
                                                                            .SelectMany(r => r)
                                                                            .Where(r => r.Name.IsAnyEqualsIgnoreCase(s) ||
                                                                                        r.FullName.IsAnyEqualsIgnoreCase(s))
                                                                            .ToList();

                                                string prefix = isGeneric ? " generic" : string.Empty;
                                                if (allSatisfied.Count == 0)
                                                    throw new IncMvdException("Not found any{0} type {1}".F(prefix, s));

                                                if (allSatisfied.Count > 1)
                                                    throw new IncMvdException("Ambiguous{0} type {1}".F(prefix, s));
                                                return allSatisfied.First();
                                            });
            }
        }

        #endregion

        protected override object ExecuteResult()
        {
            var byPair = Type.Split(UrlDispatcher.separatorByPair.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string genericType = byPair.ElementAtOrDefault(1);

            var inst = Dispatcher.Query(new FindTypeByName(byPair[0], !string.IsNullOrWhiteSpace(genericType)));
            var instanceType = IsGroup ? typeof(List<>).MakeGenericType(inst) : inst;
            if (instanceType.IsTypicalType() && IsModel)
            {
                string str = Request.Params["incValue"];
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

            if (!string.IsNullOrWhiteSpace(genericType))
            {
                instanceType = instanceType.MakeGenericType(genericType.Split(UrlDispatcher.separatorByGeneric.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                                                       .Select(name => Dispatcher.Query(new FindTypeByName(name, true)))
                                                                       .ToArray());
            }

            var instance = Activator.CreateInstance(instanceType);

            var formAndQuery = new FormCollection(Request.Form);
            formAndQuery.Add(Request.QueryString);

            new DefaultModelBinder().BindModel(new ControllerContext(), new ModelBindingContext()
                                                                        {
                                                                                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => instance, instanceType), 
                                                                                ModelState = new ModelStateDictionary(), 
                                                                                ValueProvider = formAndQuery
                                                                        });
            return instance;
        }
    }
}