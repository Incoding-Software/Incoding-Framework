namespace Incoding.UnitTest
{
    using Incoding.Data;
    using Machine.Specifications;
    using Incoding.MSpecContrib;

    [Subject(typeof(DictionaryAsString))]
    public class When_dictionary_as_string_wiht_hard_value
    {
        #region Establish value

        static string sourceCollection;

        #endregion

        Establish establish = () =>
                                  {
                                      sourceCollection = string.Empty;
                                      value = "I. P. Miroshnichenko, I. A. Parinov, E. V. Rozhkov,V. P. Sizov, and S.-H. Chang. Optic Interference Means for Measurement of Displacements of the Control Object Surfaces, P. 162-163, I. A. Parinov, T. Somnuk, S. H. Chang (Eds.). Abstracts & Schedule of 2013 International Symposium on “Physics and Mechanics of New Materials and Underwater Applications” (NKMU Press: Kaohsiung, Taiwan, June 5-8, 2013). 2013. – 204 p.";
                                  };

        Because of = () => DictionaryAsString.Join(ref sourceCollection, "key", value);

        It should_be_correct_string = () => sourceCollection.ShouldEqual("key=I.+P.+Miroshnichenko%2c+I.+A.+Parinov%2c+E.+V.+Rozhkov%2cV.+P.+Sizov%2c+and+S.-H.+Chang.+Optic+Interference+Means+for+Measurement+of+Displacements+of+the+Control+Object+Surfaces%2c+P.+162-163%2c+I.+A.+Parinov%2c+T.+Somnuk%2c+S.+H.+Chang+(Eds.).+Abstracts+%26+Schedule+of+2013+International+Symposium+on+%e2%80%9cPhysics+and+Mechanics+of+New+Materials+and+Underwater+Applications%e2%80%9d+(NKMU+Press%3a+Kaohsiung%2c+Taiwan%2c+June+5-8%2c+2013).+2013.+%e2%80%93+204+p.&");

        It should_be_parse = () => DictionaryAsString.Parse(sourceCollection).ShouldBeKeyValue("key",value);

        static string value;
    }
}