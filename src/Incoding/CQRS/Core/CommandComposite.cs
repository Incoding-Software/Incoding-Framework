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
        #region Fields

        readonly List<IMessage<object>> parts = new List<IMessage<object>>();

        #endregion

        #region Properties

        public ReadOnlyCollection<IMessage<object>> Parts { get { return this.parts.AsReadOnly(); } }

        #endregion

        #region ISettingCommandComposite Members

        public ISettingCommandComposite Quote<TResult>(IMessage<TResult> message, MessageExecuteSetting executeSetting = null)
        {
            if (message.Setting == null)
            {
                message.Setting = executeSetting != null
                                          ? new MessageExecuteSetting(executeSetting)
                                          : message.GetType().FirstOrDefaultAttribute<MessageExecuteSettingAttribute>()
                                                   .With(r => new MessageExecuteSetting(r))
                                                   .Recovery(new MessageExecuteSetting());
            }
            this.parts.Add((IMessage<object>)message);
            return this;
        }

        #endregion
    }
}