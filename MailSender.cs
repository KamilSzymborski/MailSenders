namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.MailSender.xml" path="docs/header"/>
    public class MailSender : Sender
    {
        #region Constructors
        /// <include file=".Docs/.MailSender.xml" path="docs/constructor[@name='(string, string)']"/>
        public MailSender(string Login, string Password) : base(587, "smtp.mail.com", Login, Password) { }
        #endregion
    }
}