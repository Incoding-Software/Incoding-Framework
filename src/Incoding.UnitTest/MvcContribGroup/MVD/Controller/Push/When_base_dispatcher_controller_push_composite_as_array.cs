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
        static FakeCommand command1;

        static FakeCommand command2;

        Establish establish = () =>
                              {
                                  command1 = Pleasure.Generator.Invent<FakeCommand>();
                                  command2 = Pleasure.Generator.Invent<FakeCommand>();
                                  dispatcher.StubQuery(Pleasure.Generator.Invent<CreateByTypeQuery>(dsl => dsl.Tuning(r => r.Type, typeof(FakeCommand).Name)
                                                                                                              .Tuning(r => r.ModelState, controller.ModelState)
                                                                                                              .Tuning(r => r.ControllerContext, controller.ControllerContext)
                                                                                                              .Empty(r => r.IsModel)
                                                                                                              .Tuning(r => r.IsGroup, true)), (object)new List<FakeCommand>() { command1, command2 });
                              };

        Because of = () => { result = controller.Push(typeof(FakeCommand).Name, string.Empty, true); };

        It should_be_push_1 = () => dispatcher.ShouldBePush(command1);

        It should_be_push_2 = () => dispatcher.ShouldBePush(command2);
    }
}