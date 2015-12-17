namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    #endregion

    public class CollectionAsString
    {
        #region Constants

        const string splitChar = "&";

        #endregion

        #region Factory constructors

        public static void Join(ref string source, string value)
        {
            Join(ref source, new[] { value });
        }

        public static void Join(ref string source, IEnumerable<string> values)
        {
            var collectionAsString = Parse(source).ToList();
            collectionAsString.AddRange(values.Select(HttpUtility.UrlEncode));

            source = collectionAsString.Aggregate(string.Empty, (current, item) => current += item + splitChar);
        }

        public static IEnumerable<string> Parse(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return new List<string>();

            return source
                    .Split(splitChar.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(HttpUtility.UrlDecode);
        }

        #endregion
    }
}