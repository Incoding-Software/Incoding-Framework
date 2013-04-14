namespace Incoding.MSpecContrib
{
    using Moq;

    #region << Using >>

    

    #endregion

    public static class PleasureSpyEx
    {
        #region Factory constructors

        public static void Exactly(this Mock<ISpy> spy, int count)
        {
            spy.Verify(r => r.Is(), Times.Exactly(count));
        }

        public static void Never(this Mock<ISpy> spy)
        {
            spy.Verify(r => r.Is(), Times.Never());
        }

        public static void Once(this Mock<ISpy> spy)
        {
            spy.Verify(r => r.Is(), Times.Once());
        }

        #endregion
    }
}