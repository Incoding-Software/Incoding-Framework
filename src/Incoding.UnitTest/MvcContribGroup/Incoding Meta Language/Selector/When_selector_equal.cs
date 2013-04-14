namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    // ReSharper disable ConditionIsAlwaysTrueOrFalse
    [Subject(typeof(Selector))]
    public class When_selector_equal
    {
        It should_be_selector_bool = () => (Selector.Value(5) == true).ShouldBeTrue();

        It should_be_bool_selector = () => (true == Selector.Value(5)).ShouldBeTrue();

        It should_not_be_selector_bool = () => (Selector.Value(5) != true).ShouldBeFalse();

        It should_not_be_bool_selector = () => (true != Selector.Value(5)).ShouldBeFalse();

        It should_be_selector_int = () => (Selector.Value(5) == 5).ShouldBeTrue();

        It should_be_int_selector = () => (5 == Selector.Value(5)).ShouldBeTrue();

        It should_not_be_selector_int = () => (Selector.Value(5) != 5).ShouldBeFalse();

        It should_not_be_int_selector = () => (5 != Selector.Value(5)).ShouldBeFalse();

        It should_be_selector_null = () => (Selector.Value(5) == null).ShouldBeFalse();

        It should_be_null_selector = () => (null == Selector.Value(5)).ShouldBeFalse();

        It should_not_be_selector_null = () => (Selector.Value(5) != null).ShouldBeTrue();

        It should_not_be_null_selector = () => (null != Selector.Value(5)).ShouldBeTrue();

        It should_be_selector_string = () => (Selector.Value(5) == string.Empty).ShouldBeFalse();

        It should_be_string_selector = () => (string.Empty == Selector.Value(5)).ShouldBeFalse();

        It should_be_selector_string_success = () => (Selector.Value(5) == "'5'").ShouldBeTrue();

        It should_be_string_selector_success = () => ("'5'" == Selector.Value(5)).ShouldBeTrue();

        It should_not_be_selector_string = () => (Selector.Value(5) != string.Empty).ShouldBeTrue();

        It should_not_be_string_selector = () => (string.Empty != Selector.Value(5)).ShouldBeTrue();

        It should_be_selector_decimal = () => (Selector.Value(5) == new decimal(5)).ShouldBeTrue();

        It should_be_decimal_selector = () => (new decimal(5) == Selector.Value(5)).ShouldBeTrue();

        It should_not_be_selector_decimal = () => (Selector.Value(5) != new decimal(5)).ShouldBeFalse();

        It should_not_be_decimal_selector = () => (new decimal(5) != Selector.Value(5)).ShouldBeFalse();
    }

    // ReSharper restore ConditionIsAlwaysTrueOrFalse
}