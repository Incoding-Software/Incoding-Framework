namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaContainer))]
    public class When_incoding_meta_container_add_with_errors
    {
        static Exception exception;

        Because of = () =>
                     {
                         exception = Catch.Exception(() => new IncodingMetaContainer()
                                                                   .Add(Pleasure.MockStrictAsObject<ExecutableBase>(mock => mock.Setup(r => r.GetErrors()).Returns(new Dictionary<string, string>()
                                                                                                                                                                   {
                                                                                                                                                                           { "error", "message" },
                                                                                                                                                                           { "nextError", "nextMessage" }
                                                                                                                                                                   }))));
                     };

        It should_be_throw = () => exception.Message.ShouldEqual(@"Executable ExecutableBaseProxy have problem: error-message,nextError-nextMessage
Parameter name: callback");
    }
}