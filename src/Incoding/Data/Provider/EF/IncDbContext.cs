namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;
    using Incoding.Extensions;

    #endregion

    public class IncDbContext : DbContext
    {
        #region Fields

        readonly List<Type> mapsTypes;

        #endregion

        #region Constructors

        public IncDbContext(string nameOrConnectionString, Assembly mapAssembly)
                : base(nameOrConnectionString)
        {
            this.mapsTypes = mapAssembly.GetTypes()
                                        .Where(r => r.IsImplement(typeof(EFClassMap<>)) &&
                                                    !r.IsInterface &&
                                                    !r.IsAbstract)
                                        .ToList();
        }

        public IncDbContext(string nameOrConnectionString)
                : this(nameOrConnectionString, Assembly.GetCallingAssembly()) { }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var mapsType in this.mapsTypes)
            {
                var map = Activator.CreateInstance(mapsType) as IEFClassMap;
                map.OnModelCreating(modelBuilder);
            }
        }
    }
}