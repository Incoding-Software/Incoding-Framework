namespace Incoding.UnitTest.KnowleadgeGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Machine.Specifications;

    #endregion

    [Subject(""), Tags("Knowledge")]
    public class When_understand_select_with_null_collection
    {
        #region Establish value

        static IEnumerable<int> nullCollection;

        static IEnumerable<string> newCollection;

        static Exception exception;

        #endregion

        Establish establish = () => { nullCollection = null; };

        Because of = () => { exception = Catch.Exception(() => { newCollection = nullCollection.Select(r => r.ToString()); }); };

        It should_be_with_exception = () => exception.ShouldBeAssignableTo<ArgumentException>();

        It should_be_new_collection_null = () => newCollection.ShouldBeNull();
    }
}