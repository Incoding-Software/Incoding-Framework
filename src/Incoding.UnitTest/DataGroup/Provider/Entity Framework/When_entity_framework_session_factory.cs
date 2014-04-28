namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data.Entity;
    using System.Reflection;
    using Incoding.Data;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EntityFrameworkSessionFactory))]
    public class When_entity_framework_session_factory
    {
        #region Establish value

        static void SetCurrentSession(DbContext dbContext)
        {
            typeof(EntityFrameworkSessionFactory).GetField("currentSession", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, dbContext);
        }

        #endregion

        It should_be_create_with_exist_session = () =>
                                                     {
                                                         var incDbContext = new IncDbContext("IncRealEFDb");
                                                         SetCurrentSession(incDbContext);
                                                         new EntityFrameworkSessionFactory(() => incDbContext)
                                                                 .GetCurrent()
                                                                 .ShouldNotBeNull();
                                                     };

        It should_be_get_current_without_open = () =>
                                                    {
                                                        SetCurrentSession(null);
                                                        Catch.Exception(() => new EntityFrameworkSessionFactory(() => new IncDbContext("IncRealEFDb"))
                                                                                      .GetCurrent())
                                                             .ShouldNotBeNull();
                                                    };

        It should_be_open = () =>
                                {
                                    SetCurrentSession(null);
                                    new EntityFrameworkSessionFactory(() => new IncDbContext("IncRealEFDb"))
                                            .Open(null)
                                            .ShouldNotBeNull();
                                };

        It should_be_close = () =>
                                 {
                                     SetCurrentSession(null);
                                     new EntityFrameworkSessionFactory(() => new IncDbContext("IncRealEFDb"))
                                             .Dispose();
                                 };

        It should_be_open_session_with_connection_string = () =>
                                                               {
                                                                   SetCurrentSession(null);
                                                                   new EntityFrameworkSessionFactory(() => new IncDbContext("IncRealEFDb"))
                                                                           .Open("Data Source=.;Database=IncRealEFDb;Integrated Security=true;")
                                                                           .ShouldNotBeNull();
                                                               };
    }
}