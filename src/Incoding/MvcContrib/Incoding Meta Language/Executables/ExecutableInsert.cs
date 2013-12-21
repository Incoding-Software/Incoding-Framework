namespace Incoding.MvcContrib
{
    #region << Using >>

    using Incoding.Extensions;

    #endregion

    public class ExecutableInsert : ExecutableBase
    {
        #region Constructors

        public ExecutableInsert(string insertType, string property, string template, bool prepare)
        {
            if (!string.IsNullOrWhiteSpace(template))
                this.Set("template", template);

            if (!string.IsNullOrWhiteSpace(property))
                this["property"] = property;

            if (prepare)
                this["prepare"] = true;

            this["insertType"] = insertType;
        }

        #endregion
    }
}