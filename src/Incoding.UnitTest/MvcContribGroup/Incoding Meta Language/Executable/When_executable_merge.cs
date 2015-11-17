namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ExecutableBase))]
    public class When_executable_merge
    {
        #region Establish value

        static Action<ExecutableBase.Json> verifyIf = dictionary =>
                                                      {
                                                          var dynamicData = (dynamic)dictionary.data;

                                                          List<List<object>> ands = dynamicData["ands"];
                                                          var group1 = ands[0];

                                                          group1[0].TryGetValue<string>("type").ShouldEqual("Is");
                                                          group1[0].TryGetValue<bool>("inverse").ShouldEqual(false);
                                                          group1[0].TryGetValue<string>("left").ShouldEqual("||value*TheSameString||");
                                                          group1[0].TryGetValue<string>("right").ShouldEqual(Selector.Jquery.Self().ToString());
                                                          group1[0].TryGetValue<string>("method").ShouldEqual("equal");
                                                          group1[0].TryGetValue<bool>("and").ShouldBeTrue();
                                                      };

        #endregion

        It should_be_default_merge = () => new ExecutableValidationParse()
                                                   .AsObject()
                                                   .Should(dictionary =>
                                                           {
                                                               dictionary.data.ShouldNotBeNull();
                                                               dictionary.type.ShouldEqual(typeof(ExecutableValidationParse).Name);
                                                           });

        It should_be_if = () =>
                          {
                              var executable = new ExecutableValidationParse();
                              executable.If(() => Pleasure.Generator.TheSameString() == Selector.Jquery.Self());

                              executable
                                      .AsObject()
                                      .Should(verifyIf);
                          };

        It should_be_if_after_interval = () =>
                                         {
                                             var executable = new ExecutableValidationParse();
                                             string id = string.Empty;
                                             executable.Interval(10, out id).If(() => Pleasure.Generator.TheSameString() == Selector.Jquery.Self());
                                             executable
                                                     .AsObject()
                                                     .Should(verifyIf);
                                         };

        It should_be_if_after_timeout = () =>
                                        {
                                            var executable = new ExecutableValidationParse();
                                            executable.TimeOut(10).If(() => Pleasure.Generator.TheSameString() == Selector.Jquery.Self());
                                            executable
                                                    .AsObject()
                                                    .Should(verifyIf);
                                        };

        It should_be_interval = () =>
                                {
                                    var executable = new ExecutableValidationParse();
                                    string intervalId;
                                    executable.Interval(10.Seconds(), out intervalId);
                                    executable
                                            .AsObject()
                                            .Should(dictionary =>
                                                    {
                                                        var dynamicData = (dynamic)dictionary.data;

                                                        intervalId.Length.ShouldEqual(36);
                                                        intervalId.ShouldNotContain("-");
                                                        ((int)dynamicData["interval"]).ShouldEqual(10000);
                                                        ((string)dynamicData["intervalId"]).ShouldEqual(intervalId);
                                                    });
                                };

        It should_be_time_out = () =>
                                {
                                    var executable = new ExecutableValidationParse();
                                    executable.TimeOut(10.Seconds());
                                    executable
                                            .AsObject()
                                            .Should(dictionary =>
                                                    {
                                                        var dynamicData = (dynamic)dictionary.data;
                                                        ((int)dynamicData["timeOut"]).ShouldEqual(10000);
                                                    });
                                };
    }
}