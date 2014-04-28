namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Block.Logging;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_control_with_on : Context_inc_control
    {
        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .Hidden(control =>
                                                 {
                                                     control.OnChange = dsl => dsl.Utilities.Window.Console.Log(LogType.Info, "OnChange");
                                                     control.OnEvent = dsl => dsl.Utilities.Window.Console.Log(LogType.Info, "OnEvent");
                                                     control.OnInit = dsl => dsl.Utilities.Window.Console.Log(LogType.Info, "OnInit");
                                                 });
                         };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual(@"<input id=""Prop"" incoding=""[{&quot;type&quot;:&quot;ExecutableDirectAction&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:0,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableEvalMethod&quot;,&quot;data&quot;:{&quot;method&quot;:&quot;log&quot;,&quot;args&quot;:[&quot;Info&quot;,&quot;OnInit&quot;],&quot;context&quot;:&quot;window.console&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableEvalMethod&quot;,&quot;data&quot;:{&quot;method&quot;:&quot;log&quot;,&quot;args&quot;:[&quot;Info&quot;,&quot;OnEvent&quot;],&quot;context&quot;:&quot;window.console&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableDirectAction&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;change incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableEvalMethod&quot;,&quot;data&quot;:{&quot;method&quot;:&quot;log&quot;,&quot;args&quot;:[&quot;Info&quot;,&quot;OnChange&quot;],&quot;context&quot;:&quot;window.console&quot;,&quot;onBind&quot;:&quot;change incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableEvalMethod&quot;,&quot;data&quot;:{&quot;method&quot;:&quot;log&quot;,&quot;args&quot;:[&quot;Info&quot;,&quot;OnEvent&quot;],&quot;context&quot;:&quot;window.console&quot;,&quot;onBind&quot;:&quot;change incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}}]"" name=""Prop"" type=""hidden"" value=""TheSameString"" />");
    }
}