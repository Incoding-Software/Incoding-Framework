namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(EqualsExtensions))]
    public class When_equals_extensions
    {
        #region Estabilish value

        static string theSameString = Pleasure.Generator.TheSameString();

        #endregion

        It should_be_any_contains = () => theSameString.IsAnyContains(theSameString.CutPart(), theSameString.Inverse()).ShouldBeTrue();

        It should_be_any_contains_false = () => theSameString.IsAnyContains(theSameString.Inverse()).ShouldBeFalse();

        It should_be_any_contains_with_considering_case = () => theSameString.IsAnyContains(theSameString.InverseCase()).ShouldBeFalse();

        It should_be_any_contains_with_ignore_case = () => theSameString.IsAnyContainsIgnoreCase(theSameString.InverseCase()).ShouldBeTrue();

        It should_be_any_equal = () => theSameString.IsAnyEquals(theSameString, theSameString.Inverse()).ShouldBeTrue();

        It should_be_any_equal_with_considering_case = () => theSameString.IsAnyEquals(theSameString.InverseCase()).ShouldBeFalse();

        It should_be_any_equal_false = () => theSameString.IsAnyEquals(theSameString.CutPart()).ShouldBeFalse();

        It should_be_any_equal_with_ignore_case = () => theSameString.IsAnyEqualsIgnoreCase(theSameString.ToLower(), theSameString.ToUpper()).ShouldBeTrue();

        It should_be_reference_equal = () => theSameString.IsReferenceEquals(theSameString);

        It should_be_reference_equal_with_object_to_null = () => theSameString.IsReferenceEquals(null);

        It should_be_reference_equal_with_null_to_null = () => EqualsExtensions.IsReferenceEquals(null, null);

        It should_be_all_contains = () => theSameString.IsAllContains(theSameString.CutPart(), theSameString).ShouldBeTrue();

        It should_be_all_contains_false = () => theSameString.IsAllContains(theSameString.CutPart(), theSameString, theSameString.Inverse()).ShouldBeFalse();

        It should_be_all_contains_considering_case = () => theSameString.IsAllContains(theSameString.InverseCase());

        It should_be_all_contains_with_ignore_case = () => theSameString.IsAllContainsIgnoreCase(theSameString.InverseCase());
    }
}