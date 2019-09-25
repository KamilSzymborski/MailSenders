namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.ZohoSender.xml" path="docs/header"/>
    public class ZohoSender : Sender
    {
        #region Constructors
        /// <include file=".Docs/.ZohoSender.xml" path="docs/constructor[@name='(string, string)']"/>
        public ZohoSender(string Login, string Password) : base(587, "smtp.zoho.eu", Login, Password) { }
        #endregion
    }
}