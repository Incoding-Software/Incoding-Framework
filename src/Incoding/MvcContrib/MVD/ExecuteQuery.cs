namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class ExecuteQuery : QueryBase<object>
    {
        public object Instance { get; set; }

        protected override object ExecuteResult()
        {
            Guard.NotNull("Instance", "Instance query can't be null");
            Type baseType = Instance.GetType().BaseType;
            while (baseType != typeof(object))
            {
                if (baseType.Name.StartsWith("QueryBase"))
                {
                    var defaultDispatcher = new DefaultDispatcher();
                    return defaultDispatcher.GetType().GetMethod("Query")
                                            .MakeGenericMethod(baseType.GenericTypeArguments[0])
                                            .Invoke(defaultDispatcher, new[] { Instance, null });
                }
                baseType = baseType.BaseType;
            }

            throw new ArgumentOutOfRangeException("Type", "Can't find type of result for {0}".F(Instance.GetType().FullName));
        }
    }
}