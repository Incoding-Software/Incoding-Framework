namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.ComponentModel;
    using Machine.Specifications.Annotations;

    #endregion

    public enum FakeEnum
    {
        [UsedImplicitly]
        En1 = 1, 

        [UsedImplicitly]
        En2 = 2, 

        [Description("En 3"), UsedImplicitly]
        En3 = 3
    }
}