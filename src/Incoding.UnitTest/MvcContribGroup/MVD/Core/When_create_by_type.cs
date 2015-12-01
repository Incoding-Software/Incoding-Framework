namespace Incoding.UnitTest.MvcContribGroup.Core
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CreateByTypeQuery))]
    public class When_create_by_type
    {
        Establish establish = () =>
                              {
                                  CreateByTypeQuery query = Pleasure.Generator.Invent<CreateByTypeQuery>();
                                  expected = Pleasure.Generator.Invent<UniqueNameClass>();

                                  mockQuery = MockQuery<CreateByTypeQuery, object>
                                          .When(query);
                                  //.StubQuery<CreateByTypeQuery.FindTypeByName, Type>(r => r.Tuning(s=>s.), typeof(UniqueNameClass));
                              };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        public class UniqueNameClass { }

        #region Establish value

        static MockMessage<CreateByTypeQuery, object> mockQuery;

        static object expected;

        #endregion
    }
}