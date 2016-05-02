namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(SelectorMapEx))]
    public class When_selector_map_ex
    {
        It should_be_as_hash = () =>
                               {
                                   var test = new Test();
                                   var routeValueDictionary = test.MapAsHash();
                                   routeValueDictionary.ShouldBeKeyValue("Name", Selector.Incoding.HashQueryString("Name"));
                                   routeValueDictionary.ShouldBeKeyValue("Name2", Selector.Incoding.HashQueryString("Name2"));
                               };

        It should_be_as_names = () =>
                                {
                                    var test = new Test();
                                    var routeValueDictionary = test.MapAsNames();
                                    routeValueDictionary.ShouldBeKeyValue("Name", "Name".ToName());
                                    routeValueDictionary.ShouldBeKeyValue("Name2", "Name2".ToName());
                                };

        It should_be_as_query_string = () =>
                                       {
                                           var test = new Test();
                                           var routeValueDictionary = test.MapAsHash();
                                           routeValueDictionary.ShouldBeKeyValue("Name", Selector.Incoding.QueryString("Name"));
                                           routeValueDictionary.ShouldBeKeyValue("Name2", Selector.Incoding.QueryString("Name2"));
                                       };

        public class Test
        {
            public string Name { get; set; }

            public string Name2 { get; set; }
        }
    }
}