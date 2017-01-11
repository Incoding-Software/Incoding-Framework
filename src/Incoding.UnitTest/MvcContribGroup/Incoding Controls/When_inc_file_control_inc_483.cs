namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_file_control_inc_483 : Context_inc_control
    {
        #region Establish value

        static string result;

        #endregion

        Because of = () =>
                     {
                         result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                 .File(boxControl =>
                                       {
                                           boxControl.Value = Pleasure.Generator.TheSameString();
                                           boxControl.OnChange = dsl => dsl.Document.Reload();
                                       })
                                 .ToHtmlString();
                     };

        It should_be_render = () => result.ShouldEqual("<input id=\"Prop\" incoding=\"[{&quot;type&quot;:&quot;ExecutableDirectAction&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:0,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableDirectAction&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;change incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableEval&quot;,&quot;data&quot;:{&quot;code&quot;:&quot;window.location.reload(false);&quot;,&quot;onBind&quot;:&quot;change incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}}]\" name=\"Prop\" type=\"file\" value=\"TheSameString\" />");
    }
}