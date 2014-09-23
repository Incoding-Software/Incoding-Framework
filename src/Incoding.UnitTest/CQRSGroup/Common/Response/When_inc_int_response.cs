namespace Incoding.UnitTest
{
    using Incoding.CQRS;
    using Machine.Specifications;

    [Subject(typeof(IncIntResponse))]
    public class When_inc_int_response
    {
        It should_be_ctor = () => new IncIntResponse(5).Value.ShouldEqual(5);

        It should_be_cast_to_bool = () => ((IncIntResponse)5).Value.ShouldEqual(5);

        It should_be_cast_to_inc_bool_response = () => ((int)new IncIntResponse(5)).ShouldEqual(5);

        It should_be_equal_left_bool = () => (5 == new IncIntResponse(5)).ShouldBeTrue();

        It should_be_equal_right_bool = () => (new IncIntResponse(8) == 5).ShouldBeFalse();

        It should_not_be_equal_left_bool = () => (5 != new IncIntResponse(5)).ShouldBeFalse();

        It should_not_be_equal_right_bool = () => (new IncIntResponse(5) != 6).ShouldBeTrue();
    }
}