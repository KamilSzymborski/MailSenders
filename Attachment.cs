﻿namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.Attachment.xml" path="docs/header"/>
    public class Attachment
    {
        #region Static:Methods
        /// <include file=".Docs/.Attachment.xml" path="docs/method[@name='FromFile(string, string)']"/>
        public static Attachment FromFile(string Path, string DisplayName = null)
        {
            return new Attachment(FileService.ReadAllBytes(Path), DisplayName is null? PathService.GetFileName(Path) : PathService.CombineName(DisplayName, PathService.GetFileExtension(Path)));
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
        /// <include file=".Docs/.Attachment.xml" path="docs/property[@name='Data']"/>
        public byte[] Data { get { return mData; } }
        /// <include file=".Docs/.Attachment.xml" path="docs/property[@name='FileName']"/>
        public string FileName { get { return mFileName; } }
        #endregion

        #region Variables
        private readonly byte[] mData;
        private readonly string mFileName;
        #endregion
    }
}