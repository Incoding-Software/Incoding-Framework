namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data.Entity;
    using Incoding.Data;
    using Machine.Specifications;
    using NCrunch.Framework;

    #endregion

    [Subject(typeof(EntityFrameworkRepository)), Isolated]
    public class When_entity_framework_repository : Behavior_repository
    {
        Because of = () =>
                     {
                         Database.SetInitializer(new DropCreateDatabaseIfModelChanges<IncDbContext>());
                         var dbContext = new IncDbContext("IncRealEFDb", typeof(DbEntity).Assembly);
                         dbContext.Configuration.ValidateOnSaveEnabled = false;
                         dbContext.Configuration.LazyLoadingEnabled = true;
                         repository = new EntityFrameworkRepository(dbContext);
                         repository.Init();
                     };

        Behaves_like<Behavior_repository> should_be_behavior;
    }
}