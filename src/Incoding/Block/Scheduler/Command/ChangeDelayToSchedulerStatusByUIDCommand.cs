namespace Incoding.Block
{
    #region << Using >>

    using Incoding.CQRS;

    #endregion

    public class ChangeDelayToSchedulerStatusByUIDCommand : CommandBase
    {
        protected override void Execute()
        {
            foreach (var delay in Repository.Query(whereSpecification: new DelayToScheduler.Where.ByUID(UID)))
            {
                Dispatcher.Push(new ChangeDelayToSchedulerStatusCommand
                                {
                                        Id = delay.Id,
                                        Status = Status
                                });
            }
        }

        #region Properties

        public string UID { get; set; }

        public DelayOfStatus Status { get; set; }

        #endregion
    }
}