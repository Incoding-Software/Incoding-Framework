namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;

    #endregion

    public class ExecutableStoreManipulate : ExecutableBase
    {
        #region Constructors

        public ExecutableStoreManipulate(string type, List<object> methods)
        {
            this.Add("type", type);
            this.Add("methods", methods.ToJsonString());
        }

        #endregion
    }
}