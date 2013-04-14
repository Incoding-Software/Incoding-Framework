namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;
    using Incoding.Extensions;
    using Incoding.Quality;
    using Machine.Specifications;

    #endregion

    public class CompareFactory<TActual, TExpected> : ICompareFactoryDsl<TActual, TExpected>
    {
        #region Fields

        readonly List<string> differences = new List<string>();

        readonly Dictionary<string, string> forwards = new Dictionary<string, string>();

        readonly Dictionary<string, object> forwardsToValue = new Dictionary<string, object>();

        readonly List<string> ignoreProperties = new List<string>();

        readonly Dictionary<string, Action<TActual>> forwardsToPredicate = new Dictionary<string, Action<TActual>>();

        bool includeNotPublic;

        #endregion

        #region ICompareFactoryDsl<TActual,TExpected> Members

        [DebuggerStepThrough]
        public ICompareFactoryDsl<TActual, TExpected> Forward(Expression<Func<TActual, object>> actualProp, Expression<Func<TExpected, object>> expectedProp)
        {
            Guard.NotNull("actualProp", actualProp);
            Guard.NotNull("expectedProp", expectedProp);

            return Forward((string)actualProp.GetMemberName(), expectedProp);
        }

        public ICompareFactoryDsl<TActual, TExpected> Forward(string actualProp, Expression<Func<TExpected, object>> expectedProp)
        {
            Guard.NotNullOrWhiteSpace("actualProp", actualProp);

            VerifyUniqueProperty(actualProp);
            this.forwards.Set(actualProp, expectedProp.GetMemberName());
            return this;
        }

        [DebuggerStepThrough]
        public ICompareFactoryDsl<TActual, TExpected> ForwardToValue(Expression<Func<TActual, object>> actualProp, object value)
        {
            Guard.NotNull("actualProp", actualProp);
            return ForwardToValue((string)actualProp.GetMemberName(), value);
        }

        public ICompareFactoryDsl<TActual, TExpected> ForwardToValue(string actualProp, object value)
        {
            Guard.NotNullOrWhiteSpace("actualProp", actualProp);

            VerifyUniqueProperty(actualProp);
            this.forwardsToValue.Set(actualProp, value);
            return this;
        }

        [DebuggerStepThrough]
        public ICompareFactoryDsl<TActual, TExpected> ForwardToAction(Expression<Func<TActual, object>> actualProp, Action<TActual> predicate)
        {
            Guard.NotNull("actualProp", actualProp);
            return ForwardToAction((string)actualProp.GetMemberName(), predicate);
        }

        public ICompareFactoryDsl<TActual, TExpected> ForwardToAction(string actualProp, Action<TActual> predicate)
        {
            Guard.NotNullOrWhiteSpace("actualProp", actualProp);

            VerifyUniqueProperty(actualProp);
            this.forwardsToPredicate.Set(actualProp, predicate);
            return this;
        }

        [DebuggerStepThrough]
        public ICompareFactoryDsl<TActual, TExpected> Ignore(Expression<Func<TActual, object>> actualIgnore, string reason)
        {
            Guard.NotNull("actualIgnore", actualIgnore);
            return Ignore((string)actualIgnore.GetMemberName(), reason);
        }

        public ICompareFactoryDsl<TActual, TExpected> IgnoreBecauseNotUse(Expression<Func<TActual, object>> actualIgnore)
        {
            return Ignore(actualIgnore, "Not use");
        }

        public ICompareFactoryDsl<TActual, TExpected> IgnoreBecauseCalculate(Expression<Func<TActual, object>> actualIgnore)
        {
            return Ignore(actualIgnore, "Calculate");
        }

        public ICompareFactoryDsl<TActual, TExpected> IgnoreBecauseRoot(Expression<Func<TActual, object>> actualIgnore)
        {
            return Ignore(actualIgnore, "Root");
        }

        [DebuggerStepThrough]
        public ICompareFactoryDsl<TActual, TExpected> Ignore(string actualIgnore, string reason)
        {
            Guard.NotNull("actualIgnore", actualIgnore);

            VerifyUniqueProperty(actualIgnore);
            this.ignoreProperties.Add(actualIgnore);
            return this;
        }

        public ICompareFactoryDsl<TActual, TExpected> IncludeAllFields()
        {
            this.includeNotPublic = true;
            return this;
        }

        #endregion

        public bool IsCompare()
        {
            return this.differences.Count == 0;
        }

        public string GetDifferencesAsString()
        {
            return this.differences.Aggregate(string.Empty, (aggregate, current) => aggregate += current);
        }

        void VerifyUniqueProperty(string property)
        {
            Action throwException = () => { throw new SpecificationException(SpecificationMessageRes.CompareFactory_Has_Many_Configuration.F(property)); };

            if (this.forwards.ContainsKey(property))
                throwException();

            if (this.forwardsToValue.ContainsKey(property))
                throwException();

            if (this.ignoreProperties.Contains(property))
                throwException();

            if (this.forwardsToPredicate.ContainsKey(property))
                throwException();
        }

        public void Compare(TActual actual, TExpected expected)
        {
            if (actual == null && expected == null)
                return;

            if (actual == null || expected == null)
            {
                FixedDifferent(SpecificationMessageRes.CompareFactory_Actual_Null_Or_Expected_Null.F(actual == null ? "is" : "not", expected == null ? "is" : "not"));
                return;
            }

            if (actual.GetType().IsTypicalType() || actual is IEnumerable || (actual.GetType().IsImplement<IDbConnection>() && expected.GetType().IsImplement<IDbConnection>()))
            {
                InternalShouldEqual(actual, expected, "Actual", "Expected");
                return;
            }

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
            if (this.includeNotPublic)
                bindingFlags = bindingFlags | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.NonPublic;

            var actualProperties = actual
                    .GetType()
                    .GetMembers(bindingFlags)
                    .Where(r => r is PropertyInfo || r is FieldInfo)
                    .ToArray();

            foreach (var actualMember in actualProperties)
            {
                if (actualMember.HasAttribute<IgnoreFieldCompareAttribute>())
                    continue;

                if (this.ignoreProperties.Any(r => r.Equals(actualMember.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                if (this.ignoreProperties.Any(r => actualMember.Name.Contains("<{0}>".F(r))))
                    continue;

                if (this.forwardsToPredicate.ContainsKey(actualMember.Name))
                {
                    this.forwardsToPredicate[actualMember.Name].Invoke(actual);
                    continue;
                }

                var actualValue = actual.TryGetValue(actualMember.Name);

                if (this.forwardsToValue.ContainsKey(actualMember.Name))
                {
                    InternalShouldEqual(actualValue, this.forwardsToValue[actualMember.Name], actualMember.Name, actualMember.Name);
                    continue;
                }

                string expectedPropName = this.forwards.GetOrDefault(actualMember.Name, actualMember.Name);
                var expectedMember = expected.GetType().GetMember(expectedPropName, bindingFlags).LastOrDefault();
                if (expectedMember == null)
                {
                    FixedDifferent(SpecificationMessageRes.CompareFactory_Not_Found_Property.F(actualMember.Name, expected.GetType().Name));
                    continue;
                }

                InternalShouldEqual(actualValue, expected.TryGetValue(expectedMember.Name), actualMember.Name, expectedPropName);
            }
        }

        // ReSharper disable PossibleMultipleEnumeration        
        // ReSharper disable PossibleNullReferenceException
        void InternalShouldEqual(object actual, object expected, string actualName, string expectedName)
        {
            try
            {
                if (actual == null && expected == null)
                    return;

                if (actual == null || expected == null)
                {
                    string actualMessage = (actual == null) ? "null" : actual.ToString();
                    string expectedMessage = (expected == null) ? "null" : expected.ToString();
                    FixedDifferent(CreateCompareActualVsExpected(actualName, expectedName, actualMessage, expectedMessage));
                    return;
                }

                if (actual.GetType() != expected.GetType())
                {
                    FixedDifferent(CreateCompareActualVsExpected(actualName, expectedName, "type {0}".F(actual.GetType()), "type {0}".F(expected.GetType())));
                    return;
                }

                if (actual is IEnumerable && !actual.GetType().IsAnyEquals(typeof(string)))
                {
                    var actualEnumerable = actual as IEnumerable;
                    var expectedEnumerable = expected as IEnumerable;

                    InternalShouldEqual(actualEnumerable.Cast<object>().Count(), expectedEnumerable.Cast<object>().Count(), "Count from {0}".F(actualName), "Count from {0}".F(expectedName));

                    var actualEnumerator = actualEnumerable.GetEnumerator();

                    var expectedEnumerator = expectedEnumerable.GetEnumerator();

                    while (actualEnumerator.MoveNext() && expectedEnumerator.MoveNext())
                        InternalShouldEqual(actualEnumerator.Current, expectedEnumerator.Current, "Item from {0}".F(actualName), "Item from {0}".F(expectedName));
                    return;
                }

                if (actual.GetType().IsImplement<IDbConnection>() && expected.GetType().IsImplement<IDbConnection>())
                {
                    var actualConnection = actual as IDbConnection;
                    var expectedConnection = expected as IDbConnection;
                    actualConnection.ConnectionString.ShouldEqual(expectedConnection.ConnectionString);
                    return;
                }

                if (actual.GetType().IsTypicalType() && expected.GetType().IsTypicalType())
                {
                    actual.ShouldEqual(expected);
                    return;
                }

                Console.WriteLine("Start {0}", actual.GetType());
                actual.ShouldEqualWeak(expected, dsl =>
                                                     {
                                                         if (actual.GetType().BaseType.FullName.Contains("LinqSpecs.Specification"))
                                                             dsl.IncludeAllFields();
                                                     });
            }
            catch (SpecificationException)
            {
                FixedDifferent(CreateCompareActualVsExpected(actualName, expectedName, actual, expected));
            }
        }

        // ReSharper restore PossibleMultipleEnumeration
        // ReSharper restore PossibleNullReferenceException
        string CreateCompareActualVsExpected(string actualName, string expectedName, object actualMessage, object expectedMessage)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Compare  {0} with {1}".F(actualName, expectedName));
            stringBuilder.AppendLine("Actual   {0}".F(actualMessage));
            stringBuilder.AppendLine("Expected {0}".F(expectedMessage));
            return stringBuilder.ToString();
        }

        void FixedDifferent(string message)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(message);
            stringBuilder.AppendLine("________________________________________");
            this.differences.Add(stringBuilder.ToString());
        }
    }
}