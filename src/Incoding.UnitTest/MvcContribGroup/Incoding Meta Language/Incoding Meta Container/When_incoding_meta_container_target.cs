namespace Incoding.UnitTest.MvcContribGroup
{
    using Incoding.MvcContrib;
    using Machine.Specifications;

    [Subject(typeof(IncodingMetaContainer))]
    public class When_incoding_meta_container_target
    {
        #region Establish value

        static IncodingMetaContainer meta;

        #endregion

        Establish establish = () =>
                                  {
                                      meta = new IncodingMetaContainer();                                      
                                      meta.Target = Selector.Jquery.Self();
                                  };

        Because of = () => meta.Target = Selector.Jquery.Id("Id");

        It should_be_add_target = () => meta.Target.ToString().ShouldEqual("$(this.self).add('#Id')");
    }
}