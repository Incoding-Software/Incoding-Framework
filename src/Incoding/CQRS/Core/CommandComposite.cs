namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class CommandComposite : ISettingCommandComposite
    {
        #region Fields

        readonly List<IMessage<object>> parts = new List<IMessage<object>>();

        #endregion

        #region Properties

        public ReadOnlyCollection<IMessage<object>> Parts { get { return this.parts.AsReadOnly(); } }

        #endregion

        #region ISettingCommandComposite Members

        public ISettingCommandComposite WithConnectionString(string connectionString)
        {
            this.parts[this.parts.Count - 1].Setting.Connection = connectionString;
            return this;
        }

        public ISettingCommandComposite WithDateBaseString(string dbInstance)
        {
            this.parts[this.parts.Count - 1].Setting.DataBaseInstance = dbInstance;
            return this;
        }

        public ISettingCommandComposite Mute(MuteEvent mute)
        {
            this.parts[this.parts.Count - 1].Setting.Mute = mute;
            return this;
        }

        public ISettingCommandComposite AsDelay(Action<MessageDelaySetting> configuration = null)
        {
            var delay = new MessageDelaySetting();
            configuration.Do(action => action(delay));
            this.parts[this.parts.Count - 1].Setting.Delay = delay;
            return this;
        }

        public ISettingCommandComposite OnBefore(Action<IMessage<object>> action)
        {
            this.parts[this.parts.Count - 1].Setting.OnBefore = action;
            return this;
        }

        public ISettingCommandComposite OnAfter(Action<IMessage<object>> action)
        {
            this.parts[this.parts.Count - 1].Setting.OnAfter = action;
            return this;
        }

        public ISettingCommandComposite OnError(Action<IMessage<object>, Exception> action)
        {
            this.parts[this.parts.Count - 1].Setting.OnError = action;
            return this;
        }

        public ISettingCommandComposite OnComplete(Action<IMessage<object>> action)
        {
            this.parts[this.parts.Count - 1].Setting.OnComplete = action;
            return this;
        }

        public ISettingCommandComposite Quote(IMessage<object> message, MessageExecuteSetting executeSetting = null)
        {
            if (executeSetting != null)
                message.Setting = new MessageExecuteSetting(executeSetting);
            else
            {
                message.Setting = message.GetType().FirstOrDefaultAttribute<MessageExecuteSettingAttribute>()
                                         .With(r => new MessageExecuteSetting(r))
                                         .Recovery(new MessageExecuteSetting());
            }

            this.parts.Add(message);
            return this;
        }

        #endregion
    }
}