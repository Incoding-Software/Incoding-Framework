namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(NhibernateRepository))]
    public class When_nhibernate_repository_delete_by_id : Context_nhibernate_repository
    {
        Because of = () =>
                         {
                             string id = Pleasure.Generator.TheSameString();
                             var entity = Pleasure.Mock<IEntity>();

                             session
                                     .Setup(r => r.Load<IEntity>(id))
                                     .Returns(entity.Object);

                             repository.Delete<IEntity>(id);
                         };

        It should_be_save = () => session.Verify(r => r.Delete(Pleasure.MockIt.IsNotNull<IEntity>()));
    }
}