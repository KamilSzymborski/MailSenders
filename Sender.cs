using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            mSemaphore = new SemaphoreSlim(1);
            
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
            return mSend(string.Empty, Message, Recipient, Attachments, false, null, null, null);
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='Send(string, string, string, params Attachment[])']/*"/>
        public bool Send(string Title, string Message, string Recipient, params Attachment[] Attachments)
        {
            return mSend(Title, Message, Recipient, Attachments, false, null, null, null);
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='Send(string, string, string, string, params Attachment[])']/*"/>
        public bool Send(string Title, string Message, string Recipient, string DisplayName = null, params Attachment[] Attachments)
        {
            return mSend(Title, Message, Recipient, Attachments, false, DisplayName, null, null);
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='Send(string, string, string, string, string, params Attachment[])']/*"/>
        public bool Send(string Title, string Message, string Recipient, string DisplayName = null, string Replyer = null, params Attachment[] Attachments)
        {
            return mSend(Title, Message, Recipient, Attachments, false, DisplayName, Replyer, null);
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='Send(string, string, string, string, string, Priority, params Attachment[])']/*"/>
        public bool Send(string Title, string Message, string Recipient, string DisplayName = null, string Replyer = null, Priority? Priority = null, params Attachment[] Attachments)
        {
            return mSend(Title, Message, Recipient, Attachments, false, DisplayName, Replyer, Priority);
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='SendAsync(string, string, params Attachment[])']/*"/>
        public async Task<bool> SendAsync(string Message, string Recipient, params Attachment[] Attachments)
        {
            return await Task.Run(() => mSend(string.Empty, Message, Recipient, Attachments, true, null, null, null));
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='SendAsync(string, string, string, params Attachment[])']/*"/>
        public async Task<bool> SendAsync(string Title, string Message, string Recipient, params Attachment[] Attachments)
        {
            return await Task.Run(() => mSend(Title, Message, Recipient, Attachments, true, null, null, null));
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='SendAsync(string, string, string, string, params Attachment[])']/*"/>
        public async Task<bool> SendAsync(string Title, string Message, string Recipient, string DisplayName = null, params Attachment[] Attachments)
        {
            return await Task.Run(() => mSend(Title, Message, Recipient, Attachments, true, DisplayName, null, null));
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='SendAsync(string, string, string, string, string, params Attachment[])']/*"/>
        public async Task<bool> SendAsync(string Title, string Message, string Recipient, string DisplayName = null, string Replyer = null, params Attachment[] Attachments)
        {
            return await Task.Run(() => mSend(Title, Message, Recipient, Attachments, true, DisplayName, Replyer, null));
        }
        /// <include file=".Docs/.Sender.xml" path="docs/method[@name='SendAsync(string, string, string, string, string, Priority, params Attachment[])']/*"/>
        public async Task<bool> SendAsync(string Title, string Message, string Recipient, string DisplayName = null, string Replyer = null, Priority? Priority = null, params Attachment[] Attachments)
        {
            return await Task.Run(() => mSend(Title, Message, Recipient, Attachments, true, DisplayName, Replyer, Priority));
        }

        private void mInit()
        {
            mClient.Host = mHost;
            mClient.Port = (int)mPort;
            mClient.Credentials = new NetworkCredential(mLogin, mPassword);
            mClient.EnableSsl = true;
        }
        private bool mSend(string Title, string Message, string Recipient, Attachment[] Attachments, bool RaiseEvents, string DisplayName = null, string Replyer = null, Priority? Priority = null)
        {
            mSemaphore.Wait();

            var Success = true;
            var Streams = new List<Stream>();

            try
            {
                using (var Mail = new MailMessage(mLogin, Recipient))
                {
                    Mail.Subject = Title;
                    Mail.Body = Message;
                    Mail.IsBodyHtml = true;
                    
                    if ( ! (Priority is null))
                        switch (Priority)
                        {
                            case MailSenders.Priority.Low: Mail.Priority = MailPriority.Low; break;
                            case MailSenders.Priority.Normal: Mail.Priority = MailPriority.Normal; break;
                            case MailSenders.Priority.High: Mail.Priority = MailPriority.High; break;
                        }
                    if ( ! (Replyer is null)) Mail.ReplyTo = new MailAddress(Replyer);
                    if ( ! (DisplayName is null)) Mail.From = new MailAddress(mLogin, DisplayName);
                    
                    foreach (var Attachment in Attachments)
                    {
                        var Stream = new MemoryStream(Attachment.Data)
                        {
                            Position = 0
                        };

                        Streams.Add(Stream);

                        var NAttachment = new System.Net.Mail.Attachment(Stream, MimeService.IsKnown(Attachment.FileName) ? MimeService.From(Attachment.FileName) : MimeService.From(".bin"));
                            NAttachment.ContentDisposition.FileName = Attachment.FileName;

                        Mail.Attachments.Add(NAttachment);
                    }

                    mClient.Send(Mail);
                }
            }
            catch
            {
                Success = false; // in smtpclient exceptions are useless, "one or more error occurred"
            }
            finally
            {
                Streams.ForEach((Stream) => Stream.Close());

                if (RaiseEvents) if (Success) Sended?.Invoke(); else Failed?.Invoke();

                mSemaphore.Release();
            }

            return Success;
        }
        #endregion

        #region Properties
        /// <include file=".Docs/.Sender.xml" path="docs/property[@name='Port']/*"/>
        public uint Port { get { return mPort; } }
        /// <include file=".Docs/.Sender.xml" path="docs/property[@name='Host']/*"/>
        public string Host { get { return mHost; } }
        /// <include file=".Docs/.Sender.xml" path="docs/property[@name='Login']/*"/>
        public string Login { get { return mLogin; } }
        /// <include file=".Docs/.Sender.xml" path="docs/property[@name='Password']/*"/>
        public string Password { get { return mPassword; } }
        #endregion

        #region Variables
        private readonly uint mPort;
        private readonly string mHost;
        private readonly string mLogin;
        private readonly string mPassword;
        private readonly SmtpClient mClient;
        private readonly SemaphoreSlim mSemaphore;
        #endregion
    }
}