namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Web.WebPages;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaCallbackJqueryDsl))]
    public class When_incoding_meta_language_dsl_jquery
    {
        #region Establish value

        static Selector value = Selector.Value("Value");

        #endregion

        #region Attributes

        It should_be_set_attr = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                              
                                              .OnSuccess(dsl => dsl.Self().JQuery.Attr.SetAttr(HtmlAttribute.Href, value))
                                              .GetExecutable<ExecutableEvalMethod>()
                                              .ShouldEqualData(new Dictionary<string, object>
                                                               {
                                                                       { "method", "attr" }, 
                                                                       { "args", new[] { "href", "Value" } }, 
                                                                       { "context", "$(this.target)" }
                                                               });

        It should_be_set_attr_without_value = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                                            
                                                            .OnSuccess(dsl => dsl.Self().JQuery.Attr.SetAttr(HtmlAttribute.Href))
                                                            .GetExecutable<ExecutableEvalMethod>()
                                                            .ShouldEqualData(new Dictionary<string, object>
                                                                             {
                                                                                     { "method", "attr" }, 
                                                                                     { "args", new[] { "href", "href" } }, 
                                                                                     { "context", "$(this.target)" }
                                                                             });

        It should_be_remove_attr = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                                 
                                                 .OnSuccess(dsl => dsl.Self().JQuery.Attr.Remove(HtmlAttribute.Href | HtmlAttribute.Accept))
                                                 .GetExecutable<ExecutableEvalMethod>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "method", "removeAttr" }, 
                                                                          { "args", new[] { "accept href" } }, 
                                                                          { "context", "$(this.target)" }
                                                                  });


        It should_be_remove_single = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                                 
                                                 .OnSuccess(dsl => dsl.Self().JQuery.Attr.Remove(HtmlAttribute.Disabled))
                                                 .GetExecutable<ExecutableEvalMethod>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "method", "removeAttr" }, 
                                                                          { "args", new[] { "disabled" } }, 
                                                                          { "context", "$(this.target)" }
                                                                  });

        It should_be_remove_prop = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                                 
                                                 .OnSuccess(dsl => dsl.Self().JQuery.Attr.RemoveProp(HtmlAttribute.Href))
                                                 .GetExecutable<ExecutableEvalMethod>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "method", "removeProp" }, 
                                                                          { "args", new[] { "href" } }, 
                                                                          { "context", "$(this.target)" }
                                                                  });

        It should_be_set_prop = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                              
                                              .OnSuccess(dsl => dsl.Self().JQuery.Attr.SetProp(HtmlAttribute.Href, value))
                                              .GetExecutable<ExecutableEvalMethod>()
                                              .ShouldEqualData(new Dictionary<string, object>
                                                               {
                                                                       { "method", "prop" }, 
                                                                       { "args", new[] { "href", "Value" } }, 
                                                                       { "context", "$(this.target)" }
                                                               });

        It should_be_set_prop_without_value = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                            
                                                            .OnSuccess(dsl => dsl.Self().JQuery.Attr.SetProp(HtmlAttribute.Href))
                                                            .GetExecutable<ExecutableEvalMethod>()
                                                            .ShouldEqualData(new Dictionary<string, object>
                                                                             {
                                                                                     { "method", "prop" }, 
                                                                                     { "args", new[] { "href", "href" } }, 
                                                                                     { "context", "$(this.target)" }
                                                                             });

        It should_be_set_prop_with_empty_value = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                               
                                                               .OnSuccess(dsl => dsl.Self().JQuery.Attr.SetProp(HtmlAttribute.Href, string.Empty))
                                                               .GetExecutable<ExecutableEvalMethod>()
                                                               .ShouldEqualData(new Dictionary<string, object>
                                                                                {
                                                                                        { "method", "prop" }, 
                                                                                        { "args", new[] { "href", string.Empty } }, 
                                                                                        { "context", "$(this.target)" }
                                                                                });

        It should_be_remove_class = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  .OnSuccess(r => r.Self().JQuery.Attr.RemoveClass("class"))
                                                  .GetExecutable<ExecutableJquery>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "method", 2 },
                                                                           { "args", new[] { "class" } }
                                                                   });

        It should_be_remove_class_by_bootstrap = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                               .OnSuccess(r => r.Self().JQuery.Attr.RemoveClass(B.Active))
                                                               .GetExecutable<ExecutableJquery>()
                                                               .ShouldEqualData(new Dictionary<string, object>
                                                                                {
                                                                                        { "method", 2 },
                                                                                        { "args", new[] { "active" } }
                                                                                });

        It should_be_remove_class_by_bootstrap_multiple = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                        .OnSuccess(r => r.Self().JQuery.Attr.RemoveClass(B.Active | B.Disabled))
                                                                        .GetExecutable<ExecutableJquery>()
                                                                        .ShouldEqualData(new Dictionary<string, object>
                                                                                         {
                                                                                                 { "method", 2 },
                                                                                                 { "args", new[] { "active disabled" } }
                                                                                         });
                                                                     
        It should_be_toggle_class = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  
                                                  .OnSuccess(r => r.Self().JQuery.Attr.ToggleClass("class"))
                                                  .GetExecutable<ExecutableEvalMethod>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "method", "toggleClass" }, 
                                                                           { "args", new[] { "class" } }, 
                                                                           { "context", "$(this.target)" }
                                                                   });

        It should_be_toggle_class_by_bootstrap_multiple = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                                                        
                                                                        .OnSuccess(r => r.Self().JQuery.Attr.ToggleClass(B.Active | B.Disabled))
                                                                        .GetExecutable<ExecutableEvalMethod>()
                                                                        .ShouldEqualData(new Dictionary<string, object>
                                                                                         {
                                                                                                 { "method", "toggleClass" }, 
                                                                                                 { "context", "$(this.target)" }, 
                                                                                                 { "args", new[] { "active disabled" } }
                                                                                         });

        It should_be_toggle_disabled = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                                     
                                                     .OnSuccess(r => r.Self().JQuery.Attr.ToggleDisabled())
                                                     .GetExecutable<ExecutableEvalMethod>()
                                                     .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "method", "toggleProp" }, 
                                                                              { "args", new[] { "disabled" } }, 
                                                                              { "context", "$(this.target)" }
                                                                      });

        It should_be_toggle_readonly = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                     
                                                     .OnSuccess(r => r.Self().JQuery.Attr.ToggleReadonly())
                                                     .GetExecutable<ExecutableEvalMethod>()
                                                     .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "method", "toggleProp" }, 
                                                                              { "args", new[] { "readonly" } }, 
                                                                              { "context", "$(this.target)" }
                                                                      });

        It should_be_toggle_checked = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                                    
                                                    .OnSuccess(r => r.Self().JQuery.Attr.ToggleChecked())
                                                    .GetExecutable<ExecutableEvalMethod>()
                                                    .ShouldEqualData(new Dictionary<string, object>
                                                                     {
                                                                             { "method", "toggleProp" }, 
                                                                             { "args", new[] { "checked" } }, 
                                                                             { "context", "$(this.target)" }
                                                                     });

        It should_be_add_class = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                               
                                               .OnSuccess(r => r.Self().JQuery.Attr.AddClass("class"))
                                               .GetExecutable<ExecutableJquery>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                {
                                                                        { "method", 1 },
                                                                        { "args", new[] { "class" } }
                                                                });

        It should_be_add_class_by_bootstrap = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                                            
                                                            .OnSuccess(r => r.Self().JQuery.Attr.AddClass(B.Active))
                                                            .GetExecutable<ExecutableJquery>()
                                                            .ShouldEqualData(new Dictionary<string, object>
                                                                             {
                                                                                     { "method", 1 },
                                                                                     { "args", new[] { "active" } }
                                                                             });

        It should_be_add_class_by_bootstrap_multiple = () => new IncodingMetaLanguageDsl(JqueryBind.Click)                                                                     
                                                                     .OnSuccess(r => r.Self().JQuery.Attr.AddClass(B.Active | B.Disabled))
                                                                     .GetExecutable<ExecutableJquery>()
                                                                     .ShouldEqualData(new Dictionary<string, object>
                                                                                      {
                                                                                              { "method", 1 },
                                                                                              { "args", new[] { "active disabled" } }
                                                                                      });

        It should_be_val_string = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                
                                                .OnSuccess(r => r.Self().JQuery.Attr.Val("aws"))
                                                .GetExecutable<ExecutableEval>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "code", "$(this.target).val(\"aws\");" }, 
                                                                 });

        It should_be_val_null = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                              
                                              .OnSuccess(r => r.Self().JQuery.Attr.Val(null))
                                              .GetExecutable<ExecutableEvalMethod>()
                                              .ShouldEqualData(new Dictionary<string, object>
                                                               {
                                                                       { "method", "val" }, 
                                                                       { "args", new[] { string.Empty } }, 
                                                                       { "context", "$(this.target)" }
                                                               });

        It should_be_val_selector = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  
                                                  .OnSuccess(r => r.Self().JQuery.Attr.Val(Selector.Jquery.Class("class")))
                                                  .GetExecutable<ExecutableEvalMethod>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "method", "val" }, 
                                                                           { "args", new[] { "$('.class')" } }, 
                                                                           { "context", "$(this.target)" }
                                                                   });

        It should_be_val_selector_by_func = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                          
                                                          .OnSuccess(r => r.Self().JQuery.Attr.Val(selector => selector.Class("class")))
                                                          .GetExecutable<ExecutableEvalMethod>()
                                                          .ShouldEqualData(new Dictionary<string, object>
                                                                           {
                                                                                   { "method", "val" }, 
                                                                                   { "args", new[] { "$('.class')" } }, 
                                                                                   { "context", "$(this.target)" }
                                                                           });

        It should_be_val_array = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               
                                               .OnSuccess(r => r.Self().JQuery.Attr.Val(Pleasure.ToArray("aws", "aws1")))
                                               .GetExecutable<ExecutableEval>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                {
                                                                        { "code", "$(this.target).val([\"aws\",\"aws1\"]);" }
                                                                });

        #endregion

        #region Css

        It should_be_css_set = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                             
                                             .OnSuccess(dsl => dsl.Self().JQuery.Css.Set(CssStyling.FontFamily, value))
                                             .GetExecutable<ExecutableEvalMethod>()
                                             .ShouldEqualData(new Dictionary<string, object>
                                                              {
                                                                      { "method", "css" }, 
                                                                      { "args", new[] { "font-family", "Value" } }, 
                                                                      { "context", "$(this.target)" }
                                                              });

        It should_be_css_set_empty = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                   
                                                   .OnSuccess(dsl => dsl.Self().JQuery.Css.Set(CssStyling.Width, string.Empty))
                                                   .GetExecutable<ExecutableEvalMethod>()
                                                   .ShouldEqualData(new Dictionary<string, object>
                                                                    {
                                                                            { "method", "css" }, 
                                                                            { "args", new[] { "width", string.Empty } }, 
                                                                            { "context", "$(this.target)" }
                                                                    });

        It should_be_display = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                             
                                             .OnSuccess(dsl => dsl.Self().JQuery.Css.Display(Display.TableCaption))
                                             .GetExecutable<ExecutableEvalMethod>()
                                             .ShouldEqualData(new Dictionary<string, object>
                                                              {
                                                                      { "method", "css" }, 
                                                                      { "args", new[] { "display", "table-caption" } }, 
                                                                      { "context", "$(this.target)" }
                                                              });

        It should_be_height = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            
                                            .OnSuccess(dsl => dsl.Self().JQuery.Css.Height(10))
                                            .GetExecutable<ExecutableEvalMethod>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                             {
                                                                     { "method", "height" }, 
                                                                     { "args", new[] { "10" } }, 
                                                                     { "context", "$(this.target)" }
                                                             });

        It should_be_scroll_left = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 
                                                 .OnSuccess(dsl => dsl.Self().JQuery.Css.ScrollLeft(25))
                                                 .GetExecutable<ExecutableEvalMethod>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "method", "scrollLeft" }, 
                                                                          { "args", new[] { "25" } }, 
                                                                          { "context", "$(this.target)" }
                                                                  });

        It should_be_scroll_top = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                
                                                .OnSuccess(dsl => dsl.Self().JQuery.Css.ScrollTop(25))
                                                .GetExecutable<ExecutableEvalMethod>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "method", "scrollTop" }, 
                                                                         { "args", new[] { "25" } }, 
                                                                         { "context", "$(this.target)" }
                                                                 });

        It should_be_width = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           
                                           .OnSuccess(dsl => dsl.Self().JQuery.Css.Width(25))
                                           .GetExecutable<ExecutableEvalMethod>()
                                           .ShouldEqualData(new Dictionary<string, object>
                                                            {
                                                                    { "method", "width" }, 
                                                                    { "args", new[] { "25" } }, 
                                                                    { "context", "$(this.target)" }
                                                            });

        #endregion

        #region Func

        It should_be_increment_with_selector = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                             
                                                             .OnSuccess(r => r.Self().Func.IncrementVal(Selector.Jquery.Self()))
                                                             .GetExecutable<ExecutableEvalMethod>()
                                                             .ShouldEqualData(new Dictionary<string, object>
                                                                              {
                                                                                      { "method", "increment" }, 
                                                                                      { "context", "$(this.target)" }, 
                                                                                      { "args", new[] { "$(this.self)" } }, 
                                                                              });

        It should_be_increment = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               
                                               .OnSuccess(r => r.Self().Func.IncrementVal())
                                               .GetExecutable<ExecutableEvalMethod>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                {
                                                                        { "method", "increment" }, 
                                                                        { "context", "$(this.target)" }, 
                                                                        { "args", new[] { "1" } }, 
                                                                });

        It should_be_decrement = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               
                                               .OnSuccess(r => r.Self().Func.DecrementVal())
                                               .GetExecutable<ExecutableEvalMethod>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                {
                                                                        { "method", "increment" }, 
                                                                        { "context", "$(this.target)" }, 
                                                                        { "args", new[] { "-1" } }, 
                                                                });

        It should_be_jquery_plug_in = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                    
                                                    .OnSuccess(r => r.Self().JQuery.PlugIn("MyPlugIn", new { position = "left", width = 15 }))
                                                    .GetExecutable<ExecutableEval>()
                                                    .ShouldEqualData(new Dictionary<string, object>
                                                                     {
                                                                             { "code", @"$(this.target).MyPlugIn({""position"":""left"",""width"":15});" }
                                                                     });

        It should_be_jquery_call = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 
                                                 .OnSuccess(r => r.Self().JQuery.Call("Call", "15"))
                                                 .GetExecutable<ExecutableEvalMethod>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "method", @"Call" }, 
                                                                          { "args", new[] { "15" } }, 
                                                                          { "context", "$(this.target)" }
                                                                  });

        #endregion

        #region Manipilation

        It should_be_remove = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            
                                            .OnSuccess(r => r.Self().JQuery.Manipulation.Remove())
                                            .GetExecutable<ExecutableEvalMethod>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                             {
                                                                     { "method", "remove" }, 
                                                                     { "args", new string[] { } }, 
                                                                     { "context", "$(this.target)" }
                                                             });

        It should_be_empty = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           
                                           .OnSuccess(r => r.Self().JQuery.Manipulation.Empty())
                                           .GetExecutable<ExecutableEvalMethod>()
                                           .ShouldEqualData(new Dictionary<string, object>
                                                            {
                                                                    { "method", "empty" }, 
                                                                    { "args", new string[] { } }, 
                                                                    { "context", "$(this.target)" }
                                                            });

        It should_be_detach = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            
                                            .OnSuccess(r => r.Self().JQuery.Manipulation.Detach())
                                            .GetExecutable<ExecutableEvalMethod>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                             {
                                                                     { "method", "detach" }, 
                                                                     { "args", new string[] { } }, 
                                                                     { "context", "$(this.target)" }
                                                             });

        It should_be_wrap = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                          
                                          .OnSuccess(r => r.Self().JQuery.Manipulation.Wrap(o => new HelperResult(writer => writer.WriteLine("<div>"))))
                                          .GetExecutable<ExecutableEvalMethod>()
                                          .ShouldEqualData(new Dictionary<string, object>
                                                           {
                                                                   { "method", "wrap" }, 
                                                                   { "args", new[] { "<div>" } }, 
                                                                   { "context", "$(this.target)" }
                                                           });

        It should_be_wrap_all = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                              
                                              .OnSuccess(r => r.Self().JQuery.Manipulation.WrapAll(o => new HelperResult(writer => writer.WriteLine("<div>"))))
                                              .GetExecutable<ExecutableEvalMethod>()
                                              .ShouldEqualData(new Dictionary<string, object>
                                                               {
                                                                       { "method", "wrapAll" }, 
                                                                       { "args", new[] { "<div>" } }, 
                                                                       { "context", "$(this.target)" }
                                                               });

        It should_be_text_empty = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                
                                                .OnSuccess(r => r.Self().JQuery.Manipulation.Text(string.Empty))
                                                .GetExecutable<ExecutableEvalMethod>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "method", "text" }, 
                                                                         { "args", new[] { string.Empty } }, 
                                                                         { "context", "$(this.target)" }
                                                                 });

        It should_be_text = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                          
                                          .OnSuccess(r => r.Self().JQuery.Manipulation.Text(o => new HelperResult(writer => writer.WriteLine("<div>"))))
                                          .GetExecutable<ExecutableEvalMethod>()
                                          .ShouldEqualData(new Dictionary<string, object>
                                                           {
                                                                   { "method", "text" }, 
                                                                   { "args", new[] { "<div>" } }, 
                                                                   { "context", "$(this.target)" }
                                                           });

        It should_be_append = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            
                                            .OnSuccess(r => r.Self().JQuery.Manipulation.Append(o => new HelperResult(writer => writer.WriteLine("<div>"))))
                                            .GetExecutable<ExecutableEvalMethod>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                             {
                                                                     { "method", "append" }, 
                                                                     { "args", new[] { "<div>" } }, 
                                                                     { "context", "$(this.target)" }
                                                             });

        It should_be_append_selector = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                     
                                                     .OnSuccess(r => r.Self().JQuery.Manipulation.Append(Selector.JS.Location.Href))
                                                     .GetExecutable<ExecutableEvalMethod>()
                                                     .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "method", "append" }, 
                                                                              { "args", new[] { "||javascript*window.location.href||" } }, 
                                                                              { "context", "$(this.target)" }
                                                                      });

        It should_be_prepend = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                             
                                             .OnSuccess(r => r.Self().JQuery.Manipulation.Prepend(o => new HelperResult(writer => writer.Write("<div>"))))
                                             .GetExecutable<ExecutableEvalMethod>()
                                             .ShouldEqualData(new Dictionary<string, object>
                                                              {
                                                                      { "method", "prepend" }, 
                                                                      { "args", new[] { "<div>" } }, 
                                                                      { "context", "$(this.target)" }
                                                              });

        It should_be_after = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                           
                                           .OnSuccess(r => r.Self().JQuery.Manipulation.After(o => new HelperResult(writer => writer.Write("<div>"))))
                                           .GetExecutable<ExecutableEvalMethod>()
                                           .ShouldEqualData(new Dictionary<string, object>
                                                            {
                                                                    { "method", "after" }, 
                                                                    { "args", new[] { "<div>" } }, 
                                                                    { "context", "$(this.target)" }
                                                            });

        It should_be_before = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                            
                                            .OnSuccess(r => r.Self().JQuery.Manipulation.Before(o => new HelperResult(writer => writer.Write("<div>"))))
                                            .GetExecutable<ExecutableEvalMethod>()
                                            .ShouldEqualData(new Dictionary<string, object>
                                                             {
                                                                     { "method", "before" }, 
                                                                     { "args", new[] { "<div>" } }, 
                                                                     { "context", "$(this.target)" }
                                                             });

        It should_be_html_string = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 
                                                 .OnSuccess(r => r.Self().JQuery.Manipulation.Html(o => new HelperResult(writer => writer.Write("<div>"))))
                                                 .GetExecutable<ExecutableEvalMethod>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "method", "html" }, 
                                                                          { "args", new[] { "<div>" } }, 
                                                                          { "context", "$(this.target)" }
                                                                  });

        It should_be_html_jquery_selector = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                          
                                                          .OnSuccess(r => r.Self().JQuery.Manipulation.Html(Selector.Jquery.Id("id")))
                                                          .GetExecutable<ExecutableEvalMethod>()
                                                          .ShouldEqualData(new Dictionary<string, object>
                                                                           {
                                                                                   { "method", "html" }, 
                                                                                   { "args", new[] { "$('#id')" } }, 
                                                                                   { "context", "$(this.target)" }
                                                                           });

        It should_be_replace_with = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  
                                                  .OnSuccess(r => r.Self().JQuery.Manipulation.ReplaceWith(o => new HelperResult(writer => writer.Write("<div>"))))
                                                  .GetExecutable<ExecutableEvalMethod>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "method", "replaceWith" }, 
                                                                           { "args", new[] { "<div>" } }, 
                                                                           { "context", "$(this.target)" }
                                                                   });

        #endregion
    }
}