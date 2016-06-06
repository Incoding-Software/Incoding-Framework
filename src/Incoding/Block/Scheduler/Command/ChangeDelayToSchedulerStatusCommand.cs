namespace Incoding.Block
{
    #region << Using >>

    using Incoding.CQRS;

    #endregion

    public class ChangeDelayToSchedulerStatusCommand : CommandBase
    {
        protected override void Execute()
        {
            var delay = Repository.GetById<DelayToScheduler>(Id);
            delay.Status = Status;
            delay.Description = Description;

            if (Status == DelayOfStatus.Success && delay.Recurrence != null)
            {
                var recurrency = new GetRecurrencyDateQuery
                {
                    EndDate = delay.Recurrence.EndDate,
                    RepeatCount = delay.Recurrence.RepeatCount - 1,
                    RepeatDays = delay.Recurrence.RepeatDays,
                    RepeatInterval = delay.Recurrence.RepeatInterval,
                    StartDate = delay.StartsOn,
                    Type = delay.Recurrence.Type
                };
                recurrency.NowDate = delay.StartsOn; // calculate next start depending on previously calculated start (to run every day at exactly same time for example)
                recurrency.StartDate = Dispatcher.Query(recurrency);
                if (!recurrency.StartDate.HasValue)
                    return;
                Dispatcher.Push(new AddDelayToSchedulerCommand(delay)
                                {
                                        UID = delay.UID,
                                        Priority = delay.Priority,                                                                               
                                        Recurrency = recurrency,
                                });
            }
        }

        #region Properties

        public string Id { get; set; }

        public DelayOfStatus Status { get; set; }

        public string Description { get; set; }

        #endregion
    }
}