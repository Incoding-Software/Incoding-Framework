namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.IO;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(HttpMemoryPostedFile))]
    public class When_memory_posted_file_with_empty_content
    {
        #region Estabilish value

        static MemoryStream emptyStream;

        static HttpMemoryPostedFile httpMemoryPostedFile;

        #endregion

        Establish establish = () => { emptyStream = Pleasure.Generator.EmptyStream(); };

        Because of = () => { httpMemoryPostedFile = new HttpMemoryPostedFile(emptyStream, string.Empty, string.Empty); };

        It should_be_content_as_bytes_empty = () => httpMemoryPostedFile.ContentAsBytes.ShouldBeEmpty();
    }
}