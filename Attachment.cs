namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.Attachment.xml" path="docs/header"/>
    public class Attachment
    {
        #region Static:Methods
        /// <include file=".Docs/.Attachment.xml" path="docs/method[@name='FromFile(string)']"/>
        public static Attachment FromFile(string Path)
        {
            return new Attachment(FileService.ReadAllBytes(Path), PathService.GetFileName(Path));
        }
        #endregion

        #region Constructors
        /// <include file=".Docs/.Attachment.xml" path="docs/constructor[@name='(byte[], string)']"/>
        public Attachment(byte[] Data, string FileName)
        {
            mData = Data;
            mFileName = FileName;
        }
        #endregion

        #region Properties
        internal byte[] Data { get { return mData; } }
        internal string FileName { get { return mFileName; } }
        #endregion

        #region Variables
        private readonly byte[] mData;
        private readonly string mFileName;
        #endregion
    }
}