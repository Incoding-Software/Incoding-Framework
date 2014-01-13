namespace Incoding.CQRS
{
    public class IncBoolResponse : IncStructureResponse<bool>
    {
        #region Constructors

        public IncBoolResponse(bool value)
                : base(value) { }

        #endregion

        public static implicit operator IncBoolResponse(bool s)
        {
            return new IncBoolResponse(s);
        }

        public static implicit operator bool(IncBoolResponse s)
        {
            return s.Value;
        }

        public static bool operator ==(bool left, IncBoolResponse right)
        {
            return left == right.Value;
        }

        public static bool operator !=(bool left, IncBoolResponse right)
        {
            return !(left == right);
        }

        public static bool operator ==(IncBoolResponse left, bool right)
        {
            return left.Value == right;
        }

        public static bool operator !=(IncBoolResponse left, bool right)
        {
            return !(left == right);
        }
    }
}