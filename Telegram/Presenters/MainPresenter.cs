using System.Windows;
using System.Speech.Synthesis;
using System.Windows.Controls;
using Telegram.DB;
using Telegram.Views;
using Telegram.Models;

namespace Telegram.Presenters
{
    public class MainPresenter
    {
        private readonly Database _database;
        private readonly IMainView _view;


        public MainPresenter(IMainView view)
        {
            _database = new Database();
            _view = view;

            _view.SendMessageEventHandler += SendMessage;
            _view.SelectedContactChangedEventHandler += SelectedContactChanged;
            _view.SendImageEventHandler += SendImage;
            _view.SpeakMessageEventHandler += SpeakText;
            _view.ChatClearEventHandler += ChatClear;

            #region Default data

            _database.AddUser(new User("Parvin Aliyev"));
            _database.AddUser(new User("Arthur Morgan"));
            _database.AddUser(new User("Joel Miller"));
            _database.AddUser(new User("Geralt Rivia"));

            _view.Profile = _database.GetUser("Parvin Aliyev");

            _database.AddChat( _view.Profile, _database.GetContact("Arthur Morgan") );
            _database.AddChat( _view.Profile, _database.GetContact("Joel Miller") );
            _database.AddChat( _view.Profile, _database.GetContact("Geralt Rivia") );

            #endregion
        }


        public void SendImage(object sender, RoutedEventArgs e)
        {
            var userMessage = new Image() { Source = _view.ImageContent };
            var botMessage = new TextBlock() { TextWrapping = TextWrapping.Wrap, Text = _database.GetRandomWord() };

            _view.ChatMessages.Add(new Message(userMessage, HorizontalAlignment.Right));
            _view.ChatMessages.Add(new Message(botMessage, HorizontalAlignment.Left));

            _view.ImageContent = null;
        }
        public void SendMessage(object sender, RoutedEventArgs e)
        {
            var userMessage = new TextBlock() { TextWrapping = TextWrapping.Wrap, Text = _view.SendMessageContent };
            var botMessage = new TextBlock() { TextWrapping = TextWrapping.Wrap, Text = _database.GetRandomWord() };

            _view.ChatMessages.Add(new Message(userMessage, HorizontalAlignment.Right));
            _view.ChatMessages.Add(new Message(botMessage, HorizontalAlignment.Left));

            _view.SendMessageContent = string.Empty;
        }

        public void SelectedContactChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in _view.Profile.Chats)
            {
                if (item.Key.FullName == _view.SelectedContact.FullName)
                {
                    _view.ChatMessages = _database.GetChat(item.Value);
                    return;
                }
            }
        }

        public void SpeakText(object sender, RoutedEventArgs e)
        {
            if (_view.Voice.State == SynthesizerState.Speaking) _view.Voice.SpeakAsyncCancelAll();
            else if (sender is FrameworkElement element)
            {
                var component = ((element.Parent as FrameworkElement).Parent as Panel).Children[0] as ContentControl;

                if (component.Content is Image) return;

                _view.Voice.SelectVoiceByHints(VoiceGender.Female);
                try { _view.Voice.SpeakAsync((component.Content as TextBlock).Text); }
                catch { return; }
            }
        }

        public void ChatClear(object sender, RoutedEventArgs e)
        {
            _view.ChatMessages.Clear();
        }
    }
}
