namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FluentValidation;
    using Incoding.Block.IoC;
    using Incoding.Block.Logging;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.Utilities;
    using Machine.Specifications;

    #endregion

    [Behaviors]
    public class Behaviors_get_ioc_provider : Context_IoC_Provider
    {
        private static Dictionary<Type, int> performacneDictionary = new Dictionary<Type, int>()
                                                                     {
                                                                             { typeof(StructureMapIoCProvider), 9 },
                                                                             { typeof(NinjectIoCProvider), 100 },
                                                                             { typeof(DryIocProvider), 3 },
                                                                             { typeof(SimpleInjectorIoCProvider), 1 },
                                                                     };

        private static Dictionary<Type, int> performacneSingletonDictionary = new Dictionary<Type, int>()
                                                                              {
                                                                                      { typeof(StructureMapIoCProvider), 9 },
                                                                                      { typeof(NinjectIoCProvider), 100 },
                                                                                      { typeof(DryIocProvider), 3 },
                                                                                      { typeof(SimpleInjectorIoCProvider), 1 },
                                                                              };

        private static Dictionary<Type, int> performacneAsyncDictionary = new Dictionary<Type, int>()
                                                                          {
                                                                                  { typeof(StructureMapIoCProvider), 9 },
                                                                                  { typeof(NinjectIoCProvider), 100 },
                                                                                  { typeof(DryIocProvider), 3 },
                                                                                  { typeof(SimpleInjectorIoCProvider), 1 },
                                                                          };

        It should_be__performance_get_by_type = () => Pleasure.Do(i => ioCProvider.Get<IEmailSender>(typeof(IEmailSender)).ShouldNotBeNull(), 1000)
                                                              .ShouldBeLessThanOrEqualTo(performacneDictionary[ioCProvider.GetType()]);

        It should_be_asyn_perfromance_try_get = () =>
                                                {
                                                    Pleasure.MultiThread.Do(() => ioCProvider.TryGet<IEmailSender>().ShouldNotBeNull(), 1000)
                                                            .WaitOne(performacneAsyncDictionary[ioCProvider.GetType()].Seconds())
                                                            .ShouldBeTrue();
                                                };

        It should_be_get_all = () =>
                               {
                                   int allByAssembly = Assembly
                                           .GetAssembly(typeof(IFakePlugIn)).GetTypes()
                                           .Where(r => !r.IsAnyEquals(typeof(IFakePlugIn)))
                                           .Count(r => r.IsImplement<IFakePlugIn>());

                                   ioCProvider.GetAll<IFakePlugIn>(typeof(IFakePlugIn)).Count().ShouldEqual(allByAssembly);
                               };

        It should_be_get_by_type = () => ioCProvider.Get<IEmailSender>(typeof(IEmailSender)).ShouldBeTheSameAs(defaultInstance);

        It should_be_performance_try_get = () => Pleasure.Do(i => ioCProvider.TryGet<IEmailSender>().ShouldNotBeNull(), 1000)
                                                         .ShouldBeLessThanOrEqualTo(performacneDictionary[ioCProvider.GetType()]);

        It should_be_performance_try_get_by_named = () =>
                                                    {
                                                        try
                                                        {
                                                            Pleasure.Do(i => ioCProvider.TryGetByNamed<ILogger>(consoleNameInstance).ShouldNotBeNull(), 1000)
                                                                    .ShouldBeLessThanOrEqualTo(performacneDictionary[ioCProvider.GetType()]);
                                                        }
                                                        catch (NotSupportedException) { }
                                                    };

        It should_be_try_get = () => ioCProvider.TryGet<IEmailSender>().ShouldBeTheSameAs(defaultInstance);

        It should_be_try_get_by_named = () =>
                                        {
                                            try
                                            {
                                                ioCProvider.TryGetByNamed<ILogger>(consoleNameInstance).ShouldBeAssignableTo<ConsoleLogger>();
                                            }
                                            catch (NotSupportedException) { }
                                        };

        It should_be_try_get_by_type = () => ioCProvider.TryGet<IEmailSender>(typeof(IEmailSender)).ShouldBeTheSameAs(defaultInstance);

        It should_be_try_get_validator = () => { ioCProvider.TryGet<IValidator>(typeof(AbstractValidator<FakeCommand>)).ShouldNotBeNull(); };
    }
}