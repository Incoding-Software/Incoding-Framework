namespace IncodingContrib.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetRecurrencyDateQuery))]
    public class When_get_recurrency_date_hourly_with_endless
    {
        #region Fields

        Establish establish = () =>
                                  {
                                      GetRecurrencyDateQuery query = Pleasure.Generator.Invent<GetRecurrencyDateQuery>(dsl => dsl.Tuning(r => r.Type, RepeatType.Hourly)
                                                                                                                                 .Tuning(r => r.RepeatInterval, 5)
                                                                                                                                 .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                                                                 .Tuning(r => r.EndDate, null)
                                                                                                                                 .Tuning(r => r.RepeatCount, null)
                                                                                                                                 .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2015 16:42")));
                                      mockNow = DateTime.Now;

                                      expected = new DateTime?(Convert.ToDateTime("05/15/2015 21:36"));

                                      mockQuery = MockQuery<GetRecurrencyDateQuery, DateTime?>
                                              .When(query)
                                              .StubQuery(new GetDateTimeNowQuery(), new GetDateTimeNowQuery.Response
                                                                                        {
                                                                                                Now = mockNow,
                                                                                                UtcNow = mockNow
                                                                                        });
                                  };

        Because of = () => mockQuery.Original.Execute();

        It should_be_result = () => mockQuery.ShouldBeIsResult(expected);

        #endregion

        #region Establish value

        static MockMessage<GetRecurrencyDateQuery, DateTime?> mockQuery;

        static DateTime? expected;

        static DateTime mockNow;

        #endregion
    }
}