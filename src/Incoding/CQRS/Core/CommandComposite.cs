namespace Incoding.CQRS
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class CommandComposite : ISettingCommandComposite
    {
        public CommandComposite() { }

        public CommandComposite(IMessage message, MessageExecuteSetting executeSetting = null)
        {
            Quote(message, executeSetting);
        }

        public CommandComposite(CommandBase[] commands)
        {
            foreach (var commandBase in commands)
                Quote(commandBase);
        }

        #region Fields

        readonly List<IMessage> parts = new List<IMessage>();

        #endregion

        #region Properties

        public ReadOnlyCollection<IMessage> Parts { get { return parts.AsReadOnly(); } }

        #endregion

        #region ISettingCommandComposite Members

        public ISettingCommandComposite Quote(IMessage message, MessageExecuteSetting executeSetting = null)
        {
            if (executeSetting != null)
                message.Setting = new MessageExecuteSetting(executeSetting);
            else if (message.Setting == null)
            {
                message.Setting = message.GetType().FirstOrDefaultAttribute<MessageExecuteSettingAttribute>()
                                         .With(r => new MessageExecuteSetting(r))
                                         .Recovery(new MessageExecuteSetting());
            }

            parts.Add(message);
            return this;
        }

        #endregion
    }
}