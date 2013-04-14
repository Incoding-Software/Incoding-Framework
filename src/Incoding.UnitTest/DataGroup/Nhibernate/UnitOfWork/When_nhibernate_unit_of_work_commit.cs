namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(NhibernateUnitOfWork))]
    public class When_nhibernate_unit_of_work_commit : Context_nhibernate_unit_of_work
    {
        Because of = () => nhibernateUnit.Commit();

        It should_be_session_flush = () => session.Verify(r => r.Flush());

        It should_be_transaction_commit = () => transaction.Verify(r => r.Commit());
    }
}