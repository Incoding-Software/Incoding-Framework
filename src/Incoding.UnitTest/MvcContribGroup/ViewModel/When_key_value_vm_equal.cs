namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(KeyValueVm))]
    public class When_key_value_vm_equal
    {
        It should_be_equals = () => new KeyValueVm(1, "test", true).ShouldEqual(new KeyValueVm(1, "test", true));

        It should_be_not_equals = () => new KeyValueVm(10, Pleasure.Generator.TheSameString()).ShouldNotEqual(new KeyValueVm(10, Pleasure.Generator.TheSameString().Inverse()));
    }
}