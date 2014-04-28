namespace Incoding.Data
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Incoding.Extensions;

    #endregion

    public class DictionaryAsString
    {
        #region Constants

        const string splitKeyValue = "=";

        const string splitPair = "&";

        #endregion
        
        #region Factory constructors

        public static void Join(ref string source, string key, object value)
        {
            Guard.NotNullOrWhiteSpace("key", key);
            Guard.NotNull("value", value);

            var dictionaryAsString = Parse(source);
            dictionaryAsString.Set(key, HttpUtility.UrlEncode(value.ToString()));
            source = dictionaryAsString.Aggregate(string.Empty, (current, pair) => current += string.Format("{0}{1}{2}{3}", pair.Key, splitKeyValue, pair.Value, splitPair));
        }

        public static void Join(ref string source, IDictionary<string, string> dictionary)
        {
            Guard.NotNull("dictionary", dictionary);
            foreach (var valuePair in dictionary)
                Join(ref source, valuePair.Key, valuePair.Value);
        }

        public static IDictionary<string, string> Parse(string source)
        {
            bool conditional = string.IsNullOrWhiteSpace(source) || source.Contains(splitKeyValue);
            Guard.IsConditional("source", conditional);

            if (string.IsNullOrWhiteSpace(source))
                return new Dictionary<string, string>();

            return source
                    .Split(splitPair.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                    .Select(keyValue => keyValue.Split(splitKeyValue.ToCharArray())).ToDictionary(tempArray => tempArray[0], tempArray => HttpUtility.UrlDecode(tempArray[1]));
        }

        #endregion
    }
}