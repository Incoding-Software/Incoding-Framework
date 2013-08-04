namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingResult))]
    public class When_incoding_result_error : BehaviorsJsonResultSpec
    {
        Because of = () => { result = IncodingResult.Error(); };

        Behaves_like<BehaviorsJsonResultSpec> should_be_verify_common;

        It should_be_verify_result = () =>
                                         {
                                             var data = result.Data as IncodingResult.JsonData;
                                             data.success.ShouldBeFalse();
                                             data.data.ShouldBeNull();
                                             data.redirectTo.ShouldBeEmpty();
                                         };
    }
}