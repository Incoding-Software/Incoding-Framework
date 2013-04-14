namespace Incoding.Data
{
    #region << Using >>

    using System;
    using FluentNHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;

    #endregion

    ////ncrunch: no coverage start
    public class NhibernateManagerDataBase : IManagerDataBase
    {
        #region Fields

        readonly SchemaExport schemaExport;

        readonly SchemaValidator schemaValidate;

        readonly SchemaUpdate schemaUpdate;

        #endregion

        #region Constructors

        public NhibernateManagerDataBase(FluentConfiguration builderConfiguration)
        {
            Guard.NotNull("fluentConfiguration", builderConfiguration);

            var configuration = builderConfiguration.BuildConfiguration();
            this.schemaExport = new SchemaExport(configuration);
            this.schemaUpdate = new SchemaUpdate(configuration);
            this.schemaValidate = new SchemaValidator(configuration);
        }

        #endregion

        #region IManagerDataBase Members

        public void Create()
        {
            this.schemaExport.Create(true, true);
        }

        public void Drop()
        {
            this.schemaExport.Drop(false, true);
        }

        public void Update()
        {
            this.schemaUpdate.Execute(true, true);
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
                this.schemaValidate.Validate();
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