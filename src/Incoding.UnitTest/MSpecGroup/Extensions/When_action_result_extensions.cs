namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(ActionResultExtensions))]
    public class When_action_result_extensions
    {
        #region Fake classes

        internal class HomeController : Controller
        {
            #region Api Methods

            public void Index()
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static RedirectToRouteResult RedirectTo(object route)
        {
            return new RedirectToRouteResult(new RouteValueDictionary(route));
        }

        #endregion

        It should_redirect = () =>
                                 {
                                     string url = Pleasure.Generator.Url();
                                     new RedirectResult(url).ShouldBeRedirect(url);
                                 };

        It should_be_redirect_to_action = () => RedirectTo(new { controller = "Home", action = "Index" }).ShouldBeRedirectToAction<HomeController>(x => x.Index());

        It should_be_redirect_to_action_with_wrong_action = () => Catch
                                                                          .Exception(() => RedirectTo(new { controller = "Home", action = "List" }).ShouldBeRedirectToAction<HomeController>(x => x.Index()))
                                                                          .ShouldBeOfType<SpecificationException>();

        It should_be_redirect_to_action_with_wrong_controller = () => Catch
                                                                              .Exception(() => RedirectTo(new { controller = "Person", action = "Index" }).ShouldBeRedirectToAction<HomeController>(x => x.Index()))
                                                                              .ShouldBeOfType<SpecificationException>();

        It should_be_model = () => new ViewResult { ViewData = { Model = Pleasure.Generator.TheSameString() } }.ShouldBeModel(Pleasure.Generator.TheSameString());

        It should_be_view_name = () => new ViewResult { ViewName = Pleasure.Generator.TheSameString() }.ShouldBeViewName(Pleasure.Generator.TheSameString());

        It should_be_model_with_wrong_view_result = () => Catch
                                                                  .Exception(() => new ViewResult().ShouldBeModel<string>(s => s.ShouldBeEmpty()))
                                                                  .ShouldBeOfType<SpecificationException>();

        It should_be_incoding_success_with_data = () => IncodingResult
                                                                .Success(Pleasure.Generator.TheSameString())
                                                                .ShouldBeIncodingSuccess<string>(s => s.ShouldEqual(Pleasure.Generator.TheSameString()));

        It should_be_incoding_success = () => IncodingResult
                                                      .Success()
                                                      .ShouldBeIncodingSuccess();

        It should_be_incoding_data = () => IncodingResult
                                                   .Success(Pleasure.Generator.TheSameString())
                                                   .ShouldBeIncodingData(Pleasure.Generator.TheSameString());

        It should_be_incoding_data_with_wrong_type = () => Catch
                                                                   .Exception(() => IncodingResult.Success(Pleasure.Generator.TheSameString()).ShouldBeIncodingData<int>(s => s.ShouldBeOfType<int>()))
                                                                   .ShouldBeOfType<SpecificationException>();

        It should_be_incoding_fail_with_data = () => IncodingResult
                                                             .Error(Pleasure.Generator.TheSameString())
                                                             .ShouldBeIncodingFail<string>(s => s.ShouldEqual(Pleasure.Generator.TheSameString()));

        It should_be_incoding_fail = () => IncodingResult.Error().ShouldBeIncodingFail();

        It should_be_incoding_redirect_to = () => IncodingResult
                                                          .RedirectTo(Pleasure.Generator.TheSameString())
                                                          .ShouldBeIncodingRedirect(Pleasure.Generator.TheSameString());
    }
}