namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Machine.Specifications;
    using Moq;

    #endregion

    public abstract class MockMessage<TMessage, TResult> where TMessage : IMessage<TResult> where TResult : class
    {
        #region Fields

        readonly Mock<IRepository> repository;

        readonly Mock<IEventBroker> eventBroker;

        #endregion

        #region Constructors

        protected MockMessage(TMessage instanceMessage)
        {
            this.repository = Pleasure.Mock<IRepository>();
            IoCFactory.Instance.StubTryResolve(this.repository.Object);

            this.eventBroker = Pleasure.MockStrict<IEventBroker>();
            IoCFactory.Instance.StubTryResolve(this.eventBroker.Object);

            Original = instanceMessage;
        }

        #endregion

        #region Properties

        public TMessage Original { get; set; }

        #endregion

        #region Api Methods

        public void ShouldBeDelete<TEntity>(object id, int callCount = 1) where TEntity : class, IEntity
        {
            this.repository.Verify(r => r.Delete<TEntity>(id), Times.Exactly(callCount));
        }

        public void ShouldBeDelete<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity
        {
            this.repository.Verify(r => r.Delete(Pleasure.MockIt.IsStrong(entity)), Times.Exactly(callCount));
        }

        public void ShouldBeSave<TEntity>(Action<TEntity> verify, int callCount = 1) where TEntity : class, IEntity
        {
            Func<TEntity, bool> match = s =>
                                            {
                                                verify(s);
                                                return true;
                                            };

            this.repository.Verify(r => r.Save(Pleasure.MockIt.Is<TEntity>(entity => match(entity))), Times.Exactly(callCount));
        }

        public void ShouldBeSave<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity
        {
            ShouldBeSave<TEntity>(r => r.ShouldEqualWeak(entity), callCount);
        }

        public void ShouldNotBeSave<TEntity>() where TEntity : class, IEntity
        {
            ShouldBeSave<TEntity>(r => r.ShouldBeOfType<TEntity>(), 0);
        }

        public void ShouldNotBeSaveOrUpdate<TEntity>() where TEntity : class, IEntity
        {
            ShouldBeSaveOrUpdate<TEntity>(r => r.ShouldBeOfType<TEntity>(), 0);
        }

        public void ShouldBeSaveOrUpdate<TEntity>(Action<TEntity> verify, int callCount = 1) where TEntity : class, IEntity
        {
            Func<TEntity, bool> match = s =>
                                            {
                                                verify(s);
                                                return true;
                                            };

            this.repository.Verify(r => r.SaveOrUpdate(Pleasure.MockIt.Is<TEntity>(entity => match(entity))), Times.Exactly(callCount));
        }

        public void ShouldBeSaveOrUpdate<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity
        {
            ShouldBeSaveOrUpdate<TEntity>(r => r.ShouldEqualWeak(entity), callCount);
        }

        #endregion

        #region Data

        public void ShouldBeIsResult(TResult expected)
        {
            this.Original.Result.ShouldEqualWeak(expected);
        }

        public void ShouldBeIsResult(Action<TResult> verifyResult)
        {
            verifyResult(Original.Result);
        }

        #endregion

        #region Event broker

        public MockMessage<TMessage, TResult> StubPublish<TEvent>(TEvent expectedEvent) where TEvent : class, IEvent
        {
            return StubPublish<TEvent>(@event => @event.ShouldEqualWeak(expectedEvent));
        }

        public MockMessage<TMessage, TResult> StubPublish<TEvent>() where TEvent : class, IEvent
        {
            return StubPublish<TEvent>(@event => @event.ShouldNotBeNull());
        }

        public MockMessage<TMessage, TResult> StubPublish<TEvent>(Action<TEvent> action) where TEvent : class, IEvent
        {
            this.eventBroker.Setup(r => r.Publish(Pleasure.MockIt.Is(action)));
            return this;
        }

        #endregion

        #region Stubs

        #region Api Methods

        public MockMessage<TMessage, TResult> StubQuery<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, params TEntity[] entities) where TEntity : class, IEntity
        {
            return Stub(message => message.repository.StubQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification, entities));
        }

        public MockMessage<TMessage, TResult> StubPaginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, IncPaginatedResult<TEntity> result = null) where TEntity : class, IEntity
        {
            return Stub(message => message.repository.StubPaginated(paginatedSpecification, orderSpecification, whereSpecification, fetchSpecification, result));
        }

        public MockMessage<TMessage, TResult> StubEmptyQuery<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity
        {
            return Stub(message => message.repository.StubQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification, Pleasure.ToArray<TEntity>()));
        }

        public MockMessage<TMessage, TResult> StubNotEmptyQuery<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, int countEntity = 1) where TEntity : class, IEntity, new()
        {
            var entities = Pleasure.ToList<TEntity>();
            for (int i = 0; i < countEntity; i++)
                entities.Add(Pleasure.Generator.Invent<TEntity>());

            return StubQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification, entities.ToArray());
        }

        public MockMessage<TMessage, TResult> StubGetById<TEntity>(object id, TEntity res) where TEntity : class, IEntity
        {
            return Stub(message => message.repository.StubGetById(id, res));
        }

        public void ShouldBePublished()
        {
            this.eventBroker.VerifyAll();
        }

        #endregion

        MockMessage<TMessage, TResult> Stub(Action<MockMessage<TMessage, TResult>> configureMock)
        {
            configureMock(this);
            return this;
        }

        #endregion
    }
}