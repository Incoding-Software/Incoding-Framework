namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncodingResult))]
    public class When_incoding_result_success : BehaviorsJsonResultSpec
    {
        Because of = () => { result = IncodingResult.Success(); };

        Behaves_like<BehaviorsJsonResultSpec> should_be_verify_common;

        It should_be_verify_result = () =>
                                         {
                                             var data = result.Data as IncodingResult.JsonData;
                                             data.success.ShouldBeTrue();
                                             data.data.ShouldBeNull();
                                             data.redirectTo.ShouldBeEmpty();
                                         };
    }
}