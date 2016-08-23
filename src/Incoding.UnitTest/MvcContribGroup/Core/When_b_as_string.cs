namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(B))]
    public class When_b_as_string
    {
        It should_be_multipl_3 = () => (B.Col_xs_6).AsClass().ShouldEqual("col-xs-6");

        It should_be_multiple = () => (B.Col_xs_12 | B.Active).AsClass().ShouldEqual("active col-xs-12");

        It should_be_multiple_2 = () => { (B.Col_xs_11 | B.Col_xs_offset_1).AsClass().ShouldEqual("col-xs-offset-1 col-xs-11"); };

        It should_be_multiple_3 = () => { (B.Text_right).AsClass().ShouldEqual("text-right"); };

        It should_be_multiple_4 = () => { (B.Text_right | B.Form_control).AsClass().ShouldEqual("form-control text-right"); };

        It should_be_single_class = () => B.Col_xs_12.AsClass().ShouldEqual("col-xs-12");
    }
}