namespace Incoding.UnitTest
{
    using System;
    using Incoding.CQRS;
    using Incoding.Data;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    [Subject(typeof(MessageBase<>))]
    public class When_message_base_repository_without_unit_of_work : Context_message_base_repository
    {
        #region Establish value

        static Mock<IUnitOfWork> unitOfWork;

        static Exception exception;

        #endregion

        Establish establish = () => Establish();

        Because of = () => { exception = Catch.Exception(() => message.Execute()); };

        It should_be_resolve_once = () => provider.Verify(r => r.TryGet<IRepository>(), Times.Once());

        It should_be_safety = () => exception.ShouldBeNull();
    }
}