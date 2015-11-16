namespace Incoding.Utilities
{
    #region << Using >>

    using System;
    using System.Net.Mail;

    #endregion

    [Obsolete("Please use SendEmailCommand")]
    public interface IEmailSender : IDisposable
    {
        void Send(MailMessage mailMessage);
    }
}