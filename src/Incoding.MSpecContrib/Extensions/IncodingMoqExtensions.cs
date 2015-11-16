namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Machine.Specifications;
    using Moq;
    using Moq.Language;

    #endregion

    public static class IncodingMoqExtensions
    {
        #region Dispatcher

        #region Factory constructors

        public static void ShouldBePush<TCommand>(this Mock<IDispatcher> dispatcher, TCommand command, MessageExecuteSetting executeSetting = null, int callCount = 1) where TCommand : CommandBase
        {
            ShouldBePush<TCommand>(dispatcher, arg => arg.ShouldEqualWeak(command, dsl => dsl.ForwardToValue(r => r.Setting, executeSetting ?? new MessageExecuteSetting())), callCount);
        }

        public static void ShouldBePush<TCommand>(this Mock<IDispatcher> dispatcher, Action<TCommand> verifyCommand, int callCount = 1) where TCommand : CommandBase
        {
            dispatcher.Push(verifyCommand, callCount, false);
        }

        public static void StubPush<TCommand>(this Mock<IDispatcher> dispatcher, TCommand command, MessageExecuteSetting executeSetting = null, int callCount = 1) where TCommand : CommandBase
        {
            StubPush<TCommand>(dispatcher, arg => arg.ShouldEqualWeak(command, dsl => dsl.ForwardToValue(r => r.Setting, executeSetting ?? new MessageExecuteSetting())), callCount);
        }

        public static void StubPush<TCommand>(this Mock<IDispatcher> dispatcher, Action<TCommand> verifyCommand, int callCount = 1) where TCommand : CommandBase
        {
            Guard.IsConditional("callCount", callCount > 0, "Can't stub 0 push");
            dispatcher.Push(verifyCommand, callCount, true);
        }

        public static void StubPushAsThrow<TCommand>(this Mock<IDispatcher> dispatcher, TCommand command, Exception exception, MessageExecuteSetting executeSetting = null) where TCommand : CommandBase
        {
            Action<CommandComposite> verify = commandComposite =>
                                              {
                                                  commandComposite.Parts.ShouldNotBeEmpty();
                                                  bool isAnySatisfied = commandComposite.Parts.Any(message =>
                                                                                                   {
                                                                                                       try
                                                                                                       {
                                                                                                           message.ShouldEqualWeak(command, dsl => dsl.ForwardToValue(r => r.Setting, executeSetting ?? new MessageExecuteSetting()));
                                                                                                           return true;
                                                                                                       }
                                                                                                       catch (SpecificationException)
                                                                                                       {
                                                                                                           return false;
                                                                                                       }
                                                                                                   });
                                                  if (!isAnySatisfied)
                                                      throw new SpecificationException();
                                              };
            dispatcher.Setup(r => r.Push(Pleasure.MockIt.Is(verify))).Throws(exception);
        }

        public static void StubQuery<TQuery, TResult>(this Mock<IDispatcher> dispatcher, TQuery query, TResult result, MessageExecuteSetting executeSetting = null) where TQuery : QueryBase<TResult>
        {
            dispatcher
                    .Setup(r => r.Query(Pleasure.MockIt.IsStrong(query), Pleasure.MockIt.IsStrong(executeSetting)))
                    .Returns(result);
        }

        public static void StubQuery<TQuery, TResult>(this Mock<IDispatcher> dispatcher, TQuery query, Action<ICompareFactoryDsl<TQuery, TQuery>> dsl, TResult result, MessageExecuteSetting executeSetting = null)
                where TQuery : QueryBase<TResult>                
        {
            dispatcher.Setup(r => r.Query(Pleasure.MockIt.IsStrong(query, dsl), Pleasure.MockIt.IsStrong(executeSetting)))
                      .Returns(result);
        }

        public static void StubQueryAsThrow<TQuery, TResult>(this Mock<IDispatcher> dispatcher, TQuery query, Exception exception, MessageExecuteSetting executeSetting = null) where TQuery : QueryBase<TResult>
        {
            dispatcher
                    .Setup(r => r.Query(Pleasure.MockIt.IsStrong(query), Pleasure.MockIt.IsStrong(executeSetting)))
                    .Throws(exception);
        }

        #endregion

        static void Push<TCommand>(this Mock<IDispatcher> dispatcher, Action<TCommand> verifyCommand, int callCount, bool asStub) where TCommand : CommandBase
        {
            Func<IMessage, bool> predicate = part =>
                                                     {
                                                         try
                                                         {
                                                             verifyCommand((TCommand)part);

                                                             return true;
                                                         }
                                                         catch (Exception)
                                                         {
                                                             return false;
                                                         }
                                                     };

            if (asStub)
                dispatcher.Setup(r => r.Push(Pleasure.MockIt.Is<CommandComposite>(composite => composite.Parts.Count(predicate).ShouldEqual(callCount))));
            else
            {
                if (callCount == 0)
                    dispatcher.Verify(r => r.Push(Pleasure.MockIt.Is<CommandComposite>(composite => composite.Parts.Any(predicate).ShouldBeTrue())), Times.Never());
                else
                    dispatcher.Verify(r => r.Push(Pleasure.MockIt.Is<CommandComposite>(composite => composite.Parts.Count(predicate).ShouldEqual(callCount))));
            }
        }

        #endregion

        #region Repository

        public static void StubGetById<TEntity>(this Mock<IRepository> repository, object id, TEntity entity) where TEntity : class, IEntity, new()
        {
            repository
                    .Setup(r => r.GetById<TEntity>(id))
                    .Returns(entity);
        }

        public static void StubLoadById<TEntity>(this Mock<IRepository> repository, object id, TEntity entity) where TEntity : class, IEntity, new()
        {
            repository
                    .Setup(r => r.LoadById<TEntity>(id))
                    .Returns(entity);
        }

        public static void StubPaginated<TEntity>(this Mock<IRepository> repository, PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, IncPaginatedResult<TEntity> result = null)
                where TEntity : class, IEntity, new()
        {
            repository.Setup(r => r.Paginated(Pleasure.MockIt.IsStrong(paginatedSpecification), orderSpecification, Pleasure.MockIt.IsStrong(whereSpecification, dsl => dsl.IncludeAllFields()), fetchSpecification))
                      .Returns(result);
        }

        public static void StubQuery<TEntity>(this Mock<IRepository> repository, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, params TEntity[] entities) where TEntity : class, IEntity, new()
        {
            repository
                    .Setup(r => r.Query(orderSpecification, Pleasure.MockIt.IsStrong(whereSpecification, dsl => dsl.IncludeAllFields()), fetchSpecification, Pleasure.MockIt.IsStrong(paginatedSpecification, dsl => dsl.IncludeAllFields())))
                    .Returns(Pleasure.ToQueryable(entities));
        }

        #endregion

        #region Returns

        public static void ReturnsInvent<TMock, TResult>(this IReturns<TMock, TResult> setup, Action<IInventFactoryDsl<TResult>> action = null)
                where TMock : class
                where TResult : new()
        {
            setup.Returns(Pleasure.Generator.Invent(action));
        }

        public static void ReturnsInvent<TMock, TResult>(this IReturnsGetter<TMock, TResult> setup, Action<IInventFactoryDsl<TResult>> action = null)
                where TMock : class
                where TResult : new()
        {
            setup.Returns(Pleasure.Generator.Invent(action));
        }

        public static void ReturnsNull<TMock, TResult>(this IReturns<TMock, TResult> setup)
                where TMock : class
                where TResult : new()
        {
            setup.Returns(null);
        }

        public static void ReturnsNull<TMock, TResult>(this IReturnsGetter<TMock, TResult> setup)
                where TMock : class
                where TResult : new()
        {
            setup.Returns(null);
        }

        #endregion
    }
}