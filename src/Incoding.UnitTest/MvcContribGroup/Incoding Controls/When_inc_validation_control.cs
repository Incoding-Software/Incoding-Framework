namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncTextBoxControl<,>))]
    public class When_inc_validation_control : Context_inc_control
    {
        #region Estabilish value

        static IncValidationControl control;

        #endregion

        Establish establish = () =>
                                  {
                                      mockHtmlHelper
                                              .StubFieldValidation("Prop", new FieldValidationMetadata
                                                                               {
                                                                                       FieldName = "Prop"
                                                                               });

                                      Expression<Func<FakeModel, string>> expression = fakeModel => fakeModel.Prop;
                                      control = new IncValidationControl(mockHtmlHelper.Original, expression);
                                  };

        Because of = () => { result = control.ToHtmlString(); };

        It should_be_render = () => result.ToString()
                                          .ShouldEqual("<span class=\"field-validation-valid\" id=\"Prop_validationMessage\"></span>");
    }
}