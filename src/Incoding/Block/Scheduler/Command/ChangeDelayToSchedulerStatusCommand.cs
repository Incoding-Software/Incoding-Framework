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

        #endregion

        public override void Execute()
        {
            foreach (var delay in Ids.Select(id => Repository.GetById<DelayToScheduler>(id)))
            {
                delay.Status = Status;
                delay.Description = Description;
            }
        }
    }
}