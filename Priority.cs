namespace KamilSzymborski.MailSenders
{
    /// <include file=".Docs/.Priority.xml" path="docs/header"/>
    public enum Priority
    {
        /// <include file=".Docs/.Priority.xml" path="docs/value[@name='Low']/*"/>
        Low = -1,
        /// <include file=".Docs/.Priority.xml" path="docs/value[@name='Normal']/*"/>
        Normal = 0,
        /// <include file=".Docs/.Priority.xml" path="docs/value[@name='High']/*"/>
        High = 1
    }
}