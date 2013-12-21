namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Diagnostics.CodeAnalysis;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class KeyValueVm : IKeyValueVm
    {
        ////ncrunch: no coverage start
        #region Constructors

        [ExcludeFromCodeCoverage, UsedImplicitly]
        public KeyValueVm() { }

        ////ncrunch: no coverage end
        public KeyValueVm(object value, string text, bool selected)
        {
            Value = value.With(r => r.ToString());
            Text = text;
            Selected = selected;
        }

        public KeyValueVm(object value, string text)
                : this(value, text, false) { }

        public KeyValueVm(object value)
                : this(value, value.ToString()) { }

        #endregion

        #region Properties

        [IgnoreCompare("Optional")]
        public string CssClass { get; set; }

        #endregion

        #region IKeyValueVm Members

        public string Value { get; set; }

        public string Text { get; set; }

        [IgnoreCompare("Optional")]
        public bool Selected { get; set; }

        [IgnoreCompare("Optional")]
        public string Title { get; set; }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            return Equals(obj as KeyValueVm);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 0;
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Text != null ? Text.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Selected.GetHashCode();
                hashCode = (hashCode * 397) ^ (Title != null ? Title.GetHashCode() : 0);
                return hashCode;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "IsReferenceEquals")]
        protected bool Equals(KeyValueVm other)
        {
            return this.IsReferenceEquals(other) && GetHashCode().Equals(other.GetHashCode());
        }

        #endregion
    }
}