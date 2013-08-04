namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SelectListExtensions))]
    public class When_select_list_to_key_values
    {
        #region Establish value

        static SelectList selectList;

        static List<KeyValueVm> expected;

        static IEnumerable<KeyValueVm> actual;

        #endregion

        Establish establish = () =>
                                  {
                                      expected = Pleasure.ToList(Pleasure.Generator.Invent<KeyValueVm>());

                                      selectList = new SelectList(expected, "Value", "Text");
                                  };

        Because of = () => { actual = selectList.ToKeyValues(); };

        It should_be_compare = () => actual.ShouldEqualWeakEach(expected, (dsl, i) => dsl.ForwardToValue(r => r.Title, null)
                                                                                         .ForwardToValue(r => r.Selected, false));
    }
}