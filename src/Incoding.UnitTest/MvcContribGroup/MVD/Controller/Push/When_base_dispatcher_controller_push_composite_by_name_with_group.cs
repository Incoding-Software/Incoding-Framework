namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_composite_as_array : Context_dispatcher_controller
    {
        static FakePushComposite1ByNameWithGroupCommand command1;

        static FakePushComposite1ByNameWithGroupCommand command2;

        Establish establish = () =>
                              {
                                  Establish(types: new[] { typeof(List<FakePushComposite1ByNameWithGroupCommand>) });
                                  command1 = Pleasure.Generator.Invent<FakePushComposite1ByNameWithGroupCommand>();
                                  command2 = Pleasure.Generator.Invent<FakePushComposite1ByNameWithGroupCommand>();
                                  requestBase.SetupGet(r => r.Form).Returns(new NameValueCollection()
                                                                            {
                                                                                    { "[0].Name", command1.Name },
                                                                                    { "[1].Name", command2.Name },
                                                                            });
                              };

        Because of = () => { result = controller.Push(typeof(FakePushComposite1ByNameWithGroupCommand).Name, true); };

        It should_be_push_1 = () => dispatcher.ShouldBePush(command1);

        It should_be_push_2 = () => dispatcher.ShouldBePush(command2);

        #region Fake classes

        [ExcludeFromCodeCoverage]
        public class FakePushComposite1ByNameWithGroupCommand : CommandBase
        {
            #region Properties

            [UsedImplicitly]
            public string Name { get; set; }

            #endregion

            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}