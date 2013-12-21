namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System;
    using System.Drawing;
    using Incoding.Extensions;
    using Incoding.MSpecContrib;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(OtherExtensions))]
    public class When_other_extensions
    {
        It should_be_translate_color_to_hex = () => Color.Red.ToHex().ShouldEqual("#FF0000");

        It should_be_is_empty_guid_null = () =>
                                              {
                                                  Guid? nullGuid = null;
                                                  nullGuid.IsEmpty().ShouldBeTrue();
                                              };

        It should_be_is_empty_guid_empty = () =>
                                               {
                                                   Guid? nullGuid = Guid.Empty;
                                                   nullGuid.IsEmpty().ShouldBeTrue();
                                               };

        It should_not_be_is_empty = () =>
                                        {
                                            Guid? nullGuid = Pleasure.Generator.TheSameGuid();
                                            nullGuid.IsEmpty().ShouldBeFalse();
                                        };
    }
}