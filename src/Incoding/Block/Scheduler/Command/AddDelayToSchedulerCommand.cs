namespace Incoding.Block
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class AddDelayToSchedulerCommand : CommandBase
    {
        protected override void Execute()
        {
            Recurrency = Recurrency ?? new GetRecurrencyDateQuery
                                       {
                                               Type = GetRecurrencyDateQuery.RepeatType.Once,
                                       };
            var type = Command.GetType();
            var option = type.FirstOrDefaultAttribute<OptionOfDelayAttribute>() ?? new OptionOfDelayAttribute();
            Repository.Save(new DelayToScheduler
                            {
                                    Command = Command.ToJsonString(),
                                    CreateDt = DateTime.UtcNow,
                                    Type = type.AssemblyQualifiedName,
                                    UID = UID,
                                    Priority = Priority,
                                    Status = DelayOfStatus.New,
                                    Recurrence = Recurrency,
                                    StartsOn = Recurrency.StartDate.GetValueOrDefault(DateTime.UtcNow),
                                    Option = new DelayToScheduler.OptionOfDelay()
                                             {
                                                     Async = option.Async,
                                                     TimeOut = option.TimeOutOfMillisecond
                                             }
                            });
        }

        #region Constructors

        public AddDelayToSchedulerCommand(DelayToScheduler delay)
        {
            var instance = (CommandBase)delay.Command.DeserializeFromJson(Type.GetType(delay.Type));
            Command = instance;
            Priority = delay.Priority;
            UID = delay.UID;
            Setting = instance.Setting;
        }

        public AddDelayToSchedulerCommand() { }

        #endregion

        #region Properties

        public CommandBase Command { get; set; }

        public string UID { get; set; }

        public GetRecurrencyDateQuery Recurrency { get; set; }

        public int Priority { get; set; }

        #endregion
    }
}