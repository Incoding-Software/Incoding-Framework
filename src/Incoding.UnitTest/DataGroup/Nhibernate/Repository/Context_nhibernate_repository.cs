namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;
    using Moq;
    using NHibernate;

    #endregion

    public class Context_nhibernate_repository
    {
        #region Estabilish value

        protected static NhibernateRepository repository;

        protected static Mock<ISession> session;

        #endregion

        #region Fields

        Establish establish = () =>
                                  {
                                      session = Pleasure.Mock<ISession>();
                                      var nhibernateSessionFactory = Pleasure.MockAsObject<INhibernateSessionFactory>(mock => mock.Setup(r => r.GetCurrentSession()).Returns(session.Object));
                                      repository = new NhibernateRepository(nhibernateSessionFactory);
                                  };

        #endregion
    }
}