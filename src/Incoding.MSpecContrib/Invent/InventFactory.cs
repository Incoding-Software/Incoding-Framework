namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Incoding.Extensions;
    using Incoding.Maybe;

    #endregion

    public partial class InventFactory<T> where T : new()
    {
        #region Fields

        readonly Dictionary<string, Func<object>> tuning = new Dictionary<string, Func<object>>();

        readonly List<string> ignoreProperties = new List<string>();

        readonly List<string> empties = new List<string>();

        readonly List<Action<T>> callbacks = new List<Action<T>>();

        #endregion

        void VerifyUniqueProperty(string property)
        {
            Action throwException = () => { throw new ArgumentException("Property should be unique in all dictionary", property); };

            if (this.tuning.ContainsKey(property))
                throwException();

            if (this.ignoreProperties.Contains(property))
                throwException();
        }

        public T CreateEmpty()
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
            var allSetProperties = typeof(T).GetProperties(bindingFlags).Where(r => r.CanWrite);
            var instance = new T();

            foreach (var property in allSetProperties)
                property.SetValue(instance, GenerateValueOrEmpty(property.PropertyType, true), null);

            return instance;
        }

        public T Create()
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
            var allSetProperties = typeof(T)
                    .GetProperties(bindingFlags)
                    .Where(r => !r.HasAttribute<IgnoreFieldInventAttribute>())
                    .Where(r => r.CanWrite);

            var instance = new T();

            foreach (var property in allSetProperties)
            {
                if (this.ignoreProperties.Any(r => r.Equals(property.Name)))
                    continue;

                object value = null;

                if (this.tuning.ContainsKey(property.Name))
                    value = this.tuning[property.Name].Invoke();

                if (this.empties.Contains(property.Name))
                {
                    value = GenerateValueOrEmpty(property.PropertyType, true);
                    if (value == null)
                        throw new ArgumentException("Can't found empty value for type {0} by field {1}".F(property.PropertyType, property.Name));
                }

                if (value == null && !this.tuning.ContainsKey(property.Name) && !this.empties.Contains(property.Name))
                    value = GenerateValueOrEmpty(property.PropertyType, false);

                property.SetValue(instance, value, null);
            }

            this.callbacks.DoEach(action => action(instance));
            return instance;
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Positive false")]
        object GenerateValueOrEmpty(Type propertyType, bool isEmpty)
        {
            object value = null;

            if (propertyType.IsEnum)
                value = isEmpty ? 0 : Pleasure.Generator.EnumAsInt(propertyType);
            else if (propertyType.IsAnyEquals(typeof(string), typeof(object)))
                value = isEmpty ? string.Empty : Pleasure.Generator.String();
            else if (propertyType == typeof(bool))

                    // ReSharper disable SimplifyConditionalTernaryExpression
                value = isEmpty ? false : Pleasure.Generator.Bool();
                    
                    
                    
                    
                    // ReSharper restore SimplifyConditionalTernaryExpression
            else if (propertyType == typeof(int))
                value = isEmpty ? default(int) : Pleasure.Generator.PositiveNumber(1);
            else if (propertyType == typeof(long))
                value = isEmpty ? default(long) : (long)Pleasure.Generator.PositiveNumber(1);
            else if (propertyType == typeof(float))
                value = isEmpty ? default(float) : Pleasure.Generator.PositiveFloating();
            else if (propertyType == typeof(decimal))
                value = isEmpty ? default(decimal) : Pleasure.Generator.PositiveDecimal();
            else if (propertyType == typeof(double))
                value = isEmpty ? default(double) : (double)Pleasure.Generator.PositiveFloating();
            else if (propertyType == typeof(byte))
                value = isEmpty ? default(byte) : (byte)Pleasure.Generator.PositiveNumber();
            else if (propertyType == typeof(char))
                value = isEmpty ? default(char) : Pleasure.Generator.String()[0];
            else if (propertyType == typeof(DateTime))
                value = isEmpty ? new DateTime() : Pleasure.Generator.DateTime();
            else if (propertyType == typeof(TimeSpan))
                value = isEmpty ? new TimeSpan() : Pleasure.Generator.TimeSpan();
            else if (propertyType.IsAnyEquals(typeof(Stream), typeof(MemoryStream)))
                value = isEmpty ? Pleasure.Generator.Stream(0) : Pleasure.Generator.Stream();
            else if (propertyType == typeof(byte[]))
                value = isEmpty ? Pleasure.ToArray<byte>() : Pleasure.Generator.Bytes();
            else if (propertyType == typeof(int[]))
                value = isEmpty ? Pleasure.ToArray<int>() : Pleasure.ToArray(Pleasure.Generator.PositiveNumber(1));
            else if (propertyType == typeof(string[]))
                value = isEmpty ? Pleasure.ToArray<string>() : Pleasure.ToArray(Pleasure.Generator.String());
            else if (propertyType == typeof(Dictionary<string, string>))
                value = isEmpty ? new Dictionary<string, string>() : Pleasure.ToDynamicDictionary<string>(new { key = Pleasure.Generator.String() });
            else if (propertyType == typeof(Dictionary<string, object>))
                value = isEmpty ? new Dictionary<string, object>() : Pleasure.ToDynamicDictionary<string>(new { key = Pleasure.Generator.String() }).ToDictionary(r => r.Key, r => (object)r.Value);

            return value;
        }
    }
}