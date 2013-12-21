namespace Incoding.SiteTest.VM
{
    using System.Collections.Generic;

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
        public string Value { get; set; }
        public bool Is { get; set; }

        public List<FakeModel> Inners { get; set; }
    }
}