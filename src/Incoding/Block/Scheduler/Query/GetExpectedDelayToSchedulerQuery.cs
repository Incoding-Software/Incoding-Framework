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
        public static DateTime? LastDate { get; set; }

        protected override List<Response> ExecuteResult()
        {
            var delayOfStatuses = new[] { DelayOfStatus.New, DelayOfStatus.Error }.ToList();
            if (IncludeInProgress)
                delayOfStatuses.Add(DelayOfStatus.InProgress);

            var nowInFeature = Date.AddMinutes(2);
            var isHaveForDo = !LastDate.HasValue || LastDate <= nowInFeature;
            if (!isHaveForDo)
                return new List<Response>();

            return Repository.Query(whereSpecification: new DelayToScheduler.Where.ByStatus(delayOfStatuses.ToArray())
                                            .And(new DelayToScheduler.Where.ByAsync(Async))
                                            .And(new DelayToScheduler.Where.AvailableStartsOn(Date)),
                                    orderSpecification: new DelayToScheduler.Sort.Default(),
                                    paginatedSpecification: new PaginatedSpecification(1, FetchSize))
                             .Select(s => new
                                          {
                                                  Id = s.Id,
                                                  Command = s.Command,
                                                  Timeout = s.Option.TimeOut,
                                                  Type = s.Type
                                          })
                             .Select(s => new Response()
                                          {
                                                  Id = s.Id,
                                                  Instance = s.Command.DeserializeFromJson(Type.GetType(s.Type)) as CommandBase,
                                                  TimeOut = s.Timeout
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

        public class GetLastDateQuery : QueryBase<DateTime?>
        {
            public bool Async { get; set; }

            public DateTime Date { get; set; }

            protected override DateTime? ExecuteResult()
            {
                return Repository.Query(whereSpecification: new DelayToScheduler.Where.ByStatus(new[] { DelayOfStatus.New, DelayOfStatus.Error }.ToArray())
                                                .And(new DelayToScheduler.Where.ByAsync(Async))
                                                .And(new DelayToScheduler.Where.AvailableStartsOn(Date)))
                                 .Min(s => s.StartsOn);
            }
        }

        #region Properties

        public bool IncludeInProgress { get; set; }

        public int FetchSize { get; set; }

        public DateTime Date { get; set; }

        public bool Async { get; set; }

        #endregion
    }
}