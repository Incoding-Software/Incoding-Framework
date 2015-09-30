namespace Incoding.CQRS
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using JetBrains.Annotations;

    #endregion

    [ExcludeFromCodeCoverage, UsedImplicitly]
    public class AddCookieCommand : CommandBase
    {
        #region Constructors

        public AddCookieCommand() { }

        public AddCookieCommand(string key)
        {
            Key = key;
        }

        public AddCookieCommand(string key, string value)
        {
            Key = key;
            Value = value;
        }

        #endregion

        #region Properties

        public string Key { get; set; }

        public string Value { get; set; }

        #endregion

        protected override void Execute()
        {
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(Key, Value)
                                                     {
                                                             Expires = DateTime.Now.AddYears(1)
                                                     });
        }
    }
}