namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ValueSelector : Selector
    {
        public ValueSelector(object value)
                : this("||value*{0}||".F(value)) { }

        protected ValueSelector(Selector selector)
                : base(selector) { }
    }
}