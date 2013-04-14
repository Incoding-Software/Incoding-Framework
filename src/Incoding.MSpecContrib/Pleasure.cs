namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Routing;
    using Incoding.Data;
    using Incoding.Extensions;
    using Incoding.Maybe;
    using Incoding.MvcContrib;
    using Machine.Specifications;
    using Moq;
    using It = Moq.It;

    #endregion

    public static class Pleasure
    {
        #region To

        public static T[] ToArray<T>(params T[] args)
        {
            return ToList(args).ToArray();
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(params KeyValuePair<TKey, TValue>[] keyValuePair)
        {
            return keyValuePair.ToDictionary(r => r.Key, r => r.Value);
        }

        public static Dictionary<string, TValue> ToDynamicDictionary<TValue>(object attr)
        {
            Guard.NotNull("attr", attr);

            var routeValueDictionary = new RouteValueDictionary(attr);
            return routeValueDictionary.ToDictionary(r => r.Key, r => (TValue)r.Value);
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> ToEnumerable<T>(params T[] agrs)
        {
            return ToList(agrs).AsEnumerable();
        }

        [DebuggerStepThrough]
        public static List<T> ToList<T>(params T[] args)
        {
            return args
                    .If(r => r.Length > 0)
                    .ReturnOrDefault(obj => new List<T>(obj), new List<T>());
        }

        [DebuggerStepThrough]
        public static IQueryable<T> ToQueryable<T>(params T[] args)
        {
            return ToList(args).AsQueryable();
        }

        [DebuggerStepThrough]
        public static ReadOnlyCollection<T> ToReadOnly<T>(params T[] args)
        {
            return ToList(args).AsReadOnly();
        }

        #endregion

        #region Mock

        [DebuggerStepThrough]
        public static Mock<TMock> Mock<TMock>(Action<Mock<TMock>> configure = null) where TMock : class
        {
            var res = new Mock<TMock>();
            configure.Do(action => action(res));
            return res;
        }

        [DebuggerStepThrough]
        public static TMock MockAsObject<TMock>(Action<Mock<TMock>> configure = null) where TMock : class
        {
            return Mock(configure).Object;
        }

        [DebuggerStepThrough]
        public static Mock<TMock> MockStrict<TMock>(Action<Mock<TMock>> configure = null) where TMock : class
        {
            var res = new Mock<TMock>(MockBehavior.Strict);
            configure.Do(action => action(res));
            return res;
        }

        [DebuggerStepThrough]
        public static TMock MockStrictAsObject<TMock>(Action<Mock<TMock>> configure = null) where TMock : class
        {
            return MockStrict(configure).Object;
        }

        [DebuggerStepThrough]
        public static Mock<ISpy> Spy(Action<Mock<ISpy>> configure = null)
        {
            return Mock(configure);
        }

        #endregion

        #region Action and Func 

        public static void Do(Action<int> action, int countDo)
        {
            for (int i = 0; i < countDo; i++)
                action(i);
        }

        public static void Do10(Action<int> action)
        {
            Do(action, 10);
        }

        public static void Do3(Action<int> action)
        {
            Do(action, 3);
        }

        public static T DoFunc<T>(Func<T> func, int countFunc)
        {
            var res = default(T);
            for (int i = 0; i < countFunc; i++)
                res = func();

            return res;
        }

        public static T DoFunc10<T>(Func<T> func)
        {
            return DoFunc(func, 10);
        }

        #endregion

        #region Sleep helper

        [DebuggerStepThrough]
        public static DateTime NowPlush100Milliseconds()
        {
            return DateTime.Now.AddMilliseconds(100);
        }

        [DebuggerStepThrough]
        public static void Sleep1000Milliseconds()
        {
            SleepMilliseconds(1000);
        }

        [DebuggerStepThrough]
        public static void Sleep100Milliseconds()
        {
            SleepMilliseconds(100);
        }

        [DebuggerStepThrough]
        public static void Sleep50Milliseconds()
        {
            SleepMilliseconds(50);
        }

        [DebuggerStepThrough]
        public static void SleepMilliseconds(int milliseconds)
        {
            Thread.Sleep(new TimeSpan(0, 0, 0, 0, milliseconds));
        }

        #endregion

        #region Nested classes

        public static class MultiThread
        {
            #region Factory constructors

            [DebuggerStepThrough]
            public static ManualResetEvent Do(Action action, int countThread)
            {
                var manualResetEvent = new ManualResetEvent(false);

                var threads = new List<Thread>();

                for (int i = 0; i < countThread; i++)
                {
                    int currentIndex = i;
                    var thread = new Thread(() =>
                                                {
                                                    action.Invoke();
                                                    Debug.Print("Start thread ¹ {0}".F(currentIndex));
                                                    manualResetEvent.Set();
                                                });
                    threads.Add(thread);
                }

                for (int i = threads.Count - 1; i != -1; i--)
                    threads[i].Start();

                manualResetEvent.Reset();
                return manualResetEvent;
            }

            [DebuggerStepThrough]
            public static ManualResetEvent Do10(Action action)
            {
                return Do(action, 10);
            }

            #endregion
        }

        public static class Generator
        {
            #region Factory constructors

            public static string Base64(int size = 100)
            {
                return Convert.ToBase64String(Bytes(size));
            }

            [DebuggerStepThrough]
            public static bool Bool()
            {
                return PositiveNumber(0, 100) >= 50;
            }

            [DebuggerStepThrough]
            public static byte[] Bytes(int size = 100)
            {
                var random = new Random(GetSeed());
                var bytes = new byte[size];
                for (int index = 0; index < bytes.Length; ++index)
                    bytes[index] = (byte)random.Next();

                return bytes;
            }

            [DebuggerStepThrough]
            public static DateTime DateTime()
            {
                var random = new Random(GetSeed());
                int year = random.Next(1800, 2100);
                int months = random.Next(1, 12);
                int days = random.Next(1, 25);

                var timeSpan = TimeSpan();
                return new DateTime(year, months, days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            }

            [DebuggerStepThrough]
            public static string Email()
            {
                string randomString = Path.GetRandomFileName().Replace(".", string.Empty);
                return "{0}@mail.com".F(randomString);
            }

            [DebuggerStepThrough]
            public static MemoryStream EmptyStream()
            {
                return new MemoryStream();
            }

            [DebuggerStepThrough]
            public static TEnum Enum<TEnum>()
            {
                return (TEnum)System.Enum.Parse(typeof(TEnum), EnumAsInt(typeof(TEnum)).ToString(), true);
            }

            [DebuggerStepThrough]
            public static int EnumAsInt(Type enumType)
            {
                var enumValues = System.Enum.GetValues(enumType)
                                       .Cast<int>()
                                       .OrderBy(r => r)
                                       .ToList();

                int startPosition = enumValues.Count == 1 ? 0 : 1;
                int randomIndexEnumValues = PositiveNumber(startPosition, enumValues.Count - 1);

                return enumValues[randomIndexEnumValues];
            }

            [DebuggerStepThrough]
            public static string GuidAsString()
            {
                return Guid.NewGuid().ToString();
            }

            public static HttpMemoryPostedFile HttpMemoryPostedFile(Stream stream = null, string fileName = "fileName", string contentType = "contentType")
            {
                if (stream == null)
                    stream = Stream();

                return new HttpMemoryPostedFile(stream, fileName, contentType);
            }

            public static HttpPostedFileBase HttpPostedFile(Stream stream = null, string fileName = "fileName", string contentType = "contentType")
            {
                if (stream == null)
                    stream = Stream();

                return MockAsObject<HttpPostedFileBase>(mock =>
                                                            {
                                                                mock.SetupGet(r => r.ContentLength).Returns((int)stream.Length);
                                                                mock.SetupGet(r => r.ContentType).Returns(contentType);
                                                                mock.SetupGet(r => r.FileName).Returns(fileName);
                                                                mock.SetupGet(r => r.InputStream).Returns(stream);
                                                            });
            }

            [DebuggerStepThrough]
            public static T Invent<T>(Action<IInventFactoryDsl<T>> action = null) where T : new()
            {
                var inventFactory = new InventFactory<T>();
                action.Do(r => r(inventFactory));

                return inventFactory.Create();
            }

            [DebuggerStepThrough]
            public static T InventEmpty<T>() where T : new()
            {
                var inventFactory = new InventFactory<T>();
                return inventFactory.CreateEmpty();
            }

            [DebuggerStepThrough]
            public static T InventEntity<T>(Action<IInventFactoryDsl<T>> action = null) where T : IEntity, new()
            {
                return Invent<T>(factory =>
                                     {
                                         factory.Ignore(r => r.Id, "Because id generate automate");
                                         action.Do(r => r(factory));
                                     });
            }

            [DebuggerStepThrough]
            public static KeyValuePair<string, string> KeyValuePair()
            {
                return new KeyValuePair<string, string>(String(length: 10), String());
            }

            [DebuggerStepThrough]
            public static decimal PositiveDecimal()
            {
                var rng = new Random(GetSeed());

                decimal positiveDecimal = Math.Round(new decimal(rng.Next(999999999), 0, 0, false, (byte)rng.Next(2)) * (decimal)0.5, 2);
                return positiveDecimal < 0 ? -positiveDecimal : positiveDecimal;
            }

            public static double PositiveDouble()
            {
                double positiveDouble = new Random(GetSeed()).NextDouble() * 1.8;
                return positiveDouble < 0 ? -positiveDouble : positiveDouble;
            }

            public static float PositiveFloating()
            {
                float positiveFloating = (float)(new Random(GetSeed()).NextDouble() * 1.8);
                return positiveFloating < 0 ? -positiveFloating : positiveFloating;
            }

            [DebuggerStepThrough]
            public static int PositiveNumber(int minValue = 0, int maxValue = int.MaxValue)
            {
                int positiveNumber = new Random(GetSeed()).Next(minValue, maxValue);
                return positiveNumber < 0 ? -positiveNumber : positiveNumber;
            }

            [DebuggerStepThrough]
            public static Stream Stream(int size = 10)
            {
                return new MemoryStream(Bytes(size));
            }

            [DebuggerStepThrough]
            public static string String(int length = 50)
            {
                string res = string.Empty;
                while (res.Length < length)
                    res += Email();

                return res.Substring(0, length);
            }

            [DebuggerStepThrough]
            public static DateTime The20120406Noon()
            {
                return new DateTime(2012, 4, 16);
            }

            [DebuggerStepThrough]
            public static CultureInfo TheRuCulture()
            {
                return new CultureInfo("ru-RU");
            }

            [DebuggerStepThrough]
            public static int TheSameNumber()
            {
                return 153;
            }

            [DebuggerStepThrough]
            public static string TheSameString()
            {
                return "TheSameString";
            }

            [DebuggerStepThrough]
            public static TimeSpan TimeSpan()
            {
                return new TimeSpan(PositiveNumber(1, 24), PositiveNumber(1, 60), PositiveNumber(1, 60));
            }

            [DebuggerStepThrough]
            public static Uri Uri(object queryString = null)
            {
                return new Uri(Url(queryString));
            }

            [DebuggerStepThrough]
            public static string Url(object queryString = null)
            {
                return "http://sample.com"
                        .F(String(20), String(10))
                        .AppendToQueryString(queryString);
            }

            #endregion

            static int GetSeed()
            {
                return Guid.NewGuid().GetHashCode();
            }
        }

        public static class MockIt
        {
            #region Factory constructors

            [DebuggerStepThrough]
            public static T Is<T>(Action<T> verify)
            {
                Func<T, bool> match = arg =>
                                          {
                                              try
                                              {
                                                  verify(arg);
                                                  return true;
                                              }
                                              catch (SpecificationException e)
                                              {
                                                  Console.Write(e);
                                                  return false;
                                              }
                                          };

                return It.Is<T>(arg => match(arg));
            }

            [DebuggerStepThrough]
            public static T IsAny<T>()
            {
                return It.IsAny<T>();
            }

            [DebuggerStepThrough]
            public static T IsNotNull<T>()
            {
                return Is<T>(r => r.ShouldBeOfType<T>());
            }

            [DebuggerStepThrough]
            public static T IsNull<T>()
            {
                return Is<T>(r => r.ShouldBeNull());
            }

            [DebuggerStepThrough]
            public static TActual IsStrong<TActual>(TActual expected, Action<ICompareFactoryDsl<TActual, TActual>> action)
            {
                return IsWeak(expected, action);
            }

            [DebuggerStepThrough]
            public static TActual IsStrong<TActual>(TActual expected)
            {
                // ReSharper disable IntroduceOptionalParameters.Global
                return IsStrong(expected, null);

                // ReSharper restore IntroduceOptionalParameters.Global
            }

            [DebuggerStepThrough]
            public static TActual IsWeak<TActual, TExpected>(TExpected expected)
            {
                return IsWeak<TActual, TExpected>(expected, null);
            }

            [DebuggerStepThrough]
            public static TActual IsWeak<TActual, TExpected>(TExpected expected, Action<ICompareFactoryDsl<TActual, TExpected>> action)
            {
                return Is<TActual>(arg => arg.ShouldEqualWeak(expected, action));
            }

            #endregion
        }

        #endregion
    }
}