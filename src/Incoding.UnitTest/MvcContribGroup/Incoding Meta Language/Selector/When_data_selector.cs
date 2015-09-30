namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ResultSelector))]
    public class When_result_selector
    {
        #region Fake classes

        class FakeModel
        {
            #region Properties

            public FakeModel Inner { get; set; }

            public string Reference { get; set; }

            public List<string> Strings { get; set; }

            public List<KeyValueVm> Many { get; set; }

            #endregion
        }

        #endregion

        It should_be_data = () => Selector.Result
                                          .ToString()
                                          .ShouldEqual("||result*||");

        It should_be_data_for = () => Selector.Result.For<FakeModel>(r => r.Reference)
                                              .ToString()
                                              .ShouldEqual("||result*Reference||");

        It should_be_data_for_array_as_self = () => Selector.Result.For<FakeModel[]>(r => r[0])
                                                            .ToString()
                                                            .ShouldEqual("||result*[0]||");

        It should_be_data_for_array_by_index = () => Selector.Result.For<FakeModel>(r => r.Many[0].Value)
                                                             .ToString()
                                                             .ShouldEqual("||result*Many[0].Value||");

        It should_be_data_for_array_select_many = () => Selector.Result.For<FakeModel>(r => r.Many.Select(s => s.Value))
                                                                .ToString()
                                                                .ShouldEqual("||result*Many.Select(Value)||");

        It should_be_data_for_array_string = () => Selector.Result.For<FakeModel>(model => model.Strings)
                                                           .ToString()
                                                           .ShouldEqual("||result*Strings||");

        It should_be_data_for_as_string = () => Selector.Result.For("property")
                                                        .ToString()
                                                        .ShouldEqual("||result*property||");

        It should_be_data_for_complexity = () => Selector.Result.For<FakeModel>(r => r.Inner.Reference)
                                                         .ToString()
                                                         .ShouldEqual("||result*Inner.Reference||");

        It should_be_data_for_array_first_or_default = () => Selector.Result.For<FakeModel>(r => r.Many.FirstOrDefault())
                                                                     .ToString()
                                                                     .ShouldEqual("||result*Many[0]||");

        It should_be_data_for_array_first = () => Selector.Result.For<FakeModel>(r => r.Many.First())
                                                          .ToString()
                                                          .ShouldEqual("||result*Many[0]||");

        It should_be_data_for_array_element_at = () => Selector.Result.For<FakeModel>(r => r.Many.ElementAt(5))
                                                               .ToString()
                                                               .ShouldEqual("||result*Many[5]||");

        It should_be_data_for_array_element_at_or_default = () => Selector.Result.For<FakeModel>(r => r.Many.ElementAtOrDefault(5))
                                                                          .ToString()
                                                                          .ShouldEqual("||result*Many[5]||");

        It should_be_data_for_array_any = () => Selector.Result.For<FakeModel>(r => r.Many.Any(vm => vm.CssClass == Selector.Jquery.Id("Id")))
                                                        .ToString()
                                                        .ShouldEqual("||result*Many.Any(||result*CssClass|| equal $('#Id') False)||");

        It should_be_data_for_array_any_single = () => Selector.Result.For<FakeModel>(r => r.Many.Any(vm => vm.Selected))
                                                               .ToString()
                                                               .ShouldEqual("||result*Many.Any(||value*True|| equal ||result*Selected|| False)||");

        It should_be_data_for_array_any_constant = () => Selector.Result.For<FakeModel>(r => r.Many.Any(vm => vm.Selected == false))
                                                                 .ToString()
                                                                 .ShouldEqual("||result*Many.Any(||result*Selected|| equal ||value*False|| False)||");
    }
}