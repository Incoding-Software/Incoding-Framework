namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Pleasure.Generator))]
    public class When_pleasure_generate
    {
        It should_be_generate_string = () => Pleasure.Generator.String(70).Length.ShouldEqual(70);

        It should_be_generate_string_unique = () => Pleasure.Generator.String().ShouldNotEqual(Pleasure.Generator.String());

        It should_be_generate_positive_number_range_in_min = () => Pleasure.Generator.PositiveNumber(10, 20).ShouldBeGreaterThanOrEqualTo(10);

        It should_be_generate_positive_number_range_max = () => Pleasure.Generator.PositiveNumber(10, 20).ShouldBeLessThanOrEqualTo(20);

        It should_be_generate_email_has_user_name = () => Pleasure.Generator.Email().Split("@".ToCharArray())[0].ShouldNotBeEmpty();

        It should_be_generate_email_has_domain = () => Pleasure.Generator.Email().Split("@".ToCharArray())[1].ShouldEqual("mail.com");

        It should_be_generate_bytes = () => Pleasure.Generator.Bytes().ShouldNotBeEmpty();

        It should_be_generate_bytes_unique = () =>
                                                 {
                                                     var left = Pleasure.Generator.Bytes();
                                                     var right = Pleasure.Generator.Bytes();
                                                     Catch.Exception(() => left.ShouldEqualWeakEach(right)).ShouldNotBeNull();
                                                 };

        It should_be_generate_enum_with_wrong_type = () => Catch.Exception(() => Pleasure.Generator.EnumAsInt(typeof(When_pleasure_generate))).ShouldBeOfType<ArgumentException>();

        It should_be_generate_enum = () => Pleasure.Generator.Enum<DayOfWeek>().ShouldNotEqual(DayOfWeek.Sunday);

        It should_be_generate_key_value_pair = () => Pleasure.Generator.KeyValuePair().Should(pair =>
                                                                                                  {
                                                                                                      pair.Value.ShouldNotBeEmpty();
                                                                                                      pair.Key.ShouldNotBeEmpty();
                                                                                                  });

        It should_be_generate_enum_as_int = () => Pleasure.Generator.EnumAsInt(typeof(DayOfWeek)).ShouldBeGreaterThan(0);

        It should_be_generate_the_same_date_time = () => new DateTime(2012, 4, 16).ShouldEqual(Pleasure.Generator.The20120406Noon());

        It should_be_generate_url = () => Pleasure.Generator.Url(new { Key = "Value", Key2 = "Value2" }).Should(url =>
                                                                                                                    {
                                                                                                                        url.ShouldContain("http://sample.com");
                                                                                                                        url.ShouldContain("?Key=Value&Key2=Value2");
                                                                                                                    });

        It should_be_generate_uri = () => Pleasure.Generator.Uri(new { Key = "Value", Key2 = "Value2" }).Should(url => url.ShouldNotBeNull());

        It should_be_generate_memory_stream = () => Pleasure.Generator.Stream().CanRead.ShouldBeTrue();

        It should_be_generate_guid_as_string = () => Pleasure.Generator.GuidAsString().Length.ShouldEqual(36);

        It should_be_generate_date_time_unique = () => Pleasure.Generator.DateTime().ShouldNotEqual(Pleasure.Generator.DateTime());

        It should_be_generate_the_same_string = () => Pleasure.Generator.TheSameString().ShouldEqual(Pleasure.Generator.TheSameString());

        It should_be_generate_the_same_number = () => Pleasure.Generator.TheSameNumber().ShouldEqual(Pleasure.Generator.TheSameNumber());

        It should_be_generate_http_posted_file = () => Pleasure.Generator.HttpPostedFile().Should(@base =>
                                                                                                      {
                                                                                                          @base.InputStream.ShouldNotBeNull();
                                                                                                          @base.FileName.ShouldEqual("fileName");
                                                                                                          @base.ContentLength.ShouldBeGreaterThan(0);
                                                                                                          @base.ContentType.ShouldEqual("contentType");
                                                                                                      });

        It should_be_generate_memory_posted_file = () => Pleasure.Generator.HttpMemoryPostedFile().Should(@base =>
                                                                                                              {
                                                                                                                  @base.InputStream.ShouldNotBeNull();
                                                                                                                  @base.FileName.ShouldEqual("fileName");
                                                                                                                  @base.ContentLength.ShouldBeGreaterThan(0);
                                                                                                                  @base.ContentType.ShouldEqual("contentType");
                                                                                                              });

        It should_be_generate_bool = () =>
                                         {
                                             if (Pleasure.Generator.Bool())
                                                 true.ShouldBeTrue();
                                             else
                                                 false.ShouldBeFalse();
                                         };

        It should_be_generate_time = () =>
                                         {
                                             var timeSpan = Pleasure.Generator.TimeSpan();
                                             timeSpan.Hours.ShouldBeGreaterThan(0);
                                             timeSpan.Minutes.ShouldBeGreaterThan(0);
                                             timeSpan.Seconds.ShouldBeGreaterThan(0);
                                         };

        It should_be_generate_base_64 = () => Catch
                                                      .Exception(() => Convert.FromBase64String(Pleasure.Generator.Base64()).ShouldNotBeEmpty())
                                                      .ShouldBeNull();
    }
}