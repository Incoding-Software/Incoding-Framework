namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Linq;
    using Incoding.Extensions;

    #endregion

    public class ExecutableJquery : ExecutableBase
    {
        public enum Method
        {
            AddClass = 1,
            RemoveClass = 2
        }

        public ExecutableJquery(Method method, object[] args)
        {
            this.Set("method", (int)method);
            this.Set("args", args.Select((r) =>
                                         {
                                             if (r is Selector)
                                                 return (r as Selector).ToString();
                                             return r.ToString();
                                         })
                                 .ToArray());
        }

    }
}