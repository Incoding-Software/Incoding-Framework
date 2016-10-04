namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class IncSelectList
    {
        #region Properties

        public IncOptional Optional { get; set; }

        #endregion

        public static implicit operator SelectList(IncSelectList s)
        {
            List<KeyValueVm> optValues = s.Optional;
            for (int i = 0; i < optValues.Count; i++)
                s.keyValueVms.Insert(i, optValues[i]);
            return new SelectList(s.keyValueVms, "Value", "Text");
        }

        public static implicit operator string(IncSelectList s)
        {
            return s.url;
        }

        public static implicit operator IncSelectList(List<KeyValueVm> s)
        {
            return new IncSelectList(s);
        }

        public static implicit operator IncSelectList(KeyValueVm[] s)
        {
            return new IncSelectList(s);
        }

        public static implicit operator IncSelectList(OptGroupVm s)
        {
            return new IncSelectList(s.Items);
        }

        public static implicit operator IncSelectList(List<OptGroupVm> s)
        {
            return new IncSelectList(s.SelectMany(vm => vm.Items));
        }

        public static implicit operator IncSelectList(OptGroupVm[] s)
        {
            return new IncSelectList(s.SelectMany(vm => vm.Items));
        }

        public static implicit operator IncSelectList(SelectList s)
        {
            return new IncSelectList(s);
        }

        public static implicit operator IncSelectList(string[] values)
        {
            return new IncSelectList(values.Select(r => new KeyValueVm(r)));
        }

        public static implicit operator IncSelectList(string url)
        {
            return new IncSelectList(url);
        }

        public class IncOptional
        {
            private readonly List<KeyValueVm> values = new List<KeyValueVm>();

            public IncOptional(string value)
                    : this(new KeyValueVm(value)) { }

            public IncOptional(KeyValueVm value)
                    : this(new[] { value }) { }

            public IncOptional(IEnumerable<KeyValueVm> value)
            {
                this.values = value.ToList();
            }

            public IncOptional() { }

            public static implicit operator List<KeyValueVm>(IncOptional s)
            {
                return s.values ?? new List<KeyValueVm>();
            }

            public static implicit operator IncOptional(List<KeyValueVm> s)
            {
                return new IncOptional(s);
            }

            public static implicit operator IncOptional(KeyValueVm[] s)
            {
                return new IncOptional(s);
            }

            public static implicit operator IncOptional(string s)
            {
                return new IncOptional(s);
            }

            public static implicit operator IncOptional(string[] s)
            {
                return new IncOptional(s.Select(r => new KeyValueVm(r)));
            }

            public static implicit operator IncOptional(KeyValueVm s)
            {
                return new IncOptional(s);
            }
        }

        #region Fields

        readonly IList<KeyValueVm> keyValueVms = new List<KeyValueVm>();

        private readonly string url;

        #endregion

        #region Constructors

        public IncSelectList()
        {
            Optional = new IncOptional();
        }

        public IncSelectList(IEnumerable<KeyValueVm> keyValueVms)
                : this()
        {
            this.keyValueVms = keyValueVms.ToList();
        }

        public IncSelectList(SelectList asSelectList)
                : this()
        {
            this.keyValueVms = new List<KeyValueVm>();
            foreach (var item in asSelectList.Items)
            {
                this.keyValueVms.Add(item.GetType().IsTypicalType()
                                             ? new KeyValueVm(item.ToString())
                                             : new KeyValueVm(item.TryGetValue(asSelectList.DataValueField), item.TryGetValue(asSelectList.DataTextField).With(r => r.ToString())));
            }
        }

        public IncSelectList(string asSelectList)
                : this()
        {
            this.url = asSelectList;
        }

        #endregion
    }
}