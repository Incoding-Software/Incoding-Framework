namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DictionaryExtensions))]
    public class When_add_or_get_async
    {
        private static Dictionary<string, string> dictionary;

        private static Exception exception;

        Establish establish = () => { dictionary = new Dictionary<string, string>(); };

        Because of = () => exception = Catch.Exception(() => { Pleasure.MultiThread.Do(() => dictionary.GetOrAdd(Pleasure.Generator.TheSameString(), () => Pleasure.Generator.String()).ShouldNotBeEmpty(), 1000); });

        It should_be_not_exception = () => exception.ShouldBeNull();
    }
}