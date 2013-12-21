namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using NHibernate;

    #endregion

    [Subject(typeof(NHibernateSessionExtensions))]
    public class When_nhibernate_session_extensions
    {
        It should_be_submit_entity = () =>
                                         {
                                             var entity = Pleasure.MockStrictAsObject<IEntity>();
                                             var session = Pleasure.MockStrict<ISession>(mock =>
                                                                                             {
                                                                                                 mock.Setup(r => r.Save(Pleasure.MockIt.IsNotNull<IEntity>())).Returns(entity);
                                                                                                 mock.Setup(r => r.Flush());
                                                                                                 mock.Setup(r => r.Clear());
                                                                                             });
                                             session.Object.SubmitEntity(entity);
                                             session.VerifyAll();
                                         };

        It should_be_delete_forever = () =>
                                          {
                                              var session = Pleasure.MockStrict<ISession>(mock =>
                                                                                              {
                                                                                                  mock.Setup(r => r.Get<IEntity>(Pleasure.Generator.TheSameString())).Returns(Pleasure.MockStrictAsObject<IEntity>());
                                                                                                  mock.Setup(r => r.Delete(Pleasure.MockIt.IsNotNull<IEntity>()));
                                                                                                  mock.Setup(r => r.Flush());
                                                                                                  mock.Setup(r => r.Clear());
                                                                                              });
                                              session.Object.DeleteForever<IEntity>(Pleasure.Generator.TheSameString());
                                              session.VerifyAll();
                                          };

        It should_be_delete = () =>
                                  {
                                      var entity = Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString()));
                                      var session = Pleasure.MockStrict<ISession>(mock => mock.Setup(r => r.Get<IEntity>(Pleasure.Generator.TheSameString())).Returns(() => null));
                                      Catch.Exception(() => session.Object.ShouldBeDelete(entity)).ShouldBeNull();
                                      session.VerifyAll();
                                  };

        It should_be_delete_exception = () =>
                                            {
                                                var entity = Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString()));
                                                var session = Pleasure.MockStrict<ISession>(mock => mock.Setup(r => r.Get<IEntity>(Pleasure.Generator.TheSameString())).Returns(entity));
                                                Catch.Exception(() => session.Object.ShouldBeDelete(entity)).ShouldNotBeNull();
                                                session.VerifyAll();
                                            };

        It should_be_exist_exception = () =>
                                           {
                                               var entity = Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString()));
                                               var session = Pleasure.MockStrict<ISession>(mock => mock.Setup(r => r.Get<IEntity>(Pleasure.Generator.TheSameString())));
                                               Catch.Exception(() => session.Object.ShouldBeExist(entity)).ShouldNotBeNull();
                                               session.VerifyAll();
                                           };
    }
}