namespace Incoding.UnitTest.MvcContribGroup
{
    #region << Using >>

    using System.Collections.Generic;
    using System.Linq;
    using Incoding.MSpecContrib;
    using Incoding.MvcContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(IncSelectList.IncOptional))]
    public class When_inc_optional
    {
        It should_be_cast_to_key_value_vm = () =>
                                            {
                                                var value = Pleasure.Generator.Invent<KeyValueVm>();
                                                IncSelectList.IncOptional optional = value;
                                                ((List<KeyValueVm>)optional).ShouldEqualWeak(new[] { value });
                                            };

        It should_be_cast_to_key_value_vms = () =>
                                             {
                                                 var valueVms = Pleasure.Generator.Invent<KeyValueVm[]>();
                                                 IncSelectList.IncOptional optional = valueVms;
                                                 ((List<KeyValueVm>)optional).ShouldEqualWeak(valueVms);
                                             };

        It should_be_cast_to_string = () =>
                                      {
                                          var value = Pleasure.Generator.String();
                                          IncSelectList.IncOptional optional = value;
                                          ((List<KeyValueVm>)optional).ShouldEqualWeak(new[] { new KeyValueVm(value) });
                                      };

        It should_be_cast_to_strings = () =>
                                       {
                                           var value = Pleasure.Generator.Invent<string[]>();
                                           IncSelectList.IncOptional optional = value;
                                           ((List<KeyValueVm>)optional).ShouldEqualWeak(value.Select(r => new KeyValueVm(r)));
                                       };
    }
}