namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query_to_file_with_empty_content_type : Context_dispatcher_controller
    {
        #region Establish value

        static byte[] content;

        static string contentType;

        static string fileName;

        #endregion

        Establish establish = () =>
                              {
                                  Establish(types: new[] { typeof(FakeFileByNameQuery<>) });
                                  content = Pleasure.Generator.Bytes();
                                  contentType = string.Empty;
                                  fileName = Pleasure.Generator.String();

                                  dispatcher.StubQuery(new FakeFileByNameQuery<string>(), content);
                                  responseBase.Setup(r => r.AddHeader("X-Download-Options", "Open"));
                              };

        Because of = () => { result = controller.QueryToFile("{0}|{1}".F(typeof(FakeFileByNameQuery<>).Name, typeof(string).Name), contentType, fileName); };

        It should_be_result = () => result.ShouldBeFileContent(content, 
                                                               contentType: "img", 
                                                               fileDownloadName: fileName);
    }
}