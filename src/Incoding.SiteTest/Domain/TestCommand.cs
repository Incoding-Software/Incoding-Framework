namespace Incoding.SiteTest.Controllers
{
    #region << Using >>

    using Incoding.Block;
    using Incoding.CQRS;

    #endregion

    public class TestCommand : CommandBase
    {
        protected override void Execute()
        {
            for (int i = 0; i < 100; i++)
            {
                Dispatcher.Push(new AddDelayToSchedulerCommand()
                                {
                                        Command = new AddProductCommand() { Name = i.ToString() }
                                });
            }

            for (int i = 0; i < 100; i++)
            {
                Dispatcher.Push(new AddDelayToSchedulerCommand()
                                {
                                        Command = new AddAsSyncProductCommand() { Name = i.ToString() }
                                });
            }
        }
    }
}