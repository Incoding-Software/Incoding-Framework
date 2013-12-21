namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using JetBrains.Annotations;
    using Incoding.Extensions;

    #endregion

    /// <summary>
    ///     All options description in http://jquery.malsup.com/form/#options-<c>object</c>
    /// </summary>
    public class JqueryAjaxFormOptions : MetaTypicalOptions
    {
        #region Static Fields

        ////ncrunch: no coverage start
        public static readonly JqueryAjaxFormOptions Default = new JqueryAjaxFormOptions();         
        ////ncrunch: no coverage end
        
        #endregion

        #region Constructors

        public JqueryAjaxFormOptions(MetaTypicalOptions @default)
                : base(@default) { }

        public JqueryAjaxFormOptions() { }

        #endregion

        #region Properties

        /// <summary>
        ///     Boolean value. Set to <c>true</c> to remove short delay before posting form when uploading files (or using the iframe option).
        ///     <remarks>
        ///         The delay is used to allow the browser to render DOM updates prior to performing a native form submit. This improves usability when displaying notifications to the user, such as "Please Wait..."
        ///     </remarks>
        ///     Default value: <c>false</c>.
        /// </summary>
        [UsedImplicitly]
        public bool ForceSync { set { this.Set("forceSync", value); } }

        /// <summary>
        ///     Boolean flag indicating whether data must be submitted in strict semantic order (slower).
        ///     <remarks>
        ///         Note that the normal form serialization is done in semantic order with the exception of input elements of type="image". You should only set the semantic option to true if your server has strict semantic requirements and your form contains an input element of type="image".
        ///     </remarks>
        ///     Default value: <see langword="false" />
        /// </summary>
        [UsedImplicitly]
        public bool Semantic { set { this.Set("semantic", value); } }

        /// <summary>
        ///     The method in which the form data
        ///     Default value: value of form's method attribute (or 'GET' if none found)
        /// </summary>
        [UsedImplicitly]
        public HttpVerbs Type { set { this.Set("type", value.ToString()); } }

        /// <summary>
        ///     URL to which the form data will be submitted.
        ///     Default value: value of form's action attribute
        /// </summary>
        [UsedImplicitly]
        public string Url
        {
            set
            {
                var routes = value.SelectQueryString();
                if (routes.Any())
                {
                    this.Set("url", value.Split('?')[0]);
                    this.Set("data", routes.Select(r => new { name = r.Key, value = r.Value.ToString() }));
                }
                else
                    this.Set("url", value);
            }
        }

        #endregion
    }
}