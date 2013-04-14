namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System;
    using Incoding.Maybe;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(MaybeObject))]
    public class When_maybe_object : Context_maybe
    {
        #region Estabilish value

        static FakeMaybe nullValue;

        static FakeMaybe value;

        #endregion

        Establish establish = () =>
                                  {
                                      nullValue = null;
                                      value = new FakeMaybe { Prop = Pleasure.Generator.String() };
                                  };

        It should_be_with_null_value = () => nullValue.With(r => r.Prop).ShouldBeNull();

        It should_be_with_value = () => value.With(r => r.Prop).ShouldNotBeEmpty();

        It should_be_return_or_default_with_default = () => nullValue
                                                                    .ReturnOrDefault(r => r.Prop, Pleasure.Generator.TheSameString())
                                                                    .ShouldEqual(Pleasure.Generator.TheSameString());

        It should_be_return_or_default_with_value = () => value
                                                                  .ReturnOrDefault(Pleasure.Generator.TheSameString(), Pleasure.Generator.String())
                                                                  .ShouldEqual(Pleasure.Generator.TheSameString());

        It should_be_return_or_default_with_value_as_func = () => value
                                                                          .ReturnOrDefault(r => r.Prop, Pleasure.Generator.String())
                                                                          .ShouldEqual(value.Prop);

        It should_be_return_or_throws_with_throws = () => Catch
                                                                  .Exception(() => nullValue.ReturnOrThrows(r => r.Prop, new ArgumentException()))
                                                                  .ShouldBeOfType<ArgumentException>();

        It should_be_return_or_throws_with_value = () => value
                                                                 .ReturnOrThrows(r => r.Prop, new ArgumentException())
                                                                 .ShouldEqual(value.Prop);

        It should_be_if_success_for_value = () => value
                                                          .If(r => r.Prop.Length == value.Prop.Length)
                                                          .ShouldNotBeNull();

        It should_be_if_fail_for_value = () => value
                                                       .If(r => r.Prop.Length != value.Prop.Length)
                                                       .ShouldBeNull();

        It should_be_if_for_null_value = () => nullValue
                                                       .If(r => r.Prop.Length > 0)
                                                       .ShouldBeNull();

        It should_be_not_fail_for_value = () => value
                                                        .Not(r => r.Prop.Length == value.Prop.Length)
                                                        .ShouldBeNull();

        It should_be_not_success_for_value = () => value
                                                           .Not(r => r.Prop.Length != value.Prop.Length)
                                                           .ShouldNotBeNull();

        It should_be_not_for_null = () => nullValue
                                                  .Not(r => r.Prop.Length != value.Prop.Length)
                                                  .ShouldBeNull();

        It should_be_do_for_value = () =>
                                        {
                                            string newValue = Pleasure.Generator.String();
                                            value.Do(fakeMaybe => { fakeMaybe.Prop = newValue; });
                                            value.Prop.ShouldEqual(newValue);
                                        };

        It should_be_do_for_null_value = () => Catch.Exception(() => nullValue.Do(fakeMaybe => { fakeMaybe.Prop = Pleasure.Generator.String(); })).ShouldBeNull();

        It should_be_has_for_value = () => value.Has().ShouldBeTrue();

        It should_be_has_for_null_value = () => nullValue.Has().ShouldBeFalse();

        It should_be_recovery_for_value = () =>
                                              {
                                                  var recovery = new FakeMaybe { Prop = Pleasure.Generator.String() };
                                                  value
                                                          .Recovery(recovery)
                                                          .ShouldNotBeTheSameAs(recovery);
                                              };

        It should_be_recovery_for_null_value = () =>
                                                   {
                                                       var recovery = new FakeMaybe { Prop = Pleasure.Generator.String() };
                                                       nullValue
                                                               .Recovery(() => recovery)
                                                               .ShouldBeTheSameAs(recovery);
                                                   };
    }
}