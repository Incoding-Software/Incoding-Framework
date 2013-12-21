namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Newtonsoft.Json.Linq;

    #endregion

    [Subject(typeof(ExecutableBase))]
    public class When_executable_merge
    {
        It should_be_default_merge = () => new ExecutableValidationParse()
                                                   .Merge(new RouteValueDictionary())
                                                   .Should(dictionary =>
                                                               {
                                                                   var incodingData = dictionary["incoding"].ToString().DeserializeFromJson<object>() as JContainer;
                                                                   dynamic callback = incodingData[0];

                                                                   string typeCallback = callback.type.Value as string;
                                                                   typeCallback.ShouldEqual(typeof(ExecutableValidationParse).Name);
                                                               });

        It should_be_time_out = () =>
                                    {
                                        var executable = new ExecutableValidationParse();
                                        executable.TimeOut(10.Seconds());
                                        executable
                                                .Merge(new RouteValueDictionary())
                                                .Should(dictionary =>
                                                            {
                                                                var incodingData = dictionary["incoding"].ToString().DeserializeFromJson<object>() as JContainer;
                                                                var dynamicData = ((dynamic)incodingData[0]).data;

                                                                ((int)dynamicData.timeOut).ShouldEqual(10000);
                                                            });
                                    };

        It should_be_interval = () =>
                                    {
                                        var executable = new ExecutableValidationParse();
                                        string intervalId;
                                        executable.Interval(10.Seconds(), out intervalId);
                                        executable
                                                .Merge(new RouteValueDictionary())
                                                .Should(dictionary =>
                                                            {
                                                                var incodingData = dictionary["incoding"].ToString().DeserializeFromJson<object>() as JContainer;
                                                                var dynamicData = ((dynamic)incodingData[0]).data;

                                                                intervalId.Length.ShouldEqual(36);
                                                                intervalId.ShouldNotContain("-");
                                                                ((int)dynamicData.interval).ShouldEqual(10000);
                                                                ((string)dynamicData.intervalId).ShouldEqual(intervalId);
                                                            });
                                    };

        It should_be_if = () =>
                              {
                                  var executable = new ExecutableValidationParse();
                                  executable.If(builder => builder.Eval(Pleasure.Generator.TheSameString()));
                                  executable
                                          .Merge(new RouteValueDictionary())
                                          .Should(dictionary =>
                                                      {
                                                          var incodingData = dictionary["incoding"].ToString().DeserializeFromJson<object>() as JContainer;
                                                          var dynamicData = ((dynamic)incodingData[0]).data;

                                                          var ands = dynamicData.ands;
                                                          var group1 = (JArray)ands[0];

                                                          var leftD = group1[0] as JObject;

                                                          leftD.Value<string>("code").ShouldEqual(Pleasure.Generator.TheSameString());
                                                          leftD.Value<bool>("and").ShouldBeTrue();
                                                      });
                              };
    }
}