namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;

    #endregion

    public interface IParserException
    {
        string Parse(Exception exception);
    }
}