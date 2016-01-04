namespace Incoding.SiteTest
{
    #region << Using >>

    using System.Threading;
    using FluentValidation;
    using Incoding.Block;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    [OptionOfDelay(Async = true)]
    public class AddProductCommand : CommandBase
    {
        public class Validator : AbstractValidator<AddProductCommand>
        {
            public Validator()
            {
                RuleFor(r => r.Name).NotEmpty();
            }
        }

        #region Properties

        public string Name { get; set; }
        public string FromPost { get; set; }

        public string HasValue { get; set; }

        #endregion

        protected override void Execute()
        {
            Repository.Save(new Product { Name = "Async" });
            Thread.Sleep(1.Seconds());
        }
    }
}