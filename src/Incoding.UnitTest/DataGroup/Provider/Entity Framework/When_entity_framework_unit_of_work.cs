namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Entity;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(EntityFrameworkUnitOfWork))]
    public class When_entity_framework_unit_of_work
    {
        #region Establish value

        static void Run(Action<EntityFrameworkUnitOfWork, Mock<DbContext>> action, bool isOpen = true)
        {
            var dbContext = Pleasure.Mock<DbContext>();
            string connectionString = ConfigurationManager.ConnectionStrings["IncRealEFDb"].ConnectionString;
            var sessionFactory = Pleasure.MockStrictAsObject<IEntityFrameworkSessionFactory>(mock => mock.Setup(r => r.Open(connectionString)).Returns(dbContext.Object));
            var unitOfWork = new EntityFrameworkUnitOfWork(sessionFactory, IsolationLevel.ReadCommitted, connectionString);
            if (isOpen)
                unitOfWork.Open();

            action(unitOfWork, dbContext);
        }

        #endregion

        It should_be_flush = () => Run((work, context) =>
                                           {
                                               work.Flush();
                                               context.Verify(r => r.SaveChanges(), Times.Once());
                                           });

        It should_be_flush_without_open = () => Run((work, context) =>
                                                        {
                                                            work.Flush();
                                                            context.Verify(r => r.SaveChanges(), Times.Never());
                                                        }, isOpen: false);

        It should_be_dispose = () => Run((work, context) =>
                                             {
                                                 work.Dispose();
                                                 context.Verify(r => r.Dispose(), Times.Once());
                                             });

        It should_be_dispose_without_open = () => Run((work, context) =>
                                                          {
                                                              work.Dispose();
                                                              context.Verify(r => r.Dispose(), Times.Never());
                                                          }, isOpen: false);
    }
}