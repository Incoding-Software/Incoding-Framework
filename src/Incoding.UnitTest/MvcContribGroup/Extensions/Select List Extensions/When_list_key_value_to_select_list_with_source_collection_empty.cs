namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Linq;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(SelectListExtensions))]
    public class When_list_key_value_to_select_list_with_source_collection_empty
    {
        #region Estabilish value

        static SelectList result;

        #endregion

        Because of = () => { result = Pleasure.ToList<KeyValueVm>().ToSelectList(); };

        It should_be_has_0_item = () => result.Count().ShouldEqual(0);
    }
}