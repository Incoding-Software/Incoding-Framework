namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingResult))]
    public class When_incoding_result_to_string
    {
        Establish establish = () => { result = IncodingResult.Success(Pleasure.Generator.TheSameString()); };

        Because of = () => { toString = result.ToString(); };

        It should_be_to_string = () => toString.ShouldEqual("{\"success\":true,\"data\":\"TheSameString\",\"redirectTo\":\"\",\"statusCode\":200}");

        #region Establish value

        static IncodingResult result;

        static string toString;

        #endregion
    }
}