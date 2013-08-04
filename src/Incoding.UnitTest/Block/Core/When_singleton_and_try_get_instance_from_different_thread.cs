namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using System.Threading;
    using Incoding.Block.Caching;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(CachingFactory))]
    public class When_singleton_and_try_get_instance_from_different_thread
    {
        #region Fake classes

        public class FakeSingleton
        {
            #region Static Fields

            public static int CountCreate;

            #endregion

            #region Constructors

            public FakeSingleton()
            {
                CountCreate++;
            }

            #endregion
        }

        #endregion

        #region Estabilish value

        static CachingFactory fake1;

        static CachingFactory fake2;

        static CachingFactory fake3;

        static CachingFactory fakeOriginal;

        #endregion

        Establish establish = () => { };

        Because of = () =>
                         {
                             fakeOriginal = CachingFactory.Instance;

                             var threads = new[]
                                               {
                                                       new Thread(() => fake1 = CachingFactory.Instance), 
                                                       new Thread(() => fake2 = CachingFactory.Instance), 
                                                       new Thread(() => fake3 = CachingFactory.Instance)
                                               };

                             foreach (var newThread in threads)
                                 newThread.Start();

                             Array.ForEach(threads, thread => thread.Join());
                         };

        It should_be_init_new_instance = () => fakeOriginal.ShouldNotBeNull();

        It should_be_all_instance_same_to_original = () =>
                                                         {
                                                             fake1.ShouldBeTheSameAs(fakeOriginal);
                                                             fake2.ShouldBeTheSameAs(fakeOriginal);
                                                             fake3.ShouldBeTheSameAs(fakeOriginal);
                                                         };
    }
}