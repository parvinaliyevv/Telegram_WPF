using System;
using System.Windows;

namespace Telegram.Models
{
    public class Message
    {
        public Message(object content, HorizontalAlignment align)
        {
            Content = content;
            Align = align;
            SentTime = DateTime.Now;
        }

        public object Content { get; set; }
        public HorizontalAlignment Align { get; set; }
        public DateTime SentTime { get; set; }
    }
}
