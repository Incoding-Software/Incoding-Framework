namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SelectListExtensions))]
    public class When_select_list_extensions
    {
        #region Fake classes

        public class FakeData
        {
            #region Properties

            public string Value { get; set; }

            public string Text { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static List<FakeData> items;

        #endregion

        Establish establish = () =>
                                  {
                                      items = new List<FakeData>
                                                  {
                                                          Pleasure.Generator.Invent<FakeData>(),
                                                          Pleasure.Generator.Invent<FakeData>()
                                                  };
                                  };
        
        It should_be_to_key_value_vm = () => items.ToKeyValueVm(data => data.Value, data => data.Text)
                                                  .ToList()
                                                  .Should(vms =>
                                                              {
                                                                  vms.Count.ShouldEqual(2);
                                                                  vms[0].Value = items[0].Value;
                                                                  vms[0].Text = items[0].Text;
                                                                  vms[0].Selected.ShouldBeFalse();
                                                              });

        It should_be_to_key_value_vm_only_value = () => items.ToKeyValueVm(data => data.Value)
                                                             .ToList()
                                                             .Should(vms =>
                                                                         {
                                                                             vms.Count.ShouldEqual(2);
                                                                             vms[0].Value = items[0].Value;
                                                                             vms[0].Text = items[0].Value;
                                                                             vms[0].Selected.ShouldBeFalse();
                                                                         });

        It should_be_to_select_list = () => items.ToSelectList(r => r.Value, r => r.Text)
                                                 .Should(list =>
                                                             {
                                                                 list.Count().ShouldEqual(2);
                                                                 list.Any(r => r.Selected).ShouldBeFalse();
                                                             });

        It should_be_enum_to_select_list = () => typeof(FakeEnum)
                                                         .ToSelectList()
                                                         .Should(list =>
                                                                     {
                                                                         list.Count().ShouldEqual(3);
                                                                         list.SelectedValue.ShouldBeNull();
                                                                     });

        It should_be_enum_to_select_list_with_selected = () => typeof(FakeEnum)
                                                                       .ToSelectList(FakeEnum.En2)
                                                                       .Should(list =>
                                                                                   {
                                                                                       list.Count().ShouldEqual(3);
                                                                                       list.SelectedValue.ShouldEqual(FakeEnum.En2.ToString());
                                                                                   });

        It should_be_enum_to_select_list_with_all_parameters = () => typeof(FakeEnum).ToSelectList(FakeEnum.En2, defaultOption: new KeyValueVm("All"))
                                                                                     .Should(list =>
                                                                                                 {
                                                                                                     list.Count().ShouldEqual(4);
                                                                                                     list.SelectedValue.ShouldEqual(FakeEnum.En2.ToString());
                                                                                                 });

        It should_be_key_value_vm_to_select_list = () => Pleasure.ToList(new KeyValueVm("Value", "Text", true), new KeyValueVm("ValueNotSelected", "Text"))
                                                                 .ToSelectList()
                                                                 .Should(list =>
                                                                             {
                                                                                 list.Count().ShouldEqual(2);
                                                                                 list.Count(item => item.Selected && !item.Value.Equals("ValueNotSelected", StringComparison.InvariantCultureIgnoreCase)).ShouldEqual(1);
                                                                             });

        It should_be_key_value_vm_to_select_list_with_empty_source = () => Pleasure.ToList<KeyValueVm>()
                                                                                   .ToSelectList()
                                                                                   .ShouldBeEmpty();

        It should_be_key_value_vm_to_select_list_without_selected = () => Pleasure.ToList(new KeyValueVm("Value", "Text"), new KeyValueVm("Value2", "Text"))
                                                                                  .ToSelectList()
                                                                                  .Should(list =>
                                                                                              {
                                                                                                  list.Count().ShouldEqual(2);
                                                                                                  list.Count(item => item.Selected).ShouldEqual(0);
                                                                                              });
    }
}