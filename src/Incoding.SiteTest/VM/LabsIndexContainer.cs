namespace Incoding.SiteTest.VM
{
    using System.Collections.Generic;
    using Incoding.MvcContrib;

    public class LabsIndexContainer
    {
        public enum TestEnum
        {
            Value,
            Value2,
            Value3
        }

        #region Properties

        public TestEnum DropId { get; set; }

        #endregion
    }

    public class FakeModel
    {
        public KeyValueVm Vm { get; set; }
        public string Value { get; set; }
        public bool Is { get; set; }

        public List<FakeModel> Inners { get; set; }
        public FakeModel Inner { get; set; }
        public List<string> Strings { get; set; }
    }
}