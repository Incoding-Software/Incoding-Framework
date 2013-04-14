namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;

    #endregion

    public abstract class MetaTypicalOptions
    {
        #region Fields

        readonly IDictionary<string, object> optionCollections;

        #endregion

        #region Constructors

        protected MetaTypicalOptions(MetaTypicalOptions @default)
        {
            this.optionCollections = new Dictionary<string, object>();
            foreach (var item in @default.optionCollections)
                this.optionCollections.Set(item.Key, item.Value);
        }

        protected MetaTypicalOptions()
        {
            this.optionCollections = new Dictionary<string, object>();
        }

        #endregion

        #region Properties

        public IDictionary<string, object> OptionCollections
        {
            get { return this.optionCollections; }
        }

        #endregion

        #region Api Methods

        public void Set(string key, object value)
        {
            this.optionCollections.Set(key, value);
        }

        #endregion
    }
}