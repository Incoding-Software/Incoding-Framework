namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_query_to_file : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeFileByNameQuery<T> : QueryBase<byte[]>
        {
            ////ncrunch: no coverage start
            protected override byte[] ExecuteResult()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end        
        }

        #endregion

        #region Establish value

        static byte[] content;

        static string contentType;

        static string fileName;

        #endregion

        Establish establish = () =>
                              {
                                  Establish(types: new[] { typeof(FakeFileByNameQuery<>) });
                                  content = Pleasure.Generator.Bytes();
                                  contentType = Pleasure.Generator.String();
                                  fileName = Pleasure.Generator.String();

                                  dispatcher.StubQuery(new FakeFileByNameQuery<string>(), content);
                              };

        Because of = () =>
                     {
                         {
                             result = controller.QueryToFile("{0}|{1}".F(typeof(FakeFileByNameQuery<>).Name, typeof(string).Name), contentType, fileName);
                         }

                         ;
                     };

        It should_be_result = () => result.ShouldBeFileContent(content,
                                                               contentType: contentType,
                                                               fileDownloadName: fileName);
    }
}