namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    // ReSharper disable Mvc.ActionNotResolved
    // ReSharper disable Mvc.ControllerNotResolved
    // ReSharper disable Mvc.AreaNotResolved
    [Subject(typeof(UrlExtensions))]
    public class When_url_extensions
    {
        It should_be_action_area = () =>
                                       {
                                           string expected = "/Jasmine".AppendToQueryString(new
                                                                                                {
                                                                                                        param = "id", 
                                                                                                        area = "Area"
                                                                                                });

                                           new MockUrl()
                                                   .StubAction(expected)
                                                   .Original.ActionArea("Index", "Jasmine", "Area", new { param = "id" })
                                                   .ShouldEqual(expected);
                                       };

        It should_be_hash_area = () => new MockUrl()
                                               .StubAction("/Jasmine?area=Area")
                                               .StubRequest(mock => mock.SetupGet(r => r.Url).Returns(new Uri("http://localhost:18302/Jasmine/#!existsHash")))
                                               .Original.HashArea("Index", "Jasmine", "Area", new { param = "id", param2 = "id2" })
                                               .ShouldEqual("/Jasmine#!Jasmine?area=Area/param=id/param2=id2");

        It should_be_hash_referral_area = () => new MockUrl()
                                                        .StubAction("/Jasmine?area=Area")
                                                        .StubRequest(mock => mock.SetupGet(r => r.UrlReferrer).Returns(new Uri("http://localhost:18302/Jasmine/#!existsHash")))
                                                        .Original.HashReferralArea("Index", "Jasmine", "Area", new { param = "id", param2 = "id2" })
                                                        .ShouldEqual("/Jasmine#!Jasmine?area=Area/param=id/param2=id2");

        It should_be_hash = () => new MockUrl()
                                          .StubAction("/Jasmine")
                                          .StubRequest(mock => mock.SetupGet(r => r.Url).Returns(new Uri("http://localhost:18302/Jasmine")))
                                          .Original.Hash("Index", "Jasmine", new { param = "id" })
                                          .ShouldEqual("/Jasmine#!Jasmine?param=id");

        It should_be_hash_referral = () => new MockUrl()
                                                   .StubAction("/Jasmine")
                                                   .StubRequest(mock => mock.SetupGet(r => r.UrlReferrer).Returns(new Uri("http://localhost:18302/Jasmine")))
                                                   .Original.HashReferral("Index", "Jasmine", new { param = "id" })
                                                   .ShouldEqual("/Jasmine#!Jasmine?param=id");

        It should_be_hash_without_query_string = () =>
                                                     {
                                                         const string hashUrl = "/Jasmine";
                                                         new MockUrl()
                                                                 .StubAction(hashUrl)
                                                                 .StubRequest(mock => mock.SetupGet(r => r.Url).Returns(new Uri("http://localhost:18302/Jasmine")))
                                                                 .Original.Hash("Index", "Jasmine")
                                                                 .ShouldEqual("/Jasmine#!Jasmine?");
                                                     };
    }
}