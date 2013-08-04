namespace Incoding.Block.IoC
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Block.Core;

    #endregion

    public class IoCFactory : FactoryBase<IoCInit>
    {
        #region Static Fields

        static readonly object lockObject = new object();

        static volatile IoCFactory currentInstance;

        #endregion

        #region Constructors

        public IoCFactory()
        {
            UnInitialize();
        }

        #endregion

        #region Properties

        public static IoCFactory Instance
        {
            ////ncrunch: no coverage start
            get
            {
                if (currentInstance == null)
                {
                    lock (lockObject)
                    {
                        if (currentInstance == null)
                            currentInstance = new IoCFactory();
                    }
                }

                return currentInstance;
            }

            ////ncrunch: no coverage end
        }

        #endregion

        #region Api Methods

        public void Forward<TInstance>(TInstance newInstance)
        {
            this.init.Provider.Forward(newInstance);
        }

        public void Eject<TInstance>()
        {
            this.init.Provider.Eject<TInstance>();
        }

        public TInstance Resolve<TInstance>(Type typeInstance) where TInstance : class
        {
            Guard.NotNull("typeInstance", typeInstance);

            var resolve = this.init.Provider.Get<TInstance>(typeInstance);
            return resolve;
        }

        public List<TInstance> ResolveAll<TInstance>() where TInstance : class
        {
            return ResolveAll<TInstance>(typeof(TInstance));
        }

        public List<TInstance> ResolveAll<TInstance>(Type typeInstance) where TInstance : class
        {
            Guard.NotNull("typeInstance", typeInstance);
            var allInstances = this.init.Provider.GetAll<TInstance>(typeInstance).ToList();
            return allInstances;
        }

        public TInstance TryResolve<TInstance>() where TInstance : class
        {
            var resolve = this.init.Provider.TryGet<TInstance>();
            return resolve;
        }

        public TInstance TryResolveByNamed<TInstance>(string named) where TInstance : class
        {
            var resolve = this.init.Provider.TryGetByNamed<TInstance>(named);
            return resolve;
        }

        public TInstance TryResolve<TInstance>(Type typeInstance) where TInstance : class
        {
            string errorMessage = "Can't resolve null type for" + typeof(TInstance).FullName;
            Guard.NotNull("typeInstance", typeInstance, errorMessage);

            var resolve = this.init.Provider.TryGet<TInstance>(typeInstance);
            return resolve;
        }

        #endregion

        public override void UnInitialize()
        {
            this.init = new IoCInit();
        }
    }
}