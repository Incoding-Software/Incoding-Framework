namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;
    using Incoding.Extensions;

    #endregion

    public abstract class ExecutableBase : IExecutableSetting
    {
        #region Fields

        readonly List<ConditionalBase> conditionals = new List<ConditionalBase>();

        #endregion

        #region Constructors

        protected ExecutableBase()
        {
            Data = new Dictionary<string, object>();
        }

        #endregion

        #region Properties

        public Dictionary<string, object> Data { get; set; }

        #endregion

        #region IExecutableSetting Members

        public IExecutableSetting If(Action<IConditionalBuilder> configuration)
        {
            var builder = new ConditionalBuilder();
            configuration(builder);
            this.conditionals.AddRange(builder.conditionals);
            return this;
        }

        public void TimeOut(double millisecond)
        {
            Data.Set("timeOut", millisecond);
        }

        public void Interval(double millisecond, out string intervalId)
        {
            intervalId = Guid.NewGuid().ToString().Replace("-", "_");
            Data.Set("interval", millisecond);
            Data.Set("intervalId", intervalId);
        }

        #endregion

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

                Data["ands"] = ands
                        .Select(r => r.Value)
                        .ToList();
            }

            var newCallback = new
                                  {
                                          type = GetType().Name, 
                                          data = Data
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
    }
}