namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMoqExtensions))]
    public class When_incoding_moq_extensions_returns
    {
        #region Fake classes

        public class FakeClass : IFake
        {
            #region IFake Members

            public FakeClass Method()
            {
                throw new NotImplementedException();
            }

            public FakeClass Prop { get; set; }

            #endregion
        }

        public interface IFake
        {
            FakeClass Method();

            FakeClass Prop { get; }
        }

        #endregion

        It should_be_returns_invent = () =>
                                          {
                                              var fake = Pleasure.MockStrictAsObject<IFake>(mock => mock.Setup(r => r.Method()).ReturnsInvent(dsl => dsl.GenerateTo(r => r.Prop)));
                                              fake.Method()
                                                  .Should(@class =>
                                                              {
                                                                  @class.ShouldNotBeNull();
                                                                  @class.Prop.ShouldNotBeNull();
                                                              });
                                          };

        It should_be_return_invent_getter = () =>
                                                {
                                                    var fake = Pleasure.MockStrictAsObject<IFake>(mock => mock.SetupGet(r => r.Prop).ReturnsInvent(dsl => dsl.GenerateTo(r => r.Prop)));
                                                    fake.Prop
                                                        .Should(@class =>
                                                                    {
                                                                        @class.ShouldNotBeNull();
                                                                        @class.Prop.ShouldNotBeNull();
                                                                    });
                                                };

        It should_be_returns_null = () =>
                                        {
                                            var fake = Pleasure.MockStrictAsObject<IFake>(mock => mock.Setup(r => r.Method()).ReturnsNull());
                                            fake.Method().ShouldBeNull();
                                        };

        It should_be_returns_null_getter = () =>
                                               {
                                                   var fake = Pleasure.MockStrictAsObject<IFake>(mock => mock.SetupGet(r => r.Prop).ReturnsNull());
                                                   fake.Prop.ShouldBeNull();
                                               };
    }
}