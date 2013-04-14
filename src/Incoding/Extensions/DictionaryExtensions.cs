namespace Incoding.Extensions
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Diagnostics;

    #endregion

    public static class DictionaryExtensions
    {
        #region Factory constructors

        [DebuggerStepThrough]
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            return source.GetOrDefault(key, default(TValue));
        }

        [DebuggerStepThrough]
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            TValue res;
            return source.TryGetValue(key, out res) ? res : defaultValue;
        }

        [DebuggerStepThrough]
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> src, IEnumerable<KeyValuePair<TKey, TValue>> dest)
        {
            // ReSharper disable PossibleMultipleEnumeration
            foreach (var value in dest)
                src.Set(value.Key, value.Value);

            // ReSharper restore PossibleMultipleEnumeration
        }

        [DebuggerStepThrough]
        public static void Merge(this IDictionary<string, object> src, object dest)
        {
            var dictionaryFromAnonymus = AnonymousHelper.ToDictionary(dest);
            src.Merge(dictionaryFromAnonymus);
        }

        [DebuggerStepThrough]
        public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            source.Set(new KeyValuePair<TKey, TValue>(key, value));
        }

        [DebuggerStepThrough]
        public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> source, KeyValuePair<TKey, TValue> valuePair)
        {
            if (source.ContainsKey(valuePair.Key))
                source[valuePair.Key] = valuePair.Value;
            else
                source.Add(valuePair.Key, valuePair.Value);
        }

        #endregion
    }
}