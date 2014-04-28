namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using Machine.Specifications.Annotations;

    #endregion


    public class DbEntityReference : IncEntityBase
    {
        #region Constructors

        public DbEntityReference()
        {
            Id = Guid.NewGuid();
        }

        #endregion

        #region Properties

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public new virtual Guid Id { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<DbEntityReference>
        {
            #region Constructors

            public Map()
            {
                Id(r => r.Id).GeneratedBy.Assigned();
            }

            #endregion
        }

        #endregion
    }
}