using Incoding.Maybe;

namespace Incoding.Block
{
    #region << Using >>

    using System.Linq;
    using Incoding.CQRS;

    #endregion

    public class ChangeDelayToSchedulerStatusCommand : CommandBase
    {
        #region Properties

        public string[] Ids { get; set; }

        public DelayOfStatus Status { get; set; }

        public string Description { get; set; }

        public bool UpdateNextStart { get; set; }

        #endregion

        public override void Execute()
        {
            foreach (var delay in Ids.Select(id => Repository.GetById<DelayToScheduler>(id)))
            {
                delay.Status = Status;
                delay.Description = Description;
                if (UpdateNextStart)
                {
                    var nextStartsOn = delay.Recurrence.With(r => r.NextDt());
                    if (nextStartsOn.HasValue && delay.Status == DelayOfStatus.Success)
                    {
                        delay.StartsOn = nextStartsOn.Value;
                        delay.Status = DelayOfStatus.New;
                    }
                }
            }
        }
    }
}