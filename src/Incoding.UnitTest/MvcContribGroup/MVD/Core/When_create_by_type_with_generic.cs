namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    #region << Using >>

    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    #endregion

    [Subject(typeof(CreateByTypeQuery))]
    public class When_create_by_type_with_generic
    {
        Establish establish = () =>
                              {
                                  CreateByTypeQuery query = Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.IsGroup, false)
                                                                                                                   .Tuning(r => r.IsModel, true)
                                                                                                                   .Tuning(r => r.Type, "{0}|{1}".F(typeof(FakeGenericByNameCommand<>).Name, typeof(IncEntityBase).Name)));
                                  expected = typeof(FakeGenericByNameCommand<IncEntityBase>);

                                  mockQuery = MockQuery<CreateByTypeQuery, Type>
                                          .When(query)
                                          .StubQuery<CreateByTypeQuery.FindTypeByName, Type>(dsl => dsl.Tuning(r => r.Type, typeof(FakeGenericByNameCommand<>).Name), typeof(FakeGenericByNameCommand<>))
                                          .StubQuery<CreateByTypeQuery.FindTypeByName, Type>(dsl => dsl.Tuning(r => r.Type, typeof(IncEntityBase).Name), typeof(IncEntityBase))
                                          .StubQuery<CreateByTypeQuery.GetFormCollectionsQuery, FormCollection>(new FormCollection(new NameValueCollection()));
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        #region Fake classes

        public class FakeGenericByNameCommand<TEntity> : CommandBase
        {
            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        #endregion

        #region Establish value

        static MockMessage<CreateByTypeQuery, Type> mockQuery;

        static Type expected;

        #endregion
    }
}