namespace Incoding.SiteTest
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;

    #endregion

    public class GetProductQuery : QueryBase<List<GetProductQuery.Response>>
    {
        protected override List<Response> ExecuteResult()
        {
            return Repository.Query<Product>()
                             .ToList()
                             .Select(r => new Response() { Name = r.Name, Id = r.Id.ToString() })
                             .ToList();
        }

        #region Nested classes

        public class Response
        {
            #region Properties

            public string Name { get; set; }

            public string Id { get; set; }

            #endregion
        }

        #endregion
    }
}