namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(StringUrlExtensions))]
    public class When_string_url_extensions
    {
        #region Fake classes

        class FakeRoutes
        {
            // ReSharper disable UnusedMember.Local
            #region Properties

            public int Prop1 { get; set; }

            public int Prop2 { get; set; }

            public int Prop3 { get; set; }

            public int Prop4 { get; set; }

            public int Prop5 { get; set; }

            public int Prop6 { get; set; }

            public int Prop7 { get; set; }

            public int Prop8 { get; set; }

            public int Prop9 { get; set; }

            #endregion

            // ReSharper restore UnusedMember.Local
        }

        #endregion

        It should_be_append_segment_with_slash = () => "Index/Home"
                                                               .AppendSegment("/Add")
                                                               .ShouldEqual("Index/Home/Add");

        It should_be_append_segment_with_slash_in_root = () => "Index/Home/"
                                                                       .AppendSegment("/Add")
                                                                       .ShouldEqual("Index/Home/Add");

        It should_be_append_segment_without_slash = () => "Index/Home"
                                                                  .AppendSegment("Add")
                                                                  .ShouldEqual("Index/Home/Add");

        It should_be_append_to_hash_query_string = () => "http://sample.com#!"
                                                                 .AppendToHashQueryString(new
                                                                                          {
                                                                                                  param = "value", 
                                                                                                  param2 = "value2"
                                                                                          })
                                                                 .ShouldEqual("http://sample.com#!?param=value/param2=value2");

        It should_be_append_to_hash_query_string_with_encode_value = () => "http://sample.com#!"
                                                                                   .AppendToHashQueryString(new
                                                                                                            {
                                                                                                                    param = "~%2FAreas%2FKitchen%2FViews%2FKitchen%2FTable.cshtml", 
                                                                                                                    area = string.Empty
                                                                                                            })
                                                                                   .ShouldEqual("http://sample.com#!?param=~%2FAreas%2FKitchen%2FViews%2FKitchen%2FTable.cshtml/area=");

        It should_be_append_to_hash_query_string_with_exist = () => "http://sample.com#!Index/Home?param=Value"
                                                                            .AppendToHashQueryString(new { param = "newValue" })
                                                                            .ShouldEqual("http://sample.com#!Index/Home?param=newValue");

        It should_be_append_to_hash_query_string_with_exist_empty = () => "http://sample.com#!Index/Home?param="
                                                                                  .AppendToHashQueryString(new { param = "newValue" })
                                                                                  .ShouldEqual("http://sample.com#!Index/Home?param=newValue");

        It should_be_append_to_hash_query_string_with_hash_encode_url = () => "http://sample.com#!Dispatcher/Render?incView=~%2FAreas%2FKitchen%2FViews%2FKitchen%2FTable.cshtml"
                                                                                      .AppendToHashQueryString(new
                                                                                                               {
                                                                                                                       OrderId = "value"
                                                                                                               })
                                                                                      .ShouldEqual("http://sample.com#!Dispatcher/Render?incView=~%2FAreas%2FKitchen%2FViews%2FKitchen%2FTable.cshtml/OrderId=value");

        It should_be_append_to_hash_query_string_with_hash_url = () => "http://sample.com#!Index/Home?"
                                                                               .AppendToHashQueryString(new
                                                                                                        {
                                                                                                                param = "value", 
                                                                                                                param2 = "value2"
                                                                                                        })
                                                                               .ShouldEqual("http://sample.com#!Index/Home?param=value/param2=value2");

        It should_be_append_to_hash_query_string_with_query_string = () => "http://sample.com?param=queryValue#!Index/Home"
                                                                                   .AppendToHashQueryString(new { param = "hashValue" })
                                                                                   .ShouldEqual("http://sample.com?param=queryValue#!Index/Home?param=hashValue");

        It should_be_append_to_hash_query_string_with_selector = () => "/Jasmine/GetValue?Value=$('%23sandboxTextBox')"
                                                                               .AppendToHashQueryString(new { hash = "id".ToId() })
                                                                               .ShouldEqual("/Jasmine/GetValue?Value=$('%23sandboxTextBox')#!?hash=$('%23id')");

        It should_be_append_to_query_string_with_special_symbol = () => "/PDWBIS/Dispatcher/Query?incType=GetContactFailedAttemptsQuery&Type=?="
                                                                                .AppendToQueryString(new { Type2 = 1 })
                                                                                .ShouldEqual("/PDWBIS/Dispatcher/Query?incType=GetContactFailedAttemptsQuery&Type=?=&Type2=1");

        It should_be_append_to_query_string_param_null = () =>
                                                         {
                                                             const string url = "http://domain.com";
                                                             string param = null;
                                                             url.AppendToQueryString(new { Param = param }).ShouldEqual("http://domain.com");
                                                         };

        It should_be_append_to_query_string_performance = () =>
                                                          {
                                                              var route = Pleasure.Generator.Invent<FakeRoutes>();
                                                              Pleasure.Do(i => "http://domain.com".AppendToQueryString(route), 1000)
                                                                      .ShouldBeLessThan(150);
                                                          };

        It should_be_append_to_query_string_with_exists = () => "/Dispatcher/Render?type=GetEditCategorySubQuery&incView=~%2FAreas%2FAdmin%2FViews%2FCategory%2FSub%2FEdit.cshtml"
                                                                        .AppendToQueryString(new { Param = "Value1" })
                                                                        .ShouldEqual("/Dispatcher/Render?type=GetEditCategorySubQuery&incView=~%2FAreas%2FAdmin%2FViews%2FCategory%2FSub%2FEdit.cshtml&Param=Value1");

        It should_be_append_to_query_string_complexity = () => "/Dispatcher/Render?incView={{#if IsDirect}}".AppendOnlyToQueryString(new
                                                                                                                                     {
                                                                                                                                             token = Selector.JS.Eval("AjaxAdapter.Token"), 
                                                                                                                                             admissionId = "admissionId".ToId(), 
                                                                                                                                             someType = typeof(When_string_url_extensions).Name
                                                                                                                                     })
                                                                                                            .ShouldEqual("/Dispatcher/Render?incView={{#if IsDirect}}&token=||javascript*AjaxAdapter.Token||&admissionId=$('%23admissionId')&someType=When_string_url_extensions");

        It should_be_append_to_query_string_with_hash_query = () => "http://domain.com#!/Index?Param=HashValue"
                                                                            .AppendToQueryString(new
                                                                                                 {
                                                                                                         Param = "Value1"
                                                                                                 })
                                                                            .ShouldEqual("http://domain.com?Param=Value1#!/Index?Param=HashValue");

        It should_be_append_to_query_string_with_hash_query_without_goolge_char = () => "http://domain.com#/Index?Param=HashValue"
                                                                                                .AppendToQueryString(new
                                                                                                                     {
                                                                                                                             Param = "Value1"
                                                                                                                     })
                                                                                                .ShouldEqual("http://domain.com?Param=Value1#!/Index?Param=HashValue");

        It should_be_append_to_query_string_with_name_selector = () => "/PDWBIS/Dispatcher/Query?incType=GetContactFailedAttemptsQuery&Type=$('[name=\"Type\"]')"
                                                                               .AppendToQueryString(new { Type2 = Selector.Jquery.Name("Test") })
                                                                               .ShouldEqual("/PDWBIS/Dispatcher/Query?incType=GetContactFailedAttemptsQuery&Type=$('[name=\"Type\"]')&Type2=$('[name%3D\"Test\"]')");

        It should_be_append_to_query_string_with_special_encode = () => "http://domain.com"
                                                                                .AppendToQueryString(new
                                                                                                     {
                                                                                                             Param = "Value1", 
                                                                                                             Param2 = "http://domain.com"
                                                                                                     })
                                                                                .ShouldEqual("http://domain.com?Param=Value1&Param2=http://domain.com");

        It should_be_append_to_query_string_with_selector_special_encode = () => "http://domain.com"
                                                                                         .AppendToQueryString(new { Param = "id".ToId() })
                                                                                         .ShouldEqual("http://domain.com?Param=$('%23id')");

        It should_be_append_to_query_string_with_not_selector_special_encode = () => "http://domain.com"
                                                                                             .AppendToQueryString(new { Param = "{{#if IsDirect}}" })
                                                                                             .ShouldEqual("http://domain.com?Param={{#if IsDirect}}");

        It should_be_append_to_query_string_with_ajax_selector = () => "/PDWBIS/Dispatcher/Render?incType=EditGapCommand&incIsModel=True&incView=~/Areas/PDWBIS/Views/Gaps/Edit.cshtml"
                                                                               .AppendToQueryString(new { Param = "/PDWBIS/Dispatcher/Push/PDWBIS/Dispatcher/Push?incTypes=AddGapCommand&EncounterId=d67660bc-5b94-4210-9ed0-a5080159fa28&inc_csrf_token=||javascript*AjaxAdapter.Token||&CurrentAdmissionId=||queryString*CurrentAdmissionId||&CurrentOrderId=||queryString*CurrentOrderId||".ToAjaxGet() })
                                                                               .ShouldEqual("/PDWBIS/Dispatcher/Render?incType=EditGapCommand&incIsModel=True&incView=~/Areas/PDWBIS/Views/Gaps/Edit.cshtml&Param=||ajax*{\"url\"%3A\"%2FPDWBIS%2FDispatcher%2FPush%2FPDWBIS%2FDispatcher%2FPush%3FincTypes%3DAddGapCommand%26EncounterId%3Dd67660bc-5b94-4210-9ed0-a5080159fa28%26inc_csrf_token%3D||javascript*AjaxAdapter.Token||%26CurrentAdmissionId%3D||queryString*CurrentAdmissionId||%26CurrentOrderId%3D||queryString*CurrentOrderId||\",\"type\"%3A\"GET\",\"async\"%3Afalse}||");

        It should_be_set_hash = () => "url"
                                              .SetHash("hash")
                                              .ShouldEqual("url#!hash?");

        It should_be_set_hash_with_exists = () => "url#!"
                                                          .SetHash("hash")
                                                          .ShouldEqual("url#!hash?");

        It should_be_set_hash_with_exists_but_without_google_char = () => "url#"
                                                                                  .SetHash("hash")
                                                                                  .ShouldEqual("url#!hash?");

        It should_be_set_hash_with_query_string = () => "url#!"
                                                                .SetHash("/Dispatcher/Render?TypeOfCategory=Kitchen&type=CategoriesTmpl&incView=~%2FAreas%2FKitchen%2FViews%2FKitchen%2FOrder_Details.cshtml&OrderId=value")
                                                                .ShouldEqual("url#!Dispatcher/Render?TypeOfCategory=Kitchen/type=CategoriesTmpl/incView=~%2FAreas%2FKitchen%2FViews%2FKitchen%2FOrder_Details.cshtml/OrderId=value");

        It should_be_set_hash_with_slash = () => "url#!"
                                                         .SetHash("/hash")
                                                         .ShouldEqual("url#!hash?");
    }
}