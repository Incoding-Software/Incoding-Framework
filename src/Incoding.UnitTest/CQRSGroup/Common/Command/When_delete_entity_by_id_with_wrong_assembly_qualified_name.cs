namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DeleteEntityByIdCommand))]
    public class When_delete_entity_by_id_with_wrong_assembly_qualified_name
    {
        #region Estabilish value

        static MockMessage<DeleteEntityByIdCommand, object> mockCommand;

        static Exception exception;

        #endregion

        Establish establish = () =>
                                  {
                                      mockCommand = MockCommand<DeleteEntityByIdCommand>
                                              .When(Pleasure.Generator.Invent<DeleteEntityByIdCommand>());
                                  };

        Because of = () => { exception = Catch.Exception(() => mockCommand.Original.Execute()); };

        It should_be_inc_exception = () => exception.ShouldBeOfType<IncFrameworkException>();
    }
}