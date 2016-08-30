namespace Incoding.Extensions
{
    #region << Using >>

    using System.Linq;
    using Incoding.Maybe;

    #endregion

    public static class EqualsExtensions
    {
        #region Factory constructors

        public static bool IsAllContains(this string value, params string[] param)
        {
            return param.All(value.Contains);
        }

        public static bool IsAllContainsIgnoreCase(this string value, params string[] param)
        {
            return value.ToLower().IsAllContains(param.Select(r => r.ToLower()).ToArray());
        }

        public static bool IsAnyContains(this string value, params string[] param)
        {
            return param.Any(value.Contains);
        }

        public static bool IsAnyContainsIgnoreCase(this string value, params string[] param)
        {
            return value.ToLower().IsAnyContains(param.Select(r => r.ToLower()).ToArray());
        }

        public static bool IsAnyEquals<TObject>(this TObject value, params TObject[] param)
        {
            return param.Any(r => r.Equals(value));
        }

        public static bool IsAllEquals<TObject>(this TObject value, params TObject[] param)
        {
            return param.All(r => r.Equals(value));
        }

        public static bool IsAnyEqualsIgnoreCase(this string value, params string[] param)
        {
            return value.ToLower().IsAnyEquals(param.Select(r => r.ToLower()).ToArray());
        }

        public static bool IsReferenceEquals(this object original, object expected)
        {
            if (original == null && expected == null)
                return true;

            if (original != null && expected != null)
                return true;

            if (original.With(r => r.GetType()) != expected.With(r => r.GetType()))
                return false;

            return original.With(r => r.GetType()) != expected.With(r => r.GetType());
        }

        #endregion
    }
}