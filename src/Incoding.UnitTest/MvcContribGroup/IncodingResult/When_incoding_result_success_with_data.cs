namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingResult))]
    public class When_incoding_result_success_with_data : BehaviorsJsonResultSpec
    {
        Establish establish = () => { };

        Because of = () => { result = IncodingResult.Success("Data"); };

        Behaves_like<BehaviorsJsonResultSpec> should_be_verify_common;

        It should_be_verify_result = () =>
                                         {
                                             var data = result.Data as IncodingResult.JsonData;
                                             data.success.ShouldBeTrue();
                                             data.data.ShouldEqual("Data");
                                             data.redirectTo.ShouldBeEmpty();
                                         };
    }
}