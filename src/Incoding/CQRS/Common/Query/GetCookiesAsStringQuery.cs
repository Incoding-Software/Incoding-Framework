namespace Incoding.CQRS
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Incoding.Block;
    using Incoding.Maybe;
    using JetBrains.Annotations;

    #endregion

    [ExcludeFromCodeCoverage, UsedImplicitly]
    public class GetCookiesAsStringQuery : QueryBase<string>
    {
        #region Constructors

        public GetCookiesAsStringQuery() { }

        public GetCookiesAsStringQuery(string key)
        {
            Key = key;
        }

        #endregion

        #region Properties

        public string Key { get; set; }

        #endregion

        protected override string ExecuteResult()
        {
            return HttpContext.Current.With(r => r.Request)
                              .With(r => r.Cookies[Key])
                              .With(r => r.Value);
        }
    }
}