namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class GetEntityByIdQuery<T> : QueryBase<T> where T : class, IEntity
    {
        #region Constructors

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor), ExcludeFromCodeCoverage]
        public GetEntityByIdQuery() { }

        ////ncrunch: no coverage start
        public GetEntityByIdQuery(object id)
        {
            Id = id;
        }

        #endregion

        ////ncrunch: no coverage end
        #region Properties

        public object Id { get; set; }

        #endregion

        #region Override

        protected override T ExecuteResult()
        {
            return Repository.GetById<T>(Id);
        }

        #endregion
    }
}