namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Reflection;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Raven.Client;

    #endregion

    [Subject(typeof(RavenDbSessionFactory))]
    public class When_raven_db_session_factory
    {
        #region Establish value

        static void SetCurrentSession(IDocumentSession documentSession)
        {
            typeof(RavenDbSessionFactory).GetField("currentSession", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, documentSession);
        }

        #endregion

        //It should_be_current_with_exist = () =>
        //                                      {
        //                                          SetCurrentSession(Pleasure.MockStrictAsObject<IDocumentSession>());
        //                                          new RavenDbSessionFactory(Pleasure.MockStrictAsObject<IDocumentStore>())
        //                                                  .GetCurrent()
        //                                                  .ShouldNotBeNull();
        //                                      };

        //It should_be_current_without_open = () =>
        //                                        {
        //                                            SetCurrentSession(null);
        //                                            Catch.Exception(() => new RavenDbSessionFactory(Pleasure.MockStrictAsObject<IDocumentStore>())
        //                                                                          .GetCurrent())
        //                                                 .ShouldNotBeNull();
        //                                        };

        It should_be_open = () =>
                                {
                                    SetCurrentSession(null);
                                    var initStore = Pleasure.MockStrictAsObject<IDocumentStore>(initMock => initMock.Setup(r => r.OpenSession()).Returns(Pleasure.MockStrictAsObject<IDocumentSession>()));
                                    new RavenDbSessionFactory(Pleasure.MockStrictAsObject<IDocumentStore>(mock => mock.Setup(r => r.Initialize()).Returns(initStore)))
                                            .Open(null)
                                            .ShouldNotBeNull();
                                };

        It should_be_open_session_with_connection_string = () =>
                                                               {
                                                                   SetCurrentSession(null);
                                                                   var initStore = Pleasure.MockStrictAsObject<IDocumentStore>(initMock => initMock.Setup(r => r.OpenSession("IncRealEFDb")).Returns(Pleasure.MockStrictAsObject<IDocumentSession>()));
                                                                   new RavenDbSessionFactory(Pleasure.MockStrictAsObject<IDocumentStore>(mock => mock.Setup(r => r.Initialize()).Returns(initStore)))
                                                                           .Open("IncRealEFDb")
                                                                           .ShouldNotBeNull();
                                                               };
    }


}