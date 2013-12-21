namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

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

        [Obsolete("Use method Is instead", false)]
        public IConditionalBinaryBuilder Eval(string code)
        {
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

        #endregion
    }
}