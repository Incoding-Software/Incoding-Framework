namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query_with_enable_validate : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeEnableValidateQuery : QueryBase<string>
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
                                      Establish(types: new[] { typeof(FakeEnableValidateQuery) });
                                      controller.ModelState.AddModelError("Fake", "Error");
                                  };

        Because of = () => { result = controller.Query(HttpUtility.UrlEncode(typeof(FakeEnableValidateQuery).FullName), true); };

        It should_be_result = () => result.ShouldBeIncodingFail<IEnumerable<IncodingResult.JsonModelStateData>>(state => state.ShouldNotBeEmpty());
    }
}