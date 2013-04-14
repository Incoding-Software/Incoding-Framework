namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Utilities;
    using Incoding.Extensions;

    #endregion

    public class ExecutableStoreManipulate : ExecutableBase
    {
        #region Constructors

        public ExecutableStoreManipulate(string type, List<object> methods)
        {
            Data.Add("type", type);
            Data.Add("methods", methods.ToJsonString());
        }

        #endregion
    }
}