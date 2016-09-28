namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.WebPages;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class IncodingResult : ActionResult
    {
        #region Properties

        public object Data { get; set; }

        #endregion

        public override string ToString()
        {
            return Data.ToJsonString();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            var isMultiPart = context.HttpContext.Request.ContentType.Contains("multipart/form-data");
            var isIe = context.HttpContext.Request.Browser.Browser.IsAnyEqualsIgnoreCase("IE");
            response.ContentType = isMultiPart && isIe ? "text/html" : "application/json";
            response.Write(Data.ToJsonString());
        }

        #region Factory constructors

        public static IncodingResult Error(object data = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new IncodingResult
                   {
                           Data = new JsonData(false, data, string.Empty, statusCode)
                   };
        }

        public static IncodingResult Error(ModelStateDictionary modelState)
        {
            var errorData = modelState
                    .Select(valuePair => new JsonModelStateData
                                         {
                                                 name = valuePair.Key,
                                                 isValid = !modelState[valuePair.Key].Errors.Any(),
                                                 errorMessage = modelState[valuePair.Key].Errors
                                                                                         .FirstOrDefault()
                                                                                         .ReturnOrDefault(s => s.ErrorMessage, string.Empty)
                                         });

            return new IncodingResult
                   {
                           Data = new JsonData(false, errorData, string.Empty, HttpStatusCode.OK)
                   };
        }

        public static IncodingResult RedirectTo(string url)
        {
            Guard.NotNullOrWhiteSpace("url", url);
            return new IncodingResult
                   {
                           Data = new JsonData(true, string.Empty, url, HttpStatusCode.OK)
                   };
        }

        public static IncodingResult Success(object data = null)
        {
            return new IncodingResult
                   {
                           Data = new JsonData(true, data, string.Empty, HttpStatusCode.OK),
                   };
        }

        public static IncodingResult Success(Func<object, HelperResult> text)
        {
            string data = text
                    .Invoke(null)
                    .ToHtmlString()
                    .Replace(Environment.NewLine, string.Empty);
            return Success(data);
        }

        #endregion

        #region Nested classes

        public class JsonData
        {
            #region Constructors

            public JsonData(bool success, object data, string redirectTo, HttpStatusCode statusCode)
            {
                this.success = success;
                this.data = data;
                this.redirectTo = redirectTo;
                this.statusCode = statusCode;
            }

            #endregion

            #region Properties

            public bool success { get; set; }

            public object data { get; set; }

            public string redirectTo { get; set; }

            public HttpStatusCode statusCode { get; set; }

            #endregion
        }

        public class JsonModelStateData
        {
            #region Properties

            public string name { get; set; }

            public bool isValid { get; set; }

            public string errorMessage { get; set; }

            #endregion
        }

        #endregion
    }
}