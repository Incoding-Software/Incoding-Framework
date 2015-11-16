namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data.Entity;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EntityFrameworkUnitOfWorkFactory))]
    public class When_entity_framework_unit_of_work_factory : Behavior_unit_of_work_factory
    {
        Establish establish = () =>
                              {
                                  var db = Pleasure.MockAsObject<DbContext>();
                                  var sessionFactory = Pleasure.MockStrictAsObject<IEntityFrameworkSessionFactory>(mock => mock.Setup(r => r.Open(connectionString)).Returns(db));
                                  unitOfWork = new EntityFrameworkUnitOfWorkFactory(sessionFactory).Create(isolated, isFlush, connectionString);
                              };

        Because of = () => { };

        Behaves_like<Behavior_unit_of_work_factory> verify;
    }
}