namespace Incoding.Utilities
{
    #region << Using >>

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Mail;
    using Incoding.Maybe;
    using JetBrains.Annotations;

    #endregion

    ////ncrunch: no coverage start
    [UsedImplicitly, ExcludeFromCodeCoverage, Obsolete("Please use SendEmailCommand")]
    public class NetEmailSender : IEmailSender
    {
        #region Fields

        readonly SmtpClient smtpClient;

        #endregion

        #region IEmailSender Members

        public void Send(MailMessage mailMessage)
        {
            Guard.NotNull("mailMessage", mailMessage);
            this.smtpClient.Send(mailMessage);
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            if (this.smtpClient != null)
                this.smtpClient.Dispose();
        }

        #endregion

        #region Constructors

        public NetEmailSender()
        {
            this.smtpClient = new SmtpClient();
        }

        public NetEmailSender(string host, int port, bool enableSsl)
        {
            Guard.IsConditional("port", port >= 0);
            this.smtpClient = new SmtpClient(host, port);
            this.smtpClient.EnableSsl = enableSsl;
        }

        public NetEmailSender(string host, int port, bool enableSsl, NetworkCredential credential)
                : this(host, port, enableSsl)
        {
            this.smtpClient.Credentials = credential;
        }

        #endregion
    }

    ////ncrunch: no coverage end
}