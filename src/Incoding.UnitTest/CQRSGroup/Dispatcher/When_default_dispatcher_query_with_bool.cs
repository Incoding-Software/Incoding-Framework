namespace Incoding.UnitTest
{
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    [Subject(typeof(DefaultDispatcher))]
    public class When_default_dispatcher_query_with_bool : Context_default_dispatcher
    {
        Establish establish = () => { message = Pleasure.Generator.Invent<QueryBool>(); };

        Because of = () => { result = dispatcher.Query(message); };

        It should_be_true = () => result.ShouldBeTrue();

        class QueryBool : QueryBase<bool>
        {
            protected override bool ExecuteResult()
            {
                return true;
            }
        }

        #region Establish value

        static QueryBool message;

        static bool result;

        #endregion
    }
}