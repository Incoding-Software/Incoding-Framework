namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using NHibernate.Linq;

    #endregion

    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class SqlFunctions
    {
        #region Factory constructors

        [LinqExtensionMethod("NEWID")]
        public static Guid NewID()
        {
            return Guid.NewGuid();
        }

        #endregion
    }
}