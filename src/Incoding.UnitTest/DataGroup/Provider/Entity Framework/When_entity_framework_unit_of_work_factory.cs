namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EntityFrameworkUnitOfWorkFactory))]
    public class When_entity_framework_unit_of_work_factory
    {
        #region Establish value

        static EntityFrameworkUnitOfWorkFactory factory;

        static IsolationLevel isolated;

        static string connectionString;

        static IUnitOfWork unitOfWork;

        #endregion

        Establish establish = () =>
                                  {
                                      isolated = Pleasure.Generator.Invent<IsolationLevel>();
                                      connectionString = Pleasure.Generator.String();
                                      var sessionFactory = Pleasure.MockStrictAsObject<IEntityFrameworkSessionFactory>();
                                      factory = new EntityFrameworkUnitOfWorkFactory(sessionFactory);
                                  };

        Because of = () => { unitOfWork = factory.Create(isolated, connectionString); };

        It should_be_isolated = () => unitOfWork.TryGetValue("isolationLevel").ShouldEqual(isolated);
    }
}