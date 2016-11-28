namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncDropDownControl<,>))]
    public class When_inc_drop_down_control_with_on_init : Context_inc_control
    {
        #region Establish value

        static IncDropDownControl<FakeModel, string> control;

        #endregion

        Establish establish = () =>
                                  {
                                      control = new IncDropDownControl<FakeModel, string>(mockHtmlHelper.Original, model => model.Prop);
                                      control.OnInit = dsl => dsl.Window.Alert("Test");
                                  };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result
                                            .ToString()
                                            .ShouldEqual(@"<select id=""Prop"" incoding=""[{&quot;type&quot;:&quot;ExecutableDirectAction&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:0,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableEvalMethod&quot;,&quot;data&quot;:{&quot;method&quot;:&quot;alert&quot;,&quot;args&quot;:[&quot;Test&quot;],&quot;context&quot;:&quot;window&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableDirectAction&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;change incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}}]"" name=""Prop""></select>");
    }
}