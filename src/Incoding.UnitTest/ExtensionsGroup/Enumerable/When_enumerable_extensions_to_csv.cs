namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(EnumerableExtensions))]
    public class When_enumerable_extensions_to_csv
    {
        #region Fake classes

        class FakeCsv
        {
            #region Properties

            public string Property { get; set; }

            public int Property2 { get; set; }

            #endregion
        }

        #endregion

        #region Estabilish value

        static List<FakeCsv> collection;

        static string result;

        #endregion

        Establish establish = () =>
                                  {
                                      collection = Pleasure.ToList(new FakeCsv
                                                                       {
                                                                               Property = Pleasure.Generator.TheSameString(), 
                                                                               Property2 = Pleasure.Generator.TheSameNumber()
                                                                       });
                                  };

        Because of = () => { result = collection.ToCsv(); };

        It should_be_csv_string = () => result.ShouldEqual("Property, Property2\r\nTheSameString, 153\r\n");
    }
}