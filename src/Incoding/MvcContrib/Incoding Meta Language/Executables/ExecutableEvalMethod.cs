namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Linq;
    using Incoding.Extensions;

    #endregion

    public class ExecutableEvalMethod : ExecutableBase
    {
        #region Constructors

        public ExecutableEvalMethod(string method, object[] args, string context)
        {
            this.Set("method", method);
            this.Set("args", args.Select((r) =>
                                             {
                                                 if (r is Selector)
                                                     return (r as Selector).ToString();
                                                 return r.ToString();
                                             })
                                 .ToArray());
            this.Set("context", context);
        }

        #endregion
    }
}