namespace Incoding.UnitTest
{
    using Incoding.Data;
    using Machine.Specifications;

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_flush : Context_nhibernate_repository
    {
        Because of = () => repository.Flush();

        It should_be_save = () => session.Verify(r => r.Flush());
    }
}