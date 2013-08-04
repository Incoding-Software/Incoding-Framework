namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(NhibernateUnitOfWork))]
    public class When_nhibernate_unit_of_work_disposable_after_commit : Context_nhibernate_unit_of_work
    {
        Establish establish = () =>
                                  {
                                      transaction.SetupGet(r => r.WasCommitted).Returns(false);
                                      transaction.SetupGet(r => r.WasRolledBack).Returns(false);
                                  };

        Because of = () => nhibernateUnit.Dispose();

        It should_be_rollback = () => transaction.Verify(r => r.Rollback());
    }
}