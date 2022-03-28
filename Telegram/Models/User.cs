using System;
using System.Collections.Generic;

namespace Telegram.Models
{
    public class User
    {
        public User(string fullName)
        {
            FullName = fullName;

            Chats = new Dictionary<Contact, Guid>();
        }


        public string FullName { get; set; }
        public Dictionary<Contact, Guid> Chats { get; set; }

        public override int GetHashCode() => FullName.GetHashCode();
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();
        public override string ToString() => FullName;
    }
}
