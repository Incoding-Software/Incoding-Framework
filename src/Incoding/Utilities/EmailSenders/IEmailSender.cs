namespace Incoding.Utilities
{
    #region << Using >>

    using System;
    using System.Net.Mail;

    #endregion

    public interface IEmailSender : IDisposable
    {
        void Send(MailMessage mailMessage);
    }
}