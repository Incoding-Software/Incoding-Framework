namespace Incoding
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class IncWebException : Exception
    {
        #region Static Fields

        public static readonly IncWebException Empty = new IncWebException(string.Empty, string.Empty);

        #endregion

        #region Constructors

        protected IncWebException(string property, string message)
                : base(message)
        {
            Property = property;
        }

        #endregion

        #region Factory constructors

        public static IncWebException For<TModel>(Expression<Func<TModel, object>> property, string message)
        {
            return For(property.GetMemberNameAsHtmlId(), message);
        }

        public static IncWebException For(string property, string message)
        {
            return new IncWebException(property, message);
        }

        public static IncWebException ForInput<TModel>(Expression<Func<TModel, object>> property, string message)
        {
            return new IncWebException("input.{0}".F(property.GetMemberNameAsHtmlId()), message);
        }

        public static IncWebException ForServer(string message)
        {
            return new IncWebException("Server", message);
        }

        #endregion

        #region Properties

        public string Property { get; protected set; }

        #endregion
    }
}