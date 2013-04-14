namespace Incoding.Data
{
    #region << Using >>

    using System.Diagnostics.CodeAnalysis;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.Quality;

    #endregion

    public abstract class IncEntityBase : IEntity
    {
        #region IEntity Members

        [IgnoreFieldCompare("Design fixed")]
        public virtual object Id { get; protected set; }

        #endregion

        #region Equals

        public override int GetHashCode()
        {
            return Id.ReturnOrDefault(r => r.GetHashCode(), 0);
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Positive false")]
        public override bool Equals(object obj)
        {
            return this.IsReferenceEquals(obj) && GetHashCode().Equals(obj.GetHashCode());
        }

        #endregion

        public static bool operator ==(IncEntityBase left, IncEntityBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(IncEntityBase left, IncEntityBase right)
        {
            return !Equals(left, right);
        }
    }
}