namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using FluentValidation.Results;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Machine.Specifications;

    #endregion

    public static class ShouldExtensions
    {
        #region Factory constructors

        public static bool IsEqualWeak<TActual, TExpected>(this TActual left, TExpected right, Action<ICompareFactoryDsl<TActual, TExpected>> action = null)
        {
            try
            {
                ShouldEqualWeak(left, right, action);
                return true;
            }
            catch (SpecificationException)
            {
                return false;
            }
        }

        public static void Should<TValue>(this TValue value, Action<TValue> action)
        {
            action(value);
        }

        public static void ShouldBeDate(this DateTime actual, DateTime expected)
        {
            actual.Date.ShouldEqual(expected.Date);
        }

        public static void ShouldBeDate(this DateTime? actual, DateTime? expected)
        {
            actual.HasValue.ShouldEqual(expected.HasValue);
            if (actual.HasValue && expected.HasValue)
                actual.Value.ShouldBeDate(expected.Value);
        }

        public static void ShouldBeFailure<TObject>(this ValidationResult result, Expression<Func<TObject, object>> member, params string[] errorMessages)
        {
            ShouldBeFailure(result, member.GetMemberName(), errorMessages);
        }

        public static void ShouldBeFailure(this ValidationResult result, string property, params string[] errorMessages)
        {
            var failures = result.Errors.Where(r => r.PropertyName.Equals(property)).ToList();
            int actualFailures = failures.Count;
            int expectedFailures = errorMessages.Length;

            if (actualFailures != expectedFailures)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Expected: {0} failure But was:  {1} failure".F(actualFailures, expectedFailures));
                stringBuilder.AppendLine(failures.Aggregate(string.Empty, (s, failure) => s += "{0}:{1};".F(failure.PropertyName, failure.ErrorMessage)));
                throw new SpecificationException(stringBuilder.ToString());
            }

            foreach (var validationFailure in failures)
            {
                errorMessages.Any(r =>
                                      {
                                          try
                                          {
                                              r.ShouldEqual(validationFailure.ErrorMessage.Replace("{PropertyName}", property));
                                              return true;
                                          }
                                          catch (SpecificationException exception)
                                          {
                                              Console.WriteLine(exception.Message);
                                              return false;
                                          }
                                      }).ShouldBeTrue();
            }
        }

        public static void ShouldBeKeyValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (!source.ContainsKey(key))
                throw new SpecificationException("Not found key {0} in dictionary".F(key));

            source[key].ShouldEqualWeak(value);
        }

        public static void ShouldBeTheSameString(this string actual)
        {
            actual.ShouldEqual(Pleasure.Generator.TheSameString());
        }

        public static void ShouldBeTime(this DateTime actual, DateTime expected)
        {
            actual.ShouldBeTime(expected.Hour, expected.Minute, expected.Second);
        }

        public static void ShouldBeTime(this DateTime actual, int hour, int minute, int second)
        {
            new TimeSpan(actual.Hour, actual.Minute, actual.Second).ShouldBeTime(hour, minute, second);
        }

        public static void ShouldBeTime(this TimeSpan actual, int hour, int minute, int second)
        {
            actual.Hours.ShouldEqual(hour);
            actual.Minutes.ShouldEqual(minute);
            actual.Seconds.ShouldEqual(second);
        }

        public static void ShouldBeTime(this TimeSpan actual, TimeSpan expected)
        {
            actual.ShouldBeTime(expected.Hours, expected.Minutes, expected.Seconds);
        }

        public static void ShouldEqualWeak<TActual, TExpected>(this TActual left, TExpected right, Action<ICompareFactoryDsl<TActual, TExpected>> action = null)
        {
            var factory = new CompareFactory<TActual, TExpected>();
            action.Do(r => r(factory));
            factory.Compare(left, right);
            if (!factory.IsCompare())
                throw new SpecificationException(factory.GetDifferencesAsString());
        }

        public static void ShouldEqualWeakDual<TActual, TExpected>(this TActual left, TExpected right, Action<ICompareFactoryDsl<TActual, TExpected>> leftAction = null, Action<ICompareFactoryDsl<TExpected, TActual>> rightAction = null)
        {
            left.ShouldEqualWeak(right, leftAction);
            right.ShouldEqualWeak(left, rightAction);
        }

        public static void ShouldEqualWeakEach<TActual, TExpected>(this IEnumerable<TActual> left, IEnumerable<TExpected> right, Action<ICompareFactoryDsl<TActual, TExpected>, int> action = null)
        {
            var leftList = left.ToList();
            var rightList = right.ToList();

            leftList.Count.ShouldEqual(rightList.Count);

            for (int i = 0; i < leftList.Count; i++)
            {
                int closeIndex = i;
                ShouldEqualWeak(leftList.ToList()[i], rightList.ToList()[i], factory => action.Do(r => r(factory, closeIndex)));
            }
        }

        public static void ShouldNotBeFailure<TObject>(this ValidationResult result, Expression<Func<TObject, object>> property)
        {
            ShouldNotBeFailure(result, property.GetMemberName());
        }

        public static void ShouldNotBeFailure(this ValidationResult result, string property)
        {
            var failures = result.Errors.Where(r => r.PropertyName.Equals(property)).ToList();
            if (failures.Count > 0)
                throw new SpecificationException("Actual not error for {0} but was {1}".F(property, failures.Aggregate(string.Empty, (s, failure) => s += "{0}:{1}".F(failure.PropertyName, failure.ErrorMessage))));
        }

        public static void ShouldNotBeKey<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            if (source.ContainsKey(key))
                throw new SpecificationException("Found key {0} in dictionary".F(key));
        }

        public static void ShouldSatisfy<TValue>(this TValue value, Func<TValue, bool> action)
        {
            action(value).ShouldBeTrue();
        }

        #endregion
    }
}