namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_query_with_fetch_join_many_specification : Context_nhibernate_repository_query
    {
        #region Fake classes

        class FakeFetchSpecification : FetchSpecification<RealDbEntity>
        {
            public override Action<AdHocFetchSpecification<RealDbEntity>> FetchedBy()
            {
                return specification => specification.JoinMany(r => r.Items, r => r.Parent);
            }
        }

        #endregion

        #region Estabilish value

        static List<RealDbEntity> result;

        #endregion

        Because of = () => { result = repository.Query(fetchSpecification: new FakeFetchSpecification()).ToList(); };

        It should_be_fetch_child_collections = () =>
                                                   {
                                                       result.Count.ShouldEqual(10);

                                                       var closureToList = result.ToList();
                                                       Session.Close();

                                                       foreach (var realDbEntity in closureToList)
                                                           realDbEntity.Items[0].Parent.Value.ShouldBeGreaterThan(0);
                                                   };
    }
}