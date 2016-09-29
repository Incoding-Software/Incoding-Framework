namespace Incoding.SiteTest.Domain
{
    #region << Using >>

    using System.Configuration;
    using Incoding.CQRS;
    using Incoding.Extensions;

    #endregion

    public class StadAloneTransactionCommand : CommandBase
    {
        public bool IsThrow { get; set; }

        protected override void Execute()
        {
            var product = new Product() { Name = "Will be save alone" };
            Repository.Save(product);
            if (IsThrow)
                throw IncWebException.ForServer("Test");
        }
    }


    public class MultipleTransactionCommand : CommandBase
    {
        public bool IsThrow { get; set; }
        protected override void Execute()
        {
            var product = new Product() { Name = "Will be save edition 0" };
            Repository.Save(product);
            for (int i = 1; i < 10; i++)
            {
                Dispatcher.Push(new InnerCommand() { Index = i });
            }
            if (IsThrow)
                throw IncWebException.ForServer("Test");
        }

        public class InnerCommand : CommandBase
        {
            public int Index { get; set; }

            protected override void Execute()
            {
                var product = new Product() { Name = "Will be save edition {0}".F(Index) };
                Repository.Save(product);
            }
        }
    }

    public class MultipleConnectionStringTransactionCommand : CommandBase
    {
        public bool IsThrow { get; set; }
        protected override void Execute()
        {
            var product = new Product() { Name = "Will be save TEST2 edition 0" };
            Repository.Save(product);
            for (int i = 1; i < 10; i++)
            {
                Dispatcher.Push(new InnerCommand() { Index = i }, setting => { setting.Connection = ConfigurationManager.ConnectionStrings["Main2"].ConnectionString; });
            }

            if (IsThrow)
                throw IncWebException.ForServer("Test");
        }

        public class InnerCommand : CommandBase
        {
            public int Index { get; set; }

            protected override void Execute()
            {
                var product = new Product() { Name = "Will be save TEST2 edition {0}".F(Index) };
                Repository.Save(product);
            }
        }
    }

}