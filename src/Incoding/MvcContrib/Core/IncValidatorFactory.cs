namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using FluentValidation;
    using FluentValidation.Mvc;
    using Incoding.Block.IoC;

    #endregion

    public class IncValidatorFactory : IValidatorFactory
    {
        ////ncrunch: no coverage start

        #region Constructors

        public IncValidatorFactory()
        {
            FluentValidationModelValidatorProvider.Configure();
        }

        #endregion

        ////ncrunch: no coverage end

        #region IValidatorFactory Members

        public IValidator<T> GetValidator<T>()
        {
            return IoCFactory.Instance.TryResolve<IValidator<T>>();
        }

        public IValidator GetValidator(Type type)
        {
            var genericType = typeof(AbstractValidator<>).MakeGenericType(new[] { type });
            return IoCFactory.Instance.TryResolve<IValidator>(genericType);
        }

        #endregion
    }
}