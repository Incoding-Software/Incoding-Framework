namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///     All options description in http://jquery.malsup.com/form/#options-<c>object</c>
    /// </summary>
    public class JqueryAjaxFormOptions : MetaTypicalOptions
    {
        ////ncrunch: no coverage start

        #region Static Fields

        public static readonly JqueryAjaxFormOptions Default = new JqueryAjaxFormOptions();

        #endregion

        ////ncrunch: no coverage end

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
                    Data = routes.Select(r => new JqueryAjaxRoute { name = r.Key, value = r.Value.ToString() })
                                 .ToList();
                }
                else
                    this.Set("url", value);
            }
        }

        [UsedImplicitly]
        public IEnumerable<JqueryAjaxRoute> Data
        {
            set
            {
                var currentData = (this.GetOrDefault("data") as List<JqueryAjaxRoute>) ?? new List<JqueryAjaxRoute>();
                this.Set("data", currentData.Merger(value));
            }
        }

        #endregion
    }
}