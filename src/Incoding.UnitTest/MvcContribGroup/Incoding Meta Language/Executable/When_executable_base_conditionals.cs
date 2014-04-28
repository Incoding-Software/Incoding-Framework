namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.Routing;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Newtonsoft.Json.Linq;

    #endregion

    [Subject(typeof(ExecutableBase))]
    public class When_executable_base_conditionals
    {
        #region Establish value

        static ExecutableBase executable;

        static ConditionalEval conditional1FromGroup1;

        static ConditionalEval conditional2FromGroup1;

        static ConditionalEval conditional1FromGroup2;

        static ConditionalEval conditional2FromGroup2;

        static ConditionalEval conditional3FromGroup2;

        static ConditionalEval conditional1FromGroup3;

        static void Compare(object left, object right)
        {
            var jLeft = left as JContainer;
            jLeft.Value<string>("code").ShouldEqual(right.TryGetValue("code"));
        }

        #endregion

        Establish establish = () =>
                                  {
                                      executable = new ExecutableBreak();
                                      conditional1FromGroup1 = new ConditionalEval(Pleasure.Generator.String(), true);
                                      conditional2FromGroup1 = new ConditionalEval(Pleasure.Generator.String(), true);

                                      conditional1FromGroup2 = new ConditionalEval(Pleasure.Generator.String(), false);
                                      conditional2FromGroup2 = new ConditionalEval(Pleasure.Generator.String(), true);
                                      conditional3FromGroup2 = new ConditionalEval(Pleasure.Generator.String(), true);

                                      conditional1FromGroup3 = new ConditionalEval(Pleasure.Generator.String(), false);
                                  };

        Because of = () => executable.SetValue("conditionals", new List<ConditionalBase>
                                                                   {
                                                                           conditional1FromGroup1, 
                                                                           conditional2FromGroup1, 
                                                                           conditional1FromGroup2, 
                                                                           conditional2FromGroup2, 
                                                                           conditional3FromGroup2, 
                                                                           conditional1FromGroup3, 
                                                                   });

        It should_be_group_1 = () =>
                                   {
                                       executable
                                               .Merge(new RouteValueDictionary())
                                               .Should(dictionary =>
                                                           {
                                                               var incodingData = dictionary["incoding"].ToString().DeserializeFromJson<object>() as JContainer;
                                                               dynamic callback = incodingData[0];

                                                               var dynamicData = callback.data;
                                                               var ands = dynamicData.ands;

                                                               var group1 = (JArray)ands[0];
                                                               Compare(group1[0], conditional1FromGroup1.GetData());
                                                               Compare(group1[1], conditional2FromGroup1.GetData());

                                                               var group2 = (JArray)ands[1];
                                                               Compare(group2[0], conditional1FromGroup2.GetData());
                                                               Compare(group2[1], conditional2FromGroup2.GetData());
                                                               Compare(group2[2], conditional3FromGroup2.GetData());

                                                               var group3 = (JArray)ands[2];
                                                               Compare(group3[0], conditional1FromGroup3.GetData());
                                                           });
                                   };
    }
}