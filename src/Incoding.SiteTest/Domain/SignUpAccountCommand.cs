namespace Incoding.SiteTest.Domain
{
    using Incoding.CQRS;

    public abstract class SignUpAccountCommand : CommandBase
    {


        #region Properties

        public bool IsExternal { get; set; }

        public string Name { get; set; }

        public string Telephone { get; set; }

        public string Address { get; set; }

        public string Zip { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string RePassword { get; set; }

        public string Provider { get; set; }

        public string ProviderUserId { get; set; }

        public string Token { get; set; }

        public string Viber { get; set; }

        public string Skype { get; set; }

        public string WhatsApp { get; set; }

        #endregion



        protected override void Execute()
        {
        }

        
    }
}