#region copyright

// @incoding 2011
#endregion

namespace Incoding.Block.Logging
{
    #region << Using >>

    using System;
    using System.IO;

    #endregion

    /// <summary>
    ///     Imp <see cref="ILogger" /> for stream file writer
    /// </summary>
    public class FileLogger : LoggerBase
    {
        ////ncrunch: no coverage start
        #region Static Fields

        static readonly object lockObject = new object();

        #endregion

        ////ncrunch: no coverage end
        #region Fields

        readonly bool append;

        readonly string folderPath;

        readonly Func<string> fileName;

        bool clearAtOnce;

        #endregion

        #region Constructors

        FileLogger(string folderPath, Func<string> fileName, bool append, bool clearAtOnce)
        {
            Guard.NotNullOrWhiteSpace("folderPath", folderPath);
            Guard.NotNull("fileName", fileName);

            this.folderPath = folderPath;
            this.fileName = fileName;
            this.append = append;
            this.clearAtOnce = clearAtOnce;

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
        }

        #endregion

        #region Factory constructors

        public static FileLogger WithAtOnceReplace(string folderPath, Func<string> genericFileName)
        {
            return new FileLogger(folderPath, genericFileName, true, true);
        }

        public static FileLogger WithEachReplace(string folderPath, Func<string> genericFileName)
        {
            return new FileLogger(folderPath, genericFileName, false, true);
        }

        public static FileLogger WithoutReplace(string folderPath, Func<string> genericFileName)
        {
            return new FileLogger(folderPath, genericFileName, true, false);
        }

        #endregion

        public override void Log(LogMessage logMessage)
        {
            lock (lockObject)
            {
                string fullPath = Path.Combine(this.folderPath, this.fileName.Invoke());
                if (File.Exists(fullPath))
                {
                    if (this.clearAtOnce)
                        File.Delete(fullPath);
                }

                this.clearAtOnce = false;

                using (var streamWriter = new StreamWriter(@fullPath, this.append))
                {
                    string message = this.template.Invoke(logMessage);
                    streamWriter.Write(message);
                    streamWriter.Flush();
                }
            }
        }
    }
}