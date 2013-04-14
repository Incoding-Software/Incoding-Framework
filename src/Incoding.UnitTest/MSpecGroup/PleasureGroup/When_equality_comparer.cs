namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(WeakEqualityComparer))]
    public class When_equality_comparer
    {
        It should_be_weak_equality = () => new WeakEqualityComparer().Equals(Pleasure.Generator.TheSameString(), Pleasure.Generator.TheSameString()).ShouldBeTrue();

        It should_not_be_weak_equality = () => new WeakEqualityComparer().Equals(Pleasure.Generator.String(), Pleasure.Generator.String()).ShouldBeFalse();
    }
}