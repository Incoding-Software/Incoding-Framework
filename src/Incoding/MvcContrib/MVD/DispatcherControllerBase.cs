namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    // ReSharper disable Mvc.ViewNotResolved
    // ReSharper disable MemberCanBeProtected.Global
    public class DispatcherControllerBase : IncControllerBase
    {
        object Create(string type, bool isGroup = false, bool isModel = false)
        {
            var byPair = type.Split(UrlDispatcher.separatorByPair.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string genericType = byPair.ElementAtOrDefault(1);

            var inst = FindTypeByName(byPair[0], !string.IsNullOrWhiteSpace(genericType));
            var instanceType = isGroup ? typeof(List<>).MakeGenericType(inst) : inst;
            if (instanceType.IsTypicalType() && isModel)
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
                                                                       .Select(name => FindTypeByName(name, true))
                                                                       .ToArray());
            }

            var instance = Activator.CreateInstance(instanceType);

            var formAndQuery = new FormCollection(Request.Form);
            formAndQuery.Add(Request.QueryString);

            new DefaultModelBinder().BindModel(ControllerContext, new ModelBindingContext()
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => instance, instanceType),
                ModelState = ModelState,
                ValueProvider = formAndQuery
            });
            return instance;
        }

        Type FindTypeByName(string name, bool isGeneric)
        {
            name = HttpUtility.UrlDecode(name).With(s => s.Replace(" ", "+"));
            return cache.GetOrAdd(name, s =>
            {
                var allSatisfied = types.Where(r => r.Name.IsAnyEqualsIgnoreCase(s) ||
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

        ////ncrunch: no coverage start

        #region Static Fields

        internal static readonly List<Type> duplicates = new List<Type>();

        static readonly object lockObject = new object();

        static readonly List<Type> types = new List<Type>();

        static readonly ConcurrentDictionary<string, Type> cache = new ConcurrentDictionary<string, Type>();

        #endregion

        ////ncrunch: no coverage end

        #region Constructors

        public DispatcherControllerBase(Assembly type, Func<Type, bool> filterTypes = null)
                : this(new[] { type }, filterTypes)
        { }

        public DispatcherControllerBase(Assembly[] assemblies, Func<Type, bool> filterTypes = null)
        {
            ////ncrunch: no coverage start
            if (types.Any())
                return;
            ////ncrunch: no coverage end

            lock (lockObject)
            {
                ////ncrunch: no coverage start
                if (types.Any())
                    return;
                ////ncrunch: no coverage end

                var temp = assemblies
                        .Select(s => s.GetLoadableTypes())
                        .SelectMany(r => r);

                if (filterTypes != null)
                    temp = temp.Where(filterTypes);

                types.AddRange(temp);

                var defaultTypes = new[]
                                   {
                                           typeof(int), typeof(decimal), typeof(bool), typeof(byte),
                                           typeof(char), typeof(decimal), typeof(double), typeof(float),
                                           typeof(long), typeof(object), typeof(sbyte), typeof(short),
                                           typeof(string), typeof(uint), typeof(ulong), typeof(ushort),
                                           typeof(DateTime), typeof(TimeSpan), typeof(DeleteEntityByIdCommand), typeof(DeleteEntityByIdCommand<>),
                                           typeof(GetEntitiesQuery<>), typeof(GetEntityByIdQuery<>), typeof(HasEntitiesQuery<>), typeof(KeyValueVm),
                                           typeof(IncEntityBase), typeof(IncStructureResponse<>), typeof(OptGroupVm),
                                           typeof(IncBoolResponse)
                                   };
                types.AddRange(defaultTypes.Where(r => !types.Contains(r)));
                duplicates.AddRange(types.Where(r => types.Count(s => s.Name == r.Name) > 1));
            }
        }

        #endregion

        #region Api Methods

        public virtual ActionResult Query(string incType, bool? incValidate, bool? incOnlyValidate = false)
        {
            var query = Create(incType);
            if (incOnlyValidate.GetValueOrDefault() && ModelState.IsValid)
                return IncodingResult.Success();

            if ((incValidate.GetValueOrDefault() || incOnlyValidate.GetValueOrDefault()) && !ModelState.IsValid)
                return IncodingResult.Error(ModelState);

            var result = dispatcher.GetType()
                                   .GetMethod("Query")
                                   .MakeGenericMethod(query.GetType().BaseType.With(r => r.GetGenericArguments()[0]))
                                   .Invoke(dispatcher, new[] { query, null });

            return IncJson(result);
        }

        public virtual ActionResult Render(string incView, string incType, bool? incIsModel)
        {
            incView = HttpUtility.UrlDecode(incView);
            object model = null;

            if (!string.IsNullOrWhiteSpace(incType))
            {
                var query = Create(incType, isModel: incIsModel.GetValueOrDefault());
                var baseType = query.GetType().BaseType;

                model = baseType.Name.EqualsWithInvariant("QueryBase`1") && !incIsModel.GetValueOrDefault(false)
                                ? dispatcher.GetType()
                                            .GetMethod("Query")
                                            .MakeGenericMethod(baseType.GetGenericArguments()[0])
                                            .Invoke(dispatcher, new[] { query, null })
                                : query;
            }

            ModelState.Clear();

            return HttpContext.Request.IsAjaxRequest()
                           ? (ActionResult)IncPartialView(incView, model)
                           : Content(RenderToString(incView, model));
        }

        public virtual ActionResult Push(string incTypes, bool? incIsCompositeAsArray = false, bool? incOnlyValidate = false)
        {
            Guard.NotNullOrWhiteSpace("incTypes", incTypes);

            var splitByType = incTypes.Split(UrlDispatcher.separatorByType.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            bool isCompositeAsArray = splitByType.Count() == 1 && incIsCompositeAsArray.GetValueOrDefault();
            var commands = isCompositeAsArray
                                   ? ((IEnumerable<CommandBase>)Create(splitByType[0], true)).ToList()
                                   : splitByType.Select(r => (CommandBase)Create(r)).ToList();

            if (incOnlyValidate.GetValueOrDefault() && ModelState.IsValid)
                return IncodingResult.Success();

            return TryPush(composite =>
            {
                foreach (var commandBase in commands)
                    composite.Quote(commandBase);
            }, setting => setting.SuccessResult = () =>
            {
                var data = commands.Count == 1 ? commands[0].Result : commands.Select(r => r.Result);
                return IncodingResult.Success(data);
            });
        }

        public virtual ActionResult QueryToFile(string incType, string incContentType, string incFileDownloadName)
        {
            Response.AddHeader("X-Download-Options", "Open");
            var query = Create(incType, false);
            var result = dispatcher.GetType()
                                   .GetMethod("Query")
                                   .MakeGenericMethod(query.GetType().BaseType.With(r => r.GetGenericArguments()[0]))
                                   .Invoke(dispatcher, new[] { query, null });
            return File((byte[])result, string.IsNullOrWhiteSpace(incContentType) ? "img" : incContentType, incFileDownloadName.Recovery(string.Empty));
        }


        #endregion
    }
}