namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingHtmlHelper))]
    public class When_incoding_html_helper_render_select_template
    {
        #region Estabilish value

        static MvcHtmlString result;

        static IncodingHtmlHelper incodingHtml;

        #endregion

        Establish establish = () => { incodingHtml = new IncodingHtmlHelper(MockHtmlHelper<object>.When().Original); };

        Because of = () => { result = incodingHtml.RenderDropDownTemplate(); };

        It should_be_template = () => result.ToHtmlString().ShouldContain(@"<script id=""incodingDropDownTemplate"" type=""text/x-mustache-tmpl"">{{#data}}{{#Title}} <optgroup label=""{{Title}}"">{{#Items}} <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>{{/Items}} </optgroup>{{/Title}}{{^Title}}{{#Items}} <option {{#Selected}}selected=""selected""{{/Selected}} value=""{{Value}}"">{{Text}}</option>{{/Items}}{{/Title}}{{/data}}</script>
");
    }
}