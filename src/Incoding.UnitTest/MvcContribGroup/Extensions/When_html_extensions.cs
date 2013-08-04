namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(HtmlExtensions))]
    public class When_html_extensions
    {
        #region Fake classes

        class FakeModel
        {
            #region Properties

            public string Prop { get; set; }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockHtmlHelper<FakeModel> htmlHelper;

        #endregion

        Establish establish = () =>
                                  {
                                      htmlHelper = MockHtmlHelper<FakeModel>
                                              .When();
                                  };

        It should_be_incoding = () => htmlHelper
                                              .Original
                                              .Incoding()
                                              .ShouldNotBeNull();

        It should_be_for = () => htmlHelper
                                         .Original
                                         .For(r => r.Prop)
                                         .Should(@for =>
                                                     {
                                                         var property = @for.TryGetValue("property") as Expression<Func<FakeModel, string>>;
                                                         property.GetMemberName().ShouldEqual("Prop");

                                                         @for.TryGetValue("htmlHelper").ShouldNotBeNull();
                                                     });

        It should_be_for_group = () => htmlHelper
                                               .Original
                                               .ForGroup(r => r.Prop)
                                               .Should(@for =>
                                                           {
                                                               var property = @for.TryGetValue("property") as Expression<Func<FakeModel, string>>;
                                                               property.GetMemberName().ShouldEqual("Prop");

                                                               @for.TryGetValue("htmlHelper").ShouldNotBeNull();
                                                           });

        It should_be_when = () => htmlHelper
                                          .Original
                                          .When(JqueryBind.InitIncoding | JqueryBind.IncChangeUrl)
                                          .Should(dsl =>
                                                      {
                                                          var meta = dsl.TryGetValue("meta") as IncodingMetaContainer;
                                                          meta.ShouldNotBeNull();
                                                          meta.onBind.ShouldEqual("initincoding incchangeurl incoding");
                                                      });

        It should_be_when_string = () => htmlHelper
                                                 .Original
                                                 .When("blur")
                                                 .Should(dsl =>
                                                             {
                                                                 var meta = dsl.TryGetValue("meta") as IncodingMetaContainer;
                                                                 meta.ShouldNotBeNull();
                                                                 meta.onBind.ShouldEqual("blur incoding");
                                                             });
    }
}