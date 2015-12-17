namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using FluentValidation;
    using Incoding.Block.IoC;
    using Incoding.Utilities;

    #endregion

    public class Context_IoC_Provider
    {
        public enum Named
        {
            First,
            Second
        }

        #region Static Fields

        protected static readonly IEmailSender defaultInstance = new FakeEmailSender(message => { });

        protected static IIoCProvider ioCProvider;

        // ReSharper disable ConvertToConstant.Global 
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        protected static Named consoleNameInstance = Named.Second;

        #endregion

        #region Nested classes

        public class FakePlugIn1 : IFakePlugIn { }

        public class FakePlugIn2 : IFakePlugIn { }


        public class FakeCommand
        {
            public string Name { get; set; }
        }

        public class TestValidator : AbstractValidator<FakeCommand>
        {
            public TestValidator()
            {
                RuleFor(r => r.Name).NotEmpty();
            }
        }

        #endregion

        public interface IFakePlugIn { }

        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore ConvertToConstant.Global
    }
}