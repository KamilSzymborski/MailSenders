using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.Sender.xml" path="docs/header"/>
    public abstract class Sender
    {
        #region Constructors
        /// <include file=".Docs/.Sender.xml" path="docs/constructor[@name='(uint, string, string, string)']"/>
        public Sender(uint Port, string Host, string Login, string Password)
        {
            mPort = Port;
            mHost = Host;
            mLogin = Login;
            mPassword = Password;

            mClient = new SmtpClient();

            mInit();
        }
        #endregion

        #region Events
        /// <include file=".Docs/.Sender.xml" path="docs/event[@name='Failed']/*"/>
        public event Action Failed;
        /// <include file=".Docs/.Sender.xml" path="docs/event[@name='Sended']/*"/>
        public event Action Sended;
        #endregion

        #region Methods
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='Send(string, string, params Attachment[])']/*"/>
        public bool Send(string Message, string Recipient, params Attachment[] Attachments)
        {
            return mSend(string.Empty, Message, Recipient, Attachments);
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='Send(string, string, string, params Attachment[])']/*"/>
        public bool Send(string Title, string Message, string Recipient, params Attachment[] Attachments)
        {
            return mSend(Title, Message, Recipient, Attachments);
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='SendAsync(string, string, params Attachment[])']/*"/>
        public async Task<bool> SendAsync(string Message, string Recipient, params Attachment[] Attachments)
        {
            var Success = await Task.Run(() => mSend(string.Empty, Message, Recipient, Attachments));

            if (Success)
                Sended?.Invoke();
            else
                Failed?.Invoke();

            return Success;
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='SendAsync(string, string, string, params Attachment[])']/*"/>
        public async Task<bool> SendAsync(string Title, string Message, string Recipient, params Attachment[] Attachments)
        {
            var Success = await Task.Run(() => mSend(Title, Message, Recipient, Attachments));

            if (Success)
                Sended?.Invoke();
            else
                Failed?.Invoke();

            return Success;
        }

        private void mInit()
        {
            mClient.Host = mHost;
            mClient.Port = (int)mPort;
            mClient.Credentials = new NetworkCredential(mLogin, mPassword);
            mClient.EnableSsl = true;
        }
        private bool mSend(string Title, string Message, string Recipient, params Attachment[] Attachments)
        {
            var Success = true;
            var Streams = new List<Stream>();

            try
            {
                using (var Mail = new MailMessage(mLogin, Recipient))
                {
                    Mail.Subject = Title;
                    Mail.Body = Message;
                    Mail.IsBodyHtml = true;

                    foreach (var Attachment in Attachments)
                    {
                        var AttachmentStream = new MemoryStream(Attachment.Data);
                            AttachmentStream.Position = 0;

                        Streams.Add(AttachmentStream);

                        var AttachmentMimeType = MimeService.IsKnown(Attachment.FileName) ? MimeService.From(Attachment.FileName) : MimeService.From(".bin");

                        var MailAttachment = new System.Net.Mail.Attachment(AttachmentStream, AttachmentMimeType);
                            MailAttachment.ContentDisposition.FileName = Attachment.FileName;

                        Mail.Attachments.Add(MailAttachment);
                    }

                    mClient.Send(Mail);
                }
            }
            catch (Exception)
            {
                Success = false; // in smtpclient exceptions are useless, "one or more error occurred"
            }
            finally
            {
                Streams.ForEach((Stream) => Stream.Close());
            }

            return Success;
        }
        #endregion

        #region Variables
        private readonly uint mPort;
        private readonly string mHost;
        private readonly string mLogin;
        private readonly string mPassword;
        private readonly SmtpClient mClient;
        #endregion
    }
}