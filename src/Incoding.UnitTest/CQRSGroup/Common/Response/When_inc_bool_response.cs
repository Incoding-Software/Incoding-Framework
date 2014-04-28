namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Machine.Specifications;

    #endregion

    // ReSharper disable ConditionIsAlwaysTrueOrFalse
    [Subject(typeof(IncBoolResponse))]
    public class When_inc_bool_response
    {
        It should_be_ctor = () => new IncBoolResponse(true).Value.ShouldBeTrue();

        It should_be_cast_to_bool = () => ((IncBoolResponse)true).Value.ShouldBeTrue();

        It should_be_cast_to_inc_bool_response = () => ((bool)new IncBoolResponse(false)).ShouldBeFalse();

        It should_be_equal_left_bool = () => (true == new IncBoolResponse(true)).ShouldBeTrue();

        It should_be_equal_right_bool = () => (new IncBoolResponse(true) == false).ShouldBeFalse();

        It should_not_be_equal_left_bool = () => (true != new IncBoolResponse(true)).ShouldBeFalse();

        It should_not_be_equal_right_bool = () => (new IncBoolResponse(true) != false).ShouldBeTrue();
    }
}