namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CreateByTypeQuery.FindTypeByName))]
    public class When_find_by_type_performance
    {
        It should_be_full_name = () =>
                                 {
                                     var message = MockQuery<CreateByTypeQuery.FindTypeByName, Type>
                                             .When(Pleasure.Generator.Invent<CreateByTypeQuery.FindTypeByName>(dsl => dsl.Tuning(r => r.Type, typeof(When_find_by_type_performanceCommand).FullName)));
                                     Pleasure.Do(i =>
                                                 {
                                                     message.Execute();
                                                     message.ShouldBeIsResult(typeof(When_find_by_type_performanceCommand));
                                                 }, 1000)
                                             .ShouldBeLessThan(1000);
                                 };

        It should_be_name = () =>
                            {
                                var message = MockQuery<CreateByTypeQuery.FindTypeByName, Type>
                                        .When(Pleasure.Generator.Invent<CreateByTypeQuery.FindTypeByName>(dsl => dsl.Tuning(r => r.Type, typeof(When_find_by_type_performanceCommand).Name)));
                                Pleasure.Do(i =>
                                            {
                                                message.Execute();
                                                message.ShouldBeIsResult(typeof(When_find_by_type_performanceCommand));
                                            }, 1000)
                                        .ShouldBeLessThan(600);
                            };

        It should_be_name_with_preload = () =>
                                         {
                                             var message = MockQuery<CreateByTypeQuery.FindTypeByName, Type>
                                                     .When(Pleasure.Generator.Invent<CreateByTypeQuery.FindTypeByName>(dsl => dsl.Tuning(r => r.Type, typeof(When_find_by_type_performanceCommand).Name)));
                                             Action<int> action = i =>
                                                                  {
                                                                      message.Execute();
                                                                      message.ShouldBeIsResult(typeof(When_find_by_type_performanceCommand));
                                                                  };
                                             var dictionary = new Dictionary<string, Type>();
                                             dictionary.Add(typeof(When_find_by_type_performanceCommand).Name, typeof(When_find_by_type_performanceCommand));
                                             typeof(CreateByTypeQuery.FindTypeByName).GetField("cache", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, dictionary);

                                             Pleasure.Do(action, 1000).ShouldBeLessThan(100);
                                         };

        It should_be_sizeof_type = () =>
                                   {
                                       // Measure starting point memory use
                                       var start = GC.GetTotalMemory(true);

                                       // Allocate a new byte array of 20000 elements (about 20000 bytes)
                                       var type = typeof(KeyValueVm);

                                       // Obtain measurements after creating the new byte[]
                                       var end = GC.GetTotalMemory(true);

                                       // Ensure that the Array stays in memory and doesn't get optimized away
                                       GC.KeepAlive(type);

                                       ((long)(end - start)).ShouldBeLessThan(1);
                                   };

        public class When_find_by_type_performanceCommand { }
    }
}