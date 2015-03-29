using System;
using Incoding.Block;

namespace Incoding.CQRS
{
    #region << Using >>

    

    #endregion

    public class MessageDelaySetting
    {
        #region Properties

        public string DataBaseInstance { get; set; }

        public string Connection { get; set; }

        public string UID { get; set; }

        public DelayToScheduler.Reccurence Reccurence { get; set; }
        
        #endregion
    }
}