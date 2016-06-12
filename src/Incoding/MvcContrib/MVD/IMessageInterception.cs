namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System.Web;
    using Incoding.CQRS;

    #endregion

    public interface IMessageInterception
    {
        void OnBefore(IMessage message, HttpContextBase context);

        void OnAfter(IMessage message, HttpContextBase context);
    }
}