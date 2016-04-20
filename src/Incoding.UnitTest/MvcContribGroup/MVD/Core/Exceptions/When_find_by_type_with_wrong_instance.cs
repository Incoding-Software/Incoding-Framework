namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    #endregion

    [Subject(typeof(CreateByTypeQuery.FindTypeByName))]
    public class When_find_by_type_with_wrong_instance
    {
        #region Establish value

        static MockMessage<CreateByTypeQuery.FindTypeByName, Result> mockQuery;

        #endregion

        private static Exception exception;

        Establish establish = () =>
                              {
                                  CreateByTypeQuery.FindTypeByName query = Pleasure.Generator.Invent<CreateByTypeQuery.FindTypeByName>(dsl => dsl.Tuning(r => r.Type, Pleasure.Generator.TheSameString()));

                                  mockQuery = MockQuery<CreateByTypeQuery.FindTypeByName, Result>
                                          .When(query);
                              };

        Because of = () => { exception = Catch.Exception(() => mockQuery.Execute()) as IncMvdException; };

        It should_be_exception = () => { exception.Message.ShouldEqual("Not found any type TheSameString"); };
    }
}