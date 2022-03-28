using System;
using System.Windows.Media;

namespace Telegram.Models
{
    public class Contact
    {
        public Contact(string fullName)
        {
            FullName = fullName;

            var fullnameSplited = fullName.Split();
            var random = new Random();

            WrapName = fullnameSplited[0][0].ToString() + fullnameSplited[1][0].ToString();
            Color = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)random.Next(0, 256), 244, 94));
        }

        public string FullName { get; set; }
        public string WrapName { get; set; }
        public SolidColorBrush Color { get; set; }


        public override int GetHashCode() => FullName.GetHashCode();
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();
        public override string ToString() => FullName;
    }
}
