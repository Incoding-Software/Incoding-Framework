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

        [Obsolete("Please use native C#", false)]
        public IConditionalBuilder And
        {
            get
            {
                this.inverse = false;
                this.and = true;
                return this;
            }
        }

        [Obsolete("Please use native C#", false)]
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

        [Obsolete("Please use native C#", false)]
        public ConditionalBuilder Not
        {
            get
            {
                this.inverse = true;
                return this;
            }
        }

        void IsRegistry(Expression expression, bool setAnd)
        {
            var nodeType = expression.NodeType;
            if (nodeType.IsAnyEquals(ExpressionType.AndAlso,
                                     ExpressionType.And,
                                     ExpressionType.Or,
                                     ExpressionType.OrElse))
            {
                var binary = expression as BinaryExpression;
                bool isAnd = nodeType.IsAnyEquals(ExpressionType.And, ExpressionType.AndAlso);
                IsRegistry(binary.Left, isAnd);
                IsRegistry(binary.Right, isAnd);
                return;
            }

            Registry(new ConditionalIs(expression, setAnd));
        }

        public IConditionalBinaryBuilder Is(Expression<Func<bool>> expression)
        {
            IsRegistry(expression.Body, this.and);
            return this;
        }

        [Obsolete("Use method Is instead", false)]
        public IConditionalBinaryBuilder Eval(string code)
        {
            Registry(new ConditionalEval(code, this.and));
            return this;
        }

        [Obsolete("Use method Is with Selector.Result")]
        public IConditionalBinaryBuilder Data<TModel>(Expression<Func<TModel, bool>> expression)
        {
            Registry(new ConditionalData<TModel>(expression, this.and));
            return this;
        }

        #endregion

        #region Api Methods

        public void Registry(ConditionalBase conditional)
        {
            if (this.inverse)
                this.conditionals.Add(!conditional);
            else
                this.conditionals.Add(conditional);
        }

        #endregion
    }
}