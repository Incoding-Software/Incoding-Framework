namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using FluentValidation.Results;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Machine.Specifications.Annotations;

    #endregion

    [Subject(typeof(ShouldExtensions))]
    public class When_should_extensions
    {
        #region Fake classes

        class FakeCompareObject
        {
            #region Properties

            public string Fake { get; set; }

            public string Fake2 { get; set; }

            #endregion
        }

        class FakeSpecification : Specification<IEntity>
        {
            #region Fields

            [UsedImplicitly]
            readonly string id;

            #endregion

            #region Constructors

            public FakeSpecification(string id)
            {
                this.id = id;
            }

            #endregion

            public override Expression<Func<IEntity, bool>> IsSatisfiedBy()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        public string Property { get; set; }

        It should_be_is_weak = () => Pleasure.Generator.Invent<FakeCompareObject>().IsEqualWeak(Pleasure.Generator.Invent<FakeCompareObject>()).ShouldBeFalse();

        It should_be_weak_each_with_forward_with_index = () =>
                                                             {
                                                                 var left = Pleasure.Generator.Invent<FakeCompareObject>();
                                                                 var right = new FakeCompareObject { Fake = left.Fake2, Fake2 = left.Fake };
                                                                 var rightArray = Pleasure.ToReadOnly(right);
                                                                 Pleasure.ToList(left).ShouldEqualWeakEach(rightArray, (factory, i) => factory
                                                                                                                                               .ForwardToValue(r => r.Fake, rightArray[i].Fake2)
                                                                                                                                               .ForwardToValue(r => r.Fake2, rightArray[i].Fake));
                                                             };

        It should_be_key_value = () => Pleasure
                                               .ToDictionary(new KeyValuePair<string, int>(Pleasure.Generator.TheSameString(), Pleasure.Generator.TheSameNumber()))
                                               .ShouldBeKeyValue(Pleasure.Generator.TheSameString(), Pleasure.Generator.TheSameNumber());

        It should_be_not_key = () => Pleasure
                                             .ToDictionary(new KeyValuePair<string, int>(Pleasure.Generator.TheSameString(), 1))
                                             .ShouldNotBeKey(Pleasure.Generator.String());

        It should_be_not_key_with_exists_key = () => Catch
                                                             .Exception(() => Pleasure
                                                                                      .ToDictionary(new KeyValuePair<string, int>(Pleasure.Generator.TheSameString(), 1))
                                                                                      .ShouldNotBeKey(Pleasure.Generator.TheSameString()))
                                                             .ShouldBeOfType<SpecificationException>();

        It should_be_key_value_with_wrong_key = () => Catch
                                                              .Exception(() => Pleasure
                                                                                       .ToDictionary(new KeyValuePair<string, int>(Pleasure.Generator.TheSameString(), Pleasure.Generator.TheSameNumber()))
                                                                                       .ShouldBeKeyValue(Pleasure.Generator.String(), Pleasure.Generator.TheSameNumber()))
                                                              .ShouldBeOfType<SpecificationException>();

        It should_be_key_value_with_wrong_value = () => Catch
                                                                .Exception(() => Pleasure
                                                                                         .ToDictionary(new KeyValuePair<string, int>(Pleasure.Generator.TheSameString(), Pleasure.Generator.TheSameNumber()))
                                                                                         .ShouldBeKeyValue(Pleasure.Generator.TheSameString(), Pleasure.Generator.PositiveNumber()))
                                                                .ShouldBeOfType<SpecificationException>();

        It should_be_date = () => DateTime.Now.ShouldBeDate(DateTime.Now.AddMinutes(2));

        It should_be_date_nullable = () =>
                                         {
                                             DateTime? leftNullable = DateTime.Now;
                                             var rightNullable = leftNullable;
                                             leftNullable.ShouldBeDate(rightNullable);
                                         };

        It should_be_date_nullable_with_wrong_has_value = () => Catch.Exception(() =>
                                                                                    {
                                                                                        DateTime? leftNullable = DateTime.Now;
                                                                                        DateTime? rightNullable = null;
                                                                                        leftNullable.ShouldBeDate(rightNullable);
                                                                                    }).ShouldBeOfType<SpecificationException>();

        It should_be_time_with_time_span = () => new TimeSpan(1, 2, 3).ShouldBeTime(1, 2, 3);

        It should_be_time_with_date = () => new DateTime(1, 2, 3, 4, 5, 6).ShouldBeTime(4, 5, 6);

        It should_be_time_with_date_and_date = () =>
                                                   {
                                                       var time = new DateTime(1, 2, 3, 4, 5, 6);
                                                       time.ShouldBeTime(time);
                                                   };

        It should_be_time_with_time_span_and_time_span = () =>
                                                             {
                                                                 var timeSpan = new TimeSpan(1, 2, 3);
                                                                 timeSpan.ShouldBeTime(timeSpan);
                                                             };

        It should_be_conditional = () => Pleasure.Generator.TheRuCulture().Name.ShouldEqual("ru-RU");

        It should_be_equal_weak_dictionary = () =>
                                                 {
                                                     IDictionary<string, int> left = Pleasure.ToDynamicDictionary<int>(new { value = 2 });
                                                     IDictionary<string, int> right = Pleasure.ToDynamicDictionary<int>(new { value = 2 });
                                                     left.ShouldEqualWeak(right);
                                                 };

        It should_be_equal_weak_specification = () =>
                                                    {
                                                        var spec1 = new FakeSpecification(Pleasure.Generator.TheSameString());
                                                        var spec2 = new FakeSpecification(Pleasure.Generator.TheSameString());
                                                        spec1.ShouldEqualWeak(spec2);
                                                    };

        It should_not_be_equal_weak_specification = () =>
                                                        {
                                                            var spec1 = new FakeSpecification(Pleasure.Generator.String());
                                                            var spec2 = new FakeSpecification(Pleasure.Generator.String());
                                                            spec1.ShouldEqualWeak(spec2);
                                                        };

        It should_be_equal_weak_dual = () =>
                                           {
                                               var left = new { Field = 1 };
                                               var right = new { Field = 1 };
                                               left.ShouldEqualWeakDual(right);
                                           };

        It should_be_not_failure = () =>
                                       {
                                           var valResult = new ValidationResult();
                                           valResult.Errors.Add(new ValidationFailure("Property", Pleasure.Generator.TheSameString()));
                                           string customMessage = Pleasure.Generator.String();
                                           valResult.Errors.Add(new ValidationFailure("Property", customMessage));
                                           valResult.ShouldBeFailure<When_should_extensions>(r => r.Property, Pleasure.Generator.TheSameString(), customMessage);
                                       };

        It should_be_failure_by_property = () =>
                                               {
                                                   var valResult = new ValidationResult();
                                                   valResult.Errors.Add(new ValidationFailure("AnyProperty", Pleasure.Generator.TheSameString()));
                                                   Catch.Exception(() => valResult.ShouldBeFailure<When_should_extensions>(r => r.Property, Pleasure.Generator.TheSameString())).ShouldBeOfType<SpecificationException>();
                                               };

        It should_be_failure_by_count = () =>
                                            {
                                                var valResult = new ValidationResult();
                                                valResult.Errors.Add(new ValidationFailure("Property", Pleasure.Generator.TheSameString()));
                                                string error = Pleasure.Generator.String();
                                                valResult.Errors.Add(new ValidationFailure("Property", error));
                                                var exception = Catch.Exception(() => valResult.ShouldBeFailure<When_should_extensions>(r => r.Property, Pleasure.Generator.TheSameString()));
                                                exception.ShouldBeOfType<SpecificationException>();

                                                var st = new StringBuilder();
                                                st.AppendLine("Expected: 2 failure But was:  1 failure");
                                                st.AppendLine("Property:TheSameString;Property:{0};".F(error));
                                                exception.Message.ShouldEqual(st.ToString());
                                            };

        It should_be_failure_without_error = () =>
                                                 {
                                                     var validationResult = new ValidationResult();
                                                     var errorMessages = Pleasure.ToArray(Pleasure.Generator.String(), Pleasure.Generator.String(), Pleasure.Generator.String());
                                                     var exception = Catch.Exception(() => validationResult.ShouldBeFailure<When_should_extensions>(r => r.Property, errorMessages));
                                                     exception.ShouldBeOfType<SpecificationException>();
                                                     exception.Message.ShouldContain("Expected: {0} failure But was:  {1} failure".F(validationResult.Errors.Count, errorMessages.Length));
                                                 };

        It should_not_be_failure_wrong = () =>
                                             {
                                                 var valResult = new ValidationResult(new List<ValidationFailure>
                                                                                          {
                                                                                                  new ValidationFailure("Property", Pleasure.Generator.TheSameString()), 
                                                                                                  new ValidationFailure("Property", Pleasure.Generator.String())
                                                                                          });
                                                 Catch.Exception(() => valResult.ShouldNotBeFailure<When_should_extensions>(r => r.Property)).ShouldBeOfType<SpecificationException>();
                                             };

        It should_not_be_failure = () => new ValidationResult()
                                                 .ShouldNotBeFailure<When_should_extensions>(r => r.Property);

        It should_be_the_same_string = () => Pleasure.Generator.TheSameString().ShouldBeTheSameString();
    }
}