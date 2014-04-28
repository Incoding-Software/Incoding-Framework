namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using System.Data.SqlClient;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NHibernate;

    #endregion

    [Subject(typeof(NhibernateUnitOfWorkFactory))]
    public class When_nhibernate_unit_of_work_factory_create
    {
        #region Establish value

        static NhibernateUnitOfWorkFactory unitOfWorkFactory;

        static IUnitOfWork unitOfWork;

        #endregion

        Establish establish = () => { unitOfWorkFactory = new NhibernateUnitOfWorkFactory(Pleasure.MockStrictAsObject<INhibernateSessionFactory>()); };

        Because of = () => { unitOfWork = unitOfWorkFactory.Create(IsolationLevel.ReadCommitted, @"Data Source=Work\SQLEXPRESS;Database=IncRealDb;Integrated Security=true;"); };

        It should_be_transaction_active = () => unitOfWork.ShouldNotBeNull();
    }
}