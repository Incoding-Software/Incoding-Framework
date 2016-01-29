namespace Incoding.UnitTest.MvcContribGroup.Core
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CreateByTypeQuery))]
    public class When_create_by_type
    {
        Establish establish = () =>
                              {

                                  //requestBase = Pleasure.Mock<HttpRequestBase>(mock =>
                                  //{
                                  //    mock.SetupGet(r => r.Form).Returns(new NameValueCollection());
                                  //    mock.SetupGet(r => r.QueryString).Returns(new NameValueCollection());
                                  //    if (isAjax)
                                  //        mock.SetupGet(r => r.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });
                                  //});
                                  //var modelBinderDictionary = new ModelBinderDictionary();
                                  //var modelBinder = Pleasure.MockStrictAsObject<IModelBinder>(mock => mock.Setup(r => r.BindModel(Pleasure.MockIt.IsAny<ControllerContext>(),
                                  //                                                                                                Pleasure.MockIt.IsAny<ModelBindingContext>())).Returns(null));
                                  //foreach (var type in types.Recovery(new Type[] { }))
                                  //    modelBinderDictionary.Add(type, modelBinder);
                                  //controller.SetValue("Binders", modelBinderDictionary);

                                  CreateByTypeQuery query = Pleasure.Generator.Invent<CreateByTypeQuery>();
                                  expected = Pleasure.Generator.Invent<UniqueNameClass>();

                                  mockQuery = MockQuery<CreateByTypeQuery, object>
                                          .When(query);
                                  //.StubQuery<CreateByTypeQuery.FindTypeByName, Type>(r => r.Tuning(s=>s.), typeof(UniqueNameClass));
                              };

        Because of = () => mockQuery.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        public class UniqueNameClass { }

        #region Establish value

        static MockMessage<CreateByTypeQuery, object> mockQuery;

        static object expected;

        #endregion
    }
}