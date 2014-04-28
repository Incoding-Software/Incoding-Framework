namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class AddDelayToSchedulerCommand : CommandBase
    {
        #region Properties

        public List<IMessage<object>> Commands { get; set; }

        #endregion

        public override void Execute()
        {
            string groupKey = Guid.NewGuid().ToString();
            int priority = 0;
            foreach (var item in Commands)
            {                                
                Repository.Save(new DelayToScheduler
                                    {                                        
                                            Command = item.ToJsonString(),                                            
                                            Type = item.GetType().AssemblyQualifiedName,
                                            GroupKey = groupKey,
                                            Priority = priority,
                                            UID = item.Setting.Delay.UID,
                                            Status = DelayOfStatus.New
                                    });
                priority++;
            }
        }
    }
}