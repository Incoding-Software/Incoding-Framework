namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Linq;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SelectListExtensions))]
    public class When_enum_to_select_list
    {
        #region Estabilish value

        static SelectList result;

        #endregion

        Because of = () => { result = typeof(FakeEnum).ToSelectList(); };

        It should_be_not_has_selected = () => result.Count(item => item.Selected).ShouldEqual(0);

        It should_be_3_item = () => result.Count().ShouldEqual(3);

        It should_be_with_description = () => result.FirstOrDefault(r => r.Text == "En 3").ShouldNotBeNull();

        It should_be_has_correct_value = () => result.FirstOrDefault(item => item.Text == "En1" && item.Value == "1").ShouldNotBeNull();
    }
}