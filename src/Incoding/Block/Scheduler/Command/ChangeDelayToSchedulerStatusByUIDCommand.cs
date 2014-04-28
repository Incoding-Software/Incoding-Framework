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

        public override void Execute()
        {
            Dispatcher.Push(new ChangeDelayToSchedulerStatusCommand
                                {
                                        Ids = Repository.Query(whereSpecification: new DelayToSchedulerByUIDWhereSpec(UID))
                                                        .Select(r => r.Id)
                                                        .ToArray(), 
                                        Status = Status
                                });
        }
    }
}