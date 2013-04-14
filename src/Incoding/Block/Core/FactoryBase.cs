namespace Incoding.Block.Core
{
    #region << Using >>

    using System;

    #endregion

    public abstract class FactoryBase<TInit> where TInit : class
    {
        #region Fields

        protected TInit init;

        #endregion

        #region Api Methods

        public abstract void UnInitialize();

        public void Initialize(Action<TInit> initializeAction)
        {
            Guard.NotNull("initializeAction", initializeAction);
            initializeAction(this.init);
        }

        #endregion
    }
}