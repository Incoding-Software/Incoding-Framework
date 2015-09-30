namespace Incoding.CQRS
{
    public interface ISettingCommandComposite
    {        
        ISettingCommandComposite Quote<TResult>(IMessage<TResult> message, MessageExecuteSetting executeSetting = null);
    }
}