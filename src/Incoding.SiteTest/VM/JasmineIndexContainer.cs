namespace Incoding.SiteTest.VM
{
    #region << Using >>

    using System.Collections.Generic;

    #endregion

    public class JasmineIndexContainer
    {
        #region Properties

        public string[] AllSupportedMeta { get; set; }

        public string[] AllSupportedConditional { get; set; }

        public List<string> IncSpecialBinds { get; set; }

        public string JqueryVersion { get; set; }

        #endregion
    }
}