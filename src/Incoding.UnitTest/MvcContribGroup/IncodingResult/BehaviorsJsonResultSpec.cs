namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Behaviors]
    public class BehaviorsJsonResultSpec
    {
        #region Estbilish value

        protected static IncodingResult result;

        #endregion

        It should_be_json_data_not_null = () =>
                                              {
                                                  var defaultJsonData = result.Data as IncodingResult.JsonData;
                                                  defaultJsonData.ShouldNotBeNull();
                                              };
    }
}