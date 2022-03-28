using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Speech.Synthesis;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using Emoji.Wpf;
using Telegram.Models;
using Telegram.Presenters;

namespace Telegram.Views
{
    public partial class MainView : Window, IMainView
    {
        // Events
        public event EventHandler<SelectionChangedEventArgs> SelectedContactChangedEventHandler;
        public event EventHandler<RoutedEventArgs> SendMessageEventHandler;
        public event EventHandler<RoutedEventArgs> SendImageEventHandler;
        public event EventHandler<RoutedEventArgs> SpeakMessageEventHandler;
        public event EventHandler<RoutedEventArgs> ChatClearEventHandler;


        // Properties
        public User Profile { get; set; }

        public Contact SelectedContact { get; set; }

        public ImageSource ImageContent { get; set; }

        public SpeechSynthesizer Voice { get; }

        public Picker EmojiPicker { get; }


        // Observable Properties
        public ObservableCollection<Message> ChatMessages
        {
            get { return (ObservableCollection<Message>)GetValue(MessagesProperty); }
            set { SetValue(MessagesProperty, value); ChatMessages.CollectionChanged += new NotifyCollectionChangedEventHandler(ChatMessages_Changed); }
        }
        public static readonly DependencyProperty MessagesProperty =
            DependencyProperty.Register("Messages", typeof(ObservableCollection<Message>), typeof(MainView));

        public string SendMessageContent
        {
            get { return (string)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }
        public static readonly DependencyProperty MessageTextProperty =
            DependencyProperty.Register("MessageText", typeof(string), typeof(MainView));

        public string SearchContactContent
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }
        public static readonly DependencyProperty SearchTextProperty =
           DependencyProperty.Register("SearchText", typeof(string), typeof(MainView));

        public string SearchMessageContent
        {
            get { return (string)GetValue(SearchMessageTextProperty); }
            set { SetValue(SearchMessageTextProperty, value); }
        }
        public static readonly DependencyProperty SearchMessageTextProperty =
            DependencyProperty.Register("SearchMessageText", typeof(string), typeof(MainView));


        public MainView()
        {
            InitializeComponent();

            Voice = new SpeechSynthesizer();
            EmojiPicker = new Picker();

            new MainPresenter(this);
            DataContext = this;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ContactsListBox.SelectedIndex = 0;

            #region EmojiPicker
            EmojiPicker.Children[0].Visibility = Visibility.Collapsed;

            EmojiPicker.SelectionChanged += EmojiPicker_ItemSelected;
            
            var pickerChildrens = new List<FrameworkElement>();
            
            foreach (FrameworkElement item in Picker.Children) pickerChildrens.Add(item);

            Picker.Children.Clear();
            Picker.Children.Add(EmojiPicker);

            foreach (FrameworkElement item in pickerChildrens) Picker.Children.Add(item);
            #endregion
        }

        private void SendMessage_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrWhiteSpace(SendMessageContent))
                SendMessageEventHandler.Invoke(sender, e);
        }
        private void SendMessage_ButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SendMessageContent))
                SendMessageEventHandler.Invoke(sender, e);
        }

        private void ImageSelect_ButtonClicked(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();

            fileDialog.Filter = "PNG Files(*.png)|*.png|JPEG Files(*.jpeg)|*.jpeg|JPG Files(*.jpg)|*.jpg";
            fileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            fileDialog.FilterIndex = 1;

            if (fileDialog.ShowDialog() == true)
            {
                try
                {
                    ImageContent = new ImageSourceConverter().ConvertFrom(File.ReadAllBytes(fileDialog.FileName)) as ImageSource;
                    SendImageEventHandler(fileDialog.FileName, e);
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to send the selected file.", "Image Convert Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ChatMessages_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            try { MessagesListBox.ScrollIntoView(MessagesListBox.Items[ChatMessages.Count - 1]); }
            catch { return; }
        }

        private void EmojiPicker_ItemSelected(object sender, EventArgs e) => SendMessageContent += EmojiPicker.Selection;
        private void EmojiPicker_ButtonClicked(object sender, RoutedEventArgs e)  => (EmojiPicker.Children[1] as Popup).IsOpen = true;

        private void ContactList_SelectedChanged(object sender, SelectionChangedEventArgs e)
            => SelectedContactChangedEventHandler.Invoke(sender, e);

        private void SpeakText_ButtonClicked(object sender, RoutedEventArgs e)
            => SpeakMessageEventHandler.Invoke(sender, e);

        private void ChatClear_MenuItemClicked(object sender, RoutedEventArgs e)
            => ChatClearEventHandler.Invoke(sender, e);
    }
}
