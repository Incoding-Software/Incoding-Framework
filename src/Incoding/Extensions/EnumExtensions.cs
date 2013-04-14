namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    #endregion

    public static class EnumExtensions
    {
        #region Factory constructors

        public static IEnumerable<T> ToArrayEnum<T>(this Type value)
        {            
            return Enum
                    .GetValues(value)
                    .Cast<int>()
                    .Where(r => r != 0)
                    .Cast<T>()
                    .ToArray();
        }

        public static string ToJqueryString(this Enum value)
        {
            return value
                    .ToString().Replace(",", string.Empty)
                    .ToLowerInvariant();
        }

        public static string ToLocalization(this Enum value)
        {
            var memberEnum = value.GetType().GetMember(value.ToString());
            if (memberEnum.Length == 0)
                return string.Empty;

            var description = memberEnum[0].FirstOrDefaultAttribute<DescriptionAttribute>();
            return description != null
                           ? description.Description
                           : value.ToString();
        }

        public static string ToStringInt(this Enum value)
        {
            return value.ToString("d");
        }

        public static string ToStringLower(this Enum value)
        {
            return value.ToString().ToLower();
        }

        #endregion
    }
}