namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(InventFactory<>))]
    public class When_invent_factory_object_with_property_only_get
    {
        #region Fake classes

        class FakeObjectWithReadOnlyProperty
        {
            #region Properties

            [UsedImplicitly]
            public string StrValue
            {
                get { return string.Empty; }
            }

            #endregion
        }

        #endregion

        #region Establish value

        static FakeObjectWithReadOnlyProperty fakeObject;

        static Exception exception;

        #endregion

        Because of = () => { exception = Catch.Exception(() => fakeObject = new InventFactory<FakeObjectWithReadOnlyProperty>().Create()); };

        It should_be_invent_object = () => fakeObject.ShouldNotBeNull();

        It should_be_without_exception = () => exception.ShouldBeNull();
    }
}