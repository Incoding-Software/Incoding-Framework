namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaContainer))]
    public class When_incoding_meta_container_set_filter
    {
        #region Estabilish value

        static IncodingMetaContainer meta;

        static ConditionalEval conditional;

        #endregion

        Establish establish = () =>
                                  {
                                      meta = new IncodingMetaContainer();
                                      meta.onBind = "change";
                                      meta.Add(new ExecutableDirectAction(string.Empty));
                                      meta.onBind = "click";
                                      meta.Add(new ExecutableDirectAction(string.Empty));

                                      conditional = new ConditionalEval(Pleasure.Generator.TheSameString(), true);
                                  };

        Because of = () => meta.SetFilter(conditional);

        It should_be_not_set_filter_for_change = () => ((List<ExecutableBase>)meta.TryGetValue("merges"))
                                                               [0].Data.ShouldNotBeKey("filterResult");

        It should_be_set_filter_for_click = () => ((List<ExecutableBase>)meta
                                                                                 .TryGetValue("merges"))[1]
                                                          .Data["filterResult"]
                                                          .ShouldEqualWeak(conditional.GetData());
    }
}