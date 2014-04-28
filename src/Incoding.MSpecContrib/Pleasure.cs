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

        public static IEnumerable<T> ToEnumerable<T>(params T[] agrs)
        {
            return ToList(agrs).AsEnumerable();
        }

        public static List<T> ToList<T>(params T[] args)
        {
            return args
                    .If(r => r.Length > 0)
                    .ReturnOrDefault(obj => new List<T>(obj), new List<T>());
        }

        public static IQueryable<T> ToQueryable<T>(params T[] args)
        {
            return ToList(args).AsQueryable();
        }

        public static ReadOnlyCollection<T> ToReadOnly<T>(params T[] args)
        {
            return ToList(args).AsReadOnly();
        }

        #endregion

        #region Mock

        public static Mock<TMock> Mock<TMock>(Action<Mock<TMock>> configure = null) where TMock : class
        {
            var res = new Mock<TMock>();
            configure.Do(action => action(res));
            return res;
        }

        public static TMock MockAsObject<TMock>(Action<Mock<TMock>> configure = null) where TMock : class
        {
            return Mock(configure).Object;
        }

        public static Mock<TMock> MockStrict<TMock>(Action<Mock<TMock>> configure = null) where TMock : class
        {
            var res = new Mock<TMock>(MockBehavior.Strict);
            configure.Do(action => action(res));
            return res;
        }

        public static TMock MockStrictAsObject<TMock>(Action<Mock<TMock>> configure = null) where TMock : class
        {
            return MockStrict(configure).Object;
        }

        public static Mock<ISpy> Spy(Action<Mock<ISpy>> configure = null)
        {
            return Mock(configure);
        }

        #endregion

        #region Action and Func 

        public static long Do(Action<int> action, int countDo)
        {
            return Stopwatch(() =>
                                 {
                                     for (int i = 0; i < countDo; i++)
                                         action(i);
                                 });
        }

        public static long Do10(Action<int> action)
        {
            return Do(action, 10);
        }

        public static long Do3(Action<int> action)
        {
            return Do(action, 3);
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

        public static DateTime NowPlush100Milliseconds()
        {
            return DateTime.Now.AddMilliseconds(100);
        }

        public static void Sleep1000Milliseconds()
        {
            SleepMilliseconds(1000);
        }

        public static void Sleep100Milliseconds()
        {
            SleepMilliseconds(100);
        }

        public static void Sleep50Milliseconds()
        {
            SleepMilliseconds(50);
        }

        public static void SleepMilliseconds(int milliseconds)
        {
            Thread.Sleep(new TimeSpan(0, 0, 0, 0, milliseconds));
        }

        #endregion

        #region Nested classes

        public static class MultiThread
        {
            #region Factory constructors

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

            public static bool Bool()
            {
                return PositiveNumber(0, 100) >= 50;
            }

            public static byte[] Bytes(int size = 100)
            {
                var bytes = new byte[size];
                for (int index = 0; index < bytes.Length; ++index)
                    bytes[index] = (byte)GetRandom().Next();

                return bytes;
            }

            public static DateTime DateTime()
            {
                int year = GetRandom().Next(1800, 2100);
                int months = GetRandom().Next(1, 12);
                int days = GetRandom().Next(1, 25);

                var timeSpan = TimeSpan();
                return new DateTime(year, months, days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
            }

            public static string Email()
            {
                string randomString = Path.GetRandomFileName().Replace(".", string.Empty);
                return "{0}@mail.com".F(randomString);
            }

            public static MemoryStream EmptyStream()
            {
                return new MemoryStream();
            }

            public static TEnum Enum<TEnum>()
            {
                return (TEnum)System.Enum.Parse(typeof(TEnum), EnumAsInt(typeof(TEnum)).ToString(), true);
            }

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

            public static T Invent<T>(Action<IInventFactoryDsl<T>> action = null)
            {
                var inventFactory = new InventFactory<T>();
                action.Do(r => r(inventFactory));

                return inventFactory.Create();
            }

            public static object Invent(Type type)
            {
                return typeof(Generator).GetMethods()
                                        .Single(r => r.Name.EqualsWithInvariant("Invent") && r.GetParameters().ElementAtOrDefault(0).ParameterType != typeof(Type))
                                        .MakeGenericMethod(type)
                                        .Invoke(null, new object[] { null });
            }

            public static T InventEmpty<T>() where T : new()
            {
                var inventFactory = new InventFactory<T>();
                return inventFactory.CreateEmpty();
            }

            public static T InventEntity<T>(Action<IInventFactoryDsl<T>> action = null) where T : IEntity, new()
            {
                return Invent<T>(factory =>
                                     {
                                         factory.Ignore(r => r.Id, "Because id generate automate");
                                         action.Do(r => r(factory));
                                     });
            }

            public static KeyValuePair<string, string> KeyValuePair()
            {
                return new KeyValuePair<string, string>(String(length: 10), String());
            }

            public static decimal PositiveDecimal(byte? scale = null)
            {
                decimal positiveDecimal = Math.Round(new decimal(GetRandom().Next(999999999), 0, 0, false, scale ?? (byte)GetRandom().Next(2)) * (decimal)0.5, 2);
                return positiveDecimal < 0 ? -positiveDecimal : positiveDecimal;
            }

            public static double PositiveDouble()
            {
                double positiveDouble = GetRandom().NextDouble() * 1.8;
                return positiveDouble < 0 ? -positiveDouble : positiveDouble;
            }

            public static float PositiveFloating()
            {
                float positiveFloating = (float)(GetRandom().NextDouble() * 1.8);
                return positiveFloating < 0 ? -positiveFloating : positiveFloating;
            }

            public static int PositiveNumber(int minValue = 0, int maxValue = int.MaxValue)
            {
                int positiveNumber = GetRandom().Next(minValue, maxValue);
                return positiveNumber < 0 ? -positiveNumber : positiveNumber;
            }

            public static Stream Stream(int size = 10)
            {
                return new MemoryStream(Bytes(size));
            }

            public static string String(int length = 50)
            {
                string res = string.Empty;
                while (res.Length < length)
                    res += Email();

                return res.Substring(0, length);
            }

            public static DateTime The20120406Noon()
            {
                return new DateTime(2012, 4, 16);
            }

            public static CultureInfo TheRuCulture()
            {
                return new CultureInfo("ru-RU");
            }

            public static int TheSameNumber()
            {
                return 153;
            }

            public static string TheSameString()
            {
                return "TheSameString";
            }

            public static TimeSpan TimeSpan()
            {
                return new TimeSpan(PositiveNumber(1, 24), PositiveNumber(1, 60), PositiveNumber(1, 60));
            }

            public static Uri Uri(object queryString = null)
            {
                return new Uri(Url(queryString));
            }

            public static string Url(object queryString = null)
            {
                return "http://sample.com"
                        .F(String(20), String(10))
                        .AppendToQueryString(queryString);
            }

            #endregion

            static Random GetRandom()
            {
                return new Random(Guid.NewGuid().GetHashCode());
            }

            public static Guid TheSameGuid()
            {
                return new Guid("DD2D6D88-D2E9-40E2-A60D-0E58CCD8235D");
            }
        }

        public static class MockIt
        {
            #region Factory constructors

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

            public static T IsAny<T>()
            {
                return It.IsAny<T>();
            }

            public static T IsNotNull<T>()
            {
                return Is<T>(r => r.ShouldBeOfType<T>());
            }

            public static T IsNull<T>()
            {
                return Is<T>(r => r.ShouldBeNull());
            }

            public static TActual IsStrong<TActual>(TActual expected, Action<ICompareFactoryDsl<TActual, TActual>> action)
            {
                return IsWeak(expected, action);
            }

            public static TActual IsStrong<TActual>(TActual expected)
            {
                // ReSharper disable IntroduceOptionalParameters.Global
                return IsStrong(expected, null);

                // ReSharper restore IntroduceOptionalParameters.Global
            }

            public static TActual IsWeak<TActual, TExpected>(TExpected expected)
            {
                return IsWeak<TActual, TExpected>(expected, null);
            }

            public static TActual IsWeak<TActual, TExpected>(TExpected expected, Action<ICompareFactoryDsl<TActual, TExpected>> action)
            {
                return Is<TActual>(arg => arg.ShouldEqualWeak(expected, action));
            }

            #endregion
        }

        #endregion

        public static long Stopwatch(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public static long StopwatchAsSecond(Action action)
        {
            return Stopwatch(action) / 1000;
        }
    }
}