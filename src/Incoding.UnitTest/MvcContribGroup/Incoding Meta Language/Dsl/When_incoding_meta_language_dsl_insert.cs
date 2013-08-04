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
        #region Fake classes

        class FakeInsertModel
        {
            #region Properties

            public decimal Prop1 { get; set; }

            #endregion
        }

        #endregion

        #region Estabilish value

        static readonly Func<JquerySelector, JquerySelector> targetSelector = selector => selector.Id("Selector");

        #endregion

        It should_be_with_append = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.Append())
                                                 .GetExecutable<ExecutableInsert>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "insertType", "append" }
                                                                      });

        It should_be_with_prepend = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                  .Do().Direct()
                                                  .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.Prepend())
                                                  .GetExecutable<ExecutableInsert>()
                                                  .ShouldEqualData(new Dictionary<string, object>
                                                                       {
                                                                               { "insertType", "prepend" }
                                                                       });

        It should_be_with_after = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                .Do().Direct()
                                                .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.After())
                                                .GetExecutable<ExecutableInsert>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                     {
                                                                             { "insertType", "after" }
                                                                     });

        It should_be_with_before = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                 .Do().Direct()
                                                 .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.Before())
                                                 .GetExecutable<ExecutableInsert>()
                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                      {
                                                                              { "insertType", "before" }
                                                                      });

        It should_be_with_val = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                              .Do().Direct()
                                              .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.Val())
                                              .GetExecutable<ExecutableInsert>()
                                              .ShouldEqualData(new Dictionary<string, object>
                                                                   {
                                                                           { "insertType", "val" }
                                                                   });

        It should_be_with_html = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               .Do().Direct()
                                               .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.Html())
                                               .GetExecutable<ExecutableInsert>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                    {
                                                                            { "insertType", "html" }
                                                                    });

        It should_be_with_text = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                               .Do().Direct()
                                               .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.Text())
                                               .GetExecutable<ExecutableInsert>()
                                               .ShouldEqualData(new Dictionary<string, object>
                                                                    {
                                                                            { "insertType", "text" }
                                                                    });

        It should_be_insert_with_jquery_template = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                                 .Do().Direct()
                                                                 .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.WithTemplate(Selector.Jquery.Class("new").Parent()).Text())
                                                                 .GetExecutable<ExecutableInsert>()
                                                                 .ShouldEqualData(new Dictionary<string, object>
                                                                                      {
                                                                                              { "insertType", "text" }, 
                                                                                              { "template", "$('.new').parent()" }
                                                                                      });

        It should_be_insert_with_ajax_template = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                               .Do().Direct()
                                                               .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.WithTemplate(Selector.Incoding.AjaxGet(Pleasure.Generator.TheSameString())).Text())
                                                               .GetExecutable<ExecutableInsert>()
                                                               .ShouldEqualData(new Dictionary<string, object>
                                                                                    {
                                                                                            { "insertType", "text" }, 
                                                                                            { "template", "'@@@@@@@{\"url\":\"TheSameString\",\"type\":\"GET\",\"async\":false}@@@@@@@'" }
                                                                                    });

        It should_be_insert_for = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                .Do().Direct()
                                                .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.For<FakeInsertModel>(model => model.Prop1).Text())
                                                .GetExecutable<ExecutableInsert>()
                                                .ShouldEqualData(new Dictionary<string, object>
                                                                     {
                                                                             { "insertType", "text" }, 
                                                                             { "property", "Prop1" }
                                                                     });

        It should_be_insert_prepair = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                    .Do().Direct()
                                                    .OnSuccess(callbackDsl => callbackDsl.With(targetSelector).Core().Insert.Prepare().Text())
                                                    .GetExecutable<ExecutableInsert>()
                                                    .ShouldEqualData(new Dictionary<string, object>
                                                                         {
                                                                                 { "insertType", "text" }, 
                                                                                 { "prepare", true }
                                                                         });
    }
}