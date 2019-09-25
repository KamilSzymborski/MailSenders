using System.IO;

namespace KamilSzymborski.MailSenders
{
    internal static class FileService
    {
        #region Methods
        internal static byte[] ReadAllBytes(string Path)
        {
            byte[] Data = null;

            using (var Stream = File.OpenRead(Path))
            {
                Data = new byte[(int)Stream.Length];
                Stream.Read(Data, 0, Data.Length);
            }

            return Data;
        }
        #endregion
    }
}