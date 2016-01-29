namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DispatcherControllerBase))]
    public class When_create_by_type_with_generic : Context_dispatcher_controller
    {
        #region Fake classes

        public class FakeGenericByNameCommand<TEntity> : CommandBase
        {
            ////ncrunch: no coverage start
            protected override void Execute()
            {
                throw new NotImplementedException();
            }

            ////ncrunch: no coverage end       
        }

        #endregion

        

        Because of = () => { result = controller.Push("{0}|{1}".F(HttpUtility.UrlEncode(typeof(FakeGenericByNameCommand<>).Name), typeof(IncEntityBase).Name)); };

        It should_be_push = () => dispatcher.ShouldBePush(new FakeGenericByNameCommand<IncEntityBase>());

        It should_be_result = () => result.ShouldBeIncodingSuccess();
    }
}