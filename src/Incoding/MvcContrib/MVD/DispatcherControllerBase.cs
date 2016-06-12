﻿namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.Maybe;

    #endregion

    // ReSharper disable Mvc.ViewNotResolved
    // ReSharper disable MemberCanBeProtected.Global
    public class DispatcherControllerBase : IncControllerBase
    {
        #region Api Methods

        public virtual ActionResult Query()
        {
            var parameter = dispatcher.Query(new GetMvdParameterQuery()
                                             {
                                                     Params = HttpContext.Request.Params
                                             });
            var query = dispatcher.Query(new CreateByTypeQuery() { Type = parameter.Type, ControllerContext = this.ControllerContext, ModelState = ModelState });
            if (parameter.OnlyValidate && ModelState.IsValid)
                return IncodingResult.Success();

            if ((parameter.IsValidate || parameter.OnlyValidate) && !ModelState.IsValid)
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
            var isAjaxRequest = HttpContext.Request.IsAjaxRequest();
            return isAjaxRequest
                           ? (ActionResult)IncPartialView(incView, model)
                           : Content(RenderToString(incView, model));
        }

        public virtual ActionResult Push(string incTypes, string incType = "", bool? incIsCompositeAsArray = false, bool? incOnlyValidate = false)
        {
            if (!string.IsNullOrWhiteSpace(incType))
                incTypes = incType;

            var commands = dispatcher.Query(new CreateByTypeQuery.AsCommands()
                                            {
                                                    IncTypes = incTypes,
                                                    ModelState = ModelState,
                                                    ControllerContext = ControllerContext,
                                                    IsComposite = incIsCompositeAsArray
                                            });

            if (incOnlyValidate.GetValueOrDefault() && ModelState.IsValid)
                return IncodingResult.Success();

            return TryPush(composite =>
                           {
                               foreach (var commandBase in commands)
                                   composite.Quote(commandBase);
                           }, setting => setting.SuccessResult = () =>
                                                                 {
                                                                     var data = commands.Length == 1 ? commands[0].Result : commands.Select(r => r.Result);
                                                                     return IncodingResult.Success(data);
                                                                 });
        }

        public virtual ActionResult QueryToFile(string incType, string incContentType, string incFileDownloadName)
        {
            Response.AddHeader("X-Download-Options", "Open");
            var result = dispatcher.Query(new ExecuteQuery()
                                          {
                                                  Instance = dispatcher.Query(new CreateByTypeQuery()
                                                                              {
                                                                                      Type = incType,
                                                                                      ControllerContext = ControllerContext,
                                                                                      ModelState = ModelState
                                                                              })
                                          });
            return File((byte[])result, string.IsNullOrWhiteSpace(incContentType) ? "img" : incContentType, incFileDownloadName.Recovery(string.Empty));
        }

        #endregion
    }
}