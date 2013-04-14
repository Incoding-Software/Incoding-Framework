namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(OptGroupVm))]
    public class When_opt_group_vm_create_by_ctor
    {
        It should_be_create = () => new OptGroupVm(Pleasure.Generator.TheSameString(), Pleasure.ToList(Pleasure.Generator.Invent<KeyValueVm>()))
                                            .Should(vm =>
                                                        {
                                                            vm.Title.ShouldEqual(Pleasure.Generator.TheSameString());
                                                            vm.Items.Count.ShouldEqual(1);
                                                        });

        It should_be_create_with_out_title = () => new OptGroupVm(Pleasure.ToList(Pleasure.Generator.Invent<KeyValueVm>()))
                                                           .Should(vm =>
                                                                       {
                                                                           vm.Title.ShouldBeNull();
                                                                           vm.Items.Count.ShouldEqual(1);
                                                                       });

        It should_be_create_with_empty_title = () => new OptGroupVm(string.Empty, Pleasure.ToList(Pleasure.Generator.Invent<KeyValueVm>()))
                                                             .Should(vm =>
                                                                         {
                                                                             vm.Title.ShouldBeNull();
                                                                             vm.Items.Count.ShouldEqual(1);
                                                                         });
    }
}