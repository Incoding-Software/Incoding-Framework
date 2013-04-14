namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.WebPages;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(IncodingMetaCallbackJqueryDsl))]
    public class When_incoding_meta_language_dsl_jquery
    {
        #region Estabilish value

        static Selector value = Selector.Value("Value");

        #endregion

        #region Attributes

        It should_be_set_attr_as_route_value = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                             .Do().Direct()
                                                             .OnSuccess(dsl => dsl.Self().Core().JQuery.Attributes.SetAttr(new
                                                                                                                               {
                                                                                                                                       href = value, 
                                                                                                                                       @class = "class"
                                                                                                                               }))
                                                             .GetExecutable<ExecutableEval>()
                                                             .ShouldEqualData(new Dictionary<string, object>
                                                                                  {
                                                                                          { "code", "$(this.target).attr('href', this.tryGetVal('Value'));$(this.target).attr('class', this.tryGetVal('class'));" }
                                                                                  });

        It should_be_set_attr = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                              .Do().Direct()
                                              .OnSuccess(dsl => dsl.Self().Core().JQuery.Attributes.SetAttr(HtmlAttribute.Href, value))
                                              .GetExecutable<ExecutableEval>()
                                              .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "code", "$(this.target).attr('href', this.tryGetVal('Value'));" }
                                                                   });

        It should_be_set_attr_without_value = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                            .Do().Direct()
                                                            .OnSuccess(dsl => dsl.Self().Core().JQuery.Attributes.SetAttr(HtmlAttribute.Href))
                                                            .GetExecutable<ExecutableEval>()
                                                            .ShouldEqualData(new Dictionary<string, object>
                                                                                 {
                                                                                         { "code", "$(this.target).attr('href', this.tryGetVal('href'));" }
                                                                                 });

        It should_be_remove_attr = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(dsl => dsl.Self().Core().JQuery.Attributes.RemoveAttr(HtmlAttribute.Href | HtmlAttribute.Accept))
                                                 .GetExecutable<ExecutableEval>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "code", "$(this.target).removeAttr('accept href');" }
                                                                      });

        It should_be_remove_prop = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(dsl => dsl.Self().Core().JQuery.Attributes.RemoveProp(HtmlAttribute.Href))
                                                 .GetExecutable<ExecutableEval>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "code", "$(this.target).removeProp('href');" }
                                                                      });

        It should_be_set_prop_as_route_value = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                             .Do().Direct()
                                                             .OnSuccess(dsl => dsl.Self().Core().JQuery.Attributes.SetProp(new
                                                                                                                               {
                                                                                                                                       href = value, 
                                                                                                                                       @class = "class"
                                                                                                                               }))
                                                             .GetExecutable<ExecutableEval>()
                                                             .ShouldEqualData(new Dictionary<string, object>
                                                                                  {
                                                                                          { "code", "$(this.target).prop('href', this.tryGetVal('Value'));$(this.target).prop('class', this.tryGetVal('class'));" }
                                                                                  });

        It should_be_set_prop = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                              .Do().Direct()
                                              .OnSuccess(dsl => dsl.Self().Core().JQuery.Attributes.SetProp(HtmlAttribute.Href, value))
                                              .GetExecutable<ExecutableEval>()
                                              .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "code", "$(this.target).prop('href', this.tryGetVal('Value'));" }
                                                                   });

        It should_be_set_prop_without_value = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                            .Do().Direct()
                                                            .OnSuccess(dsl => dsl.Self().Core().JQuery.Attributes.SetProp(HtmlAttribute.Href))
                                                            .GetExecutable<ExecutableEval>()
                                                            .ShouldEqualData(new Dictionary<string, object>
                                                                                 {
                                                                                         { "code", "$(this.target).prop('href', this.tryGetVal('href'));" }
                                                                                 });

        It should_be_set_prop_with_empty_value = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                               .Do().Direct()
                                                               .OnSuccess(dsl => dsl.Self().Core().JQuery.Attributes.SetProp(HtmlAttribute.Href, string.Empty))
                                                               .GetExecutable<ExecutableEval>()
                                                               .ShouldEqualData(new Dictionary<string, object>
                                                                                    {
                                                                                            { "code", "$(this.target).prop('href', this.tryGetVal(''));" }
                                                                                    });

        It should_be_remove_class = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  .Do().Direct()
                                                  .OnSuccess(r => r.Self().Core().JQuery.Attributes.RemoveClass("class"))
                                                  .GetExecutable<ExecutableEval>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                       {
                                                                               { "code", "$(this.target).removeClass('class');" }
                                                                       });

        It should_be_toggle_class = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  .Do().Direct()
                                                  .OnSuccess(r => r.Self().Core().JQuery.Attributes.ToggleClass("class"))
                                                  .GetExecutable<ExecutableEval>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                       {
                                                                               { "code", "$(this.target).toggleClass('class');" }
                                                                       });

        It should_be_toggle_disabled = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                     .Do().Direct()
                                                     .OnSuccess(r => r.Self().Core().JQuery.Attributes.ToggleDisabled())
                                                     .GetExecutable<ExecutableEval>()
                                                     .ShouldEqualData(new Dictionary<string, object>
                                                                          {
                                                                                  { "code", "$(this.target).toggleProp('disabled');" }
                                                                          });

        It should_be_toggle_readonly = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                     .Do().Direct()
                                                     .OnSuccess(r => r.Self().Core().JQuery.Attributes.ToggleReadonly())
                                                     .GetExecutable<ExecutableEval>()
                                                     .ShouldEqualData(new Dictionary<string, object>
                                                                          {
                                                                                  { "code", "$(this.target).toggleProp('readonly');" }
                                                                          });

        It should_be_toggle_checked = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                    .Do().Direct()
                                                    .OnSuccess(r => r.Self().Core().JQuery.Attributes.ToggleChecked())
                                                    .GetExecutable<ExecutableEval>()
                                                    .ShouldEqualData(new Dictionary<string, object>
                                                                         {
                                                                                 { "code", "$(this.target).toggleProp('checked');" }
                                                                         });

        It should_be_add_class = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               .Do().Direct()
                                               .OnSuccess(r => r.Self().Core().JQuery.Attributes.AddClass("class"))
                                               .GetExecutable<ExecutableEval>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                    {
                                                                            { "code", "$(this.target).addClass('class');" }
                                                                    });

        It should_be_val_string_to_self = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                        .Do().Direct()
                                                        .OnSuccess(r => r.Self().Core().JQuery.Attributes.Val("aws"))
                                                        .GetExecutable<ExecutableEval>()
                                                        .ShouldEqualData(new Dictionary<string, object>
                                                                             {
                                                                                     { "code", "$(this.target).val(\"aws\");" }
                                                                             });

        It should_be_val_string = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                .Do().Direct()
                                                .OnSuccess(r => r.Self().Core().JQuery.Attributes.Val("aws"))
                                                .GetExecutable<ExecutableEval>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                     {
                                                                             { "code", "$(this.target).val(\"aws\");" }
                                                                     });

        It should_be_val_from_selector = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                       .Do().Direct()
                                                       .OnSuccess(r => r.Self().Core().JQuery.Attributes.Val(Selector.Jquery.Class("class")))
                                                       .GetExecutable<ExecutableEval>()
                                                       .ShouldEqualData(new Dictionary<string, object>
                                                                            {
                                                                                    { "code", "$(this.target).val(this.tryGetVal($('.class')));" }
                                                                            });

        It should_be_val_array = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               .Do().Direct()
                                               .OnSuccess(r => r.Self().Core().JQuery.Attributes.Val(Pleasure.ToArray("aws", "aws1")))
                                               .GetExecutable<ExecutableEval>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                    {
                                                                            { "code", "$(this.target).val([\"aws\",\"aws1\"]);" }
                                                                    });

        #endregion

        #region Css

        It should_be_set_css = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                             .Do().Direct()
                                             .OnSuccess(dsl => dsl.Self().Core().JQuery.Css.Set(CssStyling.Font_family, value))
                                             .GetExecutable<ExecutableEval>()
                                             .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "code", "$(this.target).css('font-family', this.tryGetVal('Value'));" }
                                                                  });

        It should_be_set_css_values = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                    .Do().Direct()
                                                    .OnSuccess(dsl => dsl.Self().Core().JQuery.Css.Set(new
                                                                                                           {
                                                                                                                   width = "10px", 
                                                                                                                   height = "15px"
                                                                                                           }))
                                                    .GetExecutable<ExecutableEval>()
                                                    .ShouldEqualData(new Dictionary<string, object>
                                                                         {
                                                                                 { "code", "$(this.target).css('width', this.tryGetVal('10px'));$(this.target).css('height', this.tryGetVal('15px'));" }
                                                                         });

        It should_be_set_css_empty = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                   .Do().Direct()
                                                   .OnSuccess(dsl => dsl.Self().Core().JQuery.Css.Set(CssStyling.Width, string.Empty))
                                                   .GetExecutable<ExecutableEval>()
                                                   .ShouldEqualData(new Dictionary<string, object>
                                                                        {
                                                                                { "code", "$(this.target).css('width', this.tryGetVal(''));" }
                                                                        });

        It should_be_height = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            .Do().Direct()
                                            .OnSuccess(dsl => dsl.Self().Core().JQuery.Css.Height(10))
                                            .GetExecutable<ExecutableEval>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "code", "$(this.target).height(this.tryGetVal('10'));" }
                                                                 });

        It should_be_scroll_left = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(dsl => dsl.Self().Core().JQuery.Css.ScrollLeft(25))
                                                 .GetExecutable<ExecutableEval>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "code", "$(this.target).scrollLeft(this.tryGetVal('25'));" }
                                                                      });

        It should_be_scroll_top = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                .Do().Direct()
                                                .OnSuccess(dsl => dsl.Self().Core().JQuery.Css.ScrollTop(25))
                                                .GetExecutable<ExecutableEval>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                     {
                                                                             { "code", "$(this.target).scrollTop(this.tryGetVal('25'));" }
                                                                     });

        It should_be_width = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           .Do().Direct()
                                           .OnSuccess(dsl => dsl.Self().Core().JQuery.Css.Width(25))
                                           .GetExecutable<ExecutableEval>()
                                           .ShouldEqualData(new Dictionary<string, object>
                                                                {
                                                                        { "code", "$(this.target).width(this.tryGetVal('25'));" }
                                                                });

        #endregion

        #region Func

        It should_be_increment = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               .Do().Direct()
                                               .OnSuccess(r => r.Self().Core().JQuery.Func.IncrementVal())
                                               .GetExecutable<ExecutableEval>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                    {
                                                                            { "code", "$(this.target).val(parseInt($(this.target).val()) + 1);" }
                                                                    });

        It should_be_decrement = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               .Do().Direct()
                                               .OnSuccess(r => r.Self().Core().JQuery.Func.DecrementVal())
                                               .GetExecutable<ExecutableEval>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                    {
                                                                            { "code", "$(this.target).val(parseInt($(this.target).val()) - 1);" }
                                                                    });

        #endregion

        #region Manipilation

        It should_be_remove = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            .Do().Direct()
                                            .OnSuccess(r => r.Self().Core().JQuery.Manipulation.Remove())
                                            .GetExecutable<ExecutableEval>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "code", "$(this.target).remove();" }
                                                                 });

        It should_be_empty = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           .Do().Direct()
                                           .OnSuccess(r => r.Self().Core().JQuery.Manipulation.Empty())
                                           .GetExecutable<ExecutableEval>()
                                           .ShouldEqualData(new Dictionary<string, object>
                                                                {
                                                                        { "code", "$(this.target).empty();" }
                                                                });

        It should_be_detach = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            .Do().Direct()
                                            .OnSuccess(r => r.Self().Core().JQuery.Manipulation.Detach())
                                            .GetExecutable<ExecutableEval>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "code", "$(this.target).detach();" }
                                                                 });

        It should_be_wrap = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                          .Do().Direct()
                                          .OnSuccess(r => r.Self().Core().JQuery.Manipulation.Wrap(o => new HelperResult(writer => writer.WriteLine("<div>"))))
                                          .GetExecutable<ExecutableEval>()
                                          .ShouldEqualData(new Dictionary<string, object>
                                                               {
                                                                       { "code", "$(this.target).wrap(this.tryGetVal('<div>'));" }
                                                               });

        It should_be_wrap_all = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                              .Do().Direct()
                                              .OnSuccess(r => r.Self().Core().JQuery.Manipulation.WrapAll(o => new HelperResult(writer => writer.WriteLine("<div>"))))
                                              .GetExecutable<ExecutableEval>()
                                              .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "code", "$(this.target).wrapAll(this.tryGetVal('<div>'));" }
                                                                   });

        It should_be_text = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                          .Do().Direct()
                                          .OnSuccess(r => r.Self().Core().JQuery.Manipulation.Text(o => new HelperResult(writer => writer.WriteLine("<div>"))))
                                          .GetExecutable<ExecutableEval>()
                                          .ShouldEqualData(new Dictionary<string, object>
                                                               {
                                                                       { "code", "$(this.target).text(this.tryGetVal('<div>').toString());" }
                                                               });



        It should_be_append = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(r => r.Self().Core().JQuery.Manipulation.Append(o => new HelperResult(writer => writer.WriteLine("<div>"))))
                                                 .GetExecutable<ExecutableEval>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "code", "$(this.target).append(this.tryGetVal('<div>').toString());" }
                                                                      });

        It should_be_prepend = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                             .Do().Direct()
                                             .OnSuccess(r => r.Self().Core().JQuery.Manipulation.Prepend(o => new HelperResult(writer => writer.Write("<div>"))))
                                             .GetExecutable<ExecutableEval>()
                                             .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "code", "$(this.target).prepend(this.tryGetVal('<div>').toString());" }
                                                                  });

        It should_be_after = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           .Do().Direct()
                                           .OnSuccess(r => r.Self().Core().JQuery.Manipulation.After(o => new HelperResult(writer => writer.Write("<div>"))))
                                           .GetExecutable<ExecutableEval>()
                                           .ShouldEqualData(new Dictionary<string, object>
                                                                {
                                                                        { "code", "$(this.target).after(this.tryGetVal('<div>').toString());" }
                                                                });

        It should_be_before = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            .Do().Direct()
                                            .OnSuccess(r => r.Self().Core().JQuery.Manipulation.Before(o => new HelperResult(writer => writer.Write("<div>"))))
                                            .GetExecutable<ExecutableEval>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "code", "$(this.target).before(this.tryGetVal('<div>').toString());" }
                                                                 });

        It should_be_html_string = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(r => r.Self().Core().JQuery.Manipulation.Html(o => new HelperResult(writer => writer.Write("<div>"))))
                                                 .GetExecutable<ExecutableEval>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "code", "$(this.target).html(this.tryGetVal('<div>').toString());" }
                                                                      });

        It should_be_html_jquery_selector = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                          .Do().Direct()
                                                          .OnSuccess(r => r.Self().Core().JQuery.Manipulation.Html(Selector.Jquery.Id("id")))
                                                          .GetExecutable<ExecutableEval>()
                                                          .ShouldEqualData(new Dictionary<string, object>
                                                                               {
                                                                                       { "code", "$(this.target).html(this.tryGetVal($('#id')).toString());" }
                                                                               });

        It should_be_replace_with = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  .Do().Direct()
                                                  .OnSuccess(r => r.Self().Core().JQuery.Manipulation.ReplaceWith(o => new HelperResult(writer => writer.Write("<div>"))))
                                                  .GetExecutable<ExecutableEval>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                       {
                                                                               { "code", "$(this.target).replaceWith(this.tryGetVal('<div>'));" }
                                                                       });

        #endregion
    }
}