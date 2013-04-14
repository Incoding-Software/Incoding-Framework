namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_get_by_id_with_null : Context_nhibernate_repository
    {
        Because of = () => repository.GetById<IEntity>(null);

        It should_be_save = () => session.Verify(r => r.Get<IEntity>(Pleasure.MockIt.IsNull<string>()), Times.Never());
    }
}