namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncodingResult))]
    public class When_incoding_result_error_with_data : BehaviorsJsonResultSpec
    {
        Because of = () => { result = IncodingResult.Error(Pleasure.Generator.TheSameString()); };

        Behaves_like<BehaviorsJsonResultSpec> should_be_verify_common;

        It should_be_verify_map_collection = () =>
                                                 {
                                                     var data = result.Data as IncodingResult.JsonData;
                                                     data.success.ShouldBeFalse();
                                                     data.data.ShouldEqual(Pleasure.Generator.TheSameString());
                                                     data.redirectTo.ShouldBeEmpty();
                                                 };
    }
}