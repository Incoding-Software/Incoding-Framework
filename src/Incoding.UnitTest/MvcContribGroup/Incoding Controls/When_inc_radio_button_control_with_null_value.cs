namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncHiddenControl<,>))]
    public class When_inc_radio_button_control_with_null_value : Context_inc_control
    {
        #region Establish value

        static Exception exception;

        #endregion

        Establish establish = () => mockHtmlHelper.StubModel(new FakeModel { Prop = "Male" });

        Because of = () =>
                         {
                             exception = Catch.Exception(() => new IncodingHtmlHelperFor<FakeModel, object>(mockHtmlHelper.Original, r => r.Prop)
                                                                       .RadioButton()
                                                                       .ToHtmlString());
                         };

        It should_be_render = () => exception.Message.ShouldContain("Argument Value can't be null");
    }
}