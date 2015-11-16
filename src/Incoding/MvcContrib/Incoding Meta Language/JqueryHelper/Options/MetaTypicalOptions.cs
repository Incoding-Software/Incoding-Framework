namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;

    #endregion

    public abstract class MetaTypicalOptions : Dictionary<string, object>
    {
        #region Constructors

        protected MetaTypicalOptions(MetaTypicalOptions @default)
        {            
            foreach (var item in @default)
                Add(item.Key, item.Value);
        }

        protected MetaTypicalOptions() { }

        #endregion
    }
}