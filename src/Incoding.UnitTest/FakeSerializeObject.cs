namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Runtime.Serialization;

    #endregion

    [Serializable, DataContract]
    public class FakeSerializeObject
    {
        #region Properties

        [DataMember]
        public string Ignore { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        #endregion
    }
}