namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_get_by_id : Context_nhibernate_repository
    {
        Because of = () => repository.GetById<IEntity>(Pleasure.Generator.TheSameString());

        It should_be_save = () => session.Verify(r => r.Get<IEntity>(Pleasure.Generator.TheSameString()));
    }
}