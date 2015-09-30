namespace Incoding.UnitTest
{
    #region << Using >>

    using System.Data;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MessageExecuteSetting))]
    public class When_message_execute_setting_serialize_to_json
    {
        Establish establish = () =>
                              {
                                  expected = @"{""DataBaseInstance"":""kc21tcm4dhp@mail.comma4sztrrdmd@mail.combf1vp40jfz"",""Connection"":""xnhu0xqqv4a@mail.comclfqqyxxhrs@mail.com4guoiavzrn"",""IsolationLevel"":256}";
                                  original = Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => dsl.MuteCtor()
                                                                                                        .Tuning(r => r.IsolationLevel, IsolationLevel.ReadUncommitted)
                                                                                                        .Tuning(r => r.DataBaseInstance, "kc21tcm4dhp@mail.comma4sztrrdmd@mail.combf1vp40jfz")
                                                                                                        .Tuning(r => r.Connection, "xnhu0xqqv4a@mail.comclfqqyxxhrs@mail.com4guoiavzrn"));
                              };

        It should_be_deserialize = () => expected.DeserializeFromJson<MessageExecuteSetting>()
                                                 .ShouldEqualWeak(original);

        It should_be_serialize = () => original.ToJsonString().ShouldEqual(expected);

        #region Establish value

        static MessageExecuteSetting original;

        static string expected;

        #endregion
    }
}