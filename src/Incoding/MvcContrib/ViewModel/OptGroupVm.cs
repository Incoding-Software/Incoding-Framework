namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Collections.Generic;

    #endregion

    public class OptGroupVm
    {
        #region Constructors

        public OptGroupVm(List<KeyValueVm> items)
        {
            Items = items;
        }

        public OptGroupVm(string title, List<KeyValueVm> items)
        {
            Items = items;
            Title = string.IsNullOrWhiteSpace(title) ? null : title;
        }

        #endregion

        #region Properties

        public List<KeyValueVm> Items { get; set; }

        public string Title { get; set; }

        #endregion
    }
}