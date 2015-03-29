namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.Quality;
    using Newtonsoft.Json;

    #endregion

    public class MessageExecuteSetting
    {
        internal IDispatcher outerDispatcher;
        internal IUnitOfWork unitOfWork;

        #region Constructors

        public MessageExecuteSetting() { }

        public MessageExecuteSetting(MessageExecuteSettingAttribute attribute)
        {
            DataBaseInstance = attribute.DataBaseInstance;
            Mute = attribute.Mute;
            Connection = attribute.Connection;
        }

        public MessageExecuteSetting(MessageExecuteSetting executeSetting)
        {
            Mute = executeSetting.Mute;
            OnBefore = executeSetting.OnBefore;
            OnComplete = executeSetting.OnComplete;
            OnAfter = executeSetting.OnAfter;
            OnError = executeSetting.OnError;
            OnError = executeSetting.OnError;
            DataBaseInstance = executeSetting.DataBaseInstance;
            Connection = executeSetting.Connection;
        }

        #endregion

        #region Properties
        
        public MuteEvent Mute { get; set; }

        [IgnoreCompare("is not possible"), JsonIgnore]
        public Action<IMessage<object>> OnBefore { get; set; }

        [IgnoreCompare("is not possible"), JsonIgnore]
        public Action<IMessage<object>> OnComplete { get; set; }

        [IgnoreCompare("is not possible"), JsonIgnore]
        public Action<IMessage<object>> OnAfter { get; set; }

        [IgnoreCompare("is not possible"), JsonIgnore]
        public Action<IMessage<object>, Exception> OnError { get; set; }

        public string DataBaseInstance { get; set; }

        public string Connection { get; set; }

        #endregion

        #region Equals

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DataBaseInstance != null ? DataBaseInstance.GetHashCode() : 0) * 397) ^
                       (Connection != null ? Connection.GetHashCode() : 0);
            }
        }

        public override bool Equals(object obj)
        {
            return this.IsReferenceEquals(obj) && Equals(obj as MessageExecuteSetting);
        }

        protected bool Equals(MessageExecuteSetting other)
        {
            if (other == null)
                return false;

            return string.Equals(DataBaseInstance, other.DataBaseInstance) &&
                   string.Equals(Connection, other.Connection);
        }

        #endregion
    }
}