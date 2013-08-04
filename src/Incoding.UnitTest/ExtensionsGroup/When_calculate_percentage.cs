namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using Incoding.Extensions;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MathHelper))]
    public class When_calculate_percentage
    {
        #region Estabilish value

        static decimal resultIfCount0;

        static decimal resultIfFact0;

        static decimal resultIfFactResultIfFactGreatestAll;

        static decimal resultIfAllGreatesFact;

        #endregion

        It should_be_percentage_with_all_zero = () => MathHelper.Percentage(0, 10).ShouldEqual(1);

        It should_be_percentage_with_fact_zero = () => MathHelper.Percentage(10, 0).ShouldEqual(0);

        It should_be_percentage = () => MathHelper.Percentage(2, 5).ShouldBeGreaterThan(1);

        It should_be_percentage_with_fact_less_actual = () => MathHelper.Percentage(6, 3).ShouldEqual((decimal)0.5);
    }
}