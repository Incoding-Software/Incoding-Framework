namespace Incoding.SiteTest
{
    #region << Using >>

    using Incoding.CQRS;

    #endregion

    public class AddProductCommand : CommandBase
    {
        #region Properties

        public string Name { get; set; }

        #endregion

        public override void Execute()
        {
            Repository.Save(new Product
                                {
                                        Name = Name
                                });
            Repository.Save(new Product
                                {
                                        Name = Name
                                });
        }
    }
}