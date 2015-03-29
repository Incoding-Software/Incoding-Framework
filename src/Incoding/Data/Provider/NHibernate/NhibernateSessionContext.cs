using System;
using NHibernate;
using NHibernate.Context;
using NHibernate.Engine;

namespace Incoding.Data
{
    [Serializable]
    public class NhibernateSessionContext : CurrentSessionContext
    {
        private static ISession _session;

        /// <summary>
        /// Gets or sets the currently bound session.
        /// </summary>
        protected override ISession Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        public NhibernateSessionContext(ISessionFactoryImplementor factory)
        {
        }
    }
}