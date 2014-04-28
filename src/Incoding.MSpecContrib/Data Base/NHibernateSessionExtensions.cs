namespace Incoding.MSpecContrib
{
    #region << Using >>

    using Incoding.Data;
    using Machine.Specifications;
    using NHibernate;

    #endregion

    public static class NHibernateSessionExtensions
    {
        #region Factory constructors

        public static void DeleteForever<TEntity>(this ISession session, string id) where TEntity : IEntity
        {
            var entityFromDb = session.Get<TEntity>(id);
            session.DeleteForever(entityFromDb);
        }

        public static void DeleteForever<TEntity>(this ISession session, TEntity entity) where TEntity : IEntity
        {
            session.Delete(entity);
            Submit(session);
        }

        public static void ShouldBeDelete<TEntity>(this ISession session, TEntity entity) where TEntity : IEntity
        {
            session.Get<TEntity>(entity.Id).ShouldBeNull();
        }

        public static void ShouldBeExist<TEntity>(this ISession session, TEntity entity) where TEntity : IEntity
        {
            session.Get<TEntity>(entity.Id).ShouldNotBeNull();
        }

        public static void Submit(this ISession session)
        {
            session.Flush();
            session.Clear();
        }

        public static void SubmitEntity<TEntity>(this ISession session, TEntity entity)
        {
            session.Save(entity);
            Submit(session);
        }

        #endregion
    }
}