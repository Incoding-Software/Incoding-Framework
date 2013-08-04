namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncDropDownControl<,>))]
    public class When_inc_drop_down_control_with_optional : Context_inc_control
    {
        #region Estabilish value

        static IncDropDownControl<FakeModel, string> control;

        static string result;

        #endregion

        Establish establish = () =>
                                  {
                                      control = new IncDropDownControl<FakeModel, string>(mockHtmlHelper.Original, model => model.Prop);
                                      control.Url = "Url";
                                      control.Optional = "All";
                                  };

        Because of = () => { result = control.Render().ToHtmlString(); };

        It should_be_render = () => result.ShouldEqual(@"<select id=""Prop"" incoding=""[{&quot;type&quot;:&quot;ExecutableAjaxAction&quot;,&quot;data&quot;:{&quot;ajax&quot;:{&quot;url&quot;:&quot;Url&quot;,&quot;type&quot;:&quot;GET&quot;},&quot;hash&quot;:false,&quot;prefix&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:0,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableInsert&quot;,&quot;data&quot;:{&quot;template&quot;:&quot;$(&#39;#incodingDropDownTemplate&#39;)&quot;,&quot;insertType&quot;:&quot;html&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:&quot;$(this.self)&quot;,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableEval&quot;,&quot;data&quot;:{&quot;code&quot;:&quot;$(this.target).prepend(this.tryGetVal(&#39;&lt;option value=\&quot;\&quot;>All&lt;/option>&#39;).toString());&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:&quot;$(this.self)&quot;,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableEval&quot;,&quot;data&quot;:{&quot;code&quot;:&quot;$(this.target).val(\&quot;TheSameString\&quot;);&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:&quot;$(this.self)&quot;,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableTrigger&quot;,&quot;data&quot;:{&quot;trigger&quot;:&quot;change&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:&quot;$(this.self)&quot;,&quot;onEventStatus&quot;:1}}]"" name=""Prop""><option value="""">All</option>
</select>");
    }
}