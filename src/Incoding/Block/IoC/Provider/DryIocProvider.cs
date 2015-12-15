namespace Incoding.Block.IoC
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DryIoc;

    #endregion

    public class DryIocProvider : IIoCProvider
    {
        private readonly Container container;

        public DryIocProvider(Action<Container> evaluated)
                : this(new Container())
        {
            evaluated(this.container);
        }

        public DryIocProvider(Container container)
        {
            Guard.NotNull("container", container);
            this.container = container;
        }

        public void Dispose()
        {
            if (!this.container.IsDisposed)
                this.container.Dispose();
        }

        public void Eject<TInstance>()
        {
            this.container.Unregister<TInstance>();
        }

        public void Forward<TNew>(TNew newInstance) where TNew : class
        {
            this.container.Unregister<TNew>();
            this.container.RegisterInstance(newInstance);
        }

        public TInstance Get<TInstance>(Type type) where TInstance : class
        {
            return (TInstance)this.container.Resolve(type);
        }

        public IEnumerable<TInstance> GetAll<TInstance>(Type typeInstance)
        {
            return this.container.ResolveMany<TInstance>(typeInstance);
        }

        public TInstance TryGet<TInstance>() where TInstance : class
        {
            return this.container.Resolve<TInstance>(IfUnresolved.ReturnDefault);
        }

        public TInstance TryGet<TInstance>(Type type) where TInstance : class
        {
            return this.container.Resolve<TInstance>(type, IfUnresolved.ReturnDefault);
        }

        public TInstance TryGetByNamed<TInstance>(string named) where TInstance : class
        {
            var keyValuePairs = this.container.Resolve<KeyValuePair<string, TInstance>[]>();
            return keyValuePairs
                            .FirstOrDefault(r => r.Key == named)
                            .Value;
        }
    }
}