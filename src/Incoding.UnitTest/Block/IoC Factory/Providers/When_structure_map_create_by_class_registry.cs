namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.Utilities;
    using Machine.Specifications;
    using StructureMap.Configuration.DSL;

    #endregion

    [Subject(typeof(StructureMapIoCProvider))]
    public class When_structure_map_create_by_class_registry : Context_IoC_Provider
    {
        Establish establish = () =>
                              {
                                  ioCProvider = new StructureMapIoCProvider(new CustomRegistry());
                              };

        Behaves_like<Behaviors_disposable_ioc_provider> verify_disposable;

        Behaves_like<Behaviors_eject_ioc_provider> verify_eject;

        Behaves_like<Behaviors_expected_exception_ioc_provider> verify_expected_exception;

        Behaves_like<Behaviors_forward_ioc_provider> verify_forward;

        Behaves_like<Behaviors_get_ioc_provider> verify_get_and_try_get_operation;

        #region Fake classes

        public class CustomRegistry : Registry
        {
            #region Constructors

            public CustomRegistry()
            {
                For<IEmailSender>().Use(defaultInstance);
                For<ILogger>().Use<ConsoleLogger>().Named(consoleNameInstance.ToString());

                Scan(scanner =>
                     {
                         scanner.Assembly(typeof(IFakePlugIn).Assembly);
                         scanner.AddAllTypesOf<IFakePlugIn>();
                     });
            }

            #endregion
        }

        #endregion
    }
}