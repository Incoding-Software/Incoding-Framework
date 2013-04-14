namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Globalization;
    using System.Threading;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    ////ncrunch: no coverage start
    [UsedImplicitly]
    public class MSpecAssemblyContext : IAssemblyContext
    {
        #region IAssemblyContext Members

        public void OnAssemblyStart()
        {
            var currentUiCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = currentUiCulture;
            Thread.CurrentThread.CurrentCulture = currentUiCulture;
        }

        public void OnAssemblyComplete()
        {
            NHibernatePleasure.StopAllSession();
        }

        #endregion
    }

    ////ncrunch: no coverage end
}