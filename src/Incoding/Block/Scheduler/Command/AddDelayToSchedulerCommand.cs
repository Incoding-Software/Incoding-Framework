using Incoding.Maybe;

namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class AddDelayToSchedulerCommand : CommandBase
    {
        #region Properties

        public List<IMessage<object>> Commands { get; set; }
        public string UID { get; set; }
        public DelayToScheduler.Reccurence RecurrenceData { get; set; }

        #endregion

        #region Nested classes


        #endregion

        public override void Execute()
        {
            string groupKey = Guid.NewGuid().ToString();
            Repository.Saves(Commands.Select((message, i) => new DelayToScheduler
                                                                 {
                                                                         Command = message.ToJsonString(),
                                                                         Type = message.GetType().AssemblyQualifiedName,
                                                                         GroupKey = groupKey,
                                                                         Priority = i,
                                                                         UID = UID,
                                                                         Status = DelayOfStatus.New,
                                                                         Recurrence = RecurrenceData,
                                                                         StartsOn = RecurrenceData.With(r => r.NextDt()).GetValueOrDefault(DateTime.UtcNow)
                                                                 }));
        }
    }
}