namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;

    #endregion

    public class GetExpectedDelayToSchedulerQuery : QueryBase<List<GetExpectedDelayToSchedulerQuery.Response>>
    {
        #region Properties

        public int FetchSize { get; set; }

        public DateTime Date { get; set; }

        #endregion

        #region Nested classes

        public class Response
        {
            #region Properties

            public string Id { get; set; }

            public CommandBase Instance { get; set; }

            #endregion
        }

        #endregion

        protected override List<Response> ExecuteResult()
        {
            var all = new List<DelayToScheduler>();
            all.AddRange(Repository.Query(whereSpecification: (new DelayToSchedulerByStatusWhere(DelayOfStatus.New))
                                                  .And(new DelayToSchedulerAvailableStartsOnWhereSpec(Date)), 
                                          paginatedSpecification: new PaginatedSpecification(1, FetchSize)));
            all.AddRange(Repository.Query(whereSpecification: (new DelayToSchedulerByStatusWhere(DelayOfStatus.Error))
                                                  .And(new DelayToSchedulerAvailableStartsOnWhereSpec(Date)), 
                                          paginatedSpecification: new PaginatedSpecification(1, 10)));
            return all.ToList()
                      .Select(r => new Response()
                                   {
                                           Id = r.Id, 
                                           Instance = r.Instance
                                   })
                      .ToList();
        }
    }
}