namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(StringExtensions))]
    public class When_string_extensions
    {
        It should_be_is_email = () => Pleasure.Generator.Email().IsEmail().ShouldBeTrue();

        It should_be_is_email_with_wrong = () => Pleasure.Generator.String().IsEmail().ShouldBeFalse();

        It should_be_guid = () => Pleasure.Generator.GuidAsString().IsGuid().ShouldBeTrue();

        It should_be_guid_with_wrong = () => "416E4580/E79B-4D3A-B29F-C5548B3C96E5".IsGuid().ShouldBeFalse();

        It should_be_url = () => "http://sample.com".IsUrl().ShouldBeTrue();

        It should_be_url_with_wrong = () => Pleasure.Generator.String().IsUrl().ShouldBeFalse();

        It should_be_format = () =>
                                  {
                                      const string format = "{0}{1}";
                                      string.Format(format, 1, 2).ShouldEqual(format.F(1, 2));
                                  };

        It should_be_equal_with_invariant = () => Pleasure.Generator.TheSameString().EqualsWithInvariant(Pleasure.Generator.TheSameString().InverseCase()).ShouldBeTrue();

        It should_be_equal_with_invariant_with_wrong = () => Pleasure.Generator.TheSameString().EqualsWithInvariant(Pleasure.Generator.TheSameString().Inverse()).ShouldBeFalse();

        It should_be_to_mvc_html_string = () => Pleasure.Generator.TheSameString()
                                                        .ToMvcHtmlString()
                                                        .ToHtmlString()
                                                        .ShouldEqual(Pleasure.Generator.TheSameString());
    }
}