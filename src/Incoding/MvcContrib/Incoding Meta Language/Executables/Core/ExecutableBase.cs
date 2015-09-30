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
        #region Fields

        readonly List<ConditionalBase> conditionals = new List<ConditionalBase>();

        #endregion

        #region IExecutableSetting Members

        public IExecutableSetting If(Action<IConditionalBuilder> configuration)
        {
            var builder = new ConditionalBuilder();
            configuration(builder);
            this.conditionals.AddRange(builder.conditionals);
            return this;
        }

        public IExecutableSetting If(Expression<Func<bool>> expression)
        {
            return If(builder => builder.Is(expression));
        }

        public void TimeOut(double millisecond)
        {
            this.Set("timeOut", millisecond);
        }

        public void TimeOut(TimeSpan millisecond)
        {
            TimeOut(millisecond.TotalMilliseconds);
        }

        public void Interval(double millisecond, out string intervalId)
        {
            intervalId = Guid.NewGuid().ToString().Replace("-", "_");
            this.Set("interval", millisecond);
            this.Set("intervalId", intervalId);
        }

        public void Interval(TimeSpan millisecond, out string intervalId)
        {
            Interval(millisecond.TotalMilliseconds, out intervalId);
        }

        #endregion

        #region Api Methods

        public virtual RouteValueDictionary Merge(RouteValueDictionary dest)
        {
            const string dataIncodingKey = "incoding";

            var res = new RouteValueDictionary(dest);

            if (this.conditionals.Any())
            {
                var ands = new Dictionary<string, List<object>>();
                string key = Guid.NewGuid().ToString();
                foreach (var conditional in this.conditionals)
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

            var newCallback = new
                                  {
                                          type = GetType().Name,
                                          data = this
                                  };

            if (!dest.ContainsKey(dataIncodingKey))
                res.Add(dataIncodingKey, new[] { newCallback }.ToJsonString());
            else
            {
                var newArray = res[dataIncodingKey].ToString().DeserializeFromJson<object[]>().ToList();
                newArray.Add(newCallback);
                res[dataIncodingKey] = newArray.ToJsonString();
            }

            return res;
        }

        #endregion

        public virtual Dictionary<string, string> GetErrors()
        {
            return new Dictionary<string, string>();
        }

    }
}