namespace Incoding.Block
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public class GetExpectedDelayToSchedulerQuery : QueryBase<List<GetExpectedDelayToSchedulerQuery.Response>>
    {
        protected override List<Response> ExecuteResult()
        {
            var all = new List<DelayToScheduler>();
            Func<DelayOfStatus, int, IQueryable<DelayToScheduler>> getByType = (status, size) => Repository.Query(whereSpecification: new DelayToScheduler.Where.ByStatus(status)
                                                                                                                          .And(new DelayToScheduler.Where.ByAsync(Async))
                                                                                                                          .And(new DelayToScheduler.Where.AvailableStartsOn(Date)),
                                                                                                                  orderSpecification: new DelayToScheduler.Sort.Default(),
                                                                                                                  paginatedSpecification: new PaginatedSpecification(1, size));
            all.AddRange(getByType(DelayOfStatus.New, FetchSize));
            return all.ToList()
                      .Select(scheduler => new Response()
                                           {
                                                   Id = scheduler.Id,
                                                   Instance = scheduler.Instance,
                                                   TimeOut = scheduler.Option.With(r => r.TimeOut)
                                           })
                      .ToList();
        }

        #region Nested classes

        public class Response
        {
            #region Properties

            public string Id { get; set; }

            public CommandBase Instance { get; set; }

            public int TimeOut { get; set; }

            #endregion
        }

        #endregion

        #region Properties

        public int FetchSize { get; set; }

        public DateTime Date { get; set; }

        public bool Async { get; set; }

        #endregion
    }
}