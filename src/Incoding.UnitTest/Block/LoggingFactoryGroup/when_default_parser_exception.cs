namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using Incoding.Block.Logging;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(DefaultParserException))]
    public class When_default_parser_exception
    {
        #region Estabilish value

        static DefaultParserException parser;

        static string result;

        #endregion

        Establish establish = () => { parser = new DefaultParserException(); };

        Because of = () => { result = parser.Parse(new ArgumentException("Message", "Param", new ApplicationException("Internal exception"))); };

        It should_be_not_empty = () => result.ShouldNotBeEmpty();

        It should_be_has_inner_exception = () => result.ShouldContain("Inner Exception:");

        It should_be_has_date = () => result.ShouldContain("Date:");

        It should_be_has_body = () => result.ShouldContain("Body:");
    }
}