namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncSelectList))]
    public class When_inc_select_list_cast
    {
        It should_be_cast_to_key_value_vms_as_array = () =>
                                                      {
                                                          var vm = Pleasure.Generator.Invent<KeyValueVm[]>();
                                                          IncSelectList list = vm;
                                                          list.AsSelectList.Items.ShouldEqualWeak(vm);
                                                      };

        It should_be_cast_to_key_value_vms_as_list = () =>
                                                     {
                                                         var vm = Pleasure.Generator.Invent<List<KeyValueVm>>();
                                                         IncSelectList list = vm;
                                                         list.AsSelectList.Items.ShouldEqualWeak(vm);
                                                     };

        It should_be_cast_to_opt_group_vm = () =>
                                            {
                                                var vm = new OptGroupVm(Pleasure.Generator.Invent<KeyValueVm[]>());
                                                IncSelectList list = vm;
                                                list.AsSelectList.Items.ShouldEqualWeak(vm.Items);
                                            };

        It should_be_cast_to_string = () =>
                                      {
                                          var url = Pleasure.Generator.Url();
                                          IncSelectList selectList = url;
                                          selectList.Url.ShouldEqual(url);
                                      };

        It should_be_cast_to_strings = () =>
                                       {
                                           var strings = Pleasure.Generator.Invent<string[]>();
                                           IncSelectList list = strings;
                                           list.AsSelectList.Items.ShouldEqualWeak(strings.Select(r => new KeyValueVm(r)));
                                       };

        It should_be_casts_to_select_list = () =>
                                            {
                                                var strings = Pleasure.Generator.Invent<string[]>();
                                                var select = new SelectList(strings);
                                                IncSelectList list = select;
                                                list.AsSelectList.Items.ShouldEqualWeak(strings.Select(r => new KeyValueVm(r)));
                                            };
    }
}