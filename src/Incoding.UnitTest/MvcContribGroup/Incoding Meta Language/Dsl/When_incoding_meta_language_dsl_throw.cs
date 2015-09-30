namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaLanguageDsl))]
    public class When_incoding_meta_language_dsl_throw
    {
        It should_be_to_string = () =>
                                 {
                                     Catch.Exception(() => new IncodingMetaLanguageDsl(JqueryBind.Change)
                                                                   .ToString())
                                          .ShouldNotBeNull();
                                 };
    }
}