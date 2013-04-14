namespace Incoding.SiteTest.Contrib
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.CQRS;

    #endregion

    public static class IncodingSiteBootstrapped
    {
        #region Factory constructors

        public static void Start()
        {
            var structureMapIoCProvider = new StructureMapIoCProvider(registry => registry.For<IDispatcher>().Use<DefaultDispatcher>());
            IoCFactory.Instance.Initialize(init => init.WithProvider(structureMapIoCProvider));
        }

        #endregion
    }
}