namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_disabled_ajax
    {
        #region Fake classes

        class FakeController : IncControllerBase
        {

            #region Api Methods

            public bool IsAjax()
            {
                return HttpContext.Request.IsAjaxRequest();
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockController<FakeController> mockController;

        static bool result;

        #endregion

        Establish establish = () =>
                                  {
                                      mockController = MockController<FakeController>
                                              .When()
                                              .DisableAjax();
                                  };

        Because of = () => { result = mockController.Original.IsAjax(); };

        It should_be_not_ajax = () => result.ShouldBeFalse();
    }
}