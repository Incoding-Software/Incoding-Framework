namespace Incoding.MvcContrib
{
    public partial class IncodingMetaLanguageDsl : IIncodingMetaLanguageUtilitiesDsl
    {
        #region IIncodingMetaLanguageCallbackBodyDsl Members

        public IIncodingMetaLanguageUtilitiesDsl Utilities { get { return this; } }

        #endregion

        #region IIncodingMetaLanguageUtilitiesDsl Members

        public IncodingMetaCallbackWindowDsl Window { get { return new IncodingMetaCallbackWindowDsl(this); } }

        public IncodingMetaCallbackDocumentDsl Document { get { return new IncodingMetaCallbackDocumentDsl(this); } }

        #endregion
    }
}