namespace Incoding.SiteTest
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;

    #endregion

    public class GetProductQuery : QueryBase<List<GetProductQuery.Response>>
    {
        #region Nested classes

        public class Response
        {
            #region Properties

            public string Name { get; set; }

            #endregion
        }

        #endregion

        protected override List<Response> ExecuteResult()
        {
            return Repository.Query<Product>()
                             .Select(r => new Response() { Name = r.Name })
                             .ToList();
        }
    }
}