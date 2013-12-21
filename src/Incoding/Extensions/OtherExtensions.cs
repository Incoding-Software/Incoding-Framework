namespace Incoding.Extensions
{
    #region << Using >>

    using System;
    using System.Drawing;

    #endregion

    public static class OtherExtensions
    {
  
        public static bool IsEmpty(this Guid? value)
        {
            return value.GetValueOrDefault(Guid.Empty) == Guid.Empty;
        }

        public static string ToHex(this Color color)
        {
            return "#{0:X2}{1:X2}{2:X2}".F(color.R, color.G, color.B);
        }
    }
}