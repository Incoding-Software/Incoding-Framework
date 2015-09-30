namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Data;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using Raven.Client;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(RavenDbUnitOfWork))]
    public class When_raven_db_unit_of_work
    {
        #region Establish value

        static void Run(Action<RavenDbUnitOfWork, Mock<IDocumentSession>> action)
        {
            var session = Pleasure.MockStrict<IDocumentSession>();
            var sessionFactory = Pleasure.MockStrictAsObject<IRavenDbSessionFactory>(mock => mock.Setup(r => r.Open(Pleasure.MockIt.IsNotNull<string>())).Returns(session.Object));
            var work = new RavenDbUnitOfWork(sessionFactory, Pleasure.Generator.String(), IsolationLevel.ReadCommitted);
            action(work, session);
        }

        #endregion

        It should_be_dispose = () => Run((unitOfWork, session) =>
                                         {
                                             unitOfWork.GetRepository();
                                             session.Setup(r => r.Dispose());
                                             unitOfWork.Dispose();
                                             session.Verify(r => r.Dispose(), Times.Once());
                                         });

        It should_be_dispose_without_open = () => Run((unitOfWork, session) =>
                                                      {
                                                          unitOfWork.Dispose();
                                                          session.Verify(r => r.Dispose(), Times.Never());
                                                      });

        It should_be_flush = () => Run((unitOfWork, session) =>
                                       {
                                           unitOfWork.GetRepository();
                                           session.Setup(r => r.SaveChanges());
                                           unitOfWork.Flush();
                                           session.Verify(r => r.SaveChanges(), Times.Once());
                                       });
    }
}