namespace Incoding.EventBroker
{
    #region << Using >>

    using System;

    #endregion

    public abstract class HandlerDescriptionAttribute : Attribute
    {
        #region Fields

        readonly bool isAsync;

        readonly bool isWait;

        #endregion

        #region Constructors

        protected HandlerDescriptionAttribute(bool isAsync = false, bool isWait = false)
        {
            this.isAsync = isAsync;
            this.isWait = isWait;
        }

        #endregion

        #region Properties

        public bool IsAsync
        {
            get { return this.isAsync; }
        }

        public bool IsWait
        {
            get { return this.isWait; }
        }

        #endregion
    }
}