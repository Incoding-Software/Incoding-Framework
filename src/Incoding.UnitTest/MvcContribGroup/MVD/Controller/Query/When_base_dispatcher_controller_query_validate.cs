namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query_validate : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeDisabledValidateQuery : QueryBase<string>
        {
            ////ncrunch: no coverage start
            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        
        }

        #endregion

        #region Establish value

        static string queryResult;

        #endregion

        Establish establish = () =>
                                  {
                                      Establish(types: new[] { typeof(FakeDisabledValidateQuery) });
                                      controller.ModelState.AddModelError("Fake", "Error");
                                  };

        Because of = () => { result = controller.Query(HttpUtility.UrlEncode(typeof(FakeDisabledValidateQuery).FullName), string.Empty, false); };

        It should_be_result = () => result.ShouldBeIncodingSuccess();
    }
}