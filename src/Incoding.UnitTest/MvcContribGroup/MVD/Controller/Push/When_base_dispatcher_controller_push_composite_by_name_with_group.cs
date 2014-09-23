namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_composite_by_name_with_group : Context_dispatcher_controller
    {
        #region Fake classes

        [ExcludeFromCodeCoverage]
        public class FakePushComposite1ByNameWithGroupCommand : CommandBase
        {
            #region Properties

            [UsedImplicitly]
            public string Name { get; set; }

            #endregion

            public override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        Establish establish = () => Establish(types: new[] { typeof(List<FakePushComposite1ByNameWithGroupCommand>) });

        Because of = () => { result = controller.Composite(HttpUtility.UrlEncode(typeof(FakePushComposite1ByNameWithGroupCommand).Name)); };

        It should_be_push_1 = () => dispatcher.ShouldBePush(new FakePushComposite1ByNameWithGroupCommand());

        It should_be_push_2 = () => dispatcher.ShouldBePush(new FakePushComposite1ByNameWithGroupCommand());
    }
}