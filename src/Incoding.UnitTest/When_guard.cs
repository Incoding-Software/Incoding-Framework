namespace Incoding.UnitTest
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(Guard))]
    public class When_guard
    {
        #region Establish value

        protected static string Argument { get; set; }

        protected static IEnumerable<string> ArrayArgument { get; set; }

        #endregion

        It should_be_not_null_or_white_space = () => Catch.Exception(() => Guard.NotNullOrWhiteSpace("Argument", " ")).ShouldBeOfType<ArgumentException>();

        It should_be_conditional = () => Catch.Exception(() => Guard.IsConditional("Argument", false)).ShouldBeOfType<ArgumentException>();

        It should_be_not_null = () => Catch.Exception(() => Guard.NotNull<When_guard>("Argument", null)).ShouldBeOfType<ArgumentException>();

        It should_be_not_null_or_empty_string = () => Catch.Exception(() => Guard.NotNullOrEmpty("Argument", string.Empty)).ShouldBeOfType<ArgumentException>();
      
        It should_be_not_null_or_empty_collection_with_null_collection = () => Catch.Exception(() => Guard.NotNullOrEmpty("ArrayArgument", null)).ShouldBeOfType<ArgumentException>();
    }
}