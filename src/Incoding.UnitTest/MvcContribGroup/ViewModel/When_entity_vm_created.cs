namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(EntityVm))]
    public class When_entity_vm_created
    {
        #region Fake classes

        class FakeEntityVm : EntityVm
        {
            #region Constructors

            public FakeEntityVm(IEntity entity)
                    : base(entity) { }

            #endregion
        }

        #endregion

        #region Establish value

        static IEntity entity;

        static FakeEntityVm vm;

        #endregion

        Establish establish = () => { entity = Pleasure.MockStrictAsObject<IEntity>(mock => mock.SetupGet(r => r.Id).Returns(Pleasure.Generator.TheSameString())); };

        Because of = () => { vm = new FakeEntityVm(entity); };

        It should_be_id = () => vm.Id.ShouldEqual(Pleasure.Generator.TheSameString());

        It should_be_assembly_qualified_named = () => vm.AssemblyQualifiedName.ShouldContain("Castle.Proxies.IEntityProxy, DynamicProxyGenAssembly2");
    }
}