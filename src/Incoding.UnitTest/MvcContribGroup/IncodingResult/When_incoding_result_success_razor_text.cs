namespace Incoding.UnitTest.MvcContribGroup
{
    using System.Web.WebPages;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    [Subject(typeof(IncodingResult))]
    public class When_incoding_result_success_razor_text : BehaviorsJsonResultSpec
    {
        Because of = () => { result = IncodingResult.Success(o => new HelperResult(writer => writer.WriteLine("<div>"))); };

        Behaves_like<BehaviorsJsonResultSpec> should_be_verify_common;

        It should_be_verify_result = () =>
                                         {
                                             var data = result.Data as IncodingResult.JsonData;
                                             data.success.ShouldBeTrue();
                                             data.data.ShouldEqual("<div>");
                                             data.redirectTo.ShouldBeEmpty();
                                         };
    }
}