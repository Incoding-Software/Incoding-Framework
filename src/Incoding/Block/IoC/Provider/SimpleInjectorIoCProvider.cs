namespace Incoding.Block.IoC
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SimpleInjector;

    #endregion

    public struct SimpleInjectorIoCProvider : IIoCProvider
    {
        private readonly Container container;

        public SimpleInjectorIoCProvider(Action<Container> evaluated)
                : this(new Container())
        {
            evaluated(this.container);
        }

        public SimpleInjectorIoCProvider(Container container)
        {
            this.container = container;
            this.container.Options.AllowOverridingRegistrations = true;
        }

        public void Dispose()
        {
        }

        public void Eject<TInstance>()
        {
            throw new NotSupportedException();
        }

        public void Forward<TNew>(TNew newInstance) where TNew : class
        {
            throw new NotSupportedException();
        }

        public TInstance Get<TInstance>(Type type) where TInstance : class
        {
            return (TInstance)this.container.GetInstance(type);
        }

        public IEnumerable<TInstance> GetAll<TInstance>(Type typeInstance)
        {
            try
            {
                return this.container.GetAllInstances(typeInstance).Cast<TInstance>();
            }
            catch (Exception)
            {
                return new TInstance[0];
            }
        }

        public TInstance TryGet<TInstance>() where TInstance : class
        {
            var type = this.container.GetRegistration(typeof(TInstance));
            if (type == null)
                return default(TInstance);
            return this.container.GetInstance<TInstance>();
        }

        public TInstance TryGet<TInstance>(Type type) where TInstance : class
        {
            var registration = this.container.GetRegistration(type);
            if (registration == null)
                return default(TInstance);
            return (TInstance)this.container.GetRegistration(type).GetInstance();
        }

        public TInstance TryGetByNamed<TInstance>(object named) where TInstance : class
        {
            throw new NotSupportedException();
        }
    }
}