namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Pleasure.Generator))]
    public class When_pleasure_invent_empty
    {
        #region Fake classes

        class FakeGenerateObject
        {
            #region Properties

            public string StrValue { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static FakeGenerateObject result;

        #endregion

        Because of = () => { result = Pleasure.Generator.InventEmpty<FakeGenerateObject>(); };

        It should_be_created = () => result.ShouldNotBeNull();

        It should_be_set_string = () => result.StrValue.ShouldBeEmpty();
    }
}