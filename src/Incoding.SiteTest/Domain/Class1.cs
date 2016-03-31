namespace Incoding.SiteTest.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentValidation;
    using Incoding.CQRS;
    using Incoding.Quality;
    using JetBrains.Annotations;


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


    public class SignUpAdvertiserCommand : SignUpAccountCommand
    {

        public class Tmpl
        {
            public string Id { get; set; }
        }

        #region Properties

        public string ReEmail { get; set; }

        public new string RePassword { get; set; }

        #endregion

        #region Nested classes

        [UsedImplicitly, Obsolete(ObsoleteMessage.SerializeConstructor, true), ExcludeFromCodeCoverage]
        public class Validator : AbstractValidator<SignUpAdvertiserCommand>
        {


            #region Constructors

            public Validator()
            {
                RuleFor(r => r.Name)
                        .NotEmpty();

                RuleFor(r => r.Password)
                        .NotEmpty();
                RuleFor(r => r.RePassword)
                        .NotEmpty();
                RuleFor(r => r.Email)
                        .NotEmpty();
                RuleFor(r => r.ReEmail)
                        .NotEmpty();
                RuleFor(r => r.Telephone)
                        .NotEmpty();
            }

            #endregion
        }

        #endregion

        protected override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}