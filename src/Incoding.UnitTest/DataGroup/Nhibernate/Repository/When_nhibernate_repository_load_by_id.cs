namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_load_by_id : Context_nhibernate_repository
    {
        Because of = () => repository.LoadById<IEntity>(Pleasure.Generator.TheSameString());

        It should_be_save = () => session.Verify(r => r.Load<IEntity>(Pleasure.Generator.TheSameString()));
    }
}