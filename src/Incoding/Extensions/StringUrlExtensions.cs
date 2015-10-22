namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Incoding.Maybe;
    using Incoding.MvcContrib;

    #endregion

    public static class StringUrlExtensions
    {
        #region Factory constructors

        public static string AppendOnlyToQueryString(this string value, object routes)
        {
            return AppendTo(value, "&", routes);
        }

        public static string AppendSegment(this string root, string segment)
        {
            if (!segment.StartsWith("/"))
                segment = "/" + segment;

            if (root.EndsWith("/"))
                root = root.Substring(0, root.Length - 1);

            return "{0}{1}".F(root, segment);
        }

        public static string AppendToHashQueryString(this string value, object routes, bool isHash = false)
        {
            const string hashSeparated = "#";
            bool hasHash = value.Contains(hashSeparated);
            if (!hasHash && isHash)
                return AppendTo(value, "/", routes);

            var splitUrl = value
                    .Split(new[] { hashSeparated }, StringSplitOptions.RemoveEmptyEntries);

            string baseUrl = splitUrl.ElementAtOrDefault(0);
            string hashUrl = splitUrl.ElementAtOrDefault(1);
            if (!hasHash)
                hashUrl = "!" + hashUrl;

            return baseUrl + hashSeparated + AppendTo(hashUrl, "/", routes);
        }

        public static string AppendToQueryString(this string value, object routes)
        {
            if (!value.Contains("#"))
                return AppendTo(value, "&", routes);

            var splitUrl = value.Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string baseUrl = splitUrl[0];
            string hashUrl = splitUrl[1];

            string specialChar = hashUrl.With(r => r.StartsWith("!")) ? string.Empty : "!";
            return AppendTo(baseUrl, "&", routes) + "#" + specialChar + hashUrl;
        }

        public static string SetHash(this string value, string hash)
        {
            string root = value
                    .If(r => r.Contains("#"))
                    .Then()
                    .ReturnOrDefault(r => r.Split("#".ToCharArray())[0], value);

            hash = hash.Contains("?") ? hash.Replace("&", "/") : hash + "?";

            if (hash.StartsWith("/"))
                hash = hash.Substring(1, hash.Length - 1);

            return "{0}#!{1}".F(root, hash);
        }

        #endregion

        static string AppendTo(string value, string separateChar, object routes)
        {
            var dictionary = AnonymousHelper
                    .ToDictionary(routes)
                    .Where(r => r.Value != null)
                    .Select(r =>
                            {
                                string switcherValue = r.Value is Selector ?
                                                               r.Value.ToString().Replace("&", "%26")
                                                                .Replace("?", "%3F")
                                                                .Replace("/", "%2F")
                                                                .Replace("=", "%3D")
                                                                .Replace(":", "%3A")
                                                                .Replace("@", "%40")
                                                                .Replace("#", "%23")
                                                               : r.Value.ToString();
                                return new KeyValuePair<string, string>(r.Key, switcherValue);
                            })
                    .ToDictionary(r => r.Key, r => r.Value);

            if (dictionary.Count == 0)
                return value;

            bool hasExistsQueryString = (value ?? string.Empty).Contains("?");
            if (hasExistsQueryString)
            {
                var originalQuery = value.Split("?".ToCharArray(), 2)[1]
                        .Split(separateChar.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                        .Select(r =>
                                {
                                    var pair = r.Split("=".ToCharArray(), 2);
                                    return new KeyValuePair<string, string>(pair.ElementAtOrDefault(0), pair.ElementAtOrDefault(1));
                                })
                        .ToDictionary(r => r.Key, r => r.Value);

                foreach (var newParam in dictionary)
                    originalQuery.Set(newParam);

                dictionary = originalQuery;
                value = value.Substring(0, value.IndexOf("?", StringComparison.CurrentCultureIgnoreCase));
            }

            string hashRoutes = string.Join(separateChar, dictionary.Select(r => string.Format("{0}={1}", HttpUtility.UrlEncode(r.Key), dictionary[r.Key])));
            return value != null ? "{0}?{1}".F(value, hashRoutes)
                           : hashRoutes;
        }
    }
}