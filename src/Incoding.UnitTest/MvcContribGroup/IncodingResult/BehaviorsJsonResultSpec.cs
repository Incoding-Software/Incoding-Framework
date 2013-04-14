namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Behaviors]
    public class BehaviorsJsonResultSpec
    {
        #region Estbilish value

        protected static JsonResult result;

        #endregion

        It should_be_json_data_not_null = () =>
                                              {
                                                  var defaultJsonData = (result as IncodingResult).Data as IncodingResult.JsonData;
                                                  defaultJsonData.ShouldNotBeNull();
                                              };

        It should_be_allow_get = () => result.JsonRequestBehavior.ShouldEqual(JsonRequestBehavior.AllowGet);
    }
}