namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_push_composite_without_repository : Context_default_dispatcher
    {
        #region Establish value

        static CommandWithoutRepository command;

        static QueryWithoutRepository query;

        static CommandComposite composite;

        #endregion

        Establish establish = () =>
                              {
                                  command = Pleasure.Generator.Invent<CommandWithoutRepository>();
                                  query = Pleasure.Generator.Invent<QueryWithoutRepository>();

                                  composite = new CommandComposite();
                                  composite.Quote(command);
                                  composite.Quote(query);
                              };

        Because of = () => dispatcher.Push(composite);
        
        It should_be_committed = () => unitOfWorkFactory.Verify(r => r.Create(Pleasure.MockIt.IsAny<IsolationLevel>(), Pleasure.MockIt.IsAny<bool>(), Pleasure.MockIt.IsNull<string>()), Times.Never());

        It should_be_disposable = () => unitOfWork.Verify(r => r.Dispose(), Times.Never());

        It should_be_flush = () => unitOfWork.Verify(r => r.Flush(), Times.Never());
        
    }
}