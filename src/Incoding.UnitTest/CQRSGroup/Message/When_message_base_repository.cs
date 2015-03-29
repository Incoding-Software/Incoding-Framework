using Incoding.Extensions;

namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(MessageBase<>))]
    public class When_message_base_repository : Context_message_base_repository
    {
        #region Establish value

        static Mock<IUnitOfWork> unitOfWork;

        #endregion

        Establish establish = () =>
                                  {
                                      Establish();
                                      unitOfWork = Pleasure.Mock<IUnitOfWork>(mock => mock.Setup(r => r.IsOpen()).Returns(false));
                                      message.Setting = new MessageExecuteSetting
                                                            {
                                                                    //UnitOfWork = unitOfWork.Object
                                                            };
                                      message.Setting.SetValue("unitOfWork", unitOfWork.Object);
                                  };

        Because of = () => message.Execute();

        It should_be_resolve_once = () => provider.Verify(r => r.TryGet<IRepository>(), Times.Once());

        It should_be_open_once = () => unitOfWork.Verify(r => r.Open(), Times.Once());
    }
}