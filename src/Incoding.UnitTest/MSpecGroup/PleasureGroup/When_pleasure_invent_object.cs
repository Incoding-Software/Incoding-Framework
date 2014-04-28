namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Pleasure.Generator))]
    public class When_pleasure_invent
    {
        #region Fake classes

        class FakeGenerateObject
        {
            #region Properties

            public string StrValue { get; set; }

            #endregion
        }

        #endregion

        It should_be_by_generic = () => Pleasure.Generator.Invent<FakeGenerateObject>()
                                                .Should(o => o.StrValue.ShouldNotBeEmpty());

        It should_be_by_type = () => Pleasure.Generator.Invent(typeof(FakeGenerateObject))
                                             .ShouldNotBeNull();
    }


}