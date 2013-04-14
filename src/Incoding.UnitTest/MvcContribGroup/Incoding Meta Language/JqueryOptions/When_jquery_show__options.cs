namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MvcContrib;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject("JqueryOptions")]
    public class When_jquery_show__options
    {
        It should_be_show_options_fast = () => JqueryShowOptions.Fast
                                                                .Should(options =>
                                                                            {
                                                                                options.Easing.ShouldEqual(JqueryEasing.InExpo);
                                                                                options.Effect.ShouldEqual(JuqeryEffect.Slide);
                                                                                options.Duration.ShouldEqual(50);
                                                                            });

        It should_be_show_options_middle = () => JqueryShowOptions.Middle
                                                                  .Should(options =>
                                                                              {
                                                                                  options.Easing.ShouldEqual(JqueryEasing.InExpo);
                                                                                  options.Effect.ShouldEqual(JuqeryEffect.Slide);
                                                                                  options.Duration.ShouldEqual(500);
                                                                              });

        It should_be_show_options_slow = () => JqueryShowOptions.Slow
                                                                .Should(options =>
                                                                            {
                                                                                options.Easing.ShouldEqual(JqueryEasing.InExpo);
                                                                                options.Effect.ShouldEqual(JuqeryEffect.Slide);
                                                                                options.Duration.ShouldEqual(2000);
                                                                            });
    }
}