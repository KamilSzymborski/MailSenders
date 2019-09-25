namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.GMailSender.xml" path="docs/header"/>
    public class GMailSender : Sender
    {
        #region Constructors
        /// <include file=".Docs/.GMailSender.xml" path="docs/constructor[@name='(string, string)']"/>
        public GMailSender(string Login, string Password) : base(587, "smtp.gmail.com", Login, Password) { }
        #endregion
    }
}