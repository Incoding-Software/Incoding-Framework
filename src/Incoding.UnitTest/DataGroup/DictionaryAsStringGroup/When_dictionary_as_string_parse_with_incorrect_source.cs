namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Data;
    using Machine.Specifications;using Incoding.MSpecContrib;

    #endregion

    [Subject(typeof(DictionaryAsString))]
    public class When_dictionary_as_string_parse_with_incorrect_source
    {
        #region Estabilish value

        static string source;

        static IDictionary<string, string> dictionary;

        static Exception exception;

        #endregion

        Establish establish = () => { source = "asw&aws"; };

        Because of = () => { exception = Catch.Exception(() => dictionary = DictionaryAsString.Parse(source)); };

        It should_be_invalid_operation = () => exception.ShouldBeOfType<ArgumentException>();

        It should_be_null_dictionary = () => dictionary.ShouldBeNull();
    }
}