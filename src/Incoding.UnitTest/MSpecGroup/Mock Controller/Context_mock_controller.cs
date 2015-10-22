namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Block;
    using Incoding.CQRS;

    #endregion

    public class Context_mock_controller
    {
        #region Fake classes

        #region Nested classes

        protected class FakeCommand : CommandBase
        {
            #region Properties

            public string FakeProp { get; set; }

            #endregion

            protected override void Execute() { }
        }

        #endregion

        public interface IFakeInterface
        {
            void FakeMethod();
        }

        #endregion

        #region Nested classes

        protected class FakeQuery : QueryBase<string>
        {
            #region Properties

            public string Id { get; set; }

            #endregion

            protected override string ExecuteResult()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}