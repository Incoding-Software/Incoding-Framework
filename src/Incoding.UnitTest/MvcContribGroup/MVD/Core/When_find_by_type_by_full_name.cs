namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CreateByTypeQuery.FindTypeByName))]
    public class When_find_by_type_by_full_name
    {
        Establish establish = () =>
                              {
                                  expected = typeof(FakeInnerByFullNameCommand);
                                  CreateByTypeQuery.FindTypeByName query = Pleasure.Generator.Invent<CreateByTypeQuery.FindTypeByName>(dsl => dsl.Tuning(r => r.Type, expected.FullName));

                                  mockQuery = MockQuery<CreateByTypeQuery.FindTypeByName, Type>
                                          .When(query);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        #region Fake classes

        public class FakeInnerByFullNameCommand { }

        #endregion

        #region Establish value

        static MockMessage<CreateByTypeQuery.FindTypeByName, Type> mockQuery;

        static Type expected;

        #endregion
    }
}