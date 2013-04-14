namespace Incoding.UnitTest.KnowleadgeGroup
{
    #region << Using >>

    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(""), Tags("Knowledge")]
    public class When_understand_behavior_read_only_field_value_type
    {
        #region Fake classes

        class FakeClass
        {
            #region Fields

            public readonly FakeReadOnlyValue ReadOnlyValue = new FakeReadOnlyValue
                                                                  {
                                                                          Value = 1
                                                                  };

            #endregion
        }

        #endregion

        #region Estabilish value

        struct FakeReadOnlyValue
        {
            #region Properties

            public int Value { get; set; }

            #endregion

            #region Api Methods

            public void IncrementValue()
            {
                Value++;
            }

            #endregion
        }

        static FakeClass classWithReadOnly;

        #endregion

        Establish establish = () => { classWithReadOnly = new FakeClass(); };

        Because of = () => { };

        It should_be_call_method_for_temp_variable_but_not_for_instance_field = () =>
                                                                                    {
                                                                                        classWithReadOnly.ReadOnlyValue.IncrementValue();
                                                                                        classWithReadOnly.ReadOnlyValue.Value.ShouldEqual(1);
                                                                                    };
    }
}