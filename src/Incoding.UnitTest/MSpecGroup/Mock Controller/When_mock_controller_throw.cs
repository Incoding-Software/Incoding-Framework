namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MockController<>))]
    public class When_mock_controller_throw : Context_mock_controller
    {
        #region Fake classes

        class FakeController : IncControllerBase
        {

            #region Api Methods

            public void PushMethod(CommandBase command)
            {
                this.dispatcher.Push(command);
            }

            public void QueryMethod(QueryBase<string> query)
            {
                this.dispatcher.Query(query);
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static MockController<FakeController> fakeController;

        #endregion

        Establish establish = () =>
                                  {
                                      fakeController = MockController<FakeController>
                                              .When()
                                              .StubPushAsThrow(new FakeCommand(), new InvalidOperationException(Pleasure.Generator.TheSameString()))
                                              .StubQueryAsThrow<FakeQuery, string>(new FakeQuery(), new InvalidOperationException(Pleasure.Generator.TheSameString()));
                                  };

        It should_be_push_with_exception = () => Catch.Exception(() => fakeController.Original.PushMethod(new FakeCommand())).Message.ShouldEqual(Pleasure.Generator.TheSameString());

        It should_be_query_with_exception = () => Catch.Exception(() => fakeController.Original.QueryMethod(new FakeQuery())).Message.ShouldEqual(Pleasure.Generator.TheSameString());
    }
}