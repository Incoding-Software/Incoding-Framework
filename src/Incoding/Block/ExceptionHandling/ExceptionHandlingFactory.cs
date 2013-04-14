namespace Incoding.Block.ExceptionHandling
{
    #region << Using >>

    using System;
    using System.Linq;
    using Incoding.Block.Core;

    #endregion

    public class ExceptionHandlingFactory : FactoryBase<InitExceptionHandling>
    {
        #region Static Fields

        static readonly object lockObject = new object();

        static volatile ExceptionHandlingFactory instance;

        #endregion

        #region Constructors

        public ExceptionHandlingFactory()
        {
            UnInitialize();
        }

        #endregion

        #region Properties

        public static ExceptionHandlingFactory Instance
        {
            ////ncrunch: no coverage start
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                            instance = new ExceptionHandlingFactory();
                    }
                }

                return instance;
            }

            ////ncrunch: no coverage end
        }

        #endregion

        #region Api Methods

        public void Handler<TException>(TException exception) where TException : Exception
        {
            Guard.NotNull("exception", exception);

            var firstExceptionPolicy = this.init
                                           .GetPolicies()
                                           .FirstOrDefault(r => r.IsSatisfied(exception));

            if (firstExceptionPolicy == null)
                throw exception;

            var reThrowException = firstExceptionPolicy.Handle(exception);
            if (reThrowException != null)
                throw reThrowException;
        }

        #endregion

        public override void UnInitialize()
        {
            this.init = new InitExceptionHandling();
        }
    }
}