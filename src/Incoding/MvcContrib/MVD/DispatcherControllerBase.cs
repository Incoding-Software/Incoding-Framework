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


        #region Api Methods

        public virtual ActionResult Query(string incType, bool? incValidate, bool? incOnlyValidate = false)
        {
            var query = dispatcher.Query(new CreateByTypeQuery()
                                         {
                                                 Type = incType, 
                                                 Request = Request, 
                                         });
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
                var query = dispatcher.Query(new CreateByTypeQuery()
                                             {
                                                     IsModel = incIsModel.GetValueOrDefault(), 
                                                     Type = incType, 
                                                     Request = Request
                                             });
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
                                   ? ((IEnumerable<CommandBase>)dispatcher.Query(new CreateByTypeQuery()
                                                                                 {
                                                                                         Type = splitByType[0], 
                                                                                         IsGroup = true, 
                                                                                         Request = Request
                                                                                 })).ToList()
                                   : splitByType.Select(r => (CommandBase)dispatcher.Query(new CreateByTypeQuery()
                                                                                           {
                                                                                                   Type = r, 
                                                                                                   IsGroup = true, 
                                                                                                   Request = Request
                                                                                           })).ToList();

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
            var query = dispatcher.Query(new CreateByTypeQuery()
                                         {
                                                 Type = incType,
                                                 Request = Request
                                         });
            var result = dispatcher.GetType()
                                   .GetMethod("Query")
                                   .MakeGenericMethod(query.GetType().BaseType.With(r => r.GetGenericArguments()[0]))
                                   .Invoke(dispatcher, new[] { query, null });
            return File((byte[])result, string.IsNullOrWhiteSpace(incContentType) ? "img" : incContentType, incFileDownloadName.Recovery(string.Empty));
        }

        #endregion
    }
}