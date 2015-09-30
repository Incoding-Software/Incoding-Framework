namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncDateTimeResponse))]
    public class When_inc_date_time_as_nullable_response
    {
        static DateTime? The20120406 = Pleasure.Generator.The20120406Noon();

        It should_be_cast_to_date_time = () => ((DateTime)new IncDateTimeResponse(The20120406)).ShouldEqual(The20120406.Value);

        It should_be_cast_to_date_time_nullable = () => ((DateTime?)new IncDateTimeResponse(The20120406)).ShouldEqual(The20120406);

        It should_be_cast_to_inc = () => ((IncDateTimeResponse)The20120406).Value.ShouldEqual(The20120406);

        It should_be_ctor = () => new IncDateTimeResponse(The20120406).Value.ShouldEqual(The20120406);

        It should_be_equal_left_datetime = () => (Pleasure.Generator.The20120406Noon() == new IncDateTimeResponse(The20120406)).ShouldBeTrue();

        It should_be_equal_left_nullable = () => (The20120406 == new IncDateTimeResponse(The20120406)).ShouldBeTrue();

        It should_be_equal_left_nullable_as_true = () => ((DateTime?)DateTime.Now == new IncDateTimeResponse(The20120406)).ShouldBeFalse();

        It should_be_equal_right = () => (new IncDateTimeResponse(The20120406) == DateTime.Now).ShouldBeFalse();

        It should_be_get_hashcode = () => { new IncDateTimeResponse(The20120406).GetHashCode().ShouldEqual(The20120406.Value.GetHashCode()); };

        It should_not_be_equal_left = () => (DateTime.Now != new IncDateTimeResponse(The20120406)).ShouldBeTrue();

        It should_not_be_equal_left_and_right_null = () =>
                                                     {
                                                         IncDateTimeResponse incDateTimeResponse = null;
                                                         (DateTime.Now != incDateTimeResponse).ShouldBeTrue();
                                                     };

        It should_not_be_equal_left_as_false = () => (The20120406 != new IncDateTimeResponse(The20120406)).ShouldBeFalse();

        It should_not_be_equal_right = () => (new IncDateTimeResponse(The20120406) != The20120406).ShouldBeFalse();

        It should_not_be_equal_right_as_true = () => (new IncDateTimeResponse(The20120406) != DateTime.Now).ShouldBeTrue();
    }
}