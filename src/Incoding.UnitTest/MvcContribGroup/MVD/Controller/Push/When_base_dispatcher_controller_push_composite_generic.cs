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
    public class When_base_dispatcher_controller_push_composite_generic : Context_dispatcher_controller
    {
        Establish establish = () => Establish(types: new[] { typeof(FakePush1Command<string>), typeof(FakePush2Command<string>) });

        Because of = () =>
                     {
                         result = controller.Push("{0}|{1}&{2}|{3}".F(typeof(FakePush1Command<string>).Name,
                                                                      typeof(string).Name,
                                                                      typeof(FakePush2Command<int>).Name,
                                                                      typeof(int).Name));
                     };

        It should_be_push_1 = () => dispatcher.ShouldBePush(new FakePush1Command<string>());

        It should_be_push_2 = () => dispatcher.ShouldBePush(new FakePush2Command<int>());

        #region Fake classes

        public class FakePush1Command<T> : CommandBase
        {
            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        public class FakePush2Command<T> : CommandBase
        {
            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}