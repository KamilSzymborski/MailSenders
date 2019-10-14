using Net = System.IO;

namespace KamilSzymborski.MailSenders
{
    internal static class PathService
    {
        #region Methods
        internal static string GetFileName(string Path)
        {
            return Net.Path.GetFileName(Path);
        }
        internal static string GetFileExtension(string Path)
        {
            return Net.Path.GetExtension(Path).Substring(1);
        }
        #endregion
    }
}