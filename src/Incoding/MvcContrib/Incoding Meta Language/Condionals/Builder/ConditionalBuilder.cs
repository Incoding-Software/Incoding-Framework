namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Incoding.Extensions;

    #endregion

    public class ConditionalBuilder : IConditionalBinaryBuilder, IConditionalBuilder
    {
        #region Fields

        internal readonly List<ConditionalBase> conditionals = new List<ConditionalBase>();

        bool inverse;

        bool and = true;

        #endregion

        #region IConditionalBinaryBuilder Members

        public IConditionalBuilder And
        {
            get
            {
                this.inverse = false;
                this.and = true;
                return this;
            }
        }

        public IConditionalBuilder Or
        {
            get
            {
                this.inverse = false;
                this.and = false;
                return this;
            }
        }

        #endregion

        #region IConditionalBuilder Members

        public ConditionalBuilder Not
        {
            get
            {
                this.inverse = true;
                return this;
            }
        }

        public IConditionalBinaryBuilder Is(Expression<Func<bool>> expression)
        {
            Registry(new ConditionalIs(expression, this.and));
            return this;
        }

        public IConditionalBinaryBuilder Confirm(Selector selector)
        {
            string code = JavaScriptCodeTemplate.Conditional_Confirm.F(selector);
            Registry(new ConditionalEval(code, this.and));
            return this;
        }

        public IConditionalBinaryBuilder Eval(string code)
        {
            Registry(new ConditionalEval(code, this.and));
            return this;
        }


        public IConditionalBinaryBuilder Exist(Selector selector)
        {
            string code = selector is JquerySelector ? JavaScriptCodeTemplate.Conditional_Exists_Jquery_Selector.F(selector)
                                  : JavaScriptCodeTemplate.Conditional_Exists_Incoding_Selector.F(selector);
            Registry(new ConditionalEval(code, this.and));
            return this;
        }

        public IConditionalBinaryBuilder FormIsValid(Func<JquerySelector, JquerySelector> configuration)
        {
            return FormIsValid(configuration(Selector.Jquery));
        }

        public IConditionalBinaryBuilder FormIsValid(JquerySelector jquerySelector)
        {
            string code = JavaScriptCodeTemplate.Conditional_Is_Valid_Form.F(jquerySelector);
            Registry(new ConditionalEval(code, this.and));
            return this;
        }

        public void Registry(ConditionalBase conditional)
        {
            if (this.inverse)
                this.conditionals.Add(!conditional);
            else
                this.conditionals.Add(conditional);
        }

        public IConditionalBinaryBuilder Data<TModel>(Expression<Func<TModel, bool>> expression)
        {
            Registry(new ConditionalData<TModel>(expression, this.and));
            return this;
        }

        public IConditionalBinaryBuilder Support(ModernizrSupport modernizrSupport)
        {
            string classSupport = modernizrSupport
                    .ToStringLower()
                    .Replace(", ", " ")
                    .Replace("_", "-");

            string code = JavaScriptCodeTemplate.Conditional_ModernizrSupport.F(classSupport);
            Registry(new ConditionalEval(code, this.and));
            return this;
        }

        #endregion
    }
}