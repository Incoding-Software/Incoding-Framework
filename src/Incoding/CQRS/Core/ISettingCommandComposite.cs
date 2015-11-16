namespace Incoding.CQRS
{
    public interface ISettingCommandComposite
    {        
        ISettingCommandComposite Quote(IMessage message, MessageExecuteSetting executeSetting = null);
    }
}