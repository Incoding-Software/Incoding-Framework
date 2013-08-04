namespace Incoding.UnitTest.ExtensionsGroup
{
    #region << Using >>

    using System.Drawing;
    using Incoding.Extensions;
    using Machine.Specifications;

    #endregion

    [Subject(typeof(OtherExtensions))]
    public class When_other_extensions
    {
        It should_be_translate_color_to_hex = () => Color.Red.ToHex().ShouldEqual("#FF0000");
    }
}