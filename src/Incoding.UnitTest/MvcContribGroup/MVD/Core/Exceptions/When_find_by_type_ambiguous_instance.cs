namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Ambiguous;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CreateByTypeQuery.FindTypeByName))]
    public class When_find_by_type_ambiguous_instance
    {
        #region Establish value

        static Exception exception;

        #endregion

        private static MockMessage<CreateByTypeQuery.FindTypeByName, object> mockQuery;

        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<CreateByTypeQuery.FindTypeByName>(dsl => dsl.Tuning(r => r.Type, typeof(AmbiguousType).Name));
                                  mockQuery = MockQuery<CreateByTypeQuery.FindTypeByName, object>
                                          .When(query);
                              };

        Because of = () => { exception = Catch.Exception(() => mockQuery.Execute()); };

        It should_be_exception = () => exception.Message.ShouldEqual("Ambiguous type AmbiguousType");
    }
}