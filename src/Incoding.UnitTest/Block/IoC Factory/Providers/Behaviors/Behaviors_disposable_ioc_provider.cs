namespace Incoding.UnitTest.Block
{
    using Machine.Specifications;

    [Behaviors]
    public class Behaviors_disposable_ioc_provider : Context_IoC_Provider
    {
        It should_be_disposable_without_exception = () => Catch.Exception(() => ioCProvider.Dispose()).ShouldBeNull();
    }
}