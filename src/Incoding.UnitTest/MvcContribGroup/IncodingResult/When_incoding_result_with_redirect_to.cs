namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingResult))]
    public class When_incoding_result_with_redirect_to : BehaviorsJsonResultSpec
    {
        #region Establish value

        static string url;

        #endregion

        Because of = () =>
                         {
                             url = "http://localhost:18302/?redirectTo=http//:localhost18302/AccountManager&actionSelector=btnSignIn";
                             result = IncodingResult.RedirectTo(url);
                         };

        Behaves_like<BehaviorsJsonResultSpec> should_be_verify_common;

        It should_be_encode_url = () =>
                                      {
                                          var jsonData = result.Data as IncodingResult.JsonData;
                                          jsonData.success.ShouldBeTrue();
                                          (jsonData.data as string).ShouldBeEmpty();
                                          jsonData.redirectTo.ShouldEqual(url);
                                      };
    }
}