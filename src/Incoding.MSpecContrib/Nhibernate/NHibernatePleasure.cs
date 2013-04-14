namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Windows.Forms;
    using FluentNHibernate;
    using FluentNHibernate.Cfg;
    using Incoding.Data;
    using Incoding.Extensions;
    using NHibernate;

    #endregion

    public static class NHibernatePleasure
    {
        #region Factory constructors

        public static void StartSession(FluentConfiguration fluentConfiguration, bool reloadDb)
        {
            SpecWithNHibernateSession.Session = Start(fluentConfiguration, reloadDb);
        }

        public static void StopAllSession()
        {
            if (SpecWithNHibernateSession.Session != null)
                SpecWithNHibernateSession.Session.Dispose();
        }

        #endregion

        static ISession Start(FluentConfiguration instanceBuilderConfiguration, bool reloadDb)
        {
            try
            {
                if (reloadDb)
                {
                    IManagerDataBase managerDataBase = new NhibernateManagerDataBase(instanceBuilderConfiguration);
                    if (!managerDataBase.IsExist())
                        managerDataBase.Create();

                    managerDataBase.Update();
                }

                var sessionSource = new SessionSource(instanceBuilderConfiguration);
                return sessionSource.CreateSession();
            }
            catch (Exception e)
            {
                Clipboard.SetText("Exception in  build configuration {0}".F(e));
                return null;
            }
        }
    }
}