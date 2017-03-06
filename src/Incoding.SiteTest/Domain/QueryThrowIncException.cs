namespace Incoding.SiteTest.Domain
{
    using Incoding.CQRS;
    public class QueryThrowIncException:QueryBase<string>
    {
        protected override string ExecuteResult()
        {            
            throw IncWebException.ForServer("Test");
        }
    }
}