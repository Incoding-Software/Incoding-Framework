namespace Incoding.CQRS
{
    public class IncStructureResponse<T>
    {
        #region Constructors

        public IncStructureResponse(T value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public T Value { get; private set; }

        #endregion
    }
}