namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.YahooSender.xml" path="docs/header"/>
    public class YahooSender : Sender
    {
        #region Constructors
        /// <include file=".Docs/.YahooSender.xml" path="docs/constructor[@name='(string, string)']"/>
        public YahooSender(string Login, string Password) : base(587, "smtp.mail.yahoo.com", Login, Password) { }
        #endregion
    }
}