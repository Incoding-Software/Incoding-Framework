namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;

    #endregion

    public class ExecutableEval : ExecutableBase
    {
        #region Constructors

        public ExecutableEval(List<string> codes)
        {
            foreach (var code in codes)
            {
                if (Data.ContainsKey("code"))
                    Data["code"] += code;
                else
                    Data.Add("code", code);
            }
        }

        public ExecutableEval(string code)
        {
            Data.Set("code", code);
        }

        #endregion
    }
}