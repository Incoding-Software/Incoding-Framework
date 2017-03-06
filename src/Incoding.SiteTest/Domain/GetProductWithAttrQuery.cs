namespace Incoding.SiteTest.Domain
{
    #region << Using >>

    using System.Data;
    using Incoding.CQRS;

    #endregion

    [MessageExecuteSetting(IsolationLevel = IsolationLevel.ReadUncommitted, DataBaseInstance = "")]
    public class GetProductWithAttrQuery : QueryBase<string>
    {
        protected override string ExecuteResult()
        {
            return "";
        }
    }
}