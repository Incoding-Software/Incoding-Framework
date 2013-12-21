namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(StringExtensions))]
    public class When_string_extensions
    {
        It should_be_format = () => "{0}-{1}".F(1, 2).ShouldEqual("1-2");

        It should_be_is_guid = () => "79C1B0D8-CA0E-46E5-8275-959C9FB38A95".IsGuid()
                                                                           .ShouldBeTrue();

        It should_be_is_guid_wrong = () => Pleasure.Generator.TheSameString()
                                                   .IsGuid()
                                                   .ShouldBeFalse();

        It should_be_equal_with_invariant = () => Pleasure.Generator.TheSameString().EqualsWithInvariant(Pleasure.Generator.TheSameString().InverseCase()).ShouldBeTrue();

        It should_be_equal_with_invariant_with_wrong = () => Pleasure.Generator.TheSameString().EqualsWithInvariant(Pleasure.Generator.TheSameString().Inverse()).ShouldBeFalse();

        It should_be_to_mvc_html_string = () => Pleasure.Generator.TheSameString()
                                                        .ToMvcHtmlString()
                                                        .ToHtmlString()
                                                        .ShouldEqual(Pleasure.Generator.TheSameString());
    }
}