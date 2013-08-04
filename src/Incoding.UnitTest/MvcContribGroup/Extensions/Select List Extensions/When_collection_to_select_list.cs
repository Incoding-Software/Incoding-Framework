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

    [Subject(typeof(SelectListExtensions))]
    public class When_collection_to_select_list
    {
        #region Estabilish value

        static List<FakeItemCollection> original;

        static SelectList result;

        #endregion

        Establish establish = () => { original = Pleasure.ToList(new FakeItemCollection(), new FakeItemCollection()); };

        Because of = () => { result = original.ToSelectList(r => r.Value, r => r.Text); };

        It should_be_has_2_item = () => result.Count().ShouldEqual(2);

        It should_be_not_selected = () => result.Any(r => r.Selected).ShouldBeFalse();
    }
}