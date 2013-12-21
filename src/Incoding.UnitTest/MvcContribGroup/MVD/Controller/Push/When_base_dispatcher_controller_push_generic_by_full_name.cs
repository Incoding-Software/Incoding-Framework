namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_base_dispatcher_controller_push_generic_by_full_name : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeGenericByFullNameCommand<TEntity> : CommandBase
        {
            ////ncrunch: no coverage start
            public override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        #endregion

        Establish establish = () => Establish(types: new[] { typeof(FakeGenericByFullNameCommand<IncEntityBase>) });

        Because of = () => { result = controller.Push(HttpUtility.UrlEncode(typeof(FakeGenericByFullNameCommand<>).FullName), typeof(IncEntityBase).FullName); };

        It should_be_push = () => dispatcher.ShouldBePush(new FakeGenericByFullNameCommand<IncEntityBase>());

        It should_be_result = () => result.ShouldBeIncodingSuccess();
    }
}