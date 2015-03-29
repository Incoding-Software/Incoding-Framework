using Incoding.Extensions;

namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
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

        protected readonly Mock<IDispatcher> dispatcher;

        readonly Mock<IRepository> repository;

        readonly Mock<IEventBroker> eventBroker;

        readonly List<Action<CommandBase>> actions = new List<Action<CommandBase>>();

        readonly IUnitOfWork unitOfWork;

        #endregion

        #region Constructors

        protected MockMessage(TMessage instanceMessage)
        {
            this.unitOfWork = Pleasure.MockStrictAsObject<IUnitOfWork>(
                mock =>
                {
                    mock.Setup(x => x.GetSession()).Returns(Pleasure.Generator.String());
                    mock.Setup(x => x.Open());
                });
            instanceMessage.Setting = new MessageExecuteSetting();
            instanceMessage.Setting.SetValue("unitOfWork", unitOfWork);

            this.repository = Pleasure.Mock<IRepository>(mock => mock.Setup(r => r.SetProvider(unitOfWork)));
            IoCFactory.Instance.StubTryResolve(this.repository.Object);

            this.eventBroker = Pleasure.MockStrict<IEventBroker>();
            IoCFactory.Instance.StubTryResolve(this.eventBroker.Object);

            this.dispatcher = Pleasure.MockStrict<IDispatcher>(mock =>
                                                                   {
                                                                       mock.StubPush<CommandBase>(@base =>
                                                                                                      {
                                                                                                          bool isAny = false;
                                                                                                          foreach (var action in this.actions)
                                                                                                          {
                                                                                                              try
                                                                                                              {
                                                                                                                  action(@base);
                                                                                                                  isAny = true;
                                                                                                              }
                                                                                                              catch (Exception) { }
                                                                                                          }

                                                                                                          isAny.ShouldBeTrue();
                                                                                                      });
                                                                   });
            IoCFactory.Instance.StubTryResolve(this.dispatcher.Object);
            instanceMessage.Setting.SetValue("outerDispatcher", this.dispatcher.Object);

            Original = instanceMessage;
        }

        #endregion

        #region Properties

        public TMessage Original { get; set; }

        #endregion

        #region Api Methods

        public MockMessage<TMessage, TResult> StubPush<TCommand>(TCommand command, Action<ICompareFactoryDsl<TCommand, TCommand>> dsl = null) where TCommand : CommandBase
        {
            this.actions.Add(@base =>
                                 {
                                     var impCommand = @base as TCommand;
                                     impCommand.ShouldEqualWeak(command, dsl);
                                 });
            return this;
        }

        public MockMessage<TMessage, TResult> StubProvider<TProvider>(Mock<TProvider> provider) where TProvider : class
        {
            this.repository.Setup(r => r.GetProvider<TProvider>()).Returns(provider.Object);
            return this;
        }

        public MockMessage<TMessage, TResult> StubPush<TCommand>(Action<IInventFactoryDsl<TCommand>> configure, Action<ICompareFactoryDsl<TCommand, TCommand>> dsl = null) where TCommand : CommandBase
        {
            return StubPush(Pleasure.Generator.Invent(configure), dsl);
        }

        public void ShouldBeDeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            this.repository.Verify(r => r.DeleteByIds<TEntity>(Pleasure.MockIt.IsStrong(ids)));
        }

        public void ShouldBeDeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            this.repository.Verify(r => r.DeleteAll<TEntity>());
        }

        public void ShouldBeDelete<TEntity>(object id, int callCount = 1) where TEntity : class, IEntity, new()
        {
            this.repository.Verify(r => r.Delete<TEntity>(id), Times.Exactly(callCount));
        }

        public void ShouldBeDelete<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity, new()
        {
            this.repository.Verify(r => r.Delete(Pleasure.MockIt.IsStrong(entity)), Times.Exactly(callCount));
        }

        public void ShouldBeSave<TEntity>(Action<TEntity> verify, int callCount = 1) where TEntity : class, IEntity, new()
        {
            Func<TEntity, bool> match = s =>
                                            {
                                                verify(s);
                                                return true;
                                            };

            this.repository.Verify(r => r.Save(Pleasure.MockIt.Is<TEntity>(entity => match(entity))), Times.Exactly(callCount));
        }

        public void ShouldBeSaves<TEntity>(Action<IEnumerable<TEntity>> verify, int callCount = 1) where TEntity : class, IEntity, new()
        {
            Func<IEnumerable<TEntity>, bool> match = s =>
                                                         {
                                                             verify(s);
                                                             return true;
                                                         };

            this.repository.Verify(r => r.Saves(Pleasure.MockIt.Is<IEnumerable<TEntity>>(entities => match(entities))), Times.Exactly(callCount));
        }

        public void ShouldBeFlush(int callCount = 1)
        {
            this.repository.Verify(r => r.Flush(), Times.Exactly(callCount));
        }

        public void ShouldBeSave<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity, new()
        {
            ShouldBeSave<TEntity>(r => r.ShouldEqualWeak(entity), callCount);
        }

        public void ShouldNotBeSave<TEntity>() where TEntity : class, IEntity, new()
        {
            ShouldBeSave<TEntity>(r => r.ShouldBeOfType<TEntity>(), 0);
        }

        public void ShouldNotBeSaveOrUpdate<TEntity>() where TEntity : class, IEntity, new()
        {
            ShouldBeSaveOrUpdate<TEntity>(r => r.ShouldBeOfType<TEntity>(), 0);
        }

        public void ShouldBeSaveOrUpdate<TEntity>(Action<TEntity> verify, int callCount = 1) where TEntity : class, IEntity, new()
        {
            Func<TEntity, bool> match = s =>
                                            {
                                                verify(s);
                                                return true;
                                            };

            this.repository.Verify(r => r.SaveOrUpdate(Pleasure.MockIt.Is<TEntity>(entity => match(entity))), Times.Exactly(callCount));
        }

        public void ShouldBeSaveOrUpdate<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity, new()
        {
            ShouldBeSaveOrUpdate<TEntity>(r => r.ShouldEqualWeak(entity), callCount);
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(TQuery query, TNextResult result) where TQuery : QueryBase<TNextResult> where TNextResult : class
        {
            this.dispatcher.StubQuery(query, result, new MessageExecuteSetting());
            return this;
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(TNextResult result) where TQuery : QueryBase<TNextResult> where TNextResult : class
        {
            return StubQuery(Pleasure.Generator.Invent<TQuery>(), result);
        }

        public MockMessage<TMessage, TResult> StubQueryAsNull<TQuery, TNextResult>() where TQuery : QueryBase<TNextResult> where TNextResult : class
        {
            return StubQuery<TQuery, TNextResult>(null);
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(Action<IInventFactoryDsl<TQuery>> configure, TNextResult result) where TQuery : QueryBase<TNextResult> where TNextResult : class
        {
            return StubQuery(Pleasure.Generator.Invent(configure), result);
        }

        #endregion

        #region Data

        public void ShouldBeIsResult(TResult expected)
        {
            Original.Result.ShouldEqualWeak(expected);
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

        public MockMessage<TMessage, TResult> StubQuery<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, params TEntity[] entities) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification, entities));
        }

        public MockMessage<TMessage, TResult> StubEmptyQuery<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification, Pleasure.ToArray<TEntity>()));
        }

        public MockMessage<TMessage, TResult> StubPaginated<TEntity>(PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, IncPaginatedResult<TEntity> result = null) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubPaginated(paginatedSpecification, orderSpecification, whereSpecification, fetchSpecification, result));
        }

        public MockMessage<TMessage, TResult> StubNotEmptyQuery<TEntity>(OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, int countEntity = 1) where TEntity : class, IEntity, new()
        {
            var entities = Pleasure.ToList<TEntity>();
            for (int i = 0; i < countEntity; i++)
                entities.Add(Pleasure.Generator.Invent<TEntity>());

            return StubQuery(orderSpecification, whereSpecification, fetchSpecification, paginatedSpecification, entities.ToArray());
        }

        public MockMessage<TMessage, TResult> StubGetById<TEntity>(object id, TEntity res) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubGetById(id, res));
        }

        public MockMessage<TMessage, TResult> StubSave<TEntity>(TEntity res, object id) where TEntity : class, IEntity, new()
        {
            Action<TEntity> verify = entity =>
            {
                entity.ShouldEqualWeak(res, null);
                entity.SetValue("Id", id);
            };
            return Stub(message => message.repository.Setup(r => r.Save(Pleasure.MockIt.Is<TEntity>(verify))));
        }

        public MockMessage<TMessage, TResult> StubSave<TEntity>(Action<TEntity> action, object id) where TEntity : class, IEntity, new()
        {
            Func<TEntity, bool> match = s =>
            {
                action(s);
                return true;
            };

            Action<TEntity> verify = entity =>
            {
                match(entity);
                entity.SetValue("Id", id);
            };
            return Stub(message => message.repository.Setup(r => r.Save(Pleasure.MockIt.Is<TEntity>(verify))));
        }

        public MockMessage<TMessage, TResult> StubLoadById<TEntity>(object id, TEntity res) where TEntity : class, IEntity, new()
        {
            return Stub(message => message.repository.StubLoadById(id, res));
        }

        public void ShouldBePublished()
        {
            this.eventBroker.VerifyAll();
        }

        public void ShouldPushed()
        {
            this.dispatcher.VerifyAll();
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