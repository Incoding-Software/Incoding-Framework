namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System.Web;
    using Incoding.CQRS;

    #endregion

    public interface IMessageInterception
    {
        void OnBefore(IMessage message);

        void OnAfter(IMessage message);
    }
}