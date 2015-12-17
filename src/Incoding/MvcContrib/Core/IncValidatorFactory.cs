namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;
    using System.Linq;
    using FluentValidation;
    using FluentValidation.Mvc;
    using Incoding.Block.IoC;
    using Incoding.Extensions;

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
            return GetValidator(typeof(T)) as IValidator<T>;
        }

        public IValidator GetValidator(Type type)
        {
            var genericType = typeof(AbstractValidator<>).MakeGenericType(new[] { type });

            var validator = IoCFactory.Instance.ResolveAll<IValidator>(genericType);
            var count = validator.Count;
            if (count > 1)
                throw new IndexOutOfRangeException("{0} Validators found for instance {1}".F(count, genericType));
            return validator.FirstOrDefault();
        }

        #endregion
    }
}