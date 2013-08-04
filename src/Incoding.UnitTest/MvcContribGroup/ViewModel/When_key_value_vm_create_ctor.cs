namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(KeyValueVm))]
    public class When_key_value_vm_create_ctor
    {
        It should_be_value_as_text = () => new KeyValueVm(1).Should(vm =>
                                                                        {
                                                                            vm.Value.ShouldEqual(1.ToString());
                                                                            vm.Text.ShouldEqual(1.ToString());
                                                                            vm.Selected.ShouldBeFalse();
                                                                        });

        It should_be_selected = () => new KeyValueVm(1, 1.ToString(), true).Should(vm =>
                                                                                       {
                                                                                           vm.Value.ShouldEqual(1.ToString());
                                                                                           vm.Text.ShouldEqual(1.ToString());
                                                                                           vm.Selected.ShouldBeTrue();
                                                                                       });
    }
}