namespace Incoding.UnitTest.KnowleadgeGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject("")]
    public class When_understand_different_bit_and_bool_operator
    {
        #region Establish value

        static bool IsTrue(ISpy spy)
        {
            spy.Is();
            return true;
        }

        #endregion

        Establish establish = () => { };

        Because of = () => { };

        It should_be_bool_operator = () =>
                                         {
                                             var mock = Pleasure.Mock<ISpy>();
                                             bool result = false && IsTrue(mock.Object);
                                             result.ShouldBeFalse();
                                             mock.Never();
                                         };

        It should_be_bit_operator = () =>
                                        {
                                            var mock = Pleasure.Mock<ISpy>();
                                            bool result = false & IsTrue(mock.Object);
                                            result.ShouldBeFalse();
                                            mock.Once();
                                        };
    }
}