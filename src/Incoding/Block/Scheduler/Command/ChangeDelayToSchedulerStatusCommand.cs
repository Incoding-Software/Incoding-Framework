namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Linq;
    using Incoding.CQRS;

    #endregion

    public class ChangeDelayToSchedulerStatusCommand : CommandBase
    {
        protected override void Execute()
        {
            foreach (var delay in Ids.Select(id => Repository.GetById<DelayToScheduler>(id)).Where(r => r != null))
            {
                delay.Status = Status;
                delay.Description = Description;

                if (Status == DelayOfStatus.Success)
                {
                    if (delay.Recurrence != null)
                        delay.Recurrence.NowDate = delay.StartsOn; // calculate next start depending on previously calculated start (to run every day at exactly same time for example)
                }

                DateTime? nextStartsOn = delay.Recurrence != null ? Dispatcher.Query(delay.Recurrence) : null;
                if (nextStartsOn.HasValue && Status == DelayOfStatus.Success)
                {
                    Dispatcher.Push(new AddDelayToSchedulerCommand()
                                    {
                                            Commands = new[] { delay.Instance }.Cast<IMessage<object>>().ToList(),
                                            Recurrency = delay.Recurrence,
                                            UID = delay.UID,
                                            Setting = delay.Instance.Setting
                                    });
                }
            }
        }

        #region Properties

        public string[] Ids { get; set; }

        public DelayOfStatus Status { get; set; }

        public string Description { get; set; }

        #endregion
    }
}