namespace Incoding.MvcContrib
{
    public interface IIncodingMetaLanguageUtilitiesDsl
    {
        IncodingMetaCallbackDocumentDsl Document { get; }

        IncodingMetaCallbackWindowDsl Window { get; }
        
    }
}