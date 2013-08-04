namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Pleasure.Generator))]
    public class When_pleasure_invent_entity
    {
        #region Fake classes

        class FakeEntity : IncEntityBase
        {
            #region Properties

            public string Value { get; set; }

            public string Id { get; private set; }

            #endregion
        }

        #endregion

        #region Estabilish value

        static FakeEntity fakeEntityInvent;

        #endregion

        Establish establish = () => { };

        Because of = () => { fakeEntityInvent = Pleasure.Generator.InventEntity<FakeEntity>(factory => factory.Tuning(r => r.Value, "Value")); };

        It should_be_ignore_id = () => fakeEntityInvent.Id.ShouldBeNull();

        It should_be_set_other_properties = () => fakeEntityInvent.Value.ShouldEqual("Value");
    }
}