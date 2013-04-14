namespace Incoding.Block.ExceptionHandling
{
    #region << Using >>

    using System;
    using Incoding.Block.Core;

    #endregion

    public static class ExceptionPolicyFluentEx
    {
        #region Factory constructors

        public static ExceptionPolicy Catch(this IsSatisfied<Exception> satisfied, Action<Exception> callback)
        {
            return new ExceptionPolicy(satisfied, exception =>
                                                      {
                                                          callback(exception);
                                                          return null;
                                                      });
        }

        public static ExceptionPolicy Mute(this IsSatisfied<Exception> satisfied)
        {
            return satisfied.Wrap(exception => null);
        }

        public static ExceptionPolicy ReThrow(this IsSatisfied<Exception> satisfied)
        {
            return satisfied.Wrap(exception => exception);
        }

        public static ExceptionPolicy Wrap(this IsSatisfied<Exception> satisfied, Func<Exception, Exception> evaluator)
        {
            return new ExceptionPolicy(satisfied, evaluator);
        }

        #endregion
    }
}