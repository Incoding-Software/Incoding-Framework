namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System.Linq;
    using Incoding.Block.Logging;
    using Machine.Specifications;

    #endregion

    [Behaviors]
    public class Behaviors_eject_ioc_provider : Context_IoC_Provider
    {
        It should_be_eject = () =>
                             {
                                 ioCProvider.Eject<ILogger>();
                                 ioCProvider.GetAll<ILogger>(typeof(ILogger)).Count().ShouldEqual(0);
                             };
    }
}