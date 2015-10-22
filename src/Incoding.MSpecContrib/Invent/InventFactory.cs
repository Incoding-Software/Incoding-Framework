namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.Quality;
    using Machine.Specifications.Annotations;
 
    #endregion

    public partial class InventFactory<T>
    {
        const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;

        #region Fields

        readonly Dictionary<string, Func<object>> tunings = new Dictionary<string, Func<object>>();

        readonly List<string> ignoreProperties = new List<string>();

        readonly List<string> empties = new List<string>();

        readonly List<Action<T>> callbacks = new List<Action<T>>();

        bool isMuteCtor;

        #endregion

        #region Api Methods

        public T CreateEmpty()
        {
            
            var allSetProperties = typeof(T).GetProperties(bindingFlags).Where(r => r.CanWrite);
            var instance = Activator.CreateInstance<T>();

            foreach (var property in allSetProperties)
                property.SetValue(instance, GenerateValueOrEmpty(property.PropertyType, true), null);

            return instance;
        }

        public T Create()
        {
            var instanceType = typeof(T);

            if (instanceType.IsPrimitive() || instanceType.IsAnyEquals(typeof(SqlConnection), typeof(byte[])))
                return (T)GenerateValueOrEmpty(instanceType, false);

            if (instanceType.IsImplement<IEnumerable>())
            {
                var itemType = instanceType.IsGenericType ? instanceType.GetGenericArguments()[0] : instanceType.GetElementType();
                var collections = Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType)) as IList;
                for (int i = 0; i < Pleasure.Generator.PositiveNumber(minValue: 1, maxValue: 5); i++)
                    collections.Add(Pleasure.Generator.Invent(itemType));

                if (instanceType == typeof(ReadOnlyCollection<>).MakeGenericType(itemType))
                    return (T)Activator.CreateInstance(typeof(ReadOnlyCollection<>).MakeGenericType(itemType), new[] { collections });
                if (instanceType == typeof(List<>).MakeGenericType(itemType))
                    return (T)collections;

                var array = Array.CreateInstance(itemType, collections.Count);
                for (int index = 0; index < collections.Count; index++)
                {
                    var item = collections[index];
                    array.SetValue(item, index);
                }
                object res = array;
                return (T)res;
            }

            return CreateInstance();
        }

        public T CreateInstance()
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.NonPublic;

            var allPrivateMembers = typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            var members = typeof(T)
                    .GetMembers(bindingFlags)
                    .Where(r =>
                           {
                               var isNotInTuning = !this.tunings.ContainsKey(r.Name);
                               if (r.HasAttribute<IgnoreInventAttribute>() && isNotInTuning)
                                   return false;

                               if (allPrivateMembers.Any(info => info.Name == r.Name) && isNotInTuning)
                                   return false;

                               var prop = r as PropertyInfo;
                               if (prop != null && prop.CanWrite)
                                   return true;

                               var field = r as FieldInfo;
                               if (field != null)
                                   return true;

                               return false;
                           })
                    .ToList();

            var dictionary = new Dictionary<string, Type>();
            for (int i = 0; i < members.Count; i++)
            {
                var memberInfo = members[i];
                string memberName = memberInfo.Name;
                var declaringType = memberInfo is PropertyInfo ? ((PropertyInfo)memberInfo).PropertyType : ((FieldInfo)memberInfo).FieldType;

                if (dictionary.ContainsKey(memberName))
                {
                    if (declaringType != typeof(T) || declaringType == typeof(T).BaseType)
                        members.RemoveAt(i);
                }
                else
                    dictionary.Add(memberName, declaringType);
            }

            var instance = Activator.CreateInstance<T>();

            foreach (var member in members)
            {
                if (ignoreProperties.Any(r => r.Equals(member.Name)))
                    continue;

                var type = member is PropertyInfo ? ((PropertyInfo)member).PropertyType : ((FieldInfo)member).FieldType;
                var value = instance.TryGetValue(member.Name);
                var defValue = type.IsValueType ? Activator.CreateInstance(type) : null;
                bool isHasCtorValue = (value != null && !value.Equals(defValue)) && !isMuteCtor;

                if (tunings.ContainsKey(member.Name))
                    value = tunings[member.Name].Invoke();

                if (empties.Contains(member.Name))
                {
                    value = GenerateValueOrEmpty(type, true);
                    if (value == null)
                        throw new ArgumentException("Can't found empty value for type {0} by field {1}".F(type, member.Name));
                }

                if (!isHasCtorValue && !tunings.ContainsKey(member.Name) && !empties.Contains(member.Name))
                    value = GenerateValueOrEmpty(type, false);

                instance.SetValue(member.Name, value);
            }
            callbacks.DoEach(action => action(instance));
            return instance;
        }

        #endregion

        void VerifyUniqueProperty(string property)
        {
            Action throwException = () => { throw new ArgumentException("Property should be unique in all dictionary", property); };

            if (tunings.ContainsKey(property))
                throwException();

            if (ignoreProperties.Contains(property))
                throwException();
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Positive false")]
        object GenerateValueOrEmpty(Type propertyType, bool isEmpty)
        {
            object value = null;
            bool isNullable = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            propertyType = isNullable ? propertyType.GetGenericArguments()[0] : propertyType;

            if (propertyType.IsEnum)
                value = isEmpty ? 0 : Enum.Parse(propertyType, Pleasure.Generator.EnumAsInt(propertyType).ToString(), true);
            else if (propertyType.IsAnyEquals(typeof(string), typeof(object)))
                value = isEmpty ? string.Empty : Pleasure.Generator.String();
            else if (propertyType == typeof(bool) || propertyType == typeof(bool?))
                    // ReSharper disable SimplifyConditionalTernaryExpression
                value = isEmpty ? false : Pleasure.Generator.Bool();                                                                                                    
                    // ReSharper restore SimplifyConditionalTernaryExpression
            else if (propertyType.IsAnyEquals(typeof(int)))
                value = isEmpty ? default(int) : Pleasure.Generator.PositiveNumber(1);
            else if (propertyType.IsAnyEquals(typeof(long)))
                value = isEmpty ? default(long) : (long)Pleasure.Generator.PositiveNumber(1);
            else if (propertyType.IsAnyEquals(typeof(float)))
                value = isEmpty ? default(float) : Pleasure.Generator.PositiveFloating();
            else if (propertyType.IsAnyEquals(typeof(decimal)))
                value = isEmpty ? default(decimal) : Pleasure.Generator.PositiveDecimal();
            else if (propertyType.IsAnyEquals(typeof(double)))
                value = isEmpty ? default(double) : Pleasure.Generator.PositiveDouble();
            else if (propertyType.IsAnyEquals(typeof(byte)))
                value = isEmpty ? default(byte) : (byte)Pleasure.Generator.PositiveNumber();
            else if (propertyType == typeof(char))
                value = isEmpty ? default(char) : Pleasure.Generator.String()[0];
            else if (propertyType.IsAnyEquals(typeof(DateTime)))
                value = isEmpty ? new DateTime() : Pleasure.Generator.DateTime();
            else if (propertyType.IsAnyEquals(typeof(TimeSpan)))
                value = isEmpty ? new TimeSpan() : Pleasure.Generator.TimeSpan();
            else if (propertyType.IsAnyEquals(typeof(Stream), typeof(MemoryStream)))
                value = isEmpty ? Pleasure.Generator.Stream(0) : Pleasure.Generator.Stream();
            else if (propertyType == typeof(byte[]))
                value = isEmpty ? Pleasure.ToArray<byte>() : Pleasure.Generator.Bytes();
            else if (propertyType == typeof(Guid))
                value = isEmpty ? Guid.Empty : Guid.NewGuid();
            else if (propertyType == typeof(int[]))
                value = isEmpty ? Pleasure.ToArray<int>() : Pleasure.ToArray(Pleasure.Generator.PositiveNumber(1));
            else if (propertyType == typeof(string[]))
                value = isEmpty ? Pleasure.ToArray<string>() : Pleasure.ToArray(Pleasure.Generator.String());
            else if (propertyType.IsAnyEquals(typeof(HttpPostedFile), typeof(HttpPostedFileBase)))
                value = isEmpty ? null : Pleasure.Generator.HttpPostedFile();
            else if (propertyType == typeof(Dictionary<string, string>))
                value = isEmpty ? new Dictionary<string, string>() : Pleasure.ToDynamicDictionary<string>(new { key = Pleasure.Generator.String() });
            else if (propertyType == typeof(Dictionary<string, object>))
                value = isEmpty ? new Dictionary<string, object>() : Pleasure.ToDynamicDictionary<string>(new { key = Pleasure.Generator.String() }).ToDictionary(r => r.Key, r => (object)r.Value);
            else if (propertyType == typeof(SqlConnection))
                value = new SqlConnection(@"Data Source={0};Database={1};Integrated Security=true;".F(Pleasure.Generator.String(length: 5), Pleasure.Generator.String(length: 5)));

            return isNullable ? Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(propertyType), value) : value;
        }
    }
}