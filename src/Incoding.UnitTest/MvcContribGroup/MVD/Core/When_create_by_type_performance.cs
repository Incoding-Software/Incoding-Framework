namespace Incoding.UnitTest.MvcContribGroup.Core
{
    using System;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    [Subject(typeof(CreateByTypeQuery))]
    public class When_create_by_type_performance
    {
        Establish establish = () =>
                              {
                                  CreateByTypeQuery query = Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.IsGroup, false)
                                                                                                                   .Tuning(r => r.Type, typeof(UniqueNameClass).Name));
                                  expected = Pleasure.Generator.Invent<UniqueNameClass>();

                                  mockQuery = MockQuery<CreateByTypeQuery, object>
                                          .When(query)
                                          .StubQuery<CreateByTypeQuery.FindTypeByName, Type>(dsl => dsl.Tuning(r => r.Type, typeof(UniqueNameClass).Name), typeof(UniqueNameClass))
                                          .StubQuery<CreateByTypeQuery.GetFormCollectionsQuery, FormCollection>(new FormCollection(new NameValueCollection()));
                              };

        Because of = () => { time = Pleasure.Do(i => mockQuery.Execute(), 1000); };

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        It should_be_time = () => { time.ShouldBeLessThan(1); };

        public class UniqueNameClass { }

        #region Establish value

        static MockMessage<CreateByTypeQuery, object> mockQuery;

        static object expected;

        private static long time;

        #endregion
    }
}