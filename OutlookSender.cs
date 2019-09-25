namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.OutlookSender.xml" path="docs/header"/>
    public class OutlookSender : Sender
    {
        #region Constructors
        /// <include file=".Docs/.OutlookSender.xml" path="docs/constructor[@name='(string, string)']"/>
        public OutlookSender(string Login, string Password) : base(587, "smtp-mail.outlook.com ", Login, Password) { }
        #endregion
    }
}