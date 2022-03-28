using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Telegram.Models;

namespace Telegram.DB
{
    public class Database
    {
        private readonly List<User> _users;
        private readonly List<string> _words;
        private readonly Dictionary<Guid, ObservableCollection<Message>> _chats;

        public Database()
        {
            _users = new List<User>();
            _chats = new Dictionary<Guid, ObservableCollection<Message>>();

            _words = new List<string>()
            {
                "Object Oriented Programming",
                "Class",
                "Encapsulation",
                "Inheritance",
                "Polymorphism",
                "Abstraction",
                "Virtual",
                "Sealed",
                "Pure Virtual Method",
                "Single Inheritance",
                "Multiple Inheritance",
                "Multi Level Inheritance",
                "Hierarchical Inheritance",
                ".NET Framework",
                ".NET Core",
                "CLR Virtual Machine",
                "WinForms",
                "WPF",
                "Design Paterns",
                "SQL",
                "ADO.NET",
                "System Programming",
                "Network Programming",
                "ASP.NET",
                "JavaScript dangerous",
                "Save juniors from JavaScript",
                "Tablets from PHP",
            };
        }


        public ObservableCollection<Message> GetChat(Guid guid)
        {
            foreach (var item in _chats)
                if (item.Key == guid) return item.Value;

            return null;
        }
        public void AddChat(User profile, Contact contact)
        {
            if (profile is null) throw new ArgumentNullException(nameof(profile));
            else if (contact is null) throw new NullReferenceException(nameof(contact));

            var chatKey = Guid.NewGuid();

            GetUser(profile.FullName).Chats.Add(contact, chatKey);
            GetUser(contact.FullName).Chats.Add(new Contact(profile.FullName), chatKey);

            _chats.Add(chatKey, new ObservableCollection<Message>());
        }


        /// <param name="fullName"> Write with a guid id for a long time </param>
        public User GetUser(string fullName)
        {
            foreach (var user in _users)
                if (user.FullName == fullName) return user;

            return null;
        }
        public void AddUser(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            _users.Add(user);
        }


        public Contact GetContact(string fullName)
        {
            foreach (var user in _users)
                if (user.FullName == fullName) return new Contact(user.FullName);

            return null;
        }

        public string GetRandomWord()
        {
            return _words[new Random().Next(_words.Count)];
        }
    }
}
