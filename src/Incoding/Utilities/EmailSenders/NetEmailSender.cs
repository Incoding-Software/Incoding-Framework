namespace Incoding.Utilities
{
    #region << Using >>

    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Mail;
    using Incoding.Maybe;
    using JetBrains.Annotations;

    #endregion

    ////ncrunch: no coverage start
    [UsedImplicitly, ExcludeFromCodeCoverage]
    public class NetEmailSender : IEmailSender
    {
        #region Fields

        readonly SmtpClient smtpClient;

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
            this.smtpClient
                .Do(r => r.Dispose());
        }

        #endregion
    }

    ////ncrunch: no coverage end
}