namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(Pleasure))]
    public class When_pleasure_quick_method
    {
        It should_be_list = () => Pleasure.ToList(new FakeSerializeObject(), new FakeSerializeObject()).Count.ShouldEqual(2);

        It should_be_empty_list = () => Pleasure.ToList<string>().ShouldNotBeNull();

        It should_be_array = () => Pleasure.ToArray(1, 2).Length.ShouldEqual(2);

        It should_be_empty_array = () => Pleasure.ToArray<string>().ShouldNotBeNull();

        It should_be_dictionary = () =>
                                      {
                                          var pair = new KeyValuePair<string, int>(Pleasure.Generator.String(), Pleasure.Generator.PositiveNumber());
                                          Pleasure.ToDictionary(pair).ShouldBeKeyValue(pair.Key, pair.Value);
                                      };

        It should_be_empty_dictionary = () => Pleasure.ToDictionary<string, string>().ShouldBeEmpty();

        It should_be_dictionary_with_anonymous = () => Pleasure.ToDynamicDictionary<int>(new { Item = 2 }).ShouldBeKeyValue("Item", 2);

        It should_be_read_only_list = () => Pleasure.ToReadOnly(new FakeSerializeObject(), new FakeSerializeObject()).Count.ShouldEqual(2);

        It should_be_empty_read_only_list = () => Pleasure.ToReadOnly<string>().ShouldNotBeNull();

        It should_be_enumerable = () => Pleasure.ToEnumerable(1, 2).Count().ShouldEqual(2);

        It should_be_empty_enumerable = () => Pleasure.ToEnumerable<int>().ShouldNotBeNull();

        It should_be_queryable = () => Pleasure.ToQueryable(4, 5).Count().ShouldEqual(2);

        It should_be_empty_queryable = () => Pleasure.ToQueryable<int>().ShouldNotBeNull();

        It should_be_mock_as_object = () => Pleasure.MockAsObject<IncEntityBase>(mock => mock.SetupGet(r => r.Id).Returns("Test")).Id.ShouldEqual("Test");
    }
}