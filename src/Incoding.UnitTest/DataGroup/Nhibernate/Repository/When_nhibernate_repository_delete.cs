namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_delete : Context_nhibernate_repository
    {
        Because of = () => repository.Delete(Pleasure.MockAsObject<IEntity>());

        It should_be_save = () => session.Verify(r => r.Delete(Pleasure.MockIt.IsNotNull<IEntity>()), Times.Once());
    }
}