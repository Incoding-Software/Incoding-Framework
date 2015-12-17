namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using System.Linq;
    using Incoding.Block.Logging;
    using Machine.Specifications;

    #endregion

    [Behaviors]
    public class Behaviors_eject_ioc_provider : Context_IoC_Provider
    {
        It should_be_eject = () =>
                             {
                                 try
                                 {
                                     ioCProvider.Eject<ILogger>();
                                     ioCProvider.GetAll<ILogger>(typeof(ILogger)).Count().ShouldEqual(0);
                                 }
                                 catch (NotSupportedException)
                                 {
                                     //mute because not every can doing it                                     
                                 }
                             };
    }
}