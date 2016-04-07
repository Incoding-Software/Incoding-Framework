namespace Incoding.SiteTest.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentValidation;
    using Incoding.Quality;
    using JetBrains.Annotations;

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