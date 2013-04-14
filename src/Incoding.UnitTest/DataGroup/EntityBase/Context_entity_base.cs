namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using Incoding.Data;

    #endregion

    public class Context_entity_base
    {
        #region Nested classes

        protected class FakeEntityBase : IncEntityBase
        {
            #region Constructors

            public FakeEntityBase(Guid id)
            {
                Id = id.ToString();
            }

            public FakeEntityBase()
            {
                Id = string.Empty;
            }

            #endregion
        }

        #endregion
    }
}