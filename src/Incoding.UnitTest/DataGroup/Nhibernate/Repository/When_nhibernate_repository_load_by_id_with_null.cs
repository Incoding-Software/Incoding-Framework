namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_load_by_id_with_null : Context_nhibernate_repository
    {
        Because of = () => repository.LoadById<IEntity>(null);

        It should_be_save = () => session.Verify(r => r.Load<IEntity>(Pleasure.MockIt.IsNull<string>()), Times.Never());
    }
}