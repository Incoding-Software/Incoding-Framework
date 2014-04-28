namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(UrlDispatcher))]
    public class When_url_dispatcher_verify_schema
    {
        #region Fake classes

        public class Fake
        {
            #region Properties

            public string Test { get; set; }

            #endregion
        }

        #endregion

        #region Establish value

        static UrlDispatcher urlDispatcher;

        #endregion

        Establish establish = () =>
                                  {
                                      urlDispatcher = new UrlDispatcher(null);
                                      UrlDispatcher.IsVerifySchema = true;
                                  };

        It should_be_push = () => Catch.Exception(() => urlDispatcher.Push<Fake>(new
                                                                                     {
                                                                                             Test = Pleasure.Generator.String(),
                                                                                             TestNotFound = Pleasure.Generator.String()
                                                                                     }))
                                       .ShouldBeOfType<ArgumentException>();

        It should_be_query = () => Catch.Exception(() => urlDispatcher.Query<Fake>(new
                                                                                       {
                                                                                               Test = Pleasure.Generator.String(),
                                                                                               TestNotFound = Pleasure.Generator.String()
                                                                                       }))
                                        .ShouldBeOfType<ArgumentException>();

        It should_be_model = () => Catch.Exception(() => urlDispatcher.Model<Fake>(new
                                                                                       {
                                                                                               Test = Pleasure.Generator.String(),
                                                                                               TestNotFound = Pleasure.Generator.String()
                                                                                       }))
                                        .ShouldBeOfType<ArgumentException>();
    }
}