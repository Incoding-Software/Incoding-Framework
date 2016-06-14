namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;
    using NCrunch.Framework;

    #endregion

    [Subject(typeof(CreateByTypeQuery.FindTypeByName)), Isolated]
    public class When_find_by_type_concurrency
    {
        Establish establish = () =>
                              {
                                  expected = typeof(ConcurrencyClass);
                                  query = Pleasure.Generator.Invent<CreateByTypeQuery.FindTypeByName>(dsl => dsl.Tuning(r => r.Type, expected.Name));
                              };

        It should_be_result = () =>
                              {
                                  Pleasure.MultiThread.Do(() =>
                                                          {
                                                              var mockQuery = MockQuery<CreateByTypeQuery.FindTypeByName, Type>
                                                                      .When(query);
                                                              mockQuery.Execute();
                                                              mockQuery.ShouldBeIsResult(expected);
                                                          }, 1000);
                              };

        #region Fake classes

        public class ConcurrencyClass { }

        #endregion

        #region Establish value

        static Type expected;

        private static CreateByTypeQuery.FindTypeByName query;

        #endregion
    }
}