namespace Incoding.Extensions
{
    #region << Using >>

    using System.Diagnostics;
    using System.Linq;

    #endregion

    public static class EqualsExtensions
    {
        #region Factory constructors

        [DebuggerStepThrough]
        public static bool IsAllContains(this string value, params string[] param)
        {
            return param.All(value.Contains);
        }

        [DebuggerStepThrough]
        public static bool IsAllContainsIgnoreCase(this string value, params string[] param)
        {
            return value.ToLower().IsAnyContains(param.Select(r => r.ToLower()).ToArray());
        }

        [DebuggerStepThrough]
        public static bool IsAnyContains(this string value, params string[] param)
        {
            return param.Any(value.Contains);
        }

        [DebuggerStepThrough]
        public static bool IsAnyContainsIgnoreCase(this string value, params string[] param)
        {
            return value.ToLower().IsAnyContains(param.Select(r => r.ToLower()).ToArray());
        }

        [DebuggerStepThrough]
        public static bool IsAnyEquals<TObject>(this TObject value, params TObject[] param)
        {
            return param.Any(r => r.Equals(value));
        }

        [DebuggerStepThrough]
        public static bool IsAnyEqualsIgnoreCase(this string value, params string[] param)
        {
            return value.ToLower().IsAnyEquals(param.Select(r => r.ToLower()).ToArray());
        }

        [DebuggerStepThrough]
        public static bool IsReferenceEquals(this object original, object expected)
        {
            if (original == null && expected == null)
                return true;

            if (original == null || expected == null)
                return false;

            return true;
        }

        #endregion
    }
}