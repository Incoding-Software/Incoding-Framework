namespace Incoding.UnitTest.MSpecGroup
{
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    [Subject(typeof(InventFactory<>))]
    public class When_invent_factory_object_with_wrap_field
    {
        #region Fake classes

        class FakeObjectWithWrapField : IncEntityBase
        {
            #region Properties

            public new virtual int Id { get; protected set; }

            #endregion
        }

        class FakeObject : FakeObjectWithWrapField
        {
        }

        #endregion

        #region Establish value

        static FakeObject fakeObject;

        #endregion

        Because of = () => { fakeObject = new InventFactory<FakeObject>().Create(); };

        It should_be_invent_object = () => fakeObject.Id.ShouldBeGreaterThan(0);
    }
}