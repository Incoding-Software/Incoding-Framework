namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System.Collections;

    #endregion

    public class WeakEqualityComparer : IEqualityComparer
    {
        #region IEqualityComparer Members

        public new bool Equals(object x, object y)
        {
            return x.IsEqualWeak(y);
        }

        ////ncrunch: no coverage start
        public int GetHashCode(object obj)
        {
            return 0;
        }

        #endregion

        ////ncrunch: no coverage end
    }
}