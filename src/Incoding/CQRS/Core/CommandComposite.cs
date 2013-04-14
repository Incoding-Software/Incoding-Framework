namespace Incoding.CQRS
{
    #region << Using >>

    using System.Collections.Generic;

    #endregion

    public class CommandComposite
    {
        #region Fields

        readonly List<MessageCompositePart> parts = new List<MessageCompositePart>();

        #endregion

        #region Properties

        public List<MessageCompositePart> Parts
        {
            get { return this.parts; }
        }

        #endregion

        #region Api Methods

        public void Quote(IMessage<object> message, MessageExecuteSetting executeSetting = null)
        {
            var part = new MessageCompositePart(message, executeSetting ?? new MessageExecuteSetting());
            this.parts.Add(part);
        }

        #endregion

        #region Nested classes

        public class MessageCompositePart
        {
            #region Fields

            readonly IMessage<object> message;

            readonly MessageExecuteSetting setting;

            #endregion

            #region Constructors

            internal MessageCompositePart(IMessage<object> message, MessageExecuteSetting executeSetting)
            {
                this.message = message;
                this.setting = executeSetting;
            }

            #endregion

            #region Properties

            public IMessage<object> Message
            {
                get { return this.message; }
            }

            public MessageExecuteSetting Setting
            {
                get { return this.setting; }
            }

            #endregion
        }

        #endregion
    }
}