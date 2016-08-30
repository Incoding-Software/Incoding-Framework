namespace Incoding.UnitTest.MvcContribGroup
{
    using Machine.Specifications;

    [Subject(typeof(bool))]
    public class When_sharp_analog
    {
        It should_be_or_and_and = () => (true || true && false).ShouldBeTrue();

        It should_be_or_and_and_cover_logic = () => ( (true || true) && false).ShouldBeFalse(); //-V3001

        It should_be_and_and_or = () => (false && false || true).ShouldBeTrue(); //-V3001

        It should_be_and_and_or_cover_logic = () => (false && (false || true)).ShouldBeFalse(); //-V3017

    }
}