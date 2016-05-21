namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CreateByTypeQuery))]
    public class When_create_by_type_as_command
    {
        It should_be_array = () =>
                             {
                                 var command1 = Pleasure.Generator.Invent<Composite1>();
                                 var command2 = Pleasure.Generator.Invent<Composite2>();
                                 CreateByTypeQuery.AsCommands query = Pleasure.Generator.Invent<CreateByTypeQuery.AsCommands>(dsl => dsl.Tuning(r => r.IsComposite, false)
                                                                                                                                        .Tuning(r => r.IncTypes, "Composite1&Composite2"));

                                 var mockQuery = MockQuery<CreateByTypeQuery.AsCommands, CommandBase[]>
                                         .When(query)
                                         .StubQuery<CreateByTypeQuery, object>(dsl => dsl.Tuning(r => r.Type, typeof(Composite1).Name)
                                                                                         .Empty(r => r.IsGroup)
                                                                                         .Empty(r => r.IsModel), command1)
                                         .StubQuery<CreateByTypeQuery, object>(dsl => dsl.Tuning(r => r.Type, typeof(Composite2).Name)
                                                                                         .Empty(r => r.IsGroup)
                                                                                         .Empty(r => r.IsModel), command2);

                                 mockQuery.Execute();
                                 mockQuery.ShouldBeIsResult(new CommandBase[] { command1, command2 });
                             };

        It should_be_array_as_composite = () =>
                                          {
                                              var command1 = Pleasure.Generator.Invent<Composite1>();

                                              CreateByTypeQuery.AsCommands query = Pleasure.Generator.Invent<CreateByTypeQuery.AsCommands>(dsl => dsl.Tuning(r => r.IsComposite, true)
                                                                                                                                                     .Tuning(r => r.IncTypes, "Composite1&Composite1"));

                                              var mockQuery = MockQuery<CreateByTypeQuery.AsCommands, CommandBase[]>
                                                      .When(query)
                                                      .StubQuery<CreateByTypeQuery, object>(dsl => dsl.Tuning(r => r.Type, typeof(Composite1).Name)
                                                                                                      .Empty(r => r.IsGroup)
                                                                                                      .Empty(r => r.IsModel), command1);

                                              mockQuery.Execute();
                                              mockQuery.ShouldBeIsResult(new CommandBase[] { command1, command1 });
                                          };

        public class Composite1 : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        public class Composite2 : CommandBase
        {
            protected override void Execute()
            {
                throw new NotImplementedException();
            }
        }

        #region Establish value

        static MockMessage<CreateByTypeQuery, Type> mockQuery;

        static Type expected;

        #endregion
    }
}