namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(NhibernateUnitOfWork))]
    public class When_nhibernate_unit_of_work_disposable : Context_nhibernate_unit_of_work
    {
        Establish establish = () =>
                                  {
                                      transaction.SetupGet(r => r.WasCommitted).Returns(true);
                                      transaction.SetupGet(r => r.WasRolledBack).Returns(true);
                                  };

        Because of = () => nhibernateUnit.Dispose();

        It should_be_session_dispose = () => session.Verify(r => r.Dispose());

        It should_be_transaction_dispose = () => transaction.Verify(r => r.Dispose());
    }
}