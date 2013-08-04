namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DictionaryExtensions))]
    public class When_set_value_with_exists
    {
        #region Estabilish value

        const string key = "Key";

        static Dictionary<string, string> dictionary;

        #endregion

        Establish establish = () =>
                                  {
                                      dictionary = new Dictionary<string, string>
                                                       {
                                                               { key, "Value" }
                                                       };
                                  };

        Because of = () => dictionary.Set(key, "NewValue");

        It should_be_replace_value = () => dictionary.ShouldBeKeyValue(key, "NewValue");
    }
}