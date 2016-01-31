namespace Incoding.UnitTest.MvcContribGroup
{
    using System;
    using System.Web;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    [Subject(typeof(CreateByTypeQuery.FindTypeByName))]
    public class When_find_by_type_by_full_name_with_encode
    {
        Establish establish = () =>
                              {
                                  expected = typeof(FakeInnerByFullNameCommand.Inner);
                                  CreateByTypeQuery.FindTypeByName query = Pleasure.Generator.Invent<CreateByTypeQuery.FindTypeByName>(dsl => dsl.Tuning(r => r.Type, HttpUtility.UrlEncode(expected.FullName)));

                                  mockQuery = MockQuery<CreateByTypeQuery.FindTypeByName, Type>
                                          .When(query);
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        #region Fake classes

        public class FakeInnerByFullNameCommand
        {
            public class Inner { }
        }

        #endregion

        #region Establish value

        static MockMessage<CreateByTypeQuery.FindTypeByName, Type> mockQuery;

        static Type expected;

        #endregion
    }
}