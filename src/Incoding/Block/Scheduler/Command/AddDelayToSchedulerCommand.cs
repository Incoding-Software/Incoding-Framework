namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.Quality;
    using JetBrains.Annotations;

    #endregion

    public class AddDelayToSchedulerCommand : CommandBase
    {
        #region Constructors

        public AddDelayToSchedulerCommand(DelayToScheduler delay)
        {
            Command = delay.Instance;
            Priority = delay.Priority;
            UID = delay.UID;
            Setting = delay.Instance.Setting;
        }

        public AddDelayToSchedulerCommand() { }

        #endregion

        #region Properties

        public CommandBase Command { get; set; }

        public string UID { get; set; }

        public GetRecurrencyDateQuery Recurrency { get; set; }

        public int Priority { get; set; }

        #endregion

        protected override void Execute()
        {
            Recurrency = Recurrency ?? new GetRecurrencyDateQuery
                                       {
                                               Type = GetRecurrencyDateQuery.RepeatType.Once
                                       };
            var type = Command.GetType();
            var option = type.FirstOrDefaultAttribute<OptionOfDelayAttribute>() ?? new OptionOfDelayAttribute();
            Repository.Save(new DelayToScheduler
                            {
                                    Command = Command.ToJsonString(),
                                    Type = type.AssemblyQualifiedName,
                                    UID = UID,
                                    Priority = Priority,
                                    Status = DelayOfStatus.New,
                                    Recurrence = Recurrency,
                                    StartsOn = Dispatcher.Query(Recurrency).GetValueOrDefault(Recurrency.StartDate.GetValueOrDefault(DateTime.UtcNow)),
                                    Option = new DelayToScheduler.OptionOfDelay()
                                             {
                                                     Async = option.Async,
                                                     TimeOut = option.TimeOut.GetValueOrDefault(5)
                                             }
                            });
        }
    }
}