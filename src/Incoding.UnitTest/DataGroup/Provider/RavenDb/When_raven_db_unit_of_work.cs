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

        static void Run(Action<RavenDbUnitOfWork, Mock<IDocumentSession>> action, bool isOpen = true)
        {
            var session = Pleasure.Mock<IDocumentSession>();
            var sessionFactory = Pleasure.MockAsObject<IRavenDbSessionFactory>(mock => mock.Setup(r => r.Open(Pleasure.MockIt.IsNotNull<string>())).Returns(session.Object));
            var work = new RavenDbUnitOfWork(sessionFactory, Pleasure.Generator.String(), IsolationLevel.ReadCommitted);
            if (isOpen)
                work.Open();
            action(work, session);
        }

        #endregion

        It should_be_dispose = () => Run((unitOfWork, session) =>
                                             {
                                                 unitOfWork.Open();
                                                 unitOfWork.Dispose();
                                                 session.Verify(r => r.Dispose(), Times.Once());
                                             });

        It should_be_dispose_with_flush = () => Run((unitOfWork, session) =>
                                                        {
                                                            unitOfWork.Flush();
                                                            unitOfWork.Dispose();
                                                            session.Verify(r => r.SaveChanges(), Times.Once());
                                                        });

        It should_be_dispose_without_open = () => Run((unitOfWork, session) =>
                                                          {
                                                              unitOfWork.Dispose();
                                                              session.Verify(r => r.Dispose(), Times.Never());
                                                          },isOpen:false);

        It should_be_flush = () => Run((unitOfWork, session) =>
                                           {
                                               unitOfWork.Flush();
                                               session.Verify(r => r.SaveChanges(), Times.Once());
                                           });

        It should_be_flush_without_open = () => Run((unitOfWork, session) =>
                                                        {
                                                            {
                                                                unitOfWork.Flush();
                                                                session.Verify(r => r.SaveChanges(), Times.Never());
                                                            }
                                                        }, isOpen: false);

        It should_be_commit = () => Run((unitOfWork, session) => unitOfWork.Commit());

        It should_be_is_open = () => Run((unitOfWork, session) => unitOfWork.IsOpen().ShouldBeTrue());

        It should_be_not_is_open = () => Run((unitOfWork, session) => unitOfWork.IsOpen().ShouldBeFalse(), isOpen: false);
    }
}