namespace Incoding.SiteTest
{
    using System.Threading;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.Extensions;

    [OptionOfDelay(Async = true,TimeOut = 10000)]
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