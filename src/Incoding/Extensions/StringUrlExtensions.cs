namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Web;
    using Incoding.Maybe;

    #endregion

    public static class StringUrlExtensions
    {
        #region Factory constructors

        public static string AppendSegment(this string root, string segment)
        {
            if (!segment.StartsWith("/"))
                segment = "/" + segment;

            if (root.EndsWith("/"))
                root = root.Substring(0, root.Length - 1);

            return "{0}{1}".F(root, segment);
        }

        public static string AppendToHashQueryString(this string value, object routes)
        {
            if (!value.Contains("#!"))
                return AppendTo(value, "/", routes);

            var splitUrl = value.Split("#!".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string baseUrl = splitUrl[0];
            string hashUrl = splitUrl[1];

            return baseUrl + "#!" + AppendTo(hashUrl, "/", routes);
        }

        public static string AppendToQueryString(this string value, object routes)
        {
            if (!value.Contains("#!"))
                return AppendTo(value, "&", routes);

            var splitUrl = value.Split("#!".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string baseUrl = splitUrl[0];
            string hashUrl = splitUrl[1];

            return AppendTo(baseUrl, "&", routes) + "#!" + hashUrl;
        }

        public static string SetHash(this string value, string hash)
        {
            string root = value
                    .If(r => r.Contains("#"))
                    .Then()
                    .ReturnOrDefault(r => r.Split("#".ToCharArray())[0], value);

            hash = hash
                    .If(r => r.Contains("?"))
                    .Then()
                    .Recovery(() => hash + "?");

            hash = hash
                    .Not(r => r.StartsWith("/"))
                    .Then()
                    .Recovery(() => hash.Substring(1, hash.Length - 1));

            return "{0}#!{1}".F(root, hash);
        }

        #endregion

        static string AppendTo(string value, string separateChar, object routes)
        {
            var dictionary = AnonymousHelper
                    .ToDictionary(routes)
                    .Where(r => r.Value != null)
                    .ToDictionary(r => r.Key, r => r.Value.ToString());

            if (dictionary.Count == 0)
                return value;

            bool hasExistsQueryString = value.Contains("?");
            if (hasExistsQueryString)
            {
                var queryString = HttpUtility.ParseQueryString(value.Split("?".ToCharArray())[1]);
                var originalQuery = queryString.AllKeys.ToDictionary(r => r, r => queryString[r]);
                foreach (var newParam in dictionary)
                    originalQuery.Set(newParam);

                dictionary = originalQuery;
                value = value.Substring(0, value.IndexOf("?", StringComparison.CurrentCultureIgnoreCase));
            }

            return "{0}?{1}".F(value, string.Join(separateChar, dictionary.Select(r => string.Format("{0}={1}", HttpUtility.UrlEncode(r.Key), HttpUtility.UrlEncode(dictionary[r.Key])))));
        }
    }
}