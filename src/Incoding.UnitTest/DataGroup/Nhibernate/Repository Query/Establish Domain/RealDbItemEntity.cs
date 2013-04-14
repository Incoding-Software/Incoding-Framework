namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using Machine.Specifications.Annotations;

    #endregion

    public class RealDbItemEntity : IncEntityBase
    {
        #region Properties

        public virtual string Name { get; protected set; }

        public virtual RealDbEntity Parent { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<RealDbItemEntity>
        {
            #region Constructors

            public Map()
            {
                IdGenerateByGuid(r => r.Id);
                Map(r => r.Name);
                DefaultReference(r => r.Parent);
            }

            #endregion
        }

        #endregion
    }
}