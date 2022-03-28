using System;
using System.Windows;
using System.Windows.Media;
using System.Speech.Synthesis;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Emoji.Wpf;
using Telegram.Models;

namespace Telegram.Views
{
    public interface IMainView
    {
        // Events
        event EventHandler<SelectionChangedEventArgs> SelectedContactChangedEventHandler;
        event EventHandler<RoutedEventArgs> SpeakMessageEventHandler;
        event EventHandler<RoutedEventArgs> SendMessageEventHandler;
        event EventHandler<RoutedEventArgs> SendImageEventHandler;
        event EventHandler<RoutedEventArgs> ChatClearEventHandler;


        // Properties
        User Profile { get; set; }
        
        Contact SelectedContact { get; set; }

        ImageSource ImageContent { get; set; }

        Picker EmojiPicker { get; }

        SpeechSynthesizer Voice { get; }


        // Observable Properties
        ObservableCollection<Message> ChatMessages { get; set; }

        string SendMessageContent { get; set; }

        string SearchContactContent { get; set; }

        string SearchMessageContent { get; set; }
    }
}
