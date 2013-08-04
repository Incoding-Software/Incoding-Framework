namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(InventFactory<>))]
    public class When_invent_factory_object_with_field
    {
        #region Fake classes

        class FakeObjectWithField
        {
            #region Fields

            [UsedImplicitly]
            public string StrValue;

            #endregion
        }

        #endregion

        #region Estabilish value

        static FakeObjectWithField fakeObject;

        #endregion

        Because of = () => { fakeObject = new InventFactory<FakeObjectWithField>().Create(); };

        It should_be_invent_object = () => fakeObject.StrValue.ShouldNotBeEmpty();
    }
}