namespace Incoding.UnitTest.KnowleadgeGroup
{
    #region << Using >>

    using Machine.Specifications;

    #endregion

    [Subject(""), Tags("Knowleadge")]
    public class When_understand_different_reference_and_value_type
    {
        #region Fake classes

        class FakeClass
        {
            #region Properties

            public int Value { get; set; }

            #endregion
        }

        #endregion

        #region Estabilish value

        struct FakeStructure
        {
            #region Constructors

            public FakeStructure(int value)
                    : this()
            {
                Value = value;
            }

            #endregion

            #region Properties

            public int Value { get; set; }

            #endregion
        }

        static FakeStructure valueType;

        static void ChangeValueType(FakeStructure fakeStructure)
        {
            fakeStructure.Value = 5;
        }

        static void ChangeReferenceType(FakeClass fakeClass)
        {
            fakeClass.Value = 5;
        }

        static FakeClass referenceType;

        #endregion

        Establish establish = () =>
                                  {
                                      valueType = new FakeStructure(1);
                                      referenceType = new FakeClass { Value = 1 };
                                  };

        Because of = () => { };

        It should_be_not_changes_because_value_type = () =>
                                                          {
                                                              ChangeValueType(valueType);
                                                              valueType.Value.ShouldEqual(1);
                                                          };

        It should_be_changes_because_reference_type = () =>
                                                          {
                                                              ChangeReferenceType(referenceType);
                                                              referenceType.Value.ShouldEqual(5);
                                                          };
    }
}