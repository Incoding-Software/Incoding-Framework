namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Linq;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SelectListExtensions))]
    public class When_enum_to_select_list_with_selected
    {
        #region Estabilish value

        static SelectList result;

        #endregion

        Because of = () => { result = typeof(FakeEnum).ToSelectList(2); };

        It should_be_has_3_item_and = () => result.Count().ShouldEqual(3);

        It should_be_has_selected = () => result.Count(item => item.Selected).ShouldEqual(1);

        It should_be_has_correct_selected = () => result.FirstOrDefault(item => item.Text == "En2" && item.Value == "2" && item.Selected).ShouldNotBeNull();
    }
}