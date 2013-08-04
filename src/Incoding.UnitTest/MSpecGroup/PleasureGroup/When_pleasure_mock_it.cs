namespace Incoding.UnitTest.MSpecGroup
{
    #region << Using >>

    using Incoding.MSpecContrib;
    using Machine.Specifications;
    using Moq;
    using It = Machine.Specifications.It;

    #endregion

    [Subject(typeof(Pleasure.MockIt))]
    public class When_pleasure_mock_it
    {
        #region Fake classes

        public interface IFakeMoqInterface
        {
            void FireString(string result);

            void Fire(FireParameterFake parameter);
        }

        public class FireParameterFake
        {
            #region Properties

            public string Id { get; set; }

            #endregion
        }

        #endregion

        It should_be_mock_it_is_any = () =>
                                          {
                                              var fakeMoq = Pleasure.Mock<IFakeMoqInterface>();
                                              fakeMoq.Object.FireString(Pleasure.Generator.String());
                                              fakeMoq.Verify(r => r.FireString(Pleasure.MockIt.IsAny<string>()));
                                          };

        It should_be_mock_it_is_any_false = () =>
                                                {
                                                    var fakeMoq = Pleasure.Mock<IFakeMoqInterface>();
                                                    fakeMoq.Verify(r => r.FireString(Pleasure.MockIt.IsAny<string>()), Times.Never());
                                                };

        It should_be_mock_it_is_null = () =>
                                           {
                                               var fakeMoq = Pleasure.Mock<IFakeMoqInterface>();
                                               fakeMoq.Object.FireString(null);
                                               fakeMoq.Verify(r => r.FireString(Pleasure.MockIt.IsNull<string>()));
                                           };

        It should_be_mock_it_is_null_false = () =>
                                                 {
                                                     var fakeMoq = Pleasure.Mock<IFakeMoqInterface>();
                                                     fakeMoq.Object.FireString(string.Empty);
                                                     fakeMoq.Verify(r => r.FireString(Pleasure.MockIt.IsNull<string>()), Times.Never());
                                                 };

        It should_be_mock_it_is_not_null = () =>
                                               {
                                                   var fakeMoq = Pleasure.Mock<IFakeMoqInterface>();
                                                   fakeMoq.Object.FireString(string.Empty);
                                                   fakeMoq.Verify(r => r.FireString(Pleasure.MockIt.IsNotNull<string>()));
                                               };

        It should_be_mock_it_is_not_null_false = () =>
                                                     {
                                                         var fakeMoq = Pleasure.Mock<IFakeMoqInterface>();
                                                         fakeMoq.Object.FireString(null);
                                                         fakeMoq.Verify(r => r.FireString(Pleasure.MockIt.IsNotNull<string>()), Times.Never());
                                                     };

        It should_be_mock_it_is_strong = () =>
                                             {
                                                 var fakeMoq = Pleasure.Mock<IFakeMoqInterface>();
                                                 var fireParameterFake = new FireParameterFake { Id = Pleasure.Generator.TheSameString() };
                                                 fakeMoq.Object.Fire(fireParameterFake);
                                                 fakeMoq.Verify(r => r.Fire(Pleasure.MockIt.IsStrong(fireParameterFake)));
                                             };

        It should_be_mock_it_is_strong_false = () =>
                                                   {
                                                       var fakeMoq = Pleasure.Mock<IFakeMoqInterface>();
                                                       fakeMoq.Object.Fire(new FireParameterFake { Id = Pleasure.Generator.TheSameString() });
                                                       fakeMoq.Verify(r => r.Fire(Pleasure.MockIt.IsStrong(new FireParameterFake())), Times.Never());
                                                   };

        It should_be_mock_it_is_weak = () =>
                                           {
                                               var fakeMoq = Pleasure.Mock<IFakeMoqInterface>();
                                               var fireParameterFake = new FireParameterFake { Id = Pleasure.Generator.TheSameString() };
                                               fakeMoq.Object.Fire(fireParameterFake);
                                               fakeMoq.Verify(r => r.Fire(Pleasure.MockIt.IsWeak<FireParameterFake, FireParameterFake>(fireParameterFake)));
                                           };

        It should_be_mock_it_is_weak_false = () =>
                                                 {
                                                     var fakeMoq = Pleasure.Mock<IFakeMoqInterface>();
                                                     fakeMoq.Object.Fire(new FireParameterFake { Id = Pleasure.Generator.TheSameString() });
                                                     fakeMoq.Verify(r => r.Fire(Pleasure.MockIt.IsWeak<FireParameterFake, FireParameterFake>(new FireParameterFake())), Times.Never());
                                                 };
    }
}