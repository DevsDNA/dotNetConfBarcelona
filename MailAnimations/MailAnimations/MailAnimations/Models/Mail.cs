namespace MailAnimations.Models
{
    using System;
    using System.Collections.Generic;

    public class Mail
    {
        public string SourceMail { get; set; }
        public string SourceName { get; set; }
        public List<string> TargetMailList { get; set; }
        public List<string> CopyTargetMailList { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
    }
}
