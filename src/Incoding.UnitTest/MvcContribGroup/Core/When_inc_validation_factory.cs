namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using FluentValidation;
    using Incoding.Block.IoC;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(IncValidatorFactory))]
    public class When_inc_validation_factory
    {
        #region Fake classes

        class FakeValidation { }

        #endregion

        #region Establish value

        static IncValidatorFactory incValidationFactory;

        static Mock<IIoCProvider> iocProvider;

        #endregion

        Establish establish = () =>
                                  {
                                      incValidationFactory = new IncValidatorFactory();
                                      iocProvider = Pleasure.Mock<IIoCProvider>();
                                      IoCFactory.Instance.Initialize(init => init.WithProvider(iocProvider.Object));
                                  };

        Because of = () => incValidationFactory.GetValidator<FakeValidation>();

        It should_be_resolve = () => iocProvider.Verify(r => r.TryGet<IValidator>(typeof(AbstractValidator<FakeValidation>)));
    }
}