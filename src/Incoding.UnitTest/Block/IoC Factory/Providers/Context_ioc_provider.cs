namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.Utilities;

    #endregion

    public class Context_IoC_Provider
    {
        #region Static Fields

        protected static readonly IEmailSender defaultInstance = new FakeEmailSender(message => { });

        protected static IIoCProvider ioCProvider;

        // ReSharper disable ConvertToConstant.Global 
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        protected static string consoleNameInstance = "namedInstance";

        #endregion

        #region Nested classes

        public class FakePlugIn1 : IFakePlugIn { }

        public class FakePlugIn2 : IFakePlugIn { }

        #endregion

        public interface IFakePlugIn { }

        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore ConvertToConstant.Global
    }
}