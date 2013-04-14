namespace Incoding.Block.Caching
{
    #region << Using >>

    using System.Diagnostics.CodeAnalysis;

    #endregion

    public interface ICacheKey
    {
        #region Methods

        /// <summary>
        ///     Unique key
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "False positive. Because usage func very native")]
        string GetName();

        #endregion
    }
}