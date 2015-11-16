namespace Incoding.Block
{
    #region << Using >>

    using System.Linq;
    using Incoding.CQRS;

    #endregion

    public class ChangeDelayToSchedulerStatusByUIDCommand : CommandBase
    {
        #region Properties

        public string UID { get; set; }

        public DelayOfStatus Status { get; set; }

        #endregion

        protected override void Execute()
        {
            foreach (var delay in Repository.Query(whereSpecification: new DelayToSchedulerByUIDWhereSpec(UID)))
            {
                Dispatcher.Push(new ChangeDelayToSchedulerStatusCommand
                                {
                                        Id = delay.Id,
                                        Status = Status
                                });
            }
        }
    }
}