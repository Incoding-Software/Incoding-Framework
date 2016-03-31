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
            var query = dispatcher.Query(new CreateByTypeQuery() { Type = incType, ControllerContext = this.ControllerContext, ModelState = ModelState });
            if (incOnlyValidate.GetValueOrDefault() && ModelState.IsValid)
                return IncodingResult.Success();

            if ((incValidate.GetValueOrDefault() || incOnlyValidate.GetValueOrDefault()) && !ModelState.IsValid)
                return IncodingResult.Error(ModelState);

            return IncJson(dispatcher.Query(new ExecuteQuery() { Instance = query }));
        }

        public virtual ActionResult Render(string incView, string incType, bool? incIsModel)
        {
            object model = null;
            if (!string.IsNullOrWhiteSpace(incType))
            {
                var instance = dispatcher.Query(new CreateByTypeQuery()
                                                {
                                                        Type = incType,
                                                        ControllerContext = ControllerContext,
                                                        ModelState = ModelState,
                                                        IsModel = incIsModel.GetValueOrDefault()
                                                });

                model = incIsModel.GetValueOrDefault(false)
                                ? instance
                                : dispatcher.Query(new ExecuteQuery() { Instance = instance });
            }

            ModelState.Clear();

            incView = HttpUtility.UrlDecode(incView);
            return HttpContext.Request.IsAjaxRequest()
                           ? (ActionResult)IncPartialView(incView, model)
                           : Content(RenderToString(incView, model));
        }

        public virtual ActionResult Push(string incTypes, string incType = "", bool? incIsCompositeAsArray = false, bool? incOnlyValidate = false)
        {
            if (!string.IsNullOrWhiteSpace(incType))
                incTypes = incType;

            Guard.NotNullOrWhiteSpace("incTypes", incTypes);

            var splitByType = incTypes.Split(UrlDispatcher.separatorByType.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            bool isCompositeAsArray = splitByType.Count() == 1 && incIsCompositeAsArray.GetValueOrDefault();
            var commands = isCompositeAsArray
                                   ? ((IEnumerable<CommandBase>)dispatcher.Query(new CreateByTypeQuery()
                                                                                 {
                                                                                         Type = splitByType[0],
                                                                                         ControllerContext = this.ControllerContext,
                                                                                         ModelState = ModelState,
                                                                                         IsGroup = true
                                                                                 })).ToList()
                                   : splitByType.Select(r => (CommandBase)dispatcher.Query(new CreateByTypeQuery() { Type = r, ControllerContext = this.ControllerContext, ModelState = ModelState })).ToList();

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
            var result = dispatcher.Query(new ExecuteQuery() { Instance = dispatcher.Query(new CreateByTypeQuery() { Type = incType, ControllerContext = ControllerContext, ModelState = ModelState }) });
            return File((byte[])result, string.IsNullOrWhiteSpace(incContentType) ? "img" : incContentType, incFileDownloadName.Recovery(string.Empty));
        }

        #endregion
    }
}