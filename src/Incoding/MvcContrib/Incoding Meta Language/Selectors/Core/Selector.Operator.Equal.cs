namespace Incoding.MvcContrib
{
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

        public static bool operator ==(bool left, Selector right)
        {
            return true;
        }

        public static bool operator !=(bool left, Selector right)
        {
            return !(left == right);
        }

        public static bool operator ==(Selector left, bool right)
        {
            return true;
        }

        public static bool operator !=(Selector left, bool right)
        {
            return !(left == right);
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

        ////ncrunch: no coverage end
    }
}