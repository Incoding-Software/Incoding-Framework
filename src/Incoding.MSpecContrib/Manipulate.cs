namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Windows.Forms;

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

        public static T Inverse<T>(this T inverseValue)
        {
            var newValue = Pleasure.Generator.Invent<T>();
            while (inverseValue.IsEqualWeak(newValue))
                newValue = Pleasure.Generator.Invent<T>();

            return newValue;
        }


        public static string InverseCase(this string value)
        {
            Guard.NotNullOrWhiteSpace("value", value);
            return char.IsUpper(value[value.Length - 1]) ? value.ToLower() : value.ToUpper();
        }

        #endregion
    }
}