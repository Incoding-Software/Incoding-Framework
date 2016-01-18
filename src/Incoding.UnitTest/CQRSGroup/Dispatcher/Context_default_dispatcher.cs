namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Data;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    
    using Incoding.MSpecContrib;
    using Moq;

    #endregion

    public class Context_default_dispatcher
    {
        #region Fake classes

        protected class FakeEntity : IncEntityBase { }

        protected class CommandWithRepository : CommandBase
        {
            protected override void Execute()
            {
                Repository.Delete(new FakeEntity());
            }
        }

        protected class QueryWithRepository : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                Repository.Delete(new FakeEntity());
                return Pleasure.Generator.TheSameString();
            }
        }

        protected class QueryWithThrowAndRepository : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                Repository.Delete(new FakeEntity());
                throw new Exception();
            }
        }

        protected class QueryWithoutRepository : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                return Pleasure.Generator.TheSameString();
            }
        }

        protected class QueryWithThrowAndWithoutRepository : QueryBase<string>
        {
            protected override string ExecuteResult()
            {
                throw new Exception();
            }
        }

        protected class CommandWithoutRepository : CommandBase
        {
            protected override void Execute() { }
        }

        protected class CommandWithThrowAndRepository : CommandBase
        {
            protected override void Execute()
            {
                Repository.Delete(new FakeEntity());
                throw new Exception();
            }
        }

        protected class CommandWithThrow : CommandBase
        {
            protected override void Execute()
            {
                throw new ArgumentException();
            }
        }

        #endregion

        #region Static Fields

        protected static Mock<IUnitOfWork> unitOfWork;

        protected static DefaultDispatcher dispatcher;
        
        protected static Mock<IUnitOfWorkFactory> unitOfWorkFactory;

        #endregion

        #region Constructors

        protected Context_default_dispatcher()
        {
            unitOfWorkFactory = Pleasure.MockStrict<IUnitOfWorkFactory>(unitOfWorkFactoryMock =>
                                                                        {
                                                                            unitOfWork = Pleasure.Mock<IUnitOfWork>(mock => mock.Setup(r => r.GetRepository()).Returns(Pleasure.MockAsObject<IRepository>()));
                                                                            unitOfWorkFactoryMock.Setup(r => r.Create(IsolationLevel.ReadCommitted, true, Pleasure.MockIt.IsNull<string>())).Returns(unitOfWork.Object);
                                                                            unitOfWorkFactoryMock.Setup(r => r.Create(IsolationLevel.ReadUncommitted, false, Pleasure.MockIt.IsNull<string>())).Returns(unitOfWork.Object);
                                                                        });
            IoCFactory.Instance.StubTryResolve(unitOfWorkFactory.Object);
           

            dispatcher = new DefaultDispatcher();
        }

        #endregion
    }
}