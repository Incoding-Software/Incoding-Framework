namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections;
    using System.Web.Mvc;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingResult))]
    public class When_incoding_result_error_with_model_state : BehaviorsJsonResultSpec
    {
        #region Estabilish value

        static ModelStateDictionary modelState;

        static string emailName;

        static string emailErrorMessage;

        static string addressName;

        #endregion

        Establish establish = () =>
                                  {
                                      modelState = new ModelStateDictionary();

                                      addressName = Pleasure.Generator.String();
                                      modelState.Add(addressName, new ModelState());

                                      emailName = Pleasure.Generator.String();
                                      emailErrorMessage = Pleasure.Generator.String();
                                      modelState.AddModelError(emailName, emailErrorMessage);
                                  };

        Because of = () => { result = IncodingResult.Error(modelState); };

        Behaves_like<BehaviorsJsonResultSpec> should_be_verify_common;

        It should_be_verify_result = () =>
                                         {
                                             var data = result.Data as IncodingResult.JsonData;
                                             data.success.ShouldBeFalse();
                                             data.redirectTo.ShouldBeEmpty();

                                             var enumerator = (data.data as IEnumerable).GetEnumerator();

                                             enumerator.MoveNext();
                                             (enumerator.Current as IncodingResult.JsonModelStateData).ShouldEqualWeak(new
                                                                                                                           {
                                                                                                                                   name = addressName, 
                                                                                                                                   isValid = true, 
                                                                                                                                   errorMessage = string.Empty
                                                                                                                           });
                                             enumerator.MoveNext();
                                             (enumerator.Current as IncodingResult.JsonModelStateData).ShouldEqualWeak(new
                                                                                                                           {
                                                                                                                                   name = emailName, 
                                                                                                                                   isValid = false, 
                                                                                                                                   errorMessage = emailErrorMessage
                                                                                                                           });
                                         };
    }
}