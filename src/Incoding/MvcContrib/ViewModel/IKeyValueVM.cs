namespace Incoding.MvcContrib
{
    public interface IKeyValueVm
    {
        #region Properties

        bool Selected { get; set; }

        string Text { get; set; }

        string Title { get; set; }

        string Value { get; set; }

        string CssClass { get; set; }

        #endregion
    }
}