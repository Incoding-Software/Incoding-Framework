namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(OptGroupVm))]
    public class When_opt_group_vm_create_by_ctor
    {
        #region Establish value

        static List<KeyValueVm> items;

        #endregion

        Establish establish = () => { items = Pleasure.ToList(Pleasure.Generator.Invent<KeyValueVm>()); };

        It should_be_create = () => new OptGroupVm(Pleasure.Generator.TheSameString(), items)
                                            .Should(vm =>
                                                        {
                                                            vm.Title.ShouldBeTheSameString();
                                                            vm.Items.ShouldEqualWeak(items);
                                                        });

        It should_be_create_without_title = () => new OptGroupVm(items)
                                                          .Should(vm =>
                                                                      {
                                                                          vm.Title.ShouldBeNull();
                                                                          vm.Items.ShouldEqualWeak(items);
                                                                      });

        It should_be_create_with_empty_title = () => new OptGroupVm(string.Empty, items)
                                                             .Should(vm =>
                                                                         {
                                                                             vm.Title.ShouldBeNull();
                                                                             vm.Items.ShouldEqualWeakEach(items);
                                                                         });

        It should_be_to_opt_group_vm = () => items.ToOptGroup(Pleasure.Generator.TheSameString())
                                                  .Should(vm =>
                                                              {
                                                                  vm.Items.Count().ShouldEqual(1);
                                                                  vm.Title = Pleasure.Generator.TheSameString();
                                                              });
    }
}