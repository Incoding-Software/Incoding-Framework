namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(SelectListExtensions))]
    public class When_list_key_value_to_select_list
    {
        #region Estabilish value

        static SelectList result;

        #endregion

        Establish establish = () => { };

        Because of = () => { result = Pleasure.ToList(new KeyValueVm("Value", "Text", true), new KeyValueVm("ValueNotSelected", "Text")).ToSelectList(); };

        It should_be_has_2_item = () => result.Count().ShouldEqual(2);

        It should_be_has_one_selected = () => result.Count(item => item.Selected).ShouldEqual(1);

        It should_be_correct_selected = () => result.FirstOrDefault(item => item.Selected && !item.Value.Equals("ValueNotSelected", StringComparison.InvariantCultureIgnoreCase)).ShouldNotBeNull();
    }
}