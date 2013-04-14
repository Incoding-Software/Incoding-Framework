namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Linq;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(SelectListExtensions))]
    public class When_list_key_value_to_select_list_without_any_selected
    {
        #region Estabilish value

        static SelectList result;

        #endregion

        Establish establish = () => { };

        Because of = () => { result = Pleasure.ToList(new KeyValueVm("Value", "Text"), new KeyValueVm("Value2", "Text")).ToSelectList(); };

        It should_be_has_2 = () => result.Count().ShouldEqual(2);

        It should_be_has_one_selected = () => result.Count(item => item.Selected).ShouldEqual(0);
    }
}