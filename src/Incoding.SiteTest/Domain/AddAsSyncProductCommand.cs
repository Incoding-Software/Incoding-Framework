namespace Incoding.SiteTest
{
    #region << Using >>

    using System.Threading;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    [OptionOfDelay(Async = true, TimeOutOfMillisecond = 3000)]
    public class AddAsSyncProductCommand : CommandBase
    {
        #region Properties

        public string Name { get; set; }

        #endregion

        protected override void Execute()
        {
            Repository.Save(new Product { Name = "Sync" + Name });
            Thread.Sleep(1.Seconds());
        }
    }
}