namespace Incoding.MvcContrib
{
    public class ConditionalEval : ConditionalBase
    {
        #region Fields

        protected string code;

        #endregion

        #region Constructors

        public ConditionalEval(string code, bool and)
                : base(ConditionalOfType.Eval.ToString(), and)
        {
            this.code = code;
        }

        #endregion

        public override object GetData()
        {
            return new
                       {
                               this.type, 
                               this.inverse, 
                               this.code, 
                               this.and
                       };
        }
    }
}