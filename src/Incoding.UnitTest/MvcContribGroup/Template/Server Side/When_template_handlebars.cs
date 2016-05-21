namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Block.IoC;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(TemplateHandlebarsOnServerSide))]
    public class When_template_handlebars
    {
        private static RenderViewQuery query;

        private static HtmlHelper htmlHelper;

        private static object data;

        private static Mock<IDispatcher> dispatcher;

        Establish establish = () =>
                              {
                                  data = new List<KeyValueVm>() { new KeyValueVm(1), new KeyValueVm(2) };
                                  htmlHelper = new HtmlHelper(new ViewContext(), Pleasure.MockStrictAsObject<IViewDataContainer>(), new RouteCollection());
                                  dispatcher = Pleasure.Mock<IDispatcher>();
                                  query = Pleasure.Generator.Invent<RenderViewQuery>(dsl => dsl.Tuning(r => r.HtmlHelper, htmlHelper));
                                  var tmpl = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "handlebars_sample_tmpl.txt"));
                                  dispatcher.StubQuery(query, new MvcHtmlString(tmpl));
                                  IoCFactory.Instance.StubTryResolve(dispatcher.Object);
                              };

        It should_be_compile = () =>
                               {
                                   new TemplateHandlebarsOnServerSide()
                                           .Render(htmlHelper, query.PathToView, data, query.Model)
                                           .ShouldEqual(@"<option  value=""1"" title="""">1</option><option  value=""2"" title="""">2</option>");
                               };

        It should_be_compile_performance = () =>
                                           {
                                               Pleasure.Do(i => new TemplateHandlebarsOnServerSide()
                                                                        .Render(htmlHelper, query.PathToView, data, query.Model)
                                                                        .ShouldNotBeEmpty(), 1000).ShouldBeLessThan(100);
                                           };

        It should_be_compile_wihtout_view_model = () =>
                                                  {
                                                      query.Model = null;
                                                      var render = new TemplateHandlebarsOnServerSide()
                                                              .Render(htmlHelper, query.PathToView, data);
                                                      render.ShouldEqual(@"<option  value=""1"" title="""">1</option><option  value=""2"" title="""">2</option>");
                                                  };

        It should_be_complexity = () =>
                                  {
                                      var tmpl = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "handlebars_complexity_tmpl.txt"));
                                      var newQuery = Pleasure.Generator.Invent<RenderViewQuery>(dsl => dsl.Tuning(r => r.Model,null)
                                                                                                          .Tuning(r => r.HtmlHelper, htmlHelper));
                                      dispatcher.StubQuery(newQuery, new MvcHtmlString(tmpl));
                                      var data = new ComplexityVm()
                                                 {
                                                         BreadCrumbsLinks = new List<ComplexityVm.TabVm>()
                                                                            {
                                                                                    new ComplexityVm.TabVm() { After = "" }
                                                                            },
                                                        GroupLinks = new List<ComplexityVm.TabVm>(),
                                                         CountTabs = new[]
                                                                     {
                                                                             new ComplexityVm.TabVm()
                                                                             {
                                                                                     After = " 1-50 од 231593",
                                                                                     Active = true,
                                                                                     Url = "",
                                                                                     Title = "Сите"
                                                                             },
                                                                             new ComplexityVm.TabVm()
                                                                             {
                                                                                     After = " 1-50 од 231593",
                                                                                     Active = false,
                                                                                     Url = "private",
                                                                                     Title = "Title"
                                                                             }
                                                                     }.ToList()
                                                 };
                                      var render = new TemplateHandlebarsOnServerSide()
                                              .Render(htmlHelper, newQuery.PathToView, data, newQuery.Model);
                                      render.ShouldEqual(@"
        
    <div class=""clearfix""></div>
            <div class=""navbar small"">
                <div class=""navbar-inner"">
                    <ul class=""nav nav-count"">
                            <li class=""tab-item active"">
                                <a href=""javascript:void(0)"">
                                    &#1057;&#1080;&#1090;&#1077;
                                    <span class=""after""> 1-50 &#1086;&#1076; 231593</span>
                                </a>  
                            </li>
                            <li class=""tab-item"">
                                <a href = ""#!private"">
                                    Title
                                    <span class=""after""> 1-50 &#1086;&#1076; 231593</span>
                                </a>  
                            </li>
                    </ul>
                   

");
                                  };

        public class ComplexityVm
        {
            public List<TabVm> CountTabs { get; set; }

            public List<TabVm> SortingLinks { get; set; }

            public List<TabVm> DisplayTypeLinks { get; set; }

            public List<TabVm> GroupLinks { get; set; }

            public bool HideGroupLinks { get; set; }

            public List<TabVm> BreadCrumbsLinks { get; set; }

            public string PageTitle { get; set; }

            public List<ListingSearchAdVm> FirstPositionItems { get; set; }

            //public ListingAdsNoResultsVm NoResults { get; set; }

            public string LocationString { get; set; }

            public string CategoryString { get; set; }

            //public List<GroupTabVm> MobileGroupLinks { get; set; }

            public bool IsAuthorized { get; set; }

            public bool HasGroups { get { return GroupLinks.Any(); } }

            public bool HasBreadCrumbs { get { return BreadCrumbsLinks.Any(); } }

            public class NameWithLink
            {
                public string Name { get; set; }

                public string Link { get; set; }
            }

            public class ListingSearchAdVm
            {
                #region Constructors

                public ListingSearchAdVm() { }

                #endregion

                #region Properties

                public long AdvertiserId { get; set; }

                public List<NameWithLink> Categories { get; set; }

                public NameWithLink Category { get; set; }

                public string CreateDate { get; set; }

                public string Currency { get; set; }

                public string DateTimeBr { get; set; }

                public bool ExtColored { get; set; }

                public bool ExtShowLogo { get; set; }

                public bool ExtTopPositioned { get; set; }

                public bool IsMessageAvailable { get; set; }

                public bool HasPrice { get; set; }

                public long Id { get; set; }

                public string IdSeo { get; set; }

                public string ImageDate { get; set; }

                public bool ImageIsVideo { get; set; }

                public string ImageTitle { get; set; }

                public bool IsCompany { get; set; }

                public bool IsNew { get; set; }

                public bool IsRent { get; set; }

                public bool IsSavedAd { get; set; }

                public bool IsStore { get; set; }

                public string StoreText { get; set; }

                public NameWithLink Location { get; set; }

                public List<NameWithLink> Locations { get; set; }

                public bool ManyImages { get; set; }

                public string MapAltitude { get; set; }

                public string MapLongtitude { get; set; }

                public string Price { get; set; }

                public string PriceFrequency { get; set; }

                public bool PriceLowered { get; set; }

                public string ResponseItemId { get; set; }

                public string ShortTitle { get; set; }

                public string StoreLogin { get; set; }

                public string StoreLogoDate { get; set; }

                public string StoreLogoTitle { get; set; }

                public string Title { get; set; }

                public bool IsAuthorized { get; set; }

                #endregion
            }

            public class TabVm
            {
                public string Title { get; set; }

                public string After { get; set; }

                public string Url { get; set; }

                public bool Active { get; set; }

                public string Value { get; set; }

                public string Notice { get; set; }
            }
        }
    }
}