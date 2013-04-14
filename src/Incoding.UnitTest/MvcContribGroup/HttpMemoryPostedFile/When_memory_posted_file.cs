namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.IO;
    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(HttpMemoryPostedFile))]
    public class When_memory_posted_file
    {
        #region Estabilish value

        static HttpMemoryPostedFile httpMemoryPostFile;

        static string contentType;

        static string fileName;

        static Stream stream;

        static byte[] content;

        #endregion

        Establish establish = () =>
                                  {
                                      fileName = Pleasure.Generator.String();
                                      contentType = Pleasure.Generator.String();
                                      content = Pleasure.Generator.Bytes();
                                      stream = new MemoryStream(content);
                                  };

        Because of = () => { httpMemoryPostFile = new HttpMemoryPostedFile(stream, fileName, contentType); };

        It should_be_file_name = () => httpMemoryPostFile.FileName.ShouldEqual(fileName);

        It should_be_content_length = () => httpMemoryPostFile.ContentLength.ShouldEqual((int)stream.Length);

        It should_be_content_type = () => httpMemoryPostFile.ContentType.ShouldEqual(contentType);

        It should_be_input_stream = () => httpMemoryPostFile.InputStream.ShouldNotBeNull();

        It should_be_content_as_bytes = () =>
                                            {
                                                var firstBytes = httpMemoryPostFile.ContentAsBytes;
                                                firstBytes.ShouldNotBeEmpty();
                                                firstBytes.ShouldEqualWeakEach(content);

                                                Pleasure.Do10((i) =>
                                                                  {
                                                                      var nextBytes = httpMemoryPostFile.ContentAsBytes;
                                                                      firstBytes.ShouldEqualWeakEach(nextBytes);
                                                                  });
                                            };
    }
}