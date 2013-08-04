namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Data;
    using Incoding.Quality;

    #endregion

    public class MessageExecuteSetting
    {
        #region Constructors

        public MessageExecuteSetting()
        {
            OnBefore = () => { };
            OnComplete = () => { };
            OnAfter = () => { };
            OnError = (exception) => { };

            PublishEventOnError = true;
            PublishEventOnBefore = true;
            PublishEventOnAfter = true;
            PublishEventOnComplete = true;
            DataBaseInstance = string.Empty;

            Commit = false;
        }

        #endregion

        #region Properties

        public bool PublishEventOnError { get; set; }

        public bool PublishEventOnBefore { get; set; }

        public bool PublishEventOnAfter { get; set; }

        public bool PublishEventOnComplete { get; set; }

        [IgnoreCompare("is not possible")]
        public Action OnBefore { get; set; }

        [IgnoreCompare("is not possible")]
        public Action OnComplete { get; set; }

        [IgnoreCompare("is not possible")]
        public Action OnAfter { get; set; }

        [IgnoreCompare("is not possible")]
        public Action<Exception> OnError { get; set; }

        public bool Commit { get; set; }

        public string DataBaseInstance { get; set; }

        public IDbConnection Connection { get; set; }

        #endregion
    }
}