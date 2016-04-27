namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncDropDownControl<,>))]
    public class When_inc_drop_down_control : Context_inc_control
    {
        Because of = () =>
                         {
                             result = new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                     .DropDown(boxControl =>
                                                   {
                                                       boxControl.Data = new SelectList(new[] { Pleasure.Generator.TheSameString() });
                                                       boxControl.Data.Optional = new List<KeyValueVm>
                                                                                 {
                                                                                         new KeyValueVm("Optional")
                                                                                 };
                                                   });
                         };

        It should_be_render = () => result.ToHtmlString()
                                          .ShouldEqual(@"<select id=""Prop"" incoding=""[{&quot;type&quot;:&quot;ExecutableDirectAction&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:0,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}
},{&quot;type&quot;:&quot;ExecutableEval&quot;,&quot;data&quot;:{&quot;code&quot;:&quot;$(this.target).val(\&quot;TheSameString\&quot;);&quot;,&quot;onBind&quot;:&quot;initincoding incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:&quot;$(this.self)&quot;,&quot;onEventStatus&quot;:1}},{&quot;type&quot;:&quot;ExecutableDirectAction&quot;,&quot;data&quot;:{&quot;result&quot;:&quot;&quot;,&quot;onBind&quot;:&quot;change incoding&quot;,&quot;onStatus&quot;:2,&quot;target&quot;:null,&quot;onEventStatus&quot;:1}}]"" name=""Prop""><option value=""Optional""> Optional</option>
<option selected= ""selected"" value=""TheSameString"">TheSameString</option>
</select>");
    }
}