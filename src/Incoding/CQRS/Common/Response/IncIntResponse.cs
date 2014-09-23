namespace Incoding.CQRS
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Extensions;
    using Incoding.Quality;
    using JetBrains.Annotations;

    public class IncIntResponse : IncStructureResponse<int>
    {
        #region Constructors

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, false), ExcludeFromCodeCoverage]
        public IncIntResponse() { }

        public IncIntResponse(int value)
                : base(value) { }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            return obj.IsReferenceEquals(this) &&
                   ((IncStructureResponse<int>)obj).Value.Equals(this.Value);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        #endregion

        public static implicit operator IncIntResponse(int s)
        {
            return new IncIntResponse(s);
        }

        public static implicit operator int(IncIntResponse s)
        {
            return s.Value;
        }

        public static bool operator ==(int left, IncIntResponse right)
        {
            return new IncIntResponse(left).Equals(right);
        }

        public static bool operator !=(int left, IncIntResponse right)
        {
            return !new IncIntResponse(left).Equals(right);
        }

        public static bool operator ==(IncIntResponse left, int right)
        {
            return new IncIntResponse(right).Equals(left);
        }

        public static bool operator !=(IncIntResponse left, int right)
        {
            return !new IncIntResponse(right).Equals(left);
        }
    }
}