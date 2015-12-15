namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(B))]
    public class When_b_as_string
    {
        It should_be_single_class = () => B.Col_xs_12.AsClass().ShouldEqual("col-xs-12");

        It should_be_multiple = () => (B.Col_xs_12 | B.Active).AsClass().ShouldEqual("active col-xs-12");

        It should_be_multiple_2 = () => {(B.Col_xs_11 | B.Col_xs_offset_1).AsClass().ShouldEqual("col-xs-offset-1 col-xs-11");};
    }
}