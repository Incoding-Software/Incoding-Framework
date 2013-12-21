namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(StringUrlExtensions))]
    public class When_string_url_extensions
    {
        It should_be_append_to_query_string_without_encode = () => "http://domain.com"
                                                                           .AppendToQueryString(new
                                                                                                    {
                                                                                                            Param = "Value1", 
                                                                                                            Param2 = "http://domain.com"
                                                                                                    })
                                                                           .ShouldEqual("http://domain.com?Param=Value1&Param2=http://domain.com");

        It should_be_append_to_query_string_with_hash_query = () => "http://domain.com#!/Index?Param=HashValue"
                                                                            .AppendToQueryString(new
                                                                                                     {
                                                                                                             Param = "Value1"
                                                                                                     })
                                                                            .ShouldEqual("http://domain.com?Param=Value1#!/Index?Param=HashValue");

        It should_be_append_to_query_string_param_null = () =>
                                                             {
                                                                 const string url = "http://domain.com";
                                                                 string param = null;
                                                                 url.AppendToQueryString(new { Param = param }).ShouldEqual("http://domain.com");
                                                             };

        It should_be_append_to_query_string_with_exists = () => "/Dispatcher/Render?type=GetEditCategorySubQuery&incView=~%2FAreas%2FAdmin%2FViews%2FCategory%2FSub%2FEdit.cshtml"
                                                                        .AppendToQueryString(new { Param = "Value1" })
                                                                        .ShouldEqual("/Dispatcher/Render?type=GetEditCategorySubQuery&incView=~%2FAreas%2FAdmin%2FViews%2FCategory%2FSub%2FEdit.cshtml&Param=Value1");

        It should_be_append_segment_without_slash = () => "Index/Home"
                                                                  .AppendSegment("Add")
                                                                  .ShouldEqual("Index/Home/Add");

        It should_be_append_segment_with_slash = () => "Index/Home"
                                                               .AppendSegment("/Add")
                                                               .ShouldEqual("Index/Home/Add");

        It should_be_append_segment_with_slash_in_root = () => "Index/Home/"
                                                                       .AppendSegment("/Add")
                                                                       .ShouldEqual("Index/Home/Add");

        It should_be_set_hash = () => "url"
                                              .SetHash("hash")
                                              .ShouldEqual("url#!hash?");

        It should_be_set_hash_with_exists = () => "url#!"
                                                          .SetHash("hash")
                                                          .ShouldEqual("url#!hash?");

        It should_be_set_hash_with_slash = () => "url#!"
                                                         .SetHash("/hash")
                                                         .ShouldEqual("url#!hash?");

        It should_be_set_hash_with_query_string = () => "url#!"
                                                                .SetHash("/Dispatcher/Render?TypeOfCategory=Kitchen&type=CategoriesTmpl&incView=~%2FAreas%2FKitchen%2FViews%2FKitchen%2FOrder_Details.cshtml&OrderId=value")
                                                                .ShouldEqual("url#!Dispatcher/Render?TypeOfCategory=Kitchen/type=CategoriesTmpl/incView=~%2FAreas%2FKitchen%2FViews%2FKitchen%2FOrder_Details.cshtml/OrderId=value");

        It should_be_append_to_hash_query_string = () => "http://sample.com#!"
                                                                 .AppendToHashQueryString(new
                                                                                              {
                                                                                                      param = "value", 
                                                                                                      param2 = "value2"
                                                                                              })
                                                                 .ShouldEqual("http://sample.com#!param=value/param2=value2");

        It should_be_append_to_hash_query_string_with_hash_url = () => "http://sample.com#!Index/Home?"
                                                                               .AppendToHashQueryString(new
                                                                                                            {
                                                                                                                    param = "value", 
                                                                                                                    param2 = "value2"
                                                                                                            })
                                                                               .ShouldEqual("http://sample.com#!Index/Home?param=value/param2=value2");

        It should_be_append_to_hash_query_string_with_hash_encode_url = () => "http://sample.com#!Dispatcher/Render?incView=~%2FAreas%2FKitchen%2FViews%2FKitchen%2FTable.cshtml"
                                                                                      .AppendToHashQueryString(new
                                                                                                                   {
                                                                                                                           OrderId = "value"
                                                                                                                   })
                                                                                      .ShouldEqual("http://sample.com#!Dispatcher/Render?incView=~%2FAreas%2FKitchen%2FViews%2FKitchen%2FTable.cshtml/OrderId=value");

        It should_be_append_to_hash_query_string_with_exist = () => "http://sample.com#!Index/Home?param=Value"
                                                                            .AppendToHashQueryString(new { param = "newValue" })
                                                                            .ShouldEqual("http://sample.com#!Index/Home?param=newValue");
   
        It should_be_append_to_hash_query_string_with_exist_empty = () => "http://sample.com#!Index/Home?param="
                                                                                  .AppendToHashQueryString(new { param = "newValue" })
                                                                                  .ShouldEqual("http://sample.com#!Index/Home?param=newValue");

        It should_be_append_to_hash_query_string_with_query_string = () => "http://sample.com?param=queryValue#!Index/Home"
                                                                                   .AppendToHashQueryString(new { param = "hashValue" })
                                                                                   .ShouldEqual("http://sample.com?param=queryValue#!Index/Home?param=hashValue");

        It should_be_append_to_hash_query_string_with_encode_value = () => "http://sample.com#!"
                                                                                   .AppendToHashQueryString(new
                                                                                                                {
                                                                                                                        param = "~%2FAreas%2FKitchen%2FViews%2FKitchen%2FTable.cshtml", 
                                                                                                                        area = string.Empty
                                                                                                                })
                                                                                   .ShouldEqual("http://sample.com#!param=~%2FAreas%2FKitchen%2FViews%2FKitchen%2FTable.cshtml/area=");
    }
}