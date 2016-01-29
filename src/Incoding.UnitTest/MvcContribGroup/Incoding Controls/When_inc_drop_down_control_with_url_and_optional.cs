namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncDropDownControl<,>))]
    public class When_inc_drop_down_control_with_url_and_optional : Context_inc_control
    {
        #region Establish value

        static IncDropDownControl<FakeModel, string> control;

        #endregion

        Establish establish = () =>
                                  {
                                      control = new IncDropDownControl<FakeModel, string>(mockHtmlHelper.Original, model => model.Prop);
                                      control.Data = "Url";
                                      control.Data.Optional = new List<KeyValueVm>
                                                             {
                                                                     new KeyValueVm("All")
                                                             };
                                  };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual(@"<select id=""Prop"" incoding=""[{&quot;type&quot;:&quot;ExecutableAjaxAction&quot;,&quot;data&quot;:{&quot;ajax&quot;:{&quot;url&quot;:&quot;Url&quot;,&quot;type&quot;:&quot;GET&quot;},&quot;hash&quot;:false,&quot;prefix&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:0,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableInsert&quot;,&quot;data&quot;:{&quot;template&quot;:&quot;$(&#39;#incodingDropDownTemplate&#39;)&quot;,&quot;result&quot;:&quot;||result*||&quot;,&quot;insertType&quot;:&quot;html&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:&quot;$(this.self)&quot;,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableInsert&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&lt;option value=\&quot;All\&quot;>All&lt;/option>&quot;,&quot;insertType&quot;:&quot;prepend&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:&quot;$(this.self)&quot;,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableEval&quot;,&quot;data&quot;:{&quot;code&quot;:&quot;$(this.target).val(\&quot;TheSameString\&quot;);&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:&quot;$(this.self)&quot;,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableDirectAction&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;change incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}}]"" name=""Prop""></select>");
    }
}