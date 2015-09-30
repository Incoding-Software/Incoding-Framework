namespace Incoding.Data
{
    using System;
    using NHibernate.Linq;

    public class SqlFunctions
    {
        [LinqExtensionMethod("NEWID")]
        public static Guid NewID() { return Guid.NewGuid(); }
    }
}