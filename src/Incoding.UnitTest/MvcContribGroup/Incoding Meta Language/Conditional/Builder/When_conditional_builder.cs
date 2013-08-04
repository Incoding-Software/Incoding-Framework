namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Web.Mvc;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ConditionalBuilder))]
    public class When_conditional_builder
    {
        #region Fundamental

        It should_be_not = () => new ConditionalBuilder()
                                         .Not
                                         .Confirm(Selector.Value(Pleasure.Generator.TheSameString()))
                                         .GetFirst()
                                         .GetData()
                                         .ShouldEqualWeak(new
                                                              {
                                                                      type = ConditionalOfType.Eval.ToString(), 
                                                                      inverse = true, 
                                                                      and = true, 
                                                                      code = "window.confirm(this.tryGetVal('TheSameString'));"
                                                              }, dsl => dsl.IncludeAllFields());

        #endregion

        #region Imp

        It should_be_confirm = () => new ConditionalBuilder()
                                             .Confirm(Selector.Value(Pleasure.Generator.TheSameString()))
                                             .GetFirst()
                                             .GetData()
                                             .ShouldEqualWeak(new
                                                                  {
                                                                          type = ConditionalOfType.Eval.ToString(), 
                                                                          inverse = false, 
                                                                          and = true, 
                                                                          code = "window.confirm(this.tryGetVal('TheSameString'));"
                                                                  }, dsl => dsl.IncludeAllFields());


        It should_be_support_one = () => new ConditionalBuilder()
                                                 .Support(ModernizrSupport.BoxShadow)
                                                 .GetFirst()
                                                 .GetData()
                                                 .ShouldEqualWeak(new
                                                                      {
                                                                              type = ConditionalOfType.Eval.ToString(), 
                                                                              inverse = false, 
                                                                              and = true, 
                                                                              code = "$('html').hasClass('boxshadow');"
                                                                      }, dsl => dsl.IncludeAllFields());

        It should_be_support_more = () => new ConditionalBuilder()
                                                  .Support(ModernizrSupport.BoxShadow | ModernizrSupport.CssAnimations)
                                                  .GetFirst()
                                                  .GetData()
                                                  .ShouldEqualWeak(new
                                                                       {
                                                                               type = ConditionalOfType.Eval.ToString(), 
                                                                               inverse = false, 
                                                                               and = true, 
                                                                               code = "$('html').hasClass('boxshadow cssanimations');"
                                                                       }, dsl => dsl.IncludeAllFields());

        It should_be_support_special = () => new ConditionalBuilder()
                                                     .Support(ModernizrSupport.No_Download)
                                                     .GetFirst()
                                                     .GetData()
                                                     .ShouldEqualWeak(new
                                                                          {
                                                                                  type = ConditionalOfType.Eval.ToString(), 
                                                                                  inverse = false, 
                                                                                  and = true, 
                                                                                  code = "$('html').hasClass('no-download');"
                                                                          }, dsl => dsl.IncludeAllFields());

        It should_be_exists_jquery_selector = () => new ConditionalBuilder()
                                                            .Exist(Selector.Jquery.HasAttribute(HtmlAttribute.Accept.ToStringLower()))
                                                            .GetFirst()
                                                            .GetData()
                                                            .ShouldEqualWeak(new
                                                                                 {
                                                                                         type = ConditionalOfType.Eval.ToString(), 
                                                                                         inverse = false, 
                                                                                         and = true, 
                                                                                         code = "$('[accept]').length != 0;", 
                                                                                 }, dsl => dsl.IncludeAllFields());

        It should_be_exists_incoding_selector = () => new ConditionalBuilder()
                                                              .Exist(Selector.Incoding.HashUrl())
                                                              .GetFirst()
                                                              .GetData()
                                                              .ShouldEqualWeak(new
                                                                                   {
                                                                                           type = ConditionalOfType.Eval.ToString(), 
                                                                                           inverse = false, 
                                                                                           and = true, 
                                                                                           code = "!ExecutableHelper.IsNullOrEmpty(this.tryGetVal('@@@@root@@@@'))", 
                                                                                   }, dsl => dsl.IncludeAllFields());

        It should_be_is_valid_form = () => new ConditionalBuilder()
                                                   .FormIsValid(selector => selector.Self().Closest(r => r.Tag(HtmlTag.Form)))
                                                   .GetFirst()
                                                   .GetData()
                                                   .ShouldEqualWeak(new
                                                                        {
                                                                                type = ConditionalOfType.Eval.ToString(), 
                                                                                code = "$(this.self).closest('form').valid();", 
                                                                                and = true, 
                                                                                inverse = false, 
                                                                        }, dsl => dsl.IncludeAllFields());

        It should_be_eval = () => new ConditionalBuilder()
                                          .Eval(Pleasure.Generator.TheSameString())
                                          .GetFirst()
                                          .GetData()
                                          .ShouldEqualWeak(new
                                                               {
                                                                       type = ConditionalOfType.Eval.ToString(), 
                                                                       inverse = false, 
                                                                       and = true, 
                                                                       code = "TheSameString"
                                                               }, dsl => dsl.IncludeAllFields());

        It should_be_is = () => new ConditionalBuilder()
                                        .Is(() => Selector.Jquery.Id("id") == Selector.Value(true))
                                        .GetFirst()
                                        .GetData()
                                        .ShouldEqualWeak(new
                                                             {
                                                                     type = ConditionalOfType.Eval.ToString(), 
                                                                     inverse = false, 
                                                                     code = "ExecutableHelper.Compare(this.tryGetVal($('#id')), this.tryGetVal('True'), 'equal');", 
                                                                     and = true
                                                             }, dsl => dsl.IncludeAllFields());   
        

        It should_be_data = () => new ConditionalBuilder()
                                          .Data<IEntity>(r => r.Id == "123")
                                          .GetFirst()
                                          .GetData()
                                          .ShouldEqualWeak(new
                                                               {
                                                                       type = ConditionalOfType.Data.ToString(), 
                                                                       inverse = false, 
                                                                       property = "Id", 
                                                                       value = "123", 
                                                                       method = "equal", 
                                                                       and = true
                                                               }, dsl => dsl.IncludeAllFields());

        #endregion
    }
}