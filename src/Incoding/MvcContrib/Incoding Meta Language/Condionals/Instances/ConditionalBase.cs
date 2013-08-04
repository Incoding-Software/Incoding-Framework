namespace Incoding.MvcContrib
{
    public abstract class ConditionalBase
    {
        #region Fields

        protected readonly string type;

        protected bool and;

        protected bool inverse;

        #endregion

        #region Constructors

        protected ConditionalBase(string type, bool and)
        {
            this.type = type;
            this.inverse = false;
            this.and = and;
        }

        #endregion

        #region Api Methods

        public abstract object GetData();

        #endregion

        internal bool IsOr()
        {
            return !this.and;
        }

        public static ConditionalBase operator !(ConditionalBase conditional)
        {
            conditional.inverse = true;
            return conditional;
        }
    }
}