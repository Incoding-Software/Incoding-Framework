namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
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
            left.TryGetValue("code").ShouldEqual(right.TryGetValue("code"));
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

        It should_be_group_1 = () => executable
                                             .AsObject()
                                             .Should(dictionary =>
                                                     {
                                                         var dynamicData = (ExecutableBreak)dictionary.data;
                                                         var ands = (List<List<object>>)dynamicData["ands"];

                                                         ands.Count.ShouldEqual(3);

                                                         var group1 = ands[0];
                                                         Compare(group1[0], conditional1FromGroup1.GetData());
                                                         Compare(group1[1], conditional2FromGroup1.GetData());

                                                         var group2 = ands[1];
                                                         Compare(group2[0], conditional1FromGroup2.GetData());
                                                         Compare(group2[1], conditional2FromGroup2.GetData());
                                                         Compare(group2[2], conditional3FromGroup2.GetData());

                                                         var group3 = ands[2];
                                                         Compare(group3[0], conditional1FromGroup3.GetData());
                                                     });
    }
}