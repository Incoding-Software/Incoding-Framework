namespace Incoding.CQRS
{
    #region << Using >>

    using System;

    #endregion

    public interface ISettingCommandComposite
    {
        ISettingCommandComposite WithConnectionString(string connectionString);

        ISettingCommandComposite WithDateBaseString(string dbInstance);

        ISettingCommandComposite Mute(MuteEvent mute);

        ISettingCommandComposite AsDelay(Action<MessageDelaySetting> configuration = null);

        ISettingCommandComposite OnBefore(Action<IMessage<object>> action);

        ISettingCommandComposite OnAfter(Action<IMessage<object>> action);

        ISettingCommandComposite OnError(Action<IMessage<object>, Exception> action);

        ISettingCommandComposite OnComplete(Action<IMessage<object>> action);

        ISettingCommandComposite Quote(IMessage<object> message, MessageExecuteSetting executeSetting = null);
    }
}