namespace Incoding.MvcContrib
{
    #region << Using >>

    using System;

    #endregion

    public interface IIncodingMetaLanguageCoreDsl
    {
        IncodingMetaCallbackBindDsl Bind { get; }

        IncodingMetaCallbackInsertDsl Insert { get; }

        [Obsolete("Will be move to root on next version")]
        IExecutableSetting Break { get; }

        IncodingMetaCallbackJqueryDsl JQuery { get; }

        IncodingMetaCallbackFuncDsl Func { get; }

        IncodingMetaCallbackStoreApiDsl Store { get; }

        IncodingMetaCallbackTriggerDsl Trigger { get; }

        IncodingMetaCallbackFormApiDsl Form { get; }

        IExecutableSetting Eval(string code);

        IExecutableSetting Call(string funcName, params object[] args);
    }
}