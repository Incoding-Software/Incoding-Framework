namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CreateByTypeQuery))]
    public class When_create_by_type_with_empty_inc_types 
    {
        #region Establish value

        static Exception exception;

        #endregion

        private static MockMessage<CreateByTypeQuery, object> mockQuery;

        Establish establish = () =>
        {
            var query = Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, string.Empty));
            mockQuery = MockQuery<CreateByTypeQuery, object>
                    .When(query);
        };

        Because of = () => { exception = Catch.Exception(() => mockQuery.Execute()); };

        It should_be_exception = () => exception.Message.ShouldEqual("Ambiguous type AmbiguousType");
    }
}