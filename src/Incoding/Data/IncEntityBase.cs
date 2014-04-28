namespace Incoding.Data
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.Quality;
    using MongoDB.Bson.Serialization.Attributes;

    #endregion

    public abstract class IncEntityBase : IEntity
    {
        #region IEntity Members

        [IgnoreCompare("Design fixed")]
        public virtual object Id { get; protected set; }

        #endregion

        #region Equals

        public override int GetHashCode()
        {
            return Id.ReturnOrDefault(r => r.GetHashCode(), 0);
        }

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