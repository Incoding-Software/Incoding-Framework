namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query_to_file_with_empty_content_type : Context_dispatcher_controller
    {
        Establish establish = () =>
                              {
                                  var query = Pleasure.Generator.Invent<FakeFileByNameQuery>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeFileByNameQuery).Name)), (object)query);
                                  content = Pleasure.Generator.Bytes();
                                  contentType = string.Empty;
                                  fileName = Pleasure.Generator.String();

                                  dispatcher.StubQuery(query, content);
                                  responseBase.Setup(r => r.AddHeader("X-Download-Options", "Open"));
                              };

        Because of = () => { result = controller.QueryToFile(typeof(FakeFileByNameQuery).Name, contentType, fileName); };

        It should_be_result = () => result.ShouldBeFileContent(content,
                                                               contentType: "img",
                                                               fileDownloadName: fileName);

        #region Establish value

        static byte[] content;

        static string contentType;

        static string fileName;

        #endregion
    }
}