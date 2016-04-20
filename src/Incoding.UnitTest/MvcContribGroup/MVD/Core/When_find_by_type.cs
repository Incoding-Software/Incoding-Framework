namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib.MVD;
    using Machine.Specifications;

    #endregion

    public class GetReferencesForDDQuery<T> 
    {
    }

    public class Currency 
    {
    }



    [Subject(typeof(CreateByTypeQuery.FindTypeByName))]
    public class When_find_by_type
    {
        public enum Test
        { }

        It should_be_as_bool = () => { Run(typeof(Boolean), typeof(Boolean).Name); };

        It should_be_as_generic = () =>
                                  {
                                      var expected = typeof(GetReferencesForDDQuery<>).Name;
                                      Run(typeof(GetReferencesForDDQuery<>), "Incoding.UnitTest.MvcContribGroup.GetReferencesForDDQuery%601%5B%5BIncoding.UnitTest.MvcContribGroup.Currency%2C+Incoding.UnitTest.MvcContribGroup%2C+Version%3D1.0.0.0%2C+Culture%3Dneutral%2C+PublicKeyToken%3Dnull%5D%5D");
                                  };

        It should_be_as_by_full_name = () => { Run(typeof(FakeInnerByFullNameCommand), typeof(FakeInnerByFullNameCommand).Name); };

        It should_be_as_by_full_name_on_inner_class = () => { Run(typeof(FakeInnerByFullNameCommand.InnerByFullName), HttpUtility.UrlEncode(typeof(FakeInnerByFullNameCommand.InnerByFullName).FullName)); };

        It should_be_as_by_full_name_with_encode = () => { Run(typeof(FakeInnerByFullNameCommand), HttpUtility.UrlEncode(typeof(FakeInnerByFullNameCommand).FullName)); };

        It should_be_as_enum = () => { Run(typeof(Test), typeof(Test).FullName); };

        
        static void Run(Type expected, string findName)
        {
            CreateByTypeQuery.FindTypeByName query = Pleasure.Generator.Invent<CreateByTypeQuery.FindTypeByName>(dsl => dsl.Tuning(r => r.Type, findName));
            var mockQuery = MockQuery<CreateByTypeQuery.FindTypeByName, Type>
                    .When(query);
            mockQuery.Execute();
            mockQuery.ShouldBeIsResult(expected);
        }

        [ExcludeFromCodeCoverage]
        public class FakeInnerByFullNameCommand
        {
            #region Nested classes

            public class InnerByFullName : CommandBase
            {
                protected override void Execute()
                {
                    throw new NotImplementedException();
                }
            }

            #endregion
        }
    }
}