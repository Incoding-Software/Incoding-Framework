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

        public static void ShouldBePush<TCommand>(this Mock<IDispatcher> dispatcher, TCommand command, MessageExecuteSetting executeSetting = null, int callCount = 1) where TCommand : CommandBase
        {
            ShouldBePush<TCommand>(dispatcher, arg => arg.ShouldEqualWeak(command), executeSetting, callCount);
        }

        public static void ShouldBePush<TCommand>(this Mock<IDispatcher> dispatcher, Action<TCommand> verifyCommand, MessageExecuteSetting executeSetting = null, int callCount = 1) where TCommand : CommandBase
        {
            executeSetting = executeSetting ?? new MessageExecuteSetting();
            Func<CommandComposite.MessageCompositePart, bool> predicate = part =>
                                                                              {
                                                                                  try
                                                                                  {
                                                                                      verifyCommand((TCommand)part.Message);
                                                                                      part.Setting.ShouldEqualWeak(executeSetting);

                                                                                      return true;
                                                                                  }
                                                                                  catch (Exception)
                                                                                  {
                                                                                      return false;
                                                                                  }
                                                                              };

            if (callCount == 0)
                dispatcher.Verify(r => r.Push(Pleasure.MockIt.Is<CommandComposite>(composite => composite.Parts.Any(predicate).ShouldBeTrue())), Times.Never());
            else
                dispatcher.Verify(r => r.Push(Pleasure.MockIt.Is<CommandComposite>(composite => composite.Parts.Count(predicate).ShouldEqual(callCount))));
        }

        public static void StubPushAsThrow<TCommand>(this Mock<IDispatcher> dispatcher, TCommand command, Exception exception, MessageExecuteSetting executeSetting = null) where TCommand : CommandBase
        {
            Action<CommandComposite> verify = commandComposite =>
                                                  {
                                                      commandComposite.Parts.ShouldNotBeEmpty();
                                                      var part = commandComposite.Parts[0];
                                                      part.Message.ShouldEqualWeak(command);
                                                      part.Setting.ShouldEqualWeak(executeSetting ?? new MessageExecuteSetting());
                                                  };
            dispatcher.Setup(r => r.Push(Pleasure.MockIt.Is(verify))).Throws(exception);
        }

        public static void StubQuery<TQuery, TResult>(this Mock<IDispatcher> dispatcher, TQuery query, TResult result, MessageExecuteSetting executeSetting = null) where TQuery : QueryBase<TResult> where TResult : class
        {
            dispatcher
                    .Setup(r => r.Query(Pleasure.MockIt.IsStrong(query), Pleasure.MockIt.IsStrong(executeSetting)))
                    .Returns(result);
        }

        public static void StubQueryAsThrow<TQuery, TResult>(this Mock<IDispatcher> dispatcher, TQuery query, Exception exception, MessageExecuteSetting executeSetting = null) where TQuery : QueryBase<TResult> where TResult : class
        {
            dispatcher
                    .Setup(r => r.Query(Pleasure.MockIt.IsStrong(query), Pleasure.MockIt.IsStrong(executeSetting)))
                    .Throws(exception);
        }

        #endregion

        #region Repository

        public static void StubGetById<TEntity>(this Mock<IRepository> repository, object id, TEntity entity) where TEntity : class, IEntity
        {
            repository
                    .Setup(r => r.GetById<TEntity>(id))
                    .Returns(entity);
        }

        public static void StubPaginated<TEntity>(this Mock<IRepository> repository, PaginatedSpecification paginatedSpecification, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, IncPaginatedResult<TEntity> result = null)
                where TEntity : class, IEntity
        {
            repository.Setup(r => r.Paginated(Pleasure.MockIt.IsStrong(paginatedSpecification), orderSpecification, Pleasure.MockIt.IsStrong(whereSpecification, dsl => dsl.IncludeAllFields()), fetchSpecification))
                      .Returns(result);
        }

        public static void StubQuery<TEntity>(this Mock<IRepository> repository, OrderSpecification<TEntity> orderSpecification = null, Specification<TEntity> whereSpecification = null, FetchSpecification<TEntity> fetchSpecification = null, PaginatedSpecification paginatedSpecification = null, params TEntity[] entities) where TEntity : class, IEntity
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