namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Maybe;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MaybeObject))]
    public class When_maybe_enumerable : Context_maybe
    {
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

        It should_be_do_each_performance = () =>
                                           {
                                               var countDo = 10000;
                                               Pleasure.Do(i => collection.DoEach(maybe => { }), countDo).ShouldBeLessThan(15);
                                           };

        #region Establish value

        static IEnumerable<FakeMaybe> collection;

        static IEnumerable<FakeMaybe> nullCollection;

        #endregion
    }
}