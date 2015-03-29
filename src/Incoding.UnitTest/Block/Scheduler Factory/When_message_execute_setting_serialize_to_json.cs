namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.Block.ExceptionHandling;
    using Incoding.CQRS;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MessageExecuteSetting))]
    public class When_message_execute_setting_serialize_to_json
    {
        #region Establish value

        static MessageExecuteSetting original;

        static string expected;

        #endregion

        Establish establish = () =>
                                  {
                                      expected = @"{""Mute"":4,""DataBaseInstance"":""kc21tcm4dhp@mail.comma4sztrrdmd@mail.combf1vp40jfz"",""Connection"":""xnhu0xqqv4a@mail.comclfqqyxxhrs@mail.com4guoiavzrn""}";
                                      original = Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => dsl.MuteCtor()
                                                                                                            .Tuning(r => r.DataBaseInstance, "kc21tcm4dhp@mail.comma4sztrrdmd@mail.combf1vp40jfz")
                                                                                                            .Tuning(r => r.Connection, "xnhu0xqqv4a@mail.comclfqqyxxhrs@mail.com4guoiavzrn")
                                                                                                            .Tuning(r => r.Mute, MuteEvent.OnAfter)
                                                                                                            /*.GenerateTo(r => r.Delay, factoryDsl => factoryDsl.MuteCtor()
                                                                                                                                                              .Tuning(r => r.DataBaseInstance, "rn0djxsfm2a@mail.comdvvd1edseas@mail.comx4u32nvmpn")
                                                                                                                                                              .Tuning(r => r.Connection, "q1bvmrbf5hr@mail.comruvlcgg5ldn@mail.com3he51i0baa")
                                                                                                                                                              .Tuning(r => r.UID, "x0fjpjr1qrp@mail.comfehv2irukph@mail.comshqheypvzv"))*/);
                                  };

        It should_be_serialize = () => original.ToJsonString().ShouldEqual(expected);

        It should_be_deserialize = () => expected.DeserializeFromJson<MessageExecuteSetting>()
                                                 .ShouldEqualWeak(original);
    }
}