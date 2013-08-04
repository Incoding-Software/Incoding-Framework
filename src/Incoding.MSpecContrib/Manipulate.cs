namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Linq;

    #endregion

    public static class Manipulate
    {
        #region Factory constructors

        public static string CutPart(this string value)
        {
            Guard.NotNullOrWhiteSpace("value", value);

            int start = Pleasure.Generator.PositiveNumber(1, value.Length);
            return value.Substring(start);
        }

        public static string Inverse(this string inverseValue)
        {
            Guard.NotNull("inverseValue", inverseValue);

            while (true)
            {
                string newValue = Pleasure.Generator.String();
                if (!newValue.Equals(inverseValue, StringComparison.InvariantCultureIgnoreCase))
                    return newValue;

                ////ncrunch: no coverage start
            }

            ////ncrunch: no coverage end
        }

        public static int Inverse(this int inverseValue)
        {
            return Pleasure.Generator.PositiveNumber(inverseValue + 1);
        }

        public static TEnum Inverse<TEnum>(this Enum inverseValue)
        {
            var allEnums = Enum.GetValues(typeof(TEnum)).Cast<int>().ToList();

            int min = allEnums.Min();
            int max = allEnums.Max();

            while (true)
            {
                int nextEnum = new Random().Next(min, max);
                ////ncrunch: no coverage start
                if (!allEnums.Contains(nextEnum))
                    continue;
                ////ncrunch: no coverage end

                string enumAsString = allEnums.FirstOrDefault(r => r == nextEnum).ToString();
                var randomValue = (TEnum)Enum.Parse(typeof(TEnum), enumAsString, true);
                if (!inverseValue.Equals(randomValue))
                    return randomValue;

                ////ncrunch: no coverage start
            }

            ////ncrunch: no coverage end
        }

        public static bool Inverse(this bool inverseValue)
        {
            return !inverseValue;
        }

        public static string InverseCase(this string value)
        {
            Guard.NotNullOrWhiteSpace("value", value);
            return char.IsUpper(value[value.Length - 1]) ? value.ToLower() : value.ToUpper();
        }

        #endregion
    }
}