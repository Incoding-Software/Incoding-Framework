namespace Incoding.SiteTest
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Data;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class Product : IncEntityBase
    {
        #region Properties

        public virtual string Name { get; set; }

        #endregion

        #region Nested classes

        [Obsolete(ObsoleteMessage.ClassNotForDirectUsage, true), ExcludeFromCodeCoverage]
        public class Map : NHibernateEntityMap<Product>
        {
            ////ncrunch: no coverage start
            #region Constructors

            protected Map()
            {
                IdGenerateByGuid(r => r.Id);
                MapEscaping(r => r.Name);
            }

            #endregion

            ////ncrunch: no coverage end        
        }

        #endregion
    }
}