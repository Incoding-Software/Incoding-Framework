namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_save_or_update : Context_nhibernate_repository
    {
        Because of = () => repository.SaveOrUpdate(Pleasure.MockAsObject<IEntity>());

        It should_be_save = () => session.Verify(r => r.SaveOrUpdate(Pleasure.MockIt.IsNotNull<IEntity>()));
    }
}