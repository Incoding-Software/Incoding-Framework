namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.IO;
    using System.Web;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(HttpMemoryPostedFile))]
    public class When_memory_posted_file_from_http_posted_file
    {
        #region Estabilish value

        static Stream stream;

        static string fileName;

        static string contentType;

        static HttpPostedFileBase postFile;

        static HttpMemoryPostedFile httpMemoryPostFile;

        #endregion

        Establish establish = () =>
                                  {
                                      stream = Pleasure.Generator.Stream();
                                      fileName = Pleasure.Generator.String();
                                      contentType = Pleasure.Generator.String();

                                      postFile = Pleasure.MockAsObject<HttpPostedFileBase>(mock =>
                                                                                               {
                                                                                                   mock.SetupGet(r => r.InputStream).Returns(stream);
                                                                                                   mock.SetupGet(r => r.FileName).Returns(fileName);
                                                                                                   mock.SetupGet(r => r.ContentType).Returns(contentType);
                                                                                               });
                                  };

        Because of = () => { httpMemoryPostFile = new HttpMemoryPostedFile(postFile); };

        It should_be_file_name = () => httpMemoryPostFile.FileName.ShouldEqual(fileName);

        It should_be_content_length = () => httpMemoryPostFile.ContentLength.ShouldEqual((int)stream.Length);

        It should_be_content_type = () => httpMemoryPostFile.ContentType.ShouldEqual(contentType);

        It should_be_input_stream = () => httpMemoryPostFile.InputStream.ShouldNotBeNull();
    }
}