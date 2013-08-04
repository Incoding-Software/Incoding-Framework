namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncodingMetaCallbackStoreApiDsl))]
    public class When_incoding_meta_language_dsl_store
    {
        It should_be_store_hash_insert = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                       .Do().Direct()
                                                       .OnSuccess(r => r.Self().Core().Store.Hash.Insert(Pleasure.Generator.TheSameString()))
                                                       .GetExecutable<ExecutableStoreInsert>()
                                                       .ShouldEqualData(new Dictionary<string, object>
                                                                            {
                                                                                    { "type", "hash" }, 
                                                                                    { "replace", false }, 
                                                                                    { "prefix", Pleasure.Generator.TheSameString() }, 
                                                                            });

        It should_be_store_hash__set = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                     .Do().Direct()
                                                     .OnSuccess(r => r.Self().Core().Store.Hash.Set(Pleasure.Generator.TheSameString()))
                                                     .GetExecutable<ExecutableStoreInsert>()
                                                     .ShouldEqualData(new Dictionary<string, object>
                                                                          {
                                                                                  { "type", "hash" }, 
                                                                                  { "replace", true }, 
                                                                                  { "prefix", Pleasure.Generator.TheSameString() }, 
                                                                          });

        It should_be_store_hash_fetch = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                      .Do().Direct()
                                                      .OnSuccess(r => r.Self().Core().Store.Hash.Fetch(Pleasure.Generator.TheSameString()))
                                                      .GetExecutable<ExecutableStoreFetch>()
                                                      .ShouldEqualData(new Dictionary<string, object>
                                                                           {
                                                                                   { "type", "hash" }, 
                                                                                   { "prefix", Pleasure.Generator.TheSameString() }, 
                                                                           });

        It should_be_store_hash__manipulate = () => new IncodingMetaLanguageDsl(JqueryBind.Click)
                                                            .Do().Direct()
                                                            .OnSuccess(r => r.Self().Core().Store.Hash.Manipulate(dsl => dsl
                                                                                                                                 .Remove("removeKey", "removePrefix")
                                                                                                                                 .Set("setKey", "value", "setPrefix")))
                                                            .GetExecutable<ExecutableStoreManipulate>()
                                                            .ShouldEqualData(new Dictionary<string, object>
                                                                                 {
                                                                                         { "type", "hash" }, 
                                                                                         { "methods", "[{\"verb\":\"remove\",\"key\":\"removeKey\",\"prefix\":\"removePrefix\"},{\"verb\":\"set\",\"key\":\"setKey\",\"value\":\"value\",\"prefix\":\"setPrefix\"}]" }
                                                                                 });
    }
}