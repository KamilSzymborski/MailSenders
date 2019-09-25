namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.AOLSender.xml" path="docs/header"/>
    public class AOLSender : Sender
    {
        #region Constructors
        /// <include file=".Docs/.AOLSender.xml" path="docs/constructor[@name='(string, string)']"/>
        public AOLSender(string Login, string Password) : base(587, "smtp.aol.com", Login, Password) { }
        #endregion
    }
}