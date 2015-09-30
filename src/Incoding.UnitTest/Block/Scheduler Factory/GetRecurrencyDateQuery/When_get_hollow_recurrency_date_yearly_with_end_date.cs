namespace IncodingContrib.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(GetRecurrencyDateQuery))]
    public class When_get_hollow_recurrency_date_yearly_with_end_date
    {
        #region Fields

        Establish establish = () =>
                                  {
                                      GetRecurrencyDateQuery query = Pleasure.Generator.Invent<GetRecurrencyDateQuery>(dsl => dsl.Tuning(r => r.Type, RepeatType.Yearly)
                                                                                                                                 .Tuning(r => r.RepeatInterval, 5)
                                                                                                                                 .Tuning(r => r.StartDate, Convert.ToDateTime("05/15/2015 16:36"))
                                                                                                                                 .Tuning(r => r.EndDate, Convert.ToDateTime("05/15/2020 16:41"))
                                                                                                                                 .Tuning(r => r.NowDate, Convert.ToDateTime("05/15/2020 16:42")));
                                      mockNow = DateTime.Now;

                                      expected = new DateTime?(null);

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