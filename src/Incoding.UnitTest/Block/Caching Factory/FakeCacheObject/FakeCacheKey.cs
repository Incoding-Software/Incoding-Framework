namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.Caching;
    using Incoding.Maybe;
    using Machine.Specifications.Annotations;

    #endregion

    [Fake]
    public class FakeCacheKey : ICacheKey
    {
        #region Fields

        readonly string secretKey;

        string PrivateField;

        #endregion

        #region Constructors

        public FakeCacheKey() { }

        public FakeCacheKey(string secretKey)
        {
            this.secretKey = secretKey;
        }

        #endregion

        #region Properties

        public bool IsReadyToExpires { get; set; }

        [UsedImplicitly]
        protected string ProtectedProperty { get; set; }

        #endregion

        #region ICacheKey Members

        public string GetName()
        {
            return this.secretKey
                       .If(r => !string.IsNullOrWhiteSpace(r))
                       .ReturnOrDefault(r => r, GetType().Name);
        }

        #endregion
    }
}