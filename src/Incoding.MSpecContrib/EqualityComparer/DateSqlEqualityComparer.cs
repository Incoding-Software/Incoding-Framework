namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections;
    using Incoding.Extensions;

    #endregion

    public class DateSqlEqualityComparer : IEqualityComparer
    {
        #region IEqualityComparer Members

        public new bool Equals(object x, object y)
        {
            if (!x.IsReferenceEquals(y))
                return false;

            if (x.GetType() != typeof(DateTime) || y.GetType() != typeof(DateTime))
                return false;

            var dtLeft = (DateTime)x;
            var dtRight = (DateTime)y;

            return dtLeft.Year.Equals(dtRight.Year) && dtLeft.Month.Equals(dtRight.Month) && dtLeft.Day.Equals(dtRight.Day)
                   && dtLeft.Hour.Equals(dtRight.Hour) && dtLeft.Minute.Equals(dtRight.Minute) && dtLeft.Second.Equals(dtRight.Second);
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