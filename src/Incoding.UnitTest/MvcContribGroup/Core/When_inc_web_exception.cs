namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
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

            #endregion
        }

        #endregion

  
        It should_be_for = () => IncWebException
                                         .For<FakeModel>(r => r.Prop, Pleasure.Generator.TheSameString())
                                         .Should(exception =>
                                                     {
                                                         exception.Message.ShouldEqual(Pleasure.Generator.TheSameString());
                                                         exception.Property.ShouldEqual("Prop");
                                                     });

        It should_be_for_input = () => IncWebException
                                               .ForInput<FakeModel>(r => r.Prop, Pleasure.Generator.TheSameString())
                                               .Should(exception =>
                                                           {
                                                               exception.Message.ShouldEqual(Pleasure.Generator.TheSameString());
                                                               exception.Property.ShouldEqual("input.Prop");
                                                           });

        It should_be_for_server = () => IncWebException
                                                .ForServer(Pleasure.Generator.TheSameString())
                                                .Should(exception =>
                                                            {
                                                                exception.Message.ShouldEqual(Pleasure.Generator.TheSameString());
                                                                exception.Property.ShouldEqual("Server");
                                                            });
    }
}