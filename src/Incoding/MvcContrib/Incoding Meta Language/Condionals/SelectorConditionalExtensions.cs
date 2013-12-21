namespace Incoding.MvcContrib
{
    ////ncrunch: no coverage start
    public static class SelectorConditionalExtensions
    {
        #region Factory constructors

        public static bool IsContains(this Selector selector, Selector value)
        {
            return true;
        }


        public static bool IsContains(this string selector, Selector value)
        {
            return true;
        }

        public static bool IsEmpty(this object data)
        {
            return true;
        }

        #endregion
    }

    ////ncrunch: no coverage end
}