namespace Incoding.Block.IoC
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StructureMap;
    using StructureMap.Configuration.DSL;

    #endregion

    /// <summary>
    ///     Implement provider for StructureMap. http://docs.structuremap.net/
    /// </summary>
    public struct StructureMapIoCProvider : IIoCProvider
    {
        #region Fields

        readonly IContainer container;

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (this.container != null)
                this.container.Dispose();
        }

        #endregion

        #region Constructors

        public StructureMapIoCProvider(Registry registry)
        {
            Guard.NotNull("registry", registry);
            this.container = new Container(registry);
        }

        public StructureMapIoCProvider(Action<Registry> initRegistry)
        {
            Guard.NotNull("initRegistry", initRegistry);

            var registry = new Registry();
            initRegistry(registry);
            this.container = new Container(registry);
        }

        #endregion

        #region IIoCProvider Members

        public TInstance TryGet<TInstance>() where TInstance : class
        {
            return this.container.GetAllInstances<TInstance>().FirstOrDefault();
        }

        public TInstance Get<TInstance>(Type type) where TInstance : class
        {
            return this.container.GetAllInstances(type).OfType<TInstance>().FirstOrDefault();
        }

        public IEnumerable<TInstance> GetAll<TInstance>(Type typeInstance)
        {
            return this.container.GetAllInstances(typeInstance).OfType<TInstance>();
        }

        public TInstance TryGet<TInstance>(Type type) where TInstance : class
        {
            return this.container.TryGetInstance(type) as TInstance;
        }

        public TInstance TryGetByNamed<TInstance>(object named) where TInstance : class
        {
            return this.container.TryGetInstance<TInstance>(named.ToString());
        }

        public void Eject<TInstance>()
        {
            this.container.EjectAllInstancesOf<TInstance>();
        }

        public void Forward<TInstance>(TInstance newInstance) where TInstance : class
        {
            Eject<TInstance>();
            this.container.Configure(configurationExpression => configurationExpression.For<TInstance>().Use(newInstance));
        }

        #endregion
    }
}