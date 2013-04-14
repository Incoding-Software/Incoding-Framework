namespace Incoding.MvcContrib
{
    #region << Using >>

    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Web;

    #endregion

    public class HttpMemoryPostedFile : HttpPostedFileBase
    {
        #region Fields

        readonly Stream inputStream;

        readonly string fileName;

        readonly string contentType;

        byte[] buffer;

        #endregion

        #region Constructors

        public HttpMemoryPostedFile(Stream inputStream, string fileName, string contentType)
        {
            Guard.NotNull("inputStream", inputStream);
            Guard.IsConditional("inputStream", inputStream.Position == 0);
            Guard.NotNull("fileName", fileName);

            this.inputStream = inputStream;
            this.fileName = fileName;
            this.contentType = contentType;
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Can't validate")]
        public HttpMemoryPostedFile(HttpPostedFileBase postedFileBase)
                : this(postedFileBase.InputStream, postedFileBase.FileName, postedFileBase.ContentType) { }

        #endregion

        #region Properties

        public override string ContentType
        {
            get { return this.contentType; }
        }

        public override int ContentLength
        {
            get { return (int)this.inputStream.Length; }
        }

        public override string FileName
        {
            get { return this.fileName; }
        }

        public override Stream InputStream
        {
            get { return this.inputStream; }
        }

        public byte[] ContentAsBytes
        {
            get
            {
                if (this.buffer != null)
                    return this.buffer;

                this.buffer = new byte[ContentLength];
                this.inputStream.Read(this.buffer, 0, ContentLength);
                this.inputStream.Position = 0;
                return this.buffer;
            }
        }

        #endregion
    }
}