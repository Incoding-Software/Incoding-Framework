namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System.Reflection;
    using Incoding.CQRS;
    using Incoding.Maybe;

    #endregion

    public class ExecuteQuery : QueryBase<object>
    {
        private static readonly MethodInfo query = typeof(IDispatcher).GetType().GetMethod("Query");

        public object Instance { get; set; }

        protected override object ExecuteResult()
        {
            return query.MakeGenericMethod(Instance.GetType().BaseType.With(r => r.GetGenericArguments()[0]))
                        .Invoke(Dispatcher, new[] { Instance, null });
        }
    }
}