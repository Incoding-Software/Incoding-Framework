namespace Incoding.Extensions
{
    #region << Using >>

    using System.Diagnostics;

    #endregion

    public static class MathHelper
    {
        #region Factory constructors

        
        public static decimal Percentage(decimal all, decimal fact)
        {
            if (all == 0)
                return 1;
            if (fact == 0)
                return 0;
            decimal percentage = fact / all;
            return percentage;
        }

        #endregion
    }
}