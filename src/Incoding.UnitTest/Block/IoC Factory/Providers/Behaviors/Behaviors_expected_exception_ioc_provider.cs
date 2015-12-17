namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Block.Logging;
    using Machine.Specifications;

    #endregion

    [Behaviors]
    public class Behaviors_expected_exception_ioc_provider : Context_IoC_Provider
    {
        It should_be_not_found_get_all_without_exception_ = () =>
                                                            {
                                                                IEnumerable<IFake> allInstanceFromIoC = null;
                                                                var exception = Catch.Exception(() => { allInstanceFromIoC = ioCProvider.GetAll<IFake>(typeof(IFake)); });
                                                                exception.ShouldBeNull();
                                                                allInstanceFromIoC.Count().ShouldEqual(0);
                                                            };

        It should_be_not_found_get_by_type_with_exception_ = () => Catch.Exception(() => ioCProvider.Get<IFake>(typeof(IFake))).ShouldNotBeNull();

        It should_be_not_found_try_get_by_type_without_exception = () => Catch.Exception(() => ioCProvider.TryGet<IFake>(typeof(IFake))).ShouldBeNull();

        It should_be_not_found_try_get_named_by_type_without_exception = () => Catch.Exception(() =>
                                                                                               {
                                                                                                   try
                                                                                                   {
                                                                                                       ioCProvider.TryGetByNamed<ILogger>(Named.First);
                                                                                                   }
                                                                                                   catch (NotSupportedException) { }
                                                                                               }).ShouldBeNull();

        It should_be_not_found_try_get_without_exception = () => Catch.Exception(() => ioCProvider.TryGet<IFake>()).ShouldBeNull();
    }
}