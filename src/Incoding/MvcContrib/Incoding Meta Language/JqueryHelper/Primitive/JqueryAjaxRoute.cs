namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class JqueryAjaxRoute
    {
        #region Properties

        public string name { get; set; }

        public string value { get; set; }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            return this.IsReferenceEquals(obj) && GetHashCode().Equals(obj.GetHashCode());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((name != null ? name.GetHashCode() : 0) * 397) ^ (value != null ? value.GetHashCode() : 0);
            }
        }

        #endregion
    }
}