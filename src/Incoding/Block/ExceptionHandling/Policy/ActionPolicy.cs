namespace Incoding.Block.ExceptionHandling
{
    #region << Using >>

    using System;

    #endregion

    public class ActionPolicy
    {
        #region Fields

        /// <summary>
        ///     Action policy without behavior
        /// </summary>
        readonly Action<Action> policy;

        #endregion

        #region Constructors

        ActionPolicy(Action<Action> policy)
        {
            this.policy = policy;
        }

        #endregion

        #region Factory constructors

        public static ActionPolicy Catch(Action<Exception> catchAction)
        {
            Action<Action> actionPolicy = action =>
                                              {
                                                  try
                                                  {
                                                      action.Invoke();
                                                  }
                                                  catch (Exception exception)
                                                  {
                                                      catchAction.Invoke(exception);
                                                  }
                                              };
            return new ActionPolicy(actionPolicy);
        }

        public static ActionPolicy Direct()
        {
            return new ActionPolicy(action => action.Invoke());
        }

        public static ActionPolicy Retry(int retryCount)
        {
            Action<Action> actionPolicy = action =>
                                              {
                                                  int currentRetryCount = 0;
                                                  while (true)
                                                  {
                                                      currentRetryCount++;
                                                      try
                                                      {
                                                          action.Invoke();
                                                          return;
                                                      }
                                                      catch (Exception)
                                                      {
                                                          if (currentRetryCount == retryCount)
                                                              throw;
                                                      }
                                                  }
                                              };
            return new ActionPolicy(actionPolicy);
        }

        #endregion

        #region Api Methods

        public void Do(Action action)
        {
            this.policy(action);
        }

        #endregion
    }
}