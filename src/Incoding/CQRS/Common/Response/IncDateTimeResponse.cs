namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    [Obsolete("Please use DateTime or DateTime?")]
    public class IncDateTimeResponse : IncStructureResponse<DateTime?>
    {
        #region Constructors

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, false), ExcludeFromCodeCoverage]
        public IncDateTimeResponse() { }

        public IncDateTimeResponse(DateTime? value)
                : base(value) { }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            if (obj == null && Value == null)
                return true;
            return obj.IsReferenceEquals(this) && ((IncStructureResponse<DateTime?>)obj).Value.Equals(Value);
        }

        public override int GetHashCode()
        {
            return Value.HasValue ? Value.Value.GetHashCode() : 0;
        }

        #endregion

        public static implicit operator IncDateTimeResponse(DateTime? s)
        {
            return new IncDateTimeResponse(s);
        }

        public static implicit operator IncDateTimeResponse(DateTime s)
        {
            return new IncDateTimeResponse(s);
        }

        public static implicit operator DateTime?(IncDateTimeResponse s)
        {
            return s == null ? null : s.Value;
        }

        public static implicit operator DateTime(IncDateTimeResponse s)
        {
            return s.GetValueOrDefault();
        }

        public static bool operator ==(DateTime? left, IncDateTimeResponse right)
        {
            return new IncDateTimeResponse(left).Equals(right);
        }

        public static bool operator !=(DateTime? left, IncDateTimeResponse right)
        {
            return !new IncDateTimeResponse(left).Equals(right);
        }

        public static bool operator ==(IncDateTimeResponse left, DateTime? right)
        {
            return new IncDateTimeResponse(right).Equals(left);
        }

        public static bool operator !=(IncDateTimeResponse left, DateTime? right)
        {
            return !new IncDateTimeResponse(right).Equals(left);
        }

        public static bool operator ==(DateTime left, IncDateTimeResponse right)
        {
            return new IncDateTimeResponse(left).Equals(right);
        }

        public static bool operator !=(DateTime left, IncDateTimeResponse right)
        {
            if (!right.With(response => response.Value.HasValue))
                return true;
            return left != right.Value;
        }

        public static bool operator ==(IncDateTimeResponse left, DateTime right)
        {
            return new IncDateTimeResponse(right).Equals(left);
        }

        public static bool operator !=(IncDateTimeResponse left, DateTime right)
        {
            return !new IncDateTimeResponse(right).Equals(left);
        }
    }
}