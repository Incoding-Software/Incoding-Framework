namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Incoding.Block.Caching;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Behaviors]
    public class Behaviors_cached_provider
    {
        #region Fake classes

        [DataContract, Serializable]
        public class FakeComplexityResponse
        {
            #region Properties

            [DataMember]
            public string Test { get; set; }

            [DataMember]
            public List<FakeComplexityResponseTeacher> Teachers { get; set; }

            #endregion

            #region Nested classes

            [Serializable, DataContract]
            public class FakeComplexityResponseTeacher
            {
                #region Properties

                [DataMember]
                public string ImageThumbnailId { get; set; }

                [DataMember]
                public string First { get; set; }

                [DataMember]
                public string Last { get; set; }

                [DataMember]
                public string FatherName { get; set; }

                [DataMember]
                public string Status { get; set; }

                [DataMember]
                public string Decription { get; set; }

                [DataMember]
                public string Phone { get; set; }

                #endregion
            }

            #endregion
        }

        #endregion

        #region Establish value

        protected static FakeSerializeObject valueToCache = Pleasure.Generator.Invent<FakeSerializeObject>();

        protected static ICachedProvider cachedProvider;

        #endregion

        It should_be_delete = () =>
                              {
                                  cachedProvider.Set(new FakeCacheKey(), valueToCache);
                                  cachedProvider.Delete(new FakeCacheKey());

                                  cachedProvider.Get<FakeSerializeObject>(new FakeCacheKey()).ShouldBeNull();
                              };

        It should_be_delete_all = () =>
                                  {
                                      cachedProvider.Set(new FakeCacheKey(), valueToCache);
                                      cachedProvider.Set(new FakeCacheCustomHierarchy(), new FakeSerializeObject());


                                      cachedProvider.Get<FakeSerializeObject>(new FakeCacheKey()).ShouldBeNull();
                                      cachedProvider.DeleteAll();
                                      cachedProvider.Get<FakeSerializeObject>(new FakeCacheCustomHierarchy()).ShouldBeNull();
                                  };

        It should_be_found_in_cached = () =>
                                       {
                                           cachedProvider.Set(new FakeCacheKey(), valueToCache);
                                           Pleasure.Do10((i) => cachedProvider.Get<FakeSerializeObject>(new FakeCacheKey()).ShouldEqualWeak(valueToCache));
                                       };

        It should_be_serialization = () =>
                                     {
                                         var key = new FakeCacheKey("7D668178-0A76-4E47-9FE9-C6ECD47383DB");
                                         var instance = Pleasure.Generator.Invent<FakeComplexityResponse>(dsl => dsl.GenerateTo(r => r.Teachers));
                                         cachedProvider.Set(key, instance);
                                         cachedProvider.Get<FakeComplexityResponse>(key).ShouldEqualWeak(instance);
                                     };
    }
}