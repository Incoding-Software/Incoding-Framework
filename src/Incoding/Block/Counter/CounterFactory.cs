namespace Incoding.Block
{
    #region << Using >>

    using System.Collections.Concurrent;
    using System.Diagnostics;
    using Incoding.Block.Core;
    using Incoding.Block.ExceptionHandling;

    #endregion

    public class CounterFactory : FactoryBase<InitCounter>
    {
        #region Static Fields

        static readonly object lockObject = new object();

        static volatile CounterFactory instance;

        #endregion

        #region Constructors

        public CounterFactory()
        {
            UnInitialize();
        }

        #endregion

        #region Properties

        public static CounterFactory Instance
        {
            ////ncrunch: no coverage start
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                            instance = new CounterFactory();
                    }
                }

                return instance;
            }

            ////ncrunch: no coverage end
        }

        #endregion

        readonly ConcurrentDictionary<string, PerformanceCounter> counters = new ConcurrentDictionary<string, PerformanceCounter>();

        public void Start(string category, string name)
        {
            if (!this.init.Enable)
                return;

            var counter = this.counters.AddOrUpdate(category + name, s => { return new PerformanceCounter(category, name); });
            counter.Increment();
        }

        public void Stop(string category, string name)
        {
            if (!this.init.Enable)
                return;
        }

        public override void UnInitialize() { }
    }

    public class InitCounter
    {
        public bool Enable { get; set; }
    }
}