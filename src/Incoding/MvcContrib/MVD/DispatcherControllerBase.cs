namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    // ReSharper disable Mvc.ViewNotResolved
    // ReSharper disable MemberCanBeProtected.Global
    public class DispatcherControllerBase : IncControllerBase
    {
        ////ncrunch: no coverage start

        #region Static Fields

        

        
        #endregion

        ////ncrunch: no coverage end

        #region Constructors

        public DispatcherControllerBase(Assembly type, Func<Type, bool> filterTypes = null)
                : this(new[] { type }, filterTypes) { }

        public DispatcherControllerBase(Assembly[] assemblies, Func<Type, bool> filterTypes = null)
        {
            //////ncrunch: no coverage end

            //if (duplicates.Any())
            //    return;

            //lock (lockObject)
            //{
            //    if (duplicates.Any())
            //        return;

            //    var allTypes = AppDomain.CurrentDomain.GetAssemblies()
            //                            .Select(r => r.GetLoadableTypes())
            //                            .SelectMany(r => r)
            //                            .Where(r => !r.IsAbstract && (r.IsClass || r.IsTypicalType()))
            //                            .ToList();

            //    duplicates.AddRange(allTypes.Where(r => allTypes.Count(s => s.Name == r.Name) > 1));
            //}
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

        public virtual ActionResult Render(string incView, string incType, bool? incIsModel, bool? incValidate)
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

                if (incValidate.GetValueOrDefault() && !ModelState.IsValid)
                    return IncodingResult.Error(ModelState);

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