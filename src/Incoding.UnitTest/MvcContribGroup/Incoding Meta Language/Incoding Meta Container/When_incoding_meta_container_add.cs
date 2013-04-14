namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Extensions;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncodingMetaContainer))]
    public class When_incoding_meta_container_add
    {
        #region Estabilish value

        static IncodingMetaContainer meta;

        #endregion

        Establish establish = () =>
                                  {
                                      meta = new IncodingMetaContainer();
                                      meta.onBind = Pleasure.Generator.String();
                                      meta.onEventStatus = Pleasure.Generator.Enum<IncodingEventCanceled>();
                                      meta.onCurrentStatus = Pleasure.Generator.Enum<IncodingCallbackStatus>();
                                      meta.target = Pleasure.Generator.TheSameString();
                                  };

        Because of = () => meta.Add(new ExecutableBreak());

        It should_be_have_action = () => ((List<ExecutableBase>)meta.TryGetValue("merges"))
                                                 .OfType<ExecutableBreak>()
                                                 .FirstOrDefault()
                                                 .Data
                                                 .Should(objects =>
                                                             {
                                                                 objects.ShouldBeKeyValue("onBind", meta.onBind);
                                                                 objects.ShouldBeKeyValue("onEventStatus", (int)meta.onEventStatus);
                                                                 objects.ShouldBeKeyValue("onStatus", (int)meta.onCurrentStatus);
                                                                 objects.ShouldBeKeyValue("target", meta.target);
                                                             });
    }
}