namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using Incoding.Quality;

    #endregion

    public class Context_invent_factory
    {
        #region Fake classes

        protected class FakeGenerateObject
        {
            #region Properties

            [IgnoreInvent("Test")]
            public string IgnoreValueByAttr { get; set; }

            public string StrValue { get; set; }

            public bool BoolValue { get; set; }
            
            public int IntValue { get; set; }

            public int? IntValueNullable { get; set; }

            public float FloatValue { get; set; }

            public float? FloatValueNullable { get; set; }

            public decimal DecimalValue { get; set; }

            public decimal? DecimalValueNullable { get; set; }

            public long LongValue { get; set; }

            public long? LongValueNullable { get; set; }

            public byte ByteValue { get; set; }

            public HttpPostedFileBase HttpPostFileValue { get; set; }

            public byte? ByteValueNullable { get; set; }

            public DateTime DateTimeValue { get; set; }

            public TimeSpan TimeSpanValue { get; set; }

            public FakeGenerateObject Fake { get; set; }

            public byte[] ByteArray { get; set; }

            public int[] IntArray { get; set; }

            public string[] StrArray { get; set; }

            public Stream StreamValue { get; set; }

            public Dictionary<string, string> DictionaryValue { get; set; }

            public Dictionary<string, object> DictionaryObjectValue { get; set; }

            public DayOfWeek DayOfWeek { get; set; }

            public double DoubleValue { get; set; }

            public double? DoubleValueNullable { get; set; }

            public char CharValue { get; set; }

            public char? CharValueNullable { get; set; }

            public string CallbackValue { get; set; }

            public DayOfWeek EnumValue { get; set; }

            public object ObjValue { get; set; }

            public Guid GuidValue { get; set; }

            public Guid? GuidValueNullable { get; set; }

            #endregion
        }

        #endregion
    }
}