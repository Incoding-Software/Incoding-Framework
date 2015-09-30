namespace Incoding.MSpecContrib
{
    #region << Using >>

    using Machine.Specifications;

    #endregion

    public class InternalSpecificationException : SpecificationException
    {
        #region Constructors

        public InternalSpecificationException(string message)
                : base(message) { }

        #endregion
    }
}