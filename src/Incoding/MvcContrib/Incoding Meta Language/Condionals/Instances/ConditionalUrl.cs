namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Maybe;

    #endregion

    public class ConditionalUrl : ConditionalBase
    {
        #region Fields

        readonly IDictionary<string, object> ajax;

        #endregion

        #region Constructors

        public ConditionalUrl(string url, Action<JqueryAjaxOptions> configuration, bool and)
                : base(ConditionalOfType.Url.ToString(), and)
        {
            var def = new JqueryAjaxOptions(JqueryAjaxOptions.Default);
            def.WithUrl(url);
            def.Async = false;

            configuration.Do(action => action(def));

            this.ajax = def.OptionCollections;
        }

        #endregion

        public override object GetData()
        {
            return new
                       {
                               this.type, 
                               this.inverse, 
                               this.ajax, 
                               this.and
                       };
        }
    }
}