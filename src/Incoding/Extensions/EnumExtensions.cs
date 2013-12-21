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
                    .ToLocalization().Replace(",", string.Empty)
                    .ToLowerInvariant();
        }

        public static string ToLocalization(this Enum value)
        {
            var enumType = value.GetType();

            Func<Enum, string> getDescription = (current) =>
                                                    {
                                                        var memberEnum = enumType.GetMember(current.ToString());
                                                        if (memberEnum.Length == 0)
                                                            return string.Empty;

                                                        var description = memberEnum[0].FirstOrDefaultAttribute<DescriptionAttribute>();
                                                        return description != null
                                                                       ? description.Description
                                                                       : current.ToString();
                                                    };

            if (enumType.HasAttribute<FlagsAttribute>())
            {
                var all = Enum.GetValues(enumType)
                              .Cast<Enum>()
                              .Where(value.HasFlag)
                              .Select(getDescription)
                              .ToList();

                return string.Join(" ", all);
            }

            return getDescription(value);
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