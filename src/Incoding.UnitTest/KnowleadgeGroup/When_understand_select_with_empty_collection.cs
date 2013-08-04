namespace Incoding.UnitTest.KnowleadgeGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(""), Tags("Knowleadge")]
    public class When_understand_select_with_empty_collection
    {
        #region Estabilish value

        static IEnumerable<int> emptyCollection;

        static Exception exception;

        static IEnumerable<string> newCollection;

        #endregion

        Establish establish = () => { emptyCollection = Pleasure.ToEnumerable<int>(); };

        Because of = () => { exception = Catch.Exception(() => { newCollection = emptyCollection.Select(r => r.ToString()); }); };

        It should_be_without_exception = () => exception.ShouldBeNull();

        It should_be_new_collection_empty = () => newCollection.ShouldBeEmpty();
    }
}