namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using System.Reflection;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class ExecuteQuery : QueryBase<object>
    {
        static readonly MethodInfo query = typeof(DefaultDispatcher).GetMethod("Query");

        public object Instance { get; set; }

        protected override object ExecuteResult()
        {
            Type baseType = Instance.GetType().BaseType;
            while (baseType != typeof(object))
            {
                if (baseType.Name.StartsWith("QueryBase"))
                {
                    return query
                            .MakeGenericMethod(baseType.GenericTypeArguments[0])
                            .Invoke(new DefaultDispatcher(), new[] { Instance, null });
                }
                baseType = baseType.BaseType;
            }

            throw new ArgumentOutOfRangeException("Type", "Can't find type of result for {0}".F(Instance.GetType().FullName));
        }
    }
}