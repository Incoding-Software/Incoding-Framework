namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;
    using System.Text;
    using Incoding.Extensions;

    #endregion

    public class DefaultParserException : IParserException
    {
        #region IParserException Members

        public string Parse(Exception exception)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Date:{0}".F(DateTime.Now));
            stringBuilder.AppendLine("Body:{0}".F(exception.ToString()));

            var nextInner = exception.InnerException;
            while (nextInner != null)
            {
                stringBuilder.AppendLine("Inner Exception: {0}".F(nextInner));
                nextInner = nextInner.InnerException;
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}