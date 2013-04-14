namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Block.Caching;

    #endregion

    public class Context_CachingFactoryTestEx
    {
        #region Nested classes

        protected class FakeCache : ICacheKey
        {
            #region Fields

            readonly string name;

            #endregion

            #region Constructors

            public FakeCache(string name)
            {
                this.name = name;
            }

            #endregion

            #region ICacheKey Members

            public string GetName()
            {
                return this.name;
            }

            #endregion
        }

        #endregion
    }
}