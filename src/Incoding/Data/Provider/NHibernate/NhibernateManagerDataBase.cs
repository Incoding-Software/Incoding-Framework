namespace Incoding.Data
{
    #region << Using >>

    using System;
    using FluentNHibernate.Cfg;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;

    #endregion

    ////ncrunch: no coverage start
    public class NhibernateManagerDataBase : IManagerDataBase
    {
        #region Fields

        readonly Lazy<SchemaExport> schemaExport;

        readonly Lazy<SchemaValidator> schemaValidate;

        readonly Lazy<SchemaUpdate> schemaUpdate;

        #endregion

        #region Constructors

        public NhibernateManagerDataBase(Configuration configuration)
        {
            this.schemaExport = new Lazy<SchemaExport>(() => new SchemaExport(configuration));
            this.schemaUpdate = new Lazy<SchemaUpdate>(() => new SchemaUpdate(configuration));
            this.schemaValidate = new Lazy<SchemaValidator>(() => new SchemaValidator(configuration));
        }

        public NhibernateManagerDataBase(FluentConfiguration builderConfiguration)
                : this(builderConfiguration.BuildConfiguration()) { }

        #endregion

        #region IManagerDataBase Members

        public void Create()
        {
            this.schemaExport.Value.Create(true, true);
        }

        public void Drop()
        {
            this.schemaExport.Value.Drop(false, true);
        }

        public void Update()
        {
            this.schemaUpdate.Value.Execute(true, true);
        }

        public bool IsExist()
        {
            Exception exception;
            return IsExist(out exception);
        }

        public bool IsExist(out Exception outException)
        {
            outException = null;
            try
            {
                this.schemaValidate.Value.Validate();
                return true;
            }
            catch (Exception exception)
            {
                outException = exception;
                return false;
            }
        }

        #endregion
    }

    ////ncrunch: no coverage end
}