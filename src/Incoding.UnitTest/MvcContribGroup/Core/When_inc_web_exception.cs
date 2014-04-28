namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncWebException))]
    public class When_inc_web_exception
    {
        #region Fake classes

        class FakeModel
        {
            #region Properties

            public string Prop { get; set; }

            public string Prop2 { get; set; }

            #endregion
        }

        #endregion

        It should_be_for = () => IncWebException
                                         .For<FakeModel>(r => r.Prop, Pleasure.Generator.TheSameString())
                                         .Errors.ShouldBeKeyValue("Prop", new List<string> { Pleasure.Generator.TheSameString() });


        It should_be_for_multiple = () => IncWebException
                                                  .For<FakeModel>(r => r.Prop, "Regular expression")
                                                  .Also<FakeModel>(r => r.Prop, "Required")
                                                  .Also<FakeModel>(r => r.Prop2, "Date")
                                                  .Should(exception =>
                                                              {
                                                                  exception.Errors.ShouldBeKeyValue("Prop", new List<string>
                                                                                                                {
                                                                                                                        "Regular expression", 
                                                                                                                        "Required"
                                                                                                                });
                                                                  exception.Errors.ShouldBeKeyValue("Prop2", new List<string>
                                                                                                                 {
                                                                                                                         "Date"
                                                                                                                 });
                                                              });

        It should_be_for_server = () => IncWebException
                                                .ForServer(Pleasure.Generator.TheSameString())
                                                .Errors.ShouldBeKeyValue("Server", new List<string> { Pleasure.Generator.TheSameString() });
    }
}