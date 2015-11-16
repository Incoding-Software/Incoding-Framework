namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.EventBroker;
    using Incoding.Extensions;
    using Machine.Specifications;
    using Moq;

    #endregion

    public static class MockMessage
    {
        #region Factory constructors

        public static void Execute(this IMessage message)
        {
            message.OnExecute(IoCFactory.Instance.TryResolve<IDispatcher>(), new Lazy<IUnitOfWork>(() => IoCFactory.Instance.TryResolve<IUnitOfWork>()));
        }

        #endregion
    }

    public abstract class MockMessage<TMessage, TResult> where TMessage : IMessage
    {
        #region Fields

        protected readonly Mock<IDispatcher> dispatcher;

        readonly Dictionary<Type, List<CommandBase>> stubs = new Dictionary<Type, List<CommandBase>>();

        readonly Dictionary<Type, int> stubsOfSuccess = new Dictionary<Type, int>();

        readonly Mock<IRepository> repository;

        readonly Mock<IEventBroker> eventBroker;

        #endregion

        #region Constructors

        protected MockMessage(TMessage instanceMessage)
        {
            Original = instanceMessage;
            Original.Setting = new MessageExecuteSetting();
            repository = Pleasure.Mock<IRepository>();

            var unitOfWork = Pleasure.MockStrictAsObject<IUnitOfWork>(mock => mock.Setup(x => x.GetRepository()).Returns(repository.Object));
            IoCFactory.Instance.StubTryResolve(unitOfWork);

            eventBroker = Pleasure.MockStrict<IEventBroker>();
            IoCFactory.Instance.StubTryResolve(eventBroker.Object);

            dispatcher = Pleasure.MockStrict<IDispatcher>();
            IoCFactory.Instance.StubTryResolve(dispatcher.Object);
        }

        #endregion

        #region Properties

        [Obsolete("Use mock.Execute() instead of mock.Original.Execute()", false)]
        public TMessage Original { get; set; }

        #endregion

        #region Api Methods

        public void Execute()
        {
            Original.Execute();
            ShouldBePushed();
            eventBroker.VerifyAll();
            repository.VerifyAll();
        }

        public MockMessage<TMessage, TResult> StubPushAsThrow<TCommand>(TCommand command, Exception ex, MessageExecuteSetting setting = null) where TCommand : CommandBase
        {
            dispatcher.StubPushAsThrow(command, ex, setting);
            return this;
        }

        public void ShouldBePushed()
        {
            foreach (var stub in stubs)
            {
                if (stubsOfSuccess.GetOrDefault(stub.Key) != stub.Value.Count)
                    throw new SpecificationException("Not Stub for {0}".F(stub.Key.Name));
            }
        }

        public MockMessage<TMessage, TResult> StubPush<TCommand>(TCommand command, Action<ICompareFactoryDsl<TCommand, TCommand>> dsl = null, MessageExecuteSetting setting = null) where TCommand : CommandBase
        {
            command.Setting = command.Setting ?? (setting ?? new MessageExecuteSetting());
            var type = typeof(TCommand);
            var value = stubs.GetOrDefault(type, new List<CommandBase>());
            value.Add(command);
            if (!stubs.ContainsKey(type))
                stubs.Add(type, value);
            dispatcher.StubPush<TCommand>(arg =>
                                          {
                                              bool isAny = false;
                                              foreach (var pair in stubs[type])
                                              {
                                                  try
                                                  {
                                                      arg.ShouldEqualWeak(pair as TCommand, dsl);
                                                      isAny = true;
                                                      if (stubsOfSuccess.ContainsKey(type))
                                                          stubsOfSuccess[type]++;
                                                      else
                                                          stubsOfSuccess.Add(type, 1);
                                                      break;
                                                  }
                                                  catch (InternalSpecificationException ex)
                                                  {
                                                      Console.WriteLine(ex);
                                                  }
                                              }

                                              isAny.ShouldBeTrue();
                                          });
            return this;
        }

        public MockMessage<TMessage, TResult> StubPush<TCommand>(Action<IInventFactoryDsl<TCommand>> configure, Action<ICompareFactoryDsl<TCommand, TCommand>> dsl = null) where TCommand : CommandBase
        {
            return StubPush(Pleasure.Generator.Invent(configure), dsl);
        }

        public void ShouldBeDeleteByIds<TEntity>(IEnumerable<object> ids) where TEntity : class, IEntity, new()
        {
            repository.Verify(r => r.DeleteByIds<TEntity>(Pleasure.MockIt.IsStrong(ids)));
        }

        public void ShouldBeDeleteAll<TEntity>() where TEntity : class, IEntity, new()
        {
            repository.Verify(r => r.DeleteAll<TEntity>());
        }

        public void ShouldBeDelete<TEntity>(object id, int callCount = 1) where TEntity : class, IEntity, new()
        {
            repository.Verify(r => r.Delete<TEntity>(id), Times.Exactly(callCount));
        }

        public void ShouldBeDelete<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity, new()
        {
            repository.Verify(r => r.Delete(Pleasure.MockIt.IsStrong(entity)), Times.Exactly(callCount));
        }

        public void ShouldBeSave<TEntity>(Action<TEntity> verify, int callCount = 1) where TEntity : class, IEntity, new()
        {
            Func<TEntity, bool> match = s =>
                                        {
                                            verify(s);
                                            return true;
                                        };

            repository.Verify(r => r.Save(Pleasure.MockIt.Is<TEntity>(entity => match(entity))), Times.Exactly(callCount));
        }

        public void ShouldBeSaves<TEntity>(Action<IEnumerable<TEntity>> verify, int callCount = 1) where TEntity : class, IEntity, new()
        {
            Func<IEnumerable<TEntity>, bool> match = s =>
                                                     {
                                                         verify(s);
                                                         return true;
                                                     };

            repository.Verify(r => r.Saves(Pleasure.MockIt.Is<IEnumerable<TEntity>>(entities => match(entities))), Times.Exactly(callCount));
        }

        public void ShouldBeFlush(int callCount = 1)
        {
            repository.Verify(r => r.Flush(), Times.Exactly(callCount));
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

            repository.Verify(r => r.SaveOrUpdate(Pleasure.MockIt.Is<TEntity>(entity => match(entity))), Times.Exactly(callCount));
        }

        public void ShouldBeSaveOrUpdate<TEntity>(TEntity entity, int callCount = 1) where TEntity : class, IEntity, new()
        {
            ShouldBeSaveOrUpdate<TEntity>(r => r.ShouldEqualWeak(entity), callCount);
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(TQuery query, TNextResult result) where TQuery : QueryBase<TNextResult>
        {
            dispatcher.StubQuery(query, result, new MessageExecuteSetting());
            return this;
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(TQuery query, Action<ICompareFactoryDsl<TQuery, TQuery>> dsl, TNextResult result, MessageExecuteSetting executeSetting = null) where TQuery : QueryBase<TNextResult>
        {
            dispatcher.StubQuery(query, dsl, result, executeSetting);
            return this;
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(TNextResult result) where TQuery : QueryBase<TNextResult>
        {
            return StubQuery(Pleasure.Generator.Invent<TQuery>(), result);
        }

        public MockMessage<TMessage, TResult> StubQueryAsNull<TQuery, TNextResult>() where TQuery : QueryBase<TNextResult>
        {
            return StubQuery<TQuery, TNextResult>(default(TNextResult));
        }

        public MockMessage<TMessage, TResult> StubQuery<TQuery, TNextResult>(Action<IInventFactoryDsl<TQuery>> configure, TNextResult result) where TQuery : QueryBase<TNextResult>
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
            verifyResult((TResult)Original.Result);
        }

        #endregion

        #region Event broker

        [Obsolete("Please use StubPush")]
        public MockMessage<TMessage, TResult> StubPublish<TEvent>(TEvent expectedEvent) where TEvent : class, IEvent
        {
            return StubPublish<TEvent>(@event => @event.ShouldEqualWeak(expectedEvent));
        }

        [Obsolete("Please use StubPush")]
        public MockMessage<TMessage, TResult> StubPublish<TEvent>() where TEvent : class, IEvent
        {
            return StubPublish<TEvent>(@event => @event.ShouldNotBeNull());
        }

        [Obsolete("Please use StubPush")]
        public MockMessage<TMessage, TResult> StubPublish<TEvent>(Action<TEvent> action) where TEvent : class, IEvent
        {
            eventBroker.Setup(r => r.Publish(Pleasure.MockIt.Is(action)));
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

        #endregion

        MockMessage<TMessage, TResult> Stub(Action<MockMessage<TMessage, TResult>> configureMock)
        {
            configureMock(this);
            return this;
        }

        #endregion
    }
}