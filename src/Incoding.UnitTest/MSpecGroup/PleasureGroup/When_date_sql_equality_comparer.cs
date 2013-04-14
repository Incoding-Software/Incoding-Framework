namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(DateSqlEqualityComparer))]
    public class When_date_sql_equality_comparer
    {
        It should_be_equality = () => new DateSqlEqualityComparer().Equals(DateTime.Now, DateTime.Now).ShouldBeTrue();

        It should_be_equality_with_wrong_reference = () => new DateSqlEqualityComparer().Equals(DateTime.Now, null).ShouldBeFalse();

        It should_be_equality_with_wrong_type = () => new DateSqlEqualityComparer().Equals(DateTime.Now, 2).ShouldBeFalse();

        It should_be_equality_with_wrong_value = () => new DateSqlEqualityComparer().Equals(DateTime.Now, DateTime.Now.AddMinutes(10)).ShouldBeFalse();
    }
}