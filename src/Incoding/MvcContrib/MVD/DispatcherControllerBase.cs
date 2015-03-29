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
                : this(new[] { type }, filterTypes) { }

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
                var temp = assemblies.Select(s => s.GetTypes())
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
                                               typeof(IncEntityBase), typeof(IncStructureResponse<>), typeof(OptGroupVm)
                                       };
                types.AddRange(defaultTypes.Where(r => !types.Contains(r)));
                duplicates.AddRange(types.Where(r => types.Count(s => s.Name == r.Name) > 1));
            }
        }

        #endregion

        #region Api Methods

        public virtual ActionResult Query(string incType, string incGeneric, bool? incValidate)
        {
            var query = Create(incType, incGeneric);
            if (incValidate.GetValueOrDefault(false) && !ModelState.IsValid)
                return IncodingResult.Error(ModelState);

            var result = dispatcher.GetType()
                                   .GetMethod("Query")
                                   .MakeGenericMethod(query.GetType().BaseType.With(r => r.GetGenericArguments()[0]))
                                   .Invoke(dispatcher, new[] { query, null });

            return IncJson(result);
        }

        public virtual ActionResult Render(string incView, string incType, string incGeneric, bool? incIsModel)
        {
            TryUpdateModel("aq");
            incView = HttpUtility.UrlDecode(incView);
            object model = null;

            if (!string.IsNullOrWhiteSpace(incType))
            {
                var query = Create(incType, incGeneric);
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

        public virtual ActionResult Push(string incType, string incGeneric)
        {
            var command = (CommandBase)Create(incType, incGeneric);
            return TryPush(composite => composite.Quote(command), setting => { setting.SuccessResult = () => IncodingResult.Success(command.Result); });
        }

        public virtual ActionResult Composite(string incTypes)
        {
            if (string.IsNullOrWhiteSpace(incTypes))
                throw new ArgumentNullException("incTypes");

            var splits = incTypes.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            bool isGroup = splits.Count() == 1;

            List<CommandBase> commands;
            if (isGroup)
            {
                string firstType = splits.First();
                commands = ((IEnumerable<CommandBase>)Create(firstType, string.Empty, true)).ToList();
            }
            else
            {
                commands = splits.Select(r => (CommandBase)Create(r, string.Empty))
                                 .ToList();
            }

            return TryPush(composite =>
                           {
                               foreach (var commandBase in commands)
                                   composite.Quote(commandBase);
                           }, setting => setting.SuccessResult = () => IncodingResult.Success(commands.Select(r => r.Result)));
        }

        public virtual ActionResult QueryToFile(string incType, string incGeneric, string incContentType, string incFileDownloadName)
        {
            var query = Create(incType, incGeneric, false);
            var result = dispatcher.GetType()
                                   .GetMethod("Query")
                                   .MakeGenericMethod(query.GetType().BaseType.With(r => r.GetGenericArguments()[0]))
                                   .Invoke(dispatcher, new[] { query, null });
            return File((byte[])result, incContentType.Recovery("img"), incFileDownloadName.Recovery(string.Empty));
        }

        #endregion

        object Create(string type, string generic = "", bool isGroup = false)
        {
            var inst = FindTypeByName(type, false);
            var instanceType = isGroup ? typeof(List<>).MakeGenericType(inst) : inst;
            if (!string.IsNullOrWhiteSpace(generic))
            {
                instanceType = instanceType.MakeGenericType(generic.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                                                   .Select(name => FindTypeByName(name, true))
                                                                   .ToArray());
            }

            var instance = Activator.CreateInstance(instanceType);
            GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                     .Single(r => r.Name == "TryUpdateModel" && r.GetParameters().Length == 1)
                     .MakeGenericMethod(instanceType)
                     .Invoke(this, new[] { instance });

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
    }
}