namespace Incoding.Block.ExceptionHandling
{
    #region << Using >>

    using System;
    using Incoding.Block.Core;

    #endregion

    public sealed class ExceptionPolicy
    {
        #region Fields

        readonly Func<Exception, Exception> func;

        readonly IsSatisfied<Exception> satisfiedByException;

        #endregion

        #region Constructors

        internal ExceptionPolicy(IsSatisfied<Exception> satisfiedByException, Func<Exception, Exception> func)
        {
            Guard.NotNull("satisfiedByException", satisfiedByException);
            Guard.NotNull("func", func);

            this.satisfiedByException = satisfiedByException;
            this.func = func;
        }

        #endregion

        #region Factory constructors

        /// <summary>
        ///     See <see cref="SatisfiedSyntax" />
        /// </summary>
        public static IsSatisfied<Exception> Filter(Func<Exception, bool> conditional)
        {
            return SatisfiedSyntax.Filter(conditional);
        }

        /// <summary>
        ///     See <see cref="SatisfiedSyntax" />
        /// </summary>
        public static IsSatisfied<Exception> For<TException>()
                where TException : Exception
        {
            return SatisfiedSyntax.For<Exception, TException>();
        }

        /// <summary>
        ///     See <see cref="SatisfiedSyntax" />
        /// </summary>
        public static IsSatisfied<Exception> ForAll()
        {
            return SatisfiedSyntax.Filter<Exception>(exception => true);
        }

        /// <summary>
        ///     See <see cref="SatisfiedSyntax" />
        /// </summary>
        public static IsSatisfied<Exception> ForDeepDerived<TException>()
                where TException : Exception
        {
            return SatisfiedSyntax.ForDeepDerived<Exception, TException>();
        }

        /// <summary>
        ///     See <see cref="SatisfiedSyntax" />
        /// </summary>
        public static IsSatisfied<Exception> ForFirstDerived<TException>()
                where TException : Exception
        {
            return SatisfiedSyntax.ForFirstDerived<Exception, TException>();
        }

        #endregion

        internal bool IsSatisfied(Exception exception)
        {
            Guard.NotNull("exception", exception);
            return this.satisfiedByException.IsSatisfied(exception);
        }

        public Exception Handle(Exception exception)
        {
            Guard.NotNull("exception", exception);
            return this.func.Invoke(exception);
        }
    }
}