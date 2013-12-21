namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    public partial class Selector
    {
        ////ncrunch: no coverage start
        public static bool operator <=(Selector left, Selector right)
        {
            return true;
        }

        public static bool operator >=(Selector left, Selector right)
        {
            return true;
        }

        public static bool operator <(Selector left, Selector right)
        {
            return true;
        }

        public static bool operator >(Selector left, Selector right)
        {
            return true;
        }

        public static bool operator ==(int left, Selector right)
        {
            return true;
        }

        public static bool operator !=(int left, Selector right)
        {
            return !(left == right);
        }

        public static bool operator ==(Selector left, int right)
        {
            return true;
        }

        public static bool operator !=(Selector left, int right)
        {
            return !(left == right);
        }

        public static bool operator ==(long left, Selector right)
        {
            return true;
        }

        public static bool operator !=(long left, Selector right)
        {
            return !(left == right);
        }

        public static bool operator ==(Selector left, long right)
        {
            return true;
        }

        public static bool operator !=(Selector left, long right)
        {
            return !(left == right);
        }

        public static bool operator ==(float left, Selector right)
        {
            return true;
        }

        public static bool operator !=(float left, Selector right)
        {
            return !(left == right);
        }

        public static bool operator ==(Selector left, float right)
        {
            return true;
        }

        public static bool operator !=(Selector left, float right)
        {
            return !(left == right);
        }

        public static bool operator ==(decimal left, Selector right)
        {
            return true;
        }

        public static bool operator !=(decimal left, Selector right)
        {
            return !(left == right);
        }

        public static bool operator ==(Selector left, decimal right)
        {
            return true;
        }

        public static bool operator !=(Selector left, decimal right)
        {
            return !(left == right);
        }

        public static bool operator ==(Guid left, Selector right)
        {
            return true;
        }

        public static bool operator !=(Guid left, Selector right)
        {
            return !(left == right);
        }

        public static bool operator ==(Selector left, Guid right)
        {
            return true;
        }

        public static bool operator !=(Selector left, Guid right)
        {
            return !(left == right);
        }

        public static bool operator ==(DateTime left, Selector right)
        {
            return true;
        }

        public static bool operator !=(DateTime left, Selector right)
        {
            return !(left == right);
        }

        public static bool operator ==(Selector left, DateTime right)
        {
            return true;
        }

        public static bool operator !=(Selector left, DateTime right)
        {
            return !(left == right);
        }

        ////ncrunch: no coverage end
    }
}