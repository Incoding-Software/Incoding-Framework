namespace Incoding.UnitTest
{
    #region << Using >>

    using Incoding.CQRS;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(MessageExecuteSetting))]
    public class When_message_execute_setting_equal
    {
        It should_be_false = () => Pleasure.Generator.Invent<MessageExecuteSetting>().ShouldNotEqual(Pleasure.Generator.Invent<MessageExecuteSetting>());

        It should_be_isolation_level_null = () =>
                                            {
                                                var setting = Pleasure.Generator.Invent<MessageExecuteSetting>(dsl => dsl.Tuning(r => r.IsolationLevel, null));
                                                setting.ShouldEqual(setting);
                                            };

        It should_be_true = () =>
                            {
                                var setting = Pleasure.Generator.Invent<MessageExecuteSetting>();
                                setting.ShouldEqual(setting);
                            };
    }
}