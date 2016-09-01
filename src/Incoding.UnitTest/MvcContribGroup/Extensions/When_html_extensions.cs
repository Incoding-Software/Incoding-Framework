namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(HtmlExtensions))]
    public class When_html_extensions
    {
        #region Establish value

        static MockHtmlHelper<FakeModel> htmlHelper;

        #endregion

        Establish establish = () =>
                              {
                                  htmlHelper = MockHtmlHelper<FakeModel>
                                          .When();
                              };

        It should_be_as_view = () =>
                               {
                                   var pathToView = Pleasure.Generator.String();
                                   var data = Pleasure.Generator.Invent<KeyValueVm[]>();
                                   var model = Pleasure.Generator.String();
                                   IoCFactory.Instance.StubTryResolve(Pleasure.MockStrictAsObject<ITemplateFactory>(mock => mock.Setup(s => s.Render(Pleasure.MockIt.IsNotNull<HtmlHelper>(), pathToView, data, model)).Returns(Pleasure.Generator.TheSameString())));
                                   htmlHelper.Original.Dispatcher().AsView(data, pathToView, model).ToHtmlString().ShouldBeTheSameString();
                               };

        It should_be_as_view_from_query = () =>
                                          {
                                              var pathToView = Pleasure.Generator.String();
                                              var query = Pleasure.Generator.Invent<FakeQuery>();
                                              var model = Pleasure.Generator.String();
                                              var data = Pleasure.Generator.String();
                                              IoCFactory.Instance.StubTryResolve(Pleasure.MockStrictAsObject<IDispatcher>(mock => { mock.StubQuery<FakeQuery, string>(query, data); }));
                                              IoCFactory.Instance.StubTryResolve(Pleasure.MockStrictAsObject<ITemplateFactory>(mock => mock.Setup(s => s.Render(Pleasure.MockIt.IsNotNull<HtmlHelper>(), pathToView, data, model)).Returns(Pleasure.Generator.TheSameString())));
                                              htmlHelper.Original.Dispatcher().AsViewFromQuery(query, pathToView, model).ToHtmlString().ShouldBeTheSameString();
                                          };

        It should_be_as_view_without_model = () =>
                                             {
                                                 var pathToView = Pleasure.Generator.String();
                                                 var data = Pleasure.Generator.Invent<KeyValueVm[]>();
                                                 IoCFactory.Instance.StubTryResolve(Pleasure.MockStrictAsObject<ITemplateFactory>(mock => mock.Setup(s => s.Render(Pleasure.MockIt.IsNotNull<HtmlHelper>(), pathToView, data, null)).Returns(Pleasure.Generator.TheSameString())));
                                                 htmlHelper.Original.Dispatcher().AsView(data, pathToView).ToHtmlString().ShouldBeTheSameString();
                                             };

        It should_be_dispatcher = () =>
                                  {
                                      IoCFactory.Instance.StubTryResolve(Pleasure.Generator.Invent<DefaultDispatcher>());
                                      htmlHelper.Original.Dispatcher().ShouldNotBeNull();
                                  };

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

        It should_be_incoding = () => htmlHelper
                                              .Original
                                              .Incoding()
                                              .ShouldNotBeNull();

        It should_be_when = () => htmlHelper
                                          .Original
                                          .When(JqueryBind.InitIncoding | JqueryBind.IncChangeUrl)
                                          .Should(dsl =>
                                                  {
                                                      var meta = dsl.TryGetValue("meta") as IncodingMetaContainer;
                                                      meta.ShouldNotBeNull();
                                                      meta.OnBind.ShouldEqual("initincoding incchangeurl incoding");
                                                  });

        It should_be_when_string = () => htmlHelper
                                                 .Original
                                                 .When("blur")
                                                 .Should(dsl =>
                                                         {
                                                             var meta = dsl.TryGetValue("meta") as IncodingMetaContainer;
                                                             meta.ShouldNotBeNull();
                                                             meta.OnBind.ShouldEqual("blur incoding");
                                                         });

        public class FakeQuery : QueryBase<string>
        {
            public string Test { get; set; }

            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #region Fake classes

        class FakeModel
        {
            #region Properties

            public string Prop { get; set; }

            #endregion
        }

        #endregion
    }
}