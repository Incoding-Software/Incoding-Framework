namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Data;

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

        public string DataBaseInstance { get; set; }

        public string Connection { get; set; }

        public IsolationLevel IsolationLevel { get; set; }

        #endregion
    }
}