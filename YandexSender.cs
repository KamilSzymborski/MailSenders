namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.YandexSender.xml" path="docs/header"/>
    public class YandexSender : Sender
    {
        #region Constructors
        /// <include file=".Docs/.YandexSender.xml" path="docs/constructor[@name='(string, string)']"/>
        public YandexSender(string Login, string Password) : base(587, "smtp.yandex.com", Login, Password) { }
        #endregion
    }
}