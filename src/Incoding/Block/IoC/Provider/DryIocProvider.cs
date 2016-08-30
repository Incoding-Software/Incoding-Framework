namespace Incoding.Block.IoC
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using DryIoc;

    #endregion

    public struct DryIocProvider : IIoCProvider
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
            using (var scope = this.container.OpenScope())
                return (TInstance)scope.Resolve(type);
        }

        public IEnumerable<TInstance> GetAll<TInstance>(Type typeInstance)
        {
            using (var scope = this.container.OpenScope())
                return scope.ResolveMany<TInstance>(typeInstance, ResolveManyBehavior.AsFixedArray);
        }

        public TInstance TryGet<TInstance>() where TInstance : class
        {
            using (var scope = this.container.OpenScope())
                return scope.Resolve<TInstance>(IfUnresolved.ReturnDefault);
        }

        public TInstance TryGet<TInstance>(Type type) where TInstance : class
        {
            using (var scope = this.container.OpenScope())
                return scope.Resolve<TInstance>(type, IfUnresolved.ReturnDefault);
        }

        public TInstance TryGetByNamed<TInstance>(object named) where TInstance : class
        {
            using (var scope = this.container.OpenScope())
                return scope.Resolve<TInstance>(serviceKey: named, ifUnresolved: IfUnresolved.ReturnDefault);
        }
    }
}