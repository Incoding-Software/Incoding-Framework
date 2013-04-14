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
                Data.Set("template", template);

            if (!string.IsNullOrWhiteSpace(property))
                Data["property"] = property;

            if (prepare)
                Data["prepare"] = true;

            Data["insertType"] = insertType;
        }

        #endregion
    }
}