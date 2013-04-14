namespace Incoding.Extensions
{
    #region << Using >>

    using System.Diagnostics;
    using System.Drawing;

    #endregion

    public static class OtherExtensions
    {
        #region Factory constructors

        [DebuggerStepThrough]
        public static string ToHex(this Color color)
        {
            return "#{0:X2}{1:X2}{2:X2}".F(color.R, color.G, color.B);
        }

        #endregion
    }
}