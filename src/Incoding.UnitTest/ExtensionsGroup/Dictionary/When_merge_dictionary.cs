namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DictionaryExtensions))]
    public class When_merge_dictionary
    {
        #region Establish value

        static Dictionary<string, object> src;

        #endregion

        Establish establish = () => { src = new Dictionary<string, object> { { "key", "value" }, { "key3", "value3" } }; };

        Because of = () => src.Merge(new { key2 = "value2", key3 = "newValue3" });

        It should_be_copy_new_value = () => src.ShouldBeKeyValue("key2", "value2");

        It should_be_replace_old_value_from_dist = () => src.ShouldBeKeyValue("key3", "newValue3");

        It should_be_store_different_value = () => src.ShouldBeKeyValue("key", "value");
    }
}