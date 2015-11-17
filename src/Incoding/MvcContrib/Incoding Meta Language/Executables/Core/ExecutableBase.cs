namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Routing;
    using Incoding.Extensions;

    #endregion

    public abstract class ExecutableBase : Dictionary<string, object>, IExecutableSetting
    {
        public class Json
        {
            public string type { get; set; }

            public object data { get; set; }            
        }

        #region Fields

        readonly List<ConditionalBase> conditionals = new List<ConditionalBase>();

        #endregion

        #region IExecutableSetting Members

        [Obsolete("Please use If(() => expression) instead of this method")]
        public IExecutableSetting If(Action<IConditionalBuilder> configuration)
        {
            var builder = new ConditionalBuilder();
            configuration(builder);
            conditionals.AddRange(builder.conditionals);
            return this;
        }

        public IExecutableSetting If(Expression<Func<bool>> expression)
        {
            return If(builder => builder.Is(expression));
        }

        public IExecutableSetting TimeOut(double millisecond)
        {
            this.Set("timeOut", millisecond);
            return this;
        }

        public IExecutableSetting TimeOut(TimeSpan millisecond)
        {
            return TimeOut(millisecond.TotalMilliseconds);
        }

        public IExecutableSetting Interval(double millisecond, out string intervalId)
        {
            intervalId = Guid.NewGuid().ToString().Replace("-", "_");
            this.Set("interval", millisecond);
            this.Set("intervalId", intervalId);
            return this;
        }

        public IExecutableSetting Interval(TimeSpan millisecond, out string intervalId)
        {
            return Interval(millisecond.TotalMilliseconds, out intervalId);
        }

        #endregion

        #region Api Methods

        public Json AsObject()
        {
            if (conditionals.Any())
            {
                var ands = new Dictionary<string, List<object>>();
                string key = Guid.NewGuid().ToString();
                foreach (var conditional in conditionals)
                {
                    if (conditional.IsOr())
                        key = Guid.NewGuid().ToString();

                    if (ands.ContainsKey(key))
                        ands[key].Add(conditional.GetData());
                    else
                        ands.Add(key, new List<object> { conditional.GetData() });
                }

                this["ands"] = ands
                        .Select(r => r.Value)
                        .ToList();
            }

            return new Json
                   {
                           type = GetType().Name,
                           data = this
                   };
        }

        #endregion

        public virtual Dictionary<string, string> GetErrors()
        {
            return new Dictionary<string, string>();
        }
    }
}