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
    public class When_collection_to_select_list_with_all_parameter
    {
        #region Estabilish value

        static List<FakeItemCollection> original;

        static FakeItemCollection defaultValue;

        static SelectList result;

        #endregion

        Establish establish = () =>
                                  {
                                      original = Pleasure.ToList(new FakeItemCollection { Value = "1" }, new FakeItemCollection { Value = "2" });
                                      defaultValue = new FakeItemCollection { Value = "3", Text = "Default" };
                                  };

        Because of = () => { result = original.ToSelectList(r => r.Value, r => r.Text, 2, defaultValue); };

        It should_be_has_selected = () => result.Any(r => r.Selected && r.Value.Equals("2")).ShouldBeTrue();

        It should_be_has_default = () => result.Any(r => r.Value.Equals(defaultValue.Value) && r.Text.Equals(defaultValue.Text)).ShouldBeTrue();

        It should_be_verify_count = () => result.Count().ShouldEqual(3);
    }
}