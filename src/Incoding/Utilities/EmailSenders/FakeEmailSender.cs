namespace Incoding.Utilities
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Mail;
    using JetBrains.Annotations;

    #endregion

    ////ncrunch: no coverage start
    [UsedImplicitly, ExcludeFromCodeCoverage,Obsolete("Please use SendEmailCommand")]
    public class FakeEmailSender : IEmailSender
    {
        #region Fields

        readonly Action<MailMessage> processSendEmail;

        #endregion

        #region Constructors

        public FakeEmailSender(Action<MailMessage> processSendEmail)
        {
            this.processSendEmail = processSendEmail;
        }

        #endregion

        #region IEmailSender Members

        public void Send(MailMessage mailMessage)
        {
            Guard.NotNull("mailMessage", mailMessage);
            this.processSendEmail(mailMessage);
        }

        #endregion

        #region Disposable

        public void Dispose() { }

        #endregion
    }

    ////ncrunch: no coverage end
}