namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using Incoding.Extensions;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(ObjectExtensions))]
    public class When_extensions_object
    {
        It should_be_to_json_string = () => new Dictionary<string, object> { { "Key", "Value" } }
                                                    .ToJsonString()
                                                    .ShouldEqual("{\"Key\":\"Value\"}");

        It should_be_deserialize_from_json_as_generic = () => "{\"Key\":\"Value\"}"
                                                                      .DeserializeFromJson<Dictionary<string, object>>()
                                                                      .ShouldEqual(new Dictionary<string, object> { { "Key", "Value" } });

        It should_be_deserialize_from_json_as_type = () => "{\"Key\":\"Value\"}"
                                                                   .DeserializeFromJson(typeof(Dictionary<string, object>))
                                                                   .ShouldEqual(new Dictionary<string, object> { { "Key", "Value" } });
    }
}