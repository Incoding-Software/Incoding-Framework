namespace Incoding.Block.ExceptionHandling
{
    #region << Using >>

    using System;
    using System.Threading;
    using Incoding.Extensions;

    #endregion

    public class ActionPolicy
    {
        #region Fields

        readonly ActionOfType type;

        int attempt;

        TimeSpan? interval;

        #endregion

        #region Constructors

        ActionPolicy(ActionOfType type)
        {
            this.type = type;
        }

        #endregion

        #region Factory constructors

        public static ActionPolicy Direct()
        {
            return new ActionPolicy(ActionOfType.Direct);
        }

        public static ActionPolicy Repeat(int attempt)
        {
            return new ActionPolicy(ActionOfType.Repeat) { attempt = attempt };
        }

        #endregion

        #region Api Methods

        public void Do(Action action)
        {
            switch (this.type)
            {
                case ActionOfType.Direct:
                    action();
                    break;
                case ActionOfType.Repeat:
                    int currentRetryCount = 0;
                    while (true)
                    {
                        try
                        {
                            action.Invoke();
                            break;
                        }
                        catch (Exception)
                        {
                            if (currentRetryCount == this.attempt)
                                throw;

                            if (this.interval.HasValue)
                                Thread.Sleep(this.interval.Value);
                        }
                        finally
                        {
                            currentRetryCount++;
                        }
                    }
                    break;
            }
        }

        public ActionPolicy Interval(TimeSpan time)
        {
            this.interval = time;
            return this;
        }

        #endregion

        #region Equals

        public override bool Equals(object obj)
        {
            return this.IsReferenceEquals(obj) && Equals(obj as ActionPolicy);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int)this.type;
                hashCode = (hashCode * 397) ^ this.attempt;
                hashCode = (hashCode * 397) ^ this.interval.GetHashCode();
                return hashCode;
            }
        }

        protected bool Equals(ActionPolicy other)
        {
            if (other == null)
                return false;
            return this.type == other.type && this.attempt == other.attempt && this.interval.Equals(other.interval);
        }

        #endregion

        #region Enums

        enum ActionOfType
        {
            Direct,

            Repeat
        }

        #endregion
    }
}