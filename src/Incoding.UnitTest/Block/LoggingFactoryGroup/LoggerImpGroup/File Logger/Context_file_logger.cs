namespace Incoding.UnitTest.Block
{
    #region << Using >>

    using System;
    using System.IO;

    #endregion

    public class Context_file_logger : Context_Logger
    {
        #region Static Fields

        protected static readonly string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestLog");

        static readonly object lockObject = new object();

        #endregion

        #region Constructors

        protected Context_file_logger()
        {
            if (Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);
        }

        #endregion

        protected static string GetContent(string fileName)
        {
            lock (lockObject)
            {
                using (var streamReader = new StreamReader(Path.Combine(folderPath, fileName)))
                    return streamReader.ReadToEnd();
            }
        }
    }
}