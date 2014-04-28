namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaContainer))]
    public class When_incoding_meta_container_add
    {
        #region Establish value

        static IncodingMetaContainer meta;

        #endregion

        Establish establish = () =>
                                  {
                                      meta = new IncodingMetaContainer();
                                      meta.OnBind = Pleasure.Generator.String();
                                      meta.OnEventStatus = Pleasure.Generator.Enum<IncodingEventCanceled>();
                                      meta.OnCurrentStatus = Pleasure.Generator.Enum<IncodingCallbackStatus>();
                                      meta.Target = Selector.Jquery.Self();
                                  };

        Because of = () => meta.Add(new ExecutableBreak());

        It should_be_null_target = () => meta.Target.ShouldBeNull();

        It should_be_have_action = () => ((List<ExecutableBase>)meta.TryGetValue("merges"))
                                                 .OfType<ExecutableBreak>()
                                                 .FirstOrDefault()
                                                 .Should(objects =>
                                                             {
                                                                 objects.ShouldBeKeyValue("onBind", meta.OnBind);
                                                                 objects.ShouldBeKeyValue("onEventStatus", (int)meta.OnEventStatus);
                                                                 objects.ShouldBeKeyValue("onStatus", (int)meta.OnCurrentStatus);
                                                                 objects.ShouldBeKeyValue("target", Selector.Jquery.Self().ToString());
                                                             });
    }
}