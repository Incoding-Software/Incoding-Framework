namespace Incoding.CQRS
{
    #region << Using >>

    using System;

    #endregion

    public class MessageExecuteSettingAttribute : Attribute
    {
        #region Constructors

        public MessageExecuteSettingAttribute()
        {
            DataBaseInstance = string.Empty;
        }

        #endregion

        #region Properties

        public MuteEvent Mute { get; set; }

        public string DataBaseInstance { get; set; }

        public string Connection { get; set; }

        #endregion
    }
}