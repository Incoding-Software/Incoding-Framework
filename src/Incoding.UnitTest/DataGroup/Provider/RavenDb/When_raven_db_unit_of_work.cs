namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Data;
    using Incoding.Data;
    using Incoding.Extensions;
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
            var session = Pleasure.Mock<IDocumentSession>();
            var work = new RavenDbUnitOfWork(session.Object, IsolationLevel.ReadCommitted, true);
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
                                                          unitOfWork.TryGetValue("disposed").ShouldEqual(false);
                                                          unitOfWork.Dispose();
                                                          session.Verify(r => r.Dispose(), Times.Once());
                                                          unitOfWork.TryGetValue("disposed").ShouldEqual(true);
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