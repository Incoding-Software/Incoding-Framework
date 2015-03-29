namespace Incoding
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class IncWebException : Exception
    {
        #region Constructors

        [Obsolete("Please use factory method IncWebException.For")]
        public IncWebException(string message) : base(message)
        {
            Errors = new Dictionary<string, List<string>>();
        }

        #endregion

        #region Factory constructors

        public static IncWebException For<TModel>(Expression<Func<TModel, object>> property, string message)
        {
            return new IncWebException(message).Also(property, message);
        }

        public static IncWebException ForServer(string message)
        {
            return new IncWebException(message).Also("Server", message);
        }

        public static IncWebException For(string property, string message)
        {
            return new IncWebException(message).Also(property, message);
        }

        #endregion

        #region Properties

        public Dictionary<string, List<string>> Errors { get; set; }

        #endregion

        #region Api Methods

        public IncWebException Also<TModel>(Expression<Func<TModel, object>> property, string message)
        {
            return Also(property.GetMemberNameAsHtmlId(), message);
        }

        public IncWebException Also(string property, string message)
        {
            if (Errors.ContainsKey(property))
                Errors[property].Add(message);
            else
                Errors.Add(property, new List<string> { message });

            return this;
        }

        #endregion
    }
}