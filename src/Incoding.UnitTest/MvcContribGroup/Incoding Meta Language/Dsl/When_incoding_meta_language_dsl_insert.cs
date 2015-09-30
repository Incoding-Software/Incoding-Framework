namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaCallbackInsertDsl))]
    public class When_incoding_meta_language_dsl_insert
    {
        #region Establish value

        static readonly Func<JquerySelector, JquerySelectorExtend> targetSelector = selector => selector.Id("Selector");

        #endregion

        It should_be_exception_if_insert_not_finished_yet = () =>
                                                            {
                                                                new ExecutableInsert("", "", "", false, "")
                                                                        .GetErrors()
                                                                        .ShouldBeKeyValue("insertType", "Insert can be empty. Please choose Html/Text/Append or any type");
                                                            };

        It should_be_insert_for = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                .Do().Direct()
                                                .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.For<FakeInsertModel>(model => model.Prop1).Text())
                                                .GetExecutable<ExecutableInsert>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "insertType", "text" },
                                                                         { "result", Selector.Result.ToString() },
                                                                         { "property", "Prop1" }
                                                                 });

        It should_be_insert_on = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               .Do().Direct()
                                               .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.Use(Selector.Result.For<KeyValueVm>(r => r.Value)).Text())
                                               .GetExecutable<ExecutableInsert>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                {
                                                                        { "insertType", "text" },
                                                                        { "result", Selector.Result.For<KeyValueVm>(r => r.Value).ToString() }
                                                                });

        It should_be_insert_prepair = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                    .Do().Direct()
                                                    .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.Prepare().Text())
                                                    .GetExecutable<ExecutableInsert>()
                                                    .ShouldEqualData(new Dictionary<string, object>
                                                                     {
                                                                             { "insertType", "text" },
                                                                             { "result", Selector.Result.ToString() },
                                                                             { "prepare", true }
                                                                     });

        It should_be_insert_with_template = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                          .Do().Direct()
                                                          .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.WithTemplate(Selector.Jquery.Class("new").Parent(HtmlTag.Area)).Text())
                                                          .GetExecutable<ExecutableInsert>()
                                                          .ShouldEqualData(new Dictionary<string, object>
                                                                           {
                                                                                   { "insertType", "text" },
                                                                                   { "result", Selector.Result.ToString() },
                                                                                   { "template", "$('.new').parent('area')" }
                                                                           });

        It should_be_insert_with_template_by_id = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                .Do().Direct()
                                                                .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.WithTemplateById("id").Text())
                                                                .GetExecutable<ExecutableInsert>()
                                                                .ShouldEqualData(new Dictionary<string, object>
                                                                                 {
                                                                                         { "insertType", "text" },
                                                                                         { "result", Selector.Result.ToString() },
                                                                                         { "template", "$('#id')" }
                                                                                 });

        It should_be_insert_with_template_by_url = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                 .Do().Direct()
                                                                 .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.WithTemplateByUrl(Pleasure.Generator.TheSameString()).Text())
                                                                 .GetExecutable<ExecutableInsert>()
                                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                                  {
                                                                                          { "insertType", "text" },
                                                                                          { "result", Selector.Result.ToString() },
                                                                                          { "template", "||ajax*{\"url\":\"TheSameString\",\"type\":\"GET\",\"async\":false}||" }
                                                                                  });

        It should_be_with_after = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                .Do().Direct()
                                                .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.After())
                                                .GetExecutable<ExecutableInsert>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                 {
                                                                         { "insertType", "after" },
                                                                         { "result", Selector.Result.ToString() },
                                                                 });

        It should_be_with_append = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.Append())
                                                 .GetExecutable<ExecutableInsert>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "insertType", "append" },
                                                                          { "result", Selector.Result.ToString() },
                                                                  });

        It should_be_with_before = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.Before())
                                                 .GetExecutable<ExecutableInsert>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                  {
                                                                          { "insertType", "before" },
                                                                          { "result", Selector.Result.ToString() },
                                                                  });

        It should_be_with_html = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               .Do().Direct()
                                               .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.Html())
                                               .GetExecutable<ExecutableInsert>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                {
                                                                        { "insertType", "html" },
                                                                        { "result", Selector.Result.ToString() },
                                                                });

        It should_be_with_prepend = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  .Do().Direct()
                                                  .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.Prepend())
                                                  .GetExecutable<ExecutableInsert>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "insertType", "prepend" },
                                                                           { "result", Selector.Result.ToString() },
                                                                   });

        It should_be_with_text = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               .Do().Direct()
                                               .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.Text())
                                               .GetExecutable<ExecutableInsert>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                {
                                                                        { "insertType", "text" },
                                                                        { "result", Selector.Result.ToString() },
                                                                });

        It should_be_with_val = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                              .Do().Direct()
                                              .OnSuccess(dsl => dsl.With(targetSelector).Core().Insert.Val())
                                              .GetExecutable<ExecutableInsert>()
                                              .ShouldEqualData(new Dictionary<string, object>
                                                               {
                                                                       { "insertType", "val" },
                                                                       { "result", Selector.Result.ToString() },
                                                               });

        #region Fake classes

        class FakeInsertModel
        {
            #region Properties

            public decimal Prop1 { get; set; }

            #endregion
        }

        #endregion
    }
}