namespace Incoding.Block.IoC
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Ninject;
    using Ninject.Modules;

    #endregion

    /// <summary>
    ///     Implement provider IoC for Ninject. Get started http://dotnetslackers.com/articles/csharp/Get-Started-with-Ninject-2-0-in-C-sharp-Programming.aspx
    /// </summary>
    public struct NinjectIoCProvider : IIoCProvider
    {
        #region Fields

        readonly StandardKernel kernel;

        #endregion

        #region Constructors

        public NinjectIoCProvider(Action<IKernel> action)
        {
            Guard.NotNull("action", action);

            this.kernel = new StandardKernel();
            action(this.kernel);
        }

        public NinjectIoCProvider(StandardKernel kernel)
        {
            Guard.NotNull("kernel", kernel);
            this.kernel = kernel;
        }


        #endregion

        #region IIoCProvider Members

        public void Eject<TInstance>()
        {
            this.kernel.Unbind<TInstance>();
        }

        public void Forward<TNew>(TNew newInstance) where TNew : class
        {
            Eject<TNew>();
            this.kernel.Bind<TNew>().ToConstant(newInstance);
        }

        public IEnumerable<TInstance> GetAll<TInstance>(Type typeInstance)
        {
            return this.kernel.GetAll(typeInstance).OfType<TInstance>();
        }

        public TInstance TryGet<TInstance>() where TInstance : class
        {
            return this.kernel.TryGet<TInstance>();
        }

        public TInstance Get<TInstance>(Type type) where TInstance : class
        {
            return (TInstance)this.kernel.Get(type);
        }

        public TInstance TryGet<TInstance>(Type type) where TInstance : class
        {
            return (TInstance)this.kernel.TryGet(type);
        }

        public TInstance TryGetByNamed<TInstance>(object named) where TInstance : class
        {
            return this.kernel.TryGet<TInstance>(named.ToString());
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (this.kernel != null)
                this.kernel.Dispose();
        }

        #endregion
    }
}