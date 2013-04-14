namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Maybe;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(MaybeEnumerable))]
    public class When_maybe_enumerable : Context_maybe
    {
        #region Estabilish value

        static IEnumerable<FakeMaybe> collection;

        static IEnumerable<FakeMaybe> nullCollection;

        #endregion

        Establish establish = () =>
                                  {
                                      collection = Pleasure.ToEnumerable(Pleasure.Generator.Invent<FakeMaybe>(), Pleasure.Generator.Invent<FakeMaybe>());
                                      nullCollection = null;
                                  };

        It should_be_do_each_for_collection = () =>
                                                  {
                                                      int doForEach = 0;
                                                      collection.DoEach(r => doForEach++);
                                                      doForEach.ShouldEqual(collection.Count());
                                                  };

        It should_be_do_each_for_null_collection = () =>
                                                       {
                                                           int doForEach = 0;
                                                           nullCollection.DoEach(r => doForEach++);
                                                           doForEach.ShouldEqual(0);
                                                       };

        It should_be_with_each_for_collection = () => collection.WithEach(r => r.Prop).Count().ShouldEqual(2);

        It should_be_with_each_for_null_collection = () => nullCollection.WithEach(r => r.Prop).ShouldBeNull();

        It should_be_with_each_for_empty_collection = () => Pleasure.ToEnumerable<FakeMaybe>().WithEach(r => r.Prop).Count().ShouldEqual(0);
    }
}