namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using JetBrains.Annotations;

    #endregion

    public class JqueryAjaxOptions : MetaTypicalOptions
    {
        ////ncrunch: no coverage start
        #region Static Fields

        public static readonly JqueryAjaxOptions Default = new JqueryAjaxOptions();

        #endregion

        ////ncrunch: no coverage end
        #region Constructors

        public JqueryAjaxOptions() { }

        public JqueryAjaxOptions(MetaTypicalOptions @default)
                : base(@default) { }

        #endregion

        ////ncrunch: no coverage start

        ////ncrunch: no coverage end
        #region Properties

        /// <summary>
        ///     Default: <c>true</c> By default, all requests are sent asynchronously (i.e. <c>this</c> is set to <c>true</c> by default).
        ///     If you need synchronous requests, set <c>this</c> option to <c>false</c>.
        ///     <remarks>
        ///         Cross-domain requests and dataType: "jsonp" requests do not support synchronous operation.
        ///         Note that synchronous requests may temporarily <c>lock</c> the browser, disabling any actions <c>while</c> the request is active.
        ///     </remarks>
        /// </summary>
        public bool Async
        {
            set { Set("async", value); }
        }

        /// <summary>
        ///     Default: <c>true</c>
        ///     If set to <c>false</c>, it will force requested pages not to be cached by the browser. Setting cache to <c>false</c> also appends a query string parameter, "_=[TIMESTAMP]", to the URL.
        /// </summary>
        [UsedImplicitly]
        public bool Cache
        {
            set { Set("cache", value); }
        }

        /// <summary>
        ///     Default: <c>true</c> for same-domain requests, <c>true</c> for cross-domain requests
        ///     If you wish to force a crossDomain request (such as JSONP) on the same domain, set the value of crossDomain to <c>true</c>.
        ///     This allows, for example, server-side redirection to another domain.
        /// </summary>
        [UsedImplicitly]
        public bool CrossDomain
        {
            set { Set("crossDomain", value); }
        }

        /// <summary>
        ///     Default: <c>true</c>.
        ///     Whether to trigger global Ajax event handlers for <c>this</c> request.
        ///     The default is <c>true</c>. Set to <c>false</c> to prevent the global handlers like ajaxStart or ajaxStop from being triggered.
        ///     This can be used to control various Ajax Events.
        /// </summary>
        [UsedImplicitly]
        public bool Global
        {
            set { Set("global", value); }
        }

        /// <summary>
        ///     Default:<c>true</c>. Set <c>this</c> to <c>true</c> if you wish to use the traditional style of param serialization.
        /// </summary>
        [UsedImplicitly]
        public bool Traditional
        {
            set { Set("traditional", value); }
        }

        /// <summary>
        ///     Default: <see cref="HttpVerbs.Get" />. The type of request to make ("POST" or "GET"), default is "GET".
        ///     Note: Other HTTP request methods, such as PUT and DELETE, can also be used here, but they are not supported by all browsers.
        /// </summary>
        [UsedImplicitly]
        public HttpVerbs Type
        {
            set { Set("type", value.ToString().ToUpper()); }
        }

        /// <summary>
        ///     Set a timeout (in milliseconds) for the request.
        /// </summary>
        [UsedImplicitly]
        public int Timeout
        {
            set { Set("timeout", value); }
        }

        #endregion

        #region Api Methods

        public void WithUrl(string url)
        {
            int indexQuerySplit = url.IndexOf("?", StringComparison.InvariantCultureIgnoreCase);
            if (indexQuerySplit != -1)
            {
                string query = url.Split("?".ToCharArray())[1] ?? string.Empty;
                var queryString = HttpUtility.ParseQueryString(query);
                var dictionaryParams = queryString.AllKeys.Select(r => new KeyValuePair<string, object>(r, queryString[r].ToString())).ToDictionary(pair => pair.Key, pair => pair.Value);
                string data = dictionaryParams.Select(r => new { name = r.Key, selector = r.Value.ToString() }).ToJsonString();
                Set("data", data);

                url = url.Substring(0, indexQuerySplit);
            }

            Set("url", url);
        }

        #endregion
    }
}