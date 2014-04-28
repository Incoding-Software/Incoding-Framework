namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Data;
    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Raven.Client;
    using Raven.Imports.Newtonsoft.Json;

    #endregion

    [Subject(typeof(RavenDbRepository))]
    public class When_raven_db_repository_performance_save
    {
        #region Fake classes

        public class FakeItem : IncEntityBase
        {
            // ReSharper disable UnusedMember.Global
            #region Properties

            public string Prop1 { get; set; }

            public string Prop2 { get; set; }

            public string Prop3 { get; set; }

            #endregion

            // ReSharper restore UnusedMember.Global
        }

        public class FakeEntity : IncEntityBase
        {
            // ReSharper disable UnusedMember.Global
            #region Properties

            public string Prop1 { get; set; }

            public string Prop2 { get; set; }

            public string Prop3 { get; set; }

            public string Prop4 { get; set; }

            public string Prop5 { get; set; }

            public string Prop6 { get; set; }

            public string Prop7 { get; set; }

            public string Prop8 { get; set; }

            public string Prop9 { get; set; }

            public string Prop10 { get; set; }

            [JsonIgnore]
            public FakeEntity Reference { get; set; }

            [JsonIgnore]
            public List<FakeItem> Items { get; set; }

            #endregion

            // ReSharper restore UnusedMember.Global
        }

        #endregion

        #region Establish value

        static RavenDbRepository repository;

        static FakeEntity fakeEntity;

        #endregion

        Establish establish = () =>
                                  {
                                      var mockAsObject = Pleasure.MockAsObject<IDocumentSession>(mock => mock.Setup(r => r.Advanced.HasChanged(Pleasure.MockIt.IsAny<FakeEntity>())).Returns(false));

                                      var sessionFactory = Pleasure.MockAsObject<IRavenDbSessionFactory>(mock => mock.Setup(r => r.GetCurrent()).Returns(mockAsObject));
                                      repository = new RavenDbRepository(sessionFactory);

                                      fakeEntity = Pleasure.Generator.Invent<FakeEntity>(dsl => dsl.GenerateTo(r => r.Reference)
                                                                                                   .GenerateTo(r => r.Items));
                                  };

        It should_be_performance = () => Pleasure.Do(i => repository.Save(fakeEntity), 1000).ShouldBeLessThan(600);
    }
}