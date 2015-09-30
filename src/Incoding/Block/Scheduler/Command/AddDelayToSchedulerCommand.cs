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

        public GetRecurrencyDateQuery Recurrency { get; set; }

        #endregion

        #region Nested classes

        #endregion

        protected override void Execute()
        {
            string groupKey = Guid.NewGuid().ToString();
            Recurrency = Recurrency ?? new GetRecurrencyDateQuery
                                       {
                                           Type = GetRecurrencyDateQuery.RepeatType.Once
                                       };
            Repository.Saves(Commands.Select((message, i) => new DelayToScheduler
                                                             {
                                                                     Command = message.ToJsonString(),
                                                                     Type = message.GetType().AssemblyQualifiedName,
                                                                     GroupKey = groupKey,
                                                                     Priority = i,
                                                                     UID = UID,
                                                                     Status = DelayOfStatus.New,
                                                                     Recurrence = Recurrency,
                                                                     StartsOn = Dispatcher.Query(Recurrency).GetValueOrDefault(Recurrency.StartDate.GetValueOrDefault(DateTime.UtcNow))
                                                             }));
        }
    }
}