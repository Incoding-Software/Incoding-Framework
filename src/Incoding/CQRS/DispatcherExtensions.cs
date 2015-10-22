namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using Incoding.Maybe;

    #endregion

    public static class DispatcherExtensions
    {
        #region Factory constructors

        public static void Push(this IDispatcher dispatcher, Action<CommandComposite> configuration)
        {
            var composite = new CommandComposite();
            configuration(composite);
            dispatcher.Push(composite);
        }

        public static void Push(this IDispatcher dispatcher, CommandBase message, MessageExecuteSetting executeSetting = null)
        {
            dispatcher.Push(composite => composite.Quote(message, executeSetting));
        }

        public static void Push(this IDispatcher dispatcher, CommandBase message, Action<MessageExecuteSetting> configurationSetting)
        {
            var setting = new MessageExecuteSetting();
            configurationSetting.Do(action => action(setting));
            dispatcher.Push(message, setting);
        }

        public static TResult Query<TResult>(this IDispatcher dispatcher, QueryBase<TResult> message, Action<MessageExecuteSetting> configurationSetting) where TResult : class
        {
            var setting = new MessageExecuteSetting();
            configurationSetting.Do(action => action(setting));
            return dispatcher.Query(message, setting);
        }

        #endregion
    }
}